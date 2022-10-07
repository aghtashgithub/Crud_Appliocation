using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crud_Appliocation.Models;

namespace Crud_Appliocation.Controllers
{
    public class HomeController : Controller
    {
        DB_StudentsEntities db = new DB_StudentsEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        #region StudentList
        public ActionResult StudentList(string SearchBy , string search)
        {
            if (SearchBy == "Name")
            {
                var data = db.Tbl_Student.Where(model => model.Name.StartsWith(search)).ToList();
                return View(data);
            }

            else if (Session["Username"] == null)
            {
                return RedirectToAction("Index","LogSin") ;
            }
            else
            {
                var dta = db.Tbl_Student.ToList();
                return View(dta);
            }

        }

   
        #endregion


        #region AddProduct
        public ActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(Tbl_Student s)
        {

            string filename = Path.GetFileNameWithoutExtension(s.Imagefile.FileName);
            string Extension = Path.GetExtension(s.Imagefile.FileName);
            filename = filename + Extension;
            s.Image_Name = "~/img/" + filename;
            filename = Path.Combine(Server.MapPath("~/img/"), filename);
            s.Imagefile.SaveAs(filename);


           
                db.Tbl_Student.Add(s);
                db.SaveChanges();
                return RedirectToAction("StudentList", "Home");
            }
      
        #endregion

        #region Edit

        public ActionResult Edit(int id)
        {
            var data = db.Tbl_Student.Where(model => model.id == id).FirstOrDefault();
            Session["Image"] = data.Image_Name;
             return View(data);
        }

        [HttpPost]
        public ActionResult Edit(Tbl_Student s)
        {
            if (ModelState.IsValid == true)
            {
                if (s.Imagefile  != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(s.Imagefile.FileName);
                    string Extension = Path.GetExtension(s.Imagefile.FileName);
                    filename = filename + Extension;
                    s.Image_Name = "~/img/" + filename;
                    filename = Path.Combine(Server.MapPath("~/img/"), filename);
                    s.Imagefile.SaveAs(filename);
                    db.Entry(s).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("StudentList", "Home");
                }
                else
                {
                    s.Image_Name = Session["Image"].ToString();
                    db.Entry(s).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("StudentList", "Home");

                }

            }
            else
            {
                return View();
            }

     
        }
        #endregion

        #region Delete
        public ActionResult Delete(int id)
        {
            var data = db.Tbl_Student.Where(model => model.id == id).FirstOrDefault();
            db.Tbl_Student.Remove(data);
            db.SaveChanges();
            return RedirectToAction("StudentList","Home");
        }
        #endregion


        #region Exchange

        public ActionResult Exchange(int id)
        {
            var slect = db.Tbl_Student.Find(id);

            if (slect.Attendence == 0)
            {
                slect.Attendence = 1;
            }
            else
            {
                slect.Attendence = 0;

            }

            db.Entry(slect).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("StudentList");
        }
        #endregion

        #region LogOut
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "LogSin");
        }

        #endregion

    }
}