using SmartSchoolWebPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmartSchoolWebPortal.Controllers
{
    public class ParentController : Controller
    {
        // GET: Parent
        public ActionResult Index()
        {
            return View();
        }

        // GET: Parent
        public ActionResult Register()
        {
            return View();
        }

        // POST: Parent
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(ParentViewModel Collection)
        {
            DBSmartSchoolWebPortalEntities db = new DBSmartSchoolWebPortalEntities();
            Parent parent = new Parent();
            parent.Name = Collection.Name;
            parent.Contact = Collection.Contact;
            parent.Email = Collection.Email;
            parent.NIC = Collection.NIC;
            List<ParentViewModel> list = new List<ParentViewModel>();
            var students = db.Students.ToList();
            foreach(var i in students)
            {
                if(i.RegisterationNumber == Collection.StudentId)
                {
                    parent.StudentId = i.Id;
                }
            }
            

            db.Parents.Add(parent);
            db.SaveChanges();
            return View();
        }

        // GET: Parent
        public ActionResult Login()
        {
            return View();
        }

        // POST: Parent
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(ParentViewModel Collection)
        {
            DBSmartSchoolWebPortalEntities db = new DBSmartSchoolWebPortalEntities();
            Parent parent = new Parent();
            parent.Name = Collection.Name;
            parent.Contact = Collection.Contact;
            parent.Email = Collection.Email;
            parent.NIC = Collection.NIC;
            List<ParentViewModel> list = new List<ParentViewModel>();
            var students = db.Students.ToList();
            foreach (var i in students)
            {
                if (i.RegisterationNumber == Collection.StudentId)
                {
                    parent.StudentId = i.Id;
                }
            }


            db.Parents.Add(parent);
            db.SaveChanges();
            return View();
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
