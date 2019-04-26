using SmartSchoolWebPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSchoolWebPortal.Controllers
{
    public class ManagementController : Controller
    {

        public static int ClassIdGlobal = 0;
        public static int StudentIdGlobal = 0;
        public static int ParentIdGlobal = 0;

        public static int SectionIdGlobal = 0;
        public static DateTime GlobalDate;


        // GET: Management
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel Collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var admin = db.Managements.First();
            if(admin.UserName == Collection.UserName && admin.Password == Collection.Password)
            {
                LoginClass.LoginSession = "Admin";
                
               
                return RedirectToAction("Account");

            }
            return View();
        }

        public ActionResult Account()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var admin = db.Managements.First();
            ViewBag.Name = admin.Name;
            ViewBag.Contact = admin.Contact;
            ViewBag.CNIC = admin.NIC;
            ViewBag.Email = admin.Email;
            
            return View();
        }



        public ActionResult Attendance()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Attendance(ClassAttendanceViewModel Collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            ClassAttendance ClassAtt = new ClassAttendance();
            ClassAtt.ClassId = Collection.ClassId;
            ClassAtt.SectionId = Collection.SectionId;
            ClassAtt.Date = Collection.Date;
            ClassIdGlobal = Collection.ClassId;
            SectionIdGlobal = Collection.SectionId;
            GlobalDate = Collection.Date;
            db.ClassAttendances.Add(ClassAtt);
            db.SaveChanges();
            return RedirectToAction("StudentAttendance");

        }

        public ActionResult StudentAttendance()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var list = db.Students.ToList();
            List<StudentViewModel> LIST = new List<StudentViewModel>();
            foreach(var i in list)
            {
                StudentViewModel s = new StudentViewModel();
                s.Name = i.Name;
                s.RegisterationNumber = i.RegisterationNumber;
                s.Id = i.Id;
                LIST.Add(s);

            }
            return View(LIST);
        }

        
        public ActionResult Mark(int Id)
        {
            StudentIdGlobal = Id;
            return RedirectToAction("Marked");
        }

        public ActionResult Marked()
        {
            
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Marked(int Status)
        {

            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            StudentAttendance sd = new StudentAttendance();
            
            
            var i = db.ClassAttendances.Where(x => x.ClassId == ClassIdGlobal && x.SectionId == SectionIdGlobal && x.Date == GlobalDate).First();

            sd.ClassAttendanceId = i.Id;
            sd.StudentId = StudentIdGlobal;
            sd.Status = Status;
            db.StudentAttendances.Add(sd);
            db.SaveChanges();

            
            return RedirectToAction("StudentAttendance");
        }

        public ActionResult STudentRequests()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var list = db.Students.Where(x => x.Status != 1).ToList();
            List<StudentViewModel> PassList = new List<StudentViewModel>();
            foreach(var i in list)
            {
                StudentViewModel s = new StudentViewModel();
                s.Name = i.Name;
                s.RegisterationNumber = i.RegisterationNumber;
                s.Id = i.Id;
                PassList.Add(s);
            }

            return View(PassList);
        }

        public ActionResult SRequestAccepts(int Id)
        {
            StudentIdGlobal = Id;
            return RedirectToAction("SRequestAcceptEnd");
        }

        public ActionResult SRequestAcceptEnd()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SRequestAcceptEnd(StudentViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            int i = StudentIdGlobal;
            var student = db.Students.Where(x => x.Id == StudentIdGlobal).First();
            student.UserName = collection.UserName;
            student.Password = collection.Password;
            student.Status = 1;
            db.SaveChanges();
            return RedirectToAction("STudentRequests");
        }

        public ActionResult PTudentRequests()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var list = db.Parents.Where(x => x.Status != 1).ToList();
            List<ParentViewModel> PassList = new List<ParentViewModel>();
            foreach (var i in list)
            {
                ParentViewModel s = new ParentViewModel();
                s.Name = i.Name;
                s.NIC = i.NIC;
                s.Id = i.Id;
                PassList.Add(s);
            }

            return View(PassList);
        }

        public ActionResult PRequestAccepts(int Id)
        {
            ParentIdGlobal = Id;
            return RedirectToAction("PRequestAcceptEnd");
        }

        public ActionResult PRequestAcceptEnd()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PRequestAcceptEnd(StudentViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            int i = ParentIdGlobal;
            var parent = db.Parents.Where(x => x.Id == ParentIdGlobal).First();
            parent.UserName = collection.UserName;
            parent.Password = collection.Password;
            parent.Status = 1;
            db.SaveChanges();
            return RedirectToAction("PTudentRequests");
        }

        public ActionResult LogOff()
        {
            LoginClass.LoginSession = null;
            LoginClass.LoginId = 0;
            return RedirectToAction("Index", "Home");
        }

        // GET: Management/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Management/Create
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

        // GET: Management/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Management/Edit/5
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

        // GET: Management/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Management/Delete/5
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
