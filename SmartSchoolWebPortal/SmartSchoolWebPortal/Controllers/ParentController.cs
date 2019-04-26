using SmartSchoolWebPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSchoolWebPortal.Controllers
{
    public class ParentController : Controller
    {
        public ActionResult Register()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            List<string> list = new List<string>();
            var students = db.Students.Where(x => x.Status == 1).ToList();
            foreach(var i in students)
            {
                list.Add(i.RegisterationNumber);
            }
            list.Sort();
            ViewBag.list = new SelectList(list);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(ParentViewModel Collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            Parent s = new Parent();
            s.Name = Collection.Name;
            s.Contact = Collection.Contact;
            s.NIC = Collection.NIC;
            s.Email = Collection.Email;
            var Id = db.Students.Where(x => x.RegisterationNumber == Collection.RegNo).First();

            s.StudentId = Id.Id;
            db.Parents.Add(s);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Account(string Message)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var user = db.Parents.Where(x => x.Id == LoginClass.LoginId).First();

            ViewBag.Name = user.Name;
            ViewBag.Contact = user.Contact;
            ViewBag.Email = user.Email;
            ViewBag.NIC = user.NIC;
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
        public ActionResult Login(Parent Collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var list = db.Parents.ToList();
            foreach (var i in list)
            {
                if (Collection.UserName == i.UserName && Collection.Password == i.Password && i.Status == 1)
                {
                    LoginClass.LoginSession = "Parent";
                    LoginClass.LoginId = i.Id;
                    return RedirectToAction("Account");

                }

            }
            string message = "InValid User Name or Password!";
            return RedirectToAction("Login", "Parent", new { Message = message });
        }


        
        public ActionResult LogOff()
        {
            LoginClass.LoginSession = null;
            LoginClass.LoginId = 0;
            return RedirectToAction("Index", "Home");
        }

        // GET: Parent/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Parent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parent/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Parent/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Parent/Edit/5
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

        // GET: Parent/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Parent/Delete/5
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
