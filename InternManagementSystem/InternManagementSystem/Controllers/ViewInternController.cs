    using InternManagementSystem.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

namespace InternManagementSystem.Controllers
{
    public class ViewInternController : Controller
    {
        InternDbEntities entity = new InternDbEntities();
        public ActionResult Index()
        {
            int yetki = Convert.ToInt32(Session["adminId"]);
            if (Session["adminId"] != null)
            {
                if (yetki == 1)
                {
                    DateTime? InternShipStart = DateTime.Now.AddYears(-500);
                    DateTime? InternShipEnd = DateTime.Now.AddYears(500);
                    Index(InternShipStart, InternShipEnd);
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            return RedirectToAction("Index", "Admin");
        }
        [HttpPost]
        public ActionResult Index(DateTime? InternshipStart, DateTime? InternShipEnd)
        {
            List<InternInfos> Interns;
            if (InternshipStart != null && InternShipEnd != null)
            {
                Interns = (from a in entity.InternInfos
                           where a.InternshipStartDate >= InternshipStart
                           && a.InternshipEndDate <= InternShipEnd
                           select a).ToList();
            }
            else if (InternshipStart != null && InternShipEnd == null)
            {
                var futuredate = DateTime.Now.AddYears(5);
                Interns = (from a in entity.InternInfos
                           where a.InternshipStartDate >= InternshipStart
                           && a.InternshipEndDate <= futuredate
                           select a).ToList();
            }
            else
            {
                Interns = entity.InternInfos.ToList();

            }
            if (Interns == null || !Interns.Any())
            {
                Interns = new List<InternInfos>();
            }
            return View(Interns);
        }

        [HttpPost]
        public ActionResult DeleteIntern(int id)
        {
            var internToDelete = entity.InternInfos.FirstOrDefault(i => i.InternId == id);
            if (internToDelete != null)
            {
                entity.InternInfos.Remove(internToDelete);
                entity.SaveChanges();
            }
            return RedirectToAction("Index", "ViewIntern");
        }
        public ActionResult ChangeIntern(int id)
        {
            var intern = entity.InternInfos.Find(id);
            if (intern == null)
            {
                return HttpNotFound();
            }
            if (intern.UniversityId != null)
            {
                ViewBag.Universities = new SelectList(entity.Universities, "UniversityId", "UniversityName", intern.UniversityId);
            }
            else
            {
                ViewBag.Universities = new SelectList(entity.Universities, "UniversityId", "UniversityName");
            }
            if (intern.HschoolId != null)
            {
                ViewBag.Hschools = new SelectList(entity.Hschools, "HschoolId", "HschoolName", intern.HschoolId);
            }
            else
            {
                ViewBag.Hschools = new SelectList(entity.Hschools, "HschoolId", "HschoolName");
            }
            ViewBag.Departments = new SelectList(entity.Departments, "DepartmentId", "DepartmentName", intern.DepartmentId);
            ViewBag.Cities = new SelectList(entity.Cities, "CityId", "CityName", intern.CityId);
            ViewBag.OtherName = intern.OtherName;
            return View("ChangeIntern", intern);
        }

        public ActionResult Change(InternInfos p1)
        {
            var intern = entity.InternInfos.Find(p1.InternId);

            intern.FirstName = p1.FirstName;
            intern.LastName = p1.LastName;
            intern.Email = p1.Email;
            if (p1.UniversityId != null)
            {
                intern.UniversityId = p1.UniversityId;
            }
            else if (p1.HschoolId != null)
            {
                intern.HschoolId = p1.HschoolId;
            }
            else if (!string.IsNullOrEmpty(p1.OtherName))
            {
                intern.OtherName = p1.OtherName;
            }
            intern.DepartmentId = p1.DepartmentId;
            intern.CityId = p1.CityId;
            intern.InternshipStartDate = p1.InternshipStartDate;
            intern.InternshipEndDate = p1.InternshipEndDate;

            entity.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}