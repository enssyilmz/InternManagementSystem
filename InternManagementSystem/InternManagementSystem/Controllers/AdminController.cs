using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternManagementSystem.Models;

namespace InternManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        InternDbEntities entity=new InternDbEntities();
        // GET: Intern
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username,string password)
        {
            var admin = (from a in entity.AdminInfos
                         where a.UserName == username && a.Password == password select a).FirstOrDefault();

            if (admin != null)
            {
                Session["adminId"] = admin.AdminId;
                Session["adminUserName"] = admin.UserName;
                Session["adminpassword"] = admin.Password;

                switch (admin.AdminId)
                {
                    case 1:
                        return RedirectToAction("Index", "Intern");
                    default:
                        return View();
                }
            }
            else
            {
                return View();
            }
            
        }
        public ActionResult Cikis()
        {
            Session["adminId"] = null;
            return RedirectToAction("Index");
        }
    }
}