using SmartSchoolWebPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSchoolWebPortal.Controllers
{
    public class StudentController : Controller
    {

        public ActionResult LogOff()
        {
            LoginClass.LoginSession = null;
            LoginClass.LoginId = 0;
            return RedirectToAction("Index", "Home");
        }


        // GET: Student
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(StudentViewModel Collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            Student s = new Student();
            s.Name = Collection.Name;
            s.Contact = Collection.Contact;
            s.RegisterationNumber = Collection.RegisterationNumber;
            s.Email = Collection.Email;
            db.Students.Add(s);
            db.SaveChanges();
            return View();
        }

        public ActionResult Account(string Message)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var user = db.Students.Where(x => x.Id == LoginClass.LoginId).First();

            ViewBag.Name = user.Name;
            ViewBag.Contact = user.Contact;
            ViewBag.Email = user.Email;
            ViewBag.RegNo = user.RegisterationNumber;
            ViewBag.Message = Message;
            return View();
        }

        public ActionResult Login(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(StudentViewModel Collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var list = db.Students.ToList();
            foreach(var i in list)
            {
                if (Collection.UserName == i.UserName && Collection.Password == i.Password && i.Status == 1)
                {
                    LoginClass.LoginSession = "Student";
                    LoginClass.LoginId = i.Id;
                    return RedirectToAction("Account");

                }

            }
            string message = "InValid User Name or Password!";
            return RedirectToAction("Login", "Student", new { Message = message });
        }



        [HttpPost]
        [AllowAnonymous]
        public ActionResult Change(StudentViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var user = db.Students.Where(x => x.Id == LoginClass.LoginId).First();
            user.UserName = collection.UserName;
            user.Password = collection.Password;
            db.SaveChanges();
            string message = "User Name & Password Updated!";
            return RedirectToAction("Account", "Student", new { Message = message });
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Student/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
