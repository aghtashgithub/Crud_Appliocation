using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crud_Appliocation.Models;

namespace Crud_Appliocation.Controllers
{
    public class LogSInController : Controller
    {
        DB_StudentsEntities db = new DB_StudentsEntities();
        // GET: LogSIn


        #region Login
        public ActionResult Index(LoginSingup s)
        {

       
            var data = db.LoginSingups.Where(model => model.Username == s.Username && model.Password == s.Password).FirstOrDefault();
            if (data != null)
            {
                Session["Username"] = s.Username;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
          
        }

        #endregion
        #region SignUp
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(LoginSingup lg)
        {
            if (ModelState.IsValid == true)
            {
                db.LoginSingups.Add(lg);
                db.SaveChanges();
                return RedirectToAction("Index", "LogSin");
            }
            else
            {
                return View();
            }
        
        }
        #endregion
    }
}