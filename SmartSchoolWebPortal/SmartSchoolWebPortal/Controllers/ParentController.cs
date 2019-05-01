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

        public ActionResult ViewHostels()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var hostels = db.Hostels.ToList();
            List<HostelViewModel> List = new List<HostelViewModel>();
            foreach (var i in hostels)
            {
                HostelViewModel h = new HostelViewModel();
                h.HostelName = i.Name;
                h.HostelLocation = i.Location;
                h.ImagePath = i.ImagePath;
                h.Id = i.Id;
                h.HostelRent = Convert.ToInt32(i.Rent);
                List.Add(h);
            }
            return View(List);
        }

        public ActionResult RequestHostel(int id)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            HostelRequest request = new HostelRequest();
            request.HostelId = id;
            request.ParentId = LoginClass.LoginId;
            db.HostelRequests.Add(request);
            db.SaveChanges();

            string message = "Hostel Requested!";
            return RedirectToAction("Account", "Parent", new { Message = message });
        }

        public ActionResult ViewAttendance()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var studentId = db.Parents.Where(x => x.Id == LoginClass.LoginId).First();
            var AttendanceList = db.StudentAttendances.Where(x => x.StudentId == studentId.StudentId).ToList();
            List<StudentAttendanceViewModel> PassList = new List<StudentAttendanceViewModel>();

            foreach (var i in AttendanceList)
            {
                StudentAttendanceViewModel att = new StudentAttendanceViewModel();
                var classattendance = db.ClassAttendances.Where(x => x.Id == i.ClassAttendanceId).First();
                att.Date = Convert.ToDateTime(classattendance.Date);
                if (i.Status == 1)
                {
                    att.Status = "Present";
                }
                else if (i.Status == 2)
                {
                    att.Status = "Absent";
                }
                else if (i.Status == 3)
                {
                    att.Status = "Leave";
                }
                else
                {
                    att.Status = "Late";
                }

                PassList.Add(att);
            }
            return View(PassList);
        }
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



        [HttpPost]
        [AllowAnonymous]
        public ActionResult Change(ParentViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var user = db.Parents.Where(x => x.Id == LoginClass.LoginId).First();
            user.UserName = collection.UserName;
            user.Password = collection.Password;
            db.SaveChanges();
            string message = "User Name & Password Updated!";
            return RedirectToAction("Account", "Parent", new { Message = message });
        }

        public ActionResult PLeaveRequests()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PLeaveRequests(LeaveViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            LeavesRequest request = new LeavesRequest();
            var Parent = db.Parents.Where(x => x.Id == LoginClass.LoginId).First();
            var Student = db.Students.Where(x => x.Id == Parent.StudentId).First();
            request.Reason = collection.Reason;
            request.Date = DateTime.Now;
            request.StudentId = Student.Id;

            db.LeavesRequests.Add(request);
            db.SaveChanges();

            return View();
        }

        public ActionResult ViewNews()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            List<NewsViewModel> PassList = new List<NewsViewModel>();
            var List = db.News.Where(x => x.Status == 1).ToList();
            foreach (var i in List)
            {
                NewsViewModel n = new NewsViewModel();
                n.Date = Convert.ToDateTime(i.Date);
                n.Description = i.Description;
                n.Title = i.Title;

                PassList.Add(n);

            }
            return View(PassList);
        }


        public ActionResult ViewEvents()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var List = db.Events.ToList();
            List<EventViewModel> PassList = new List<EventViewModel>();
            foreach (var i in List)
            {

                EventViewModel e = new EventViewModel();
                e.Description = i.Desciption;
                e.Id = i.Id;
                e.Date = Convert.ToDateTime(i.Date);
                PassList.Add(e);


            }

            return View(PassList);
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
