using InternManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
public class MyViewModel
{
    public List<Universities> Universities { get; set; }
    public List<Hschools> Hschools { get; set; }
    public List<Cities> Cities { get; set; }
    public List<Departments> Departments { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int SelectedDepartmentId { get; set; }
    public int? SelectedHschoolsId { get; set; }
    public int? SelectedUniversityId { get; set; }
    public int SelectedCityId { get; set; }
    public string Email { get; set; }
    public DateTime InternshipStartDate { get; set; }
    public DateTime InternshipEndDate { get; set; }
    public string OtherTextInput { get; set; }
}


namespace InternManagementSystem.Controllers
{
    public class InternController : Controller
    {
        InternDbEntities entity = new InternDbEntities();
        public ActionResult Index()
        {
            int yetki = Convert.ToInt32(Session["adminId"]);
            if (Session["adminId"] != null)
            {
                if (yetki == 1)
                {
                    var universities = entity.Universities.ToList();
                    var hschools = entity.Hschools.ToList();
                    var cities = entity.Cities.ToList();
                    var departments = entity.Departments.ToList();
                    var viewModel = new MyViewModel
                    {
                        Hschools = hschools,
                        Universities = universities,
                        Cities = cities,
                        Departments = departments
                    };

                    return View(viewModel);
                }
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public ActionResult Index(MyViewModel model,DateTime? InternshipStart, DateTime? InternShipEnd)
        {

            if (ModelState.IsValid)
            {
                InternInfos internInfo = new InternInfos
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DepartmentId = model.SelectedDepartmentId,
                    CityId = model.SelectedCityId,
                    Email = model.Email,
                    UniversityId = model.SelectedUniversityId,
                    HschoolId = model.SelectedHschoolsId,
                    InternshipStartDate = InternshipStart ?? DateTime.Now,
                    InternshipEndDate = InternShipEnd ?? DateTime.Now
                };
                if (model.SelectedUniversityId.HasValue)
                {
                    internInfo.UniversityId = model.SelectedUniversityId;
                }
                else if (model.SelectedHschoolsId.HasValue)
                {
                    internInfo.HschoolId = model.SelectedHschoolsId;
                }
                else if(!string.IsNullOrWhiteSpace(model.OtherTextInput))
                {
                    internInfo.OtherName = model.OtherTextInput;
                }
       
                using (var db = new InternDbEntities())
                {
                    db.InternInfos.Add(internInfo);
                    db.SaveChanges();
                }
                model.Universities = GetUniversities().ToList();
                model.Hschools = GetHschools().ToList();
                model.Departments = GetDepartments().ToList();
                model.Cities = GetCities().ToList();
                return View(model);
            }

            model.Universities = GetUniversities().ToList();
            model.Hschools = GetHschools().ToList();
            model.Departments = GetDepartments().ToList();
            model.Cities = GetCities().ToList();
            return View(model);
        }
        

        private IEnumerable<Universities> GetUniversities()
        {
            using (var db = new InternDbEntities())
            {
                return db.Universities.ToList();
            }
        }

        private IEnumerable<Departments> GetDepartments()
        {
            using (var db = new InternDbEntities())
            {
                return db.Departments.ToList();
            }
        }

        private IEnumerable<Cities> GetCities()
        {
            using (var db = new InternDbEntities())
            {
                return db.Cities.ToList();
            }
        }  
        private IEnumerable<Hschools> GetHschools()
        {
            using (var db = new InternDbEntities())
            {
                return db.Hschools.ToList();
            }
        }

        public ActionResult SubmitInternForm()
        {
            return RedirectToAction("Index", "ViewIntern");
        }

    }
}