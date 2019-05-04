using CrystalDecisions.CrystalReports.Engine;
using SmartSchoolWebPortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
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


        public ActionResult SLeaveView()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var parent = db.Parents.Where(x => x.Id == LoginClass.LoginId).First();
            var student = db.Students.Where(x => x.Id == parent.StudentId).First();
            var List = db.LeavesRequests.Where(x => x.StudentId == student.Id).ToList();
            List<LeaveViewModel> PassList = new List<LeaveViewModel>();
            foreach (var i in List)
            {
                LeaveViewModel l = new LeaveViewModel();
                l.Id = i.Id;
                l.Reason = i.Reason;
                l.Date = Convert.ToDateTime(i.Date);
                if (i.Status == 1)
                {
                    l.Status = "Approved";
                }
                else
                {
                    l.Status = "Pending";
                }
                PassList.Add(l);
            }
            return View(PassList);
        }

        public ActionResult RequestHostel(int id)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var parent = db.Parents.Where(x => x.Id == LoginClass.LoginId).First();
            var student = db.Students.Where(x => x.Id == parent.StudentId).First();
            HostelRequest request = new HostelRequest();
            request.HostelId = id;
            request.StudentId = student.Id;
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

        public ActionResult SViewTimetable()
        {
            return View();
        }


        public ActionResult ComplaintStatus()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var List = db.Complaints.Where(x => x.ParentId == LoginClass.LoginId).ToList();
            List<ComplainViewModel> PassList = new List<ComplainViewModel>();
            foreach (var i in List)
            {
                ComplainViewModel c = new ComplainViewModel();
                c.Details = i.Details;
                if (i.Status != 1)
                {
                    c.Status = "UnNoticed";
                }
                else
                {
                    c.Status = "Noticed!";
                }
                c.Date = Convert.ToDateTime(i.Date);
                PassList.Add(c);
            }
            return View(PassList);
        }


        public ActionResult AddComplaint()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddComplaint(ComplainViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            Complaint complaint = new Complaint();
            complaint.ParentId = LoginClass.LoginId;
            complaint.Details = collection.Details;

            db.Complaints.Add(complaint);
            db.SaveChanges();

            return RedirectToAction("Account");
        }


        public ActionResult ViewRegCourses()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var parent = db.Parents.Where(x => x.Id == LoginClass.LoginId).First();
            var List = db.RegisteredCourses.Where(x => x.StudentId == parent.StudentId).ToList();
            List<RegisteredCourseViewModel> PassList = new List<RegisteredCourseViewModel>();
            foreach (var i in List)
            {
                RegisteredCourseViewModel r = new RegisteredCourseViewModel();
                var c = db.Courses.Where(x => x.Id == i.CourseId).First();
                r.Name = c.Title;
                r.Date = Convert.ToDateTime(i.RegisterationDate);
                PassList.Add(r);
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

        public ActionResult SAnnualStatus()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var parent = db.Parents.Where(x => x.Id == LoginClass.LoginId).First();
            var fees = db.StudentFees.Where(x => x.StudentId == parent.StudentId).ToList();
            List<StudentFeeViewModel> PassList = new List<StudentFeeViewModel>();
            foreach (var i in fees)
            {
                StudentFeeViewModel sf = new StudentFeeViewModel();
                sf.Amount = Convert.ToInt32(i.Amount);
                sf.Date = Convert.ToDateTime(i.Date);
                sf.Status = i.Status;
                sf.Id = i.Id;
                PassList.Add(sf);
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

        //Reports

        public ActionResult GenerateReportCourses()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var List = db.RegisteredCourses.Where(x => x.StudentId == LoginClass.LoginId).ToList();
            List<RegisteredCourseViewModel> PassList = new List<RegisteredCourseViewModel>();
            foreach (var i in List)
            {
                RegisteredCourseViewModel r = new RegisteredCourseViewModel();
                var c = db.Courses.Where(x => x.Id == i.CourseId).First();
                r.Name = c.Title;
                r.Date = Convert.ToDateTime(i.RegisterationDate);
                PassList.Add(r);
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReportCourses.rpt"));
            rd.SetDataSource(PassList);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "RegisteredCoursesList.pdf");

            }
            catch
            {
                throw;
            }
        }

        public ActionResult GenerateReportAttendance()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();

            var AttendanceList = db.StudentAttendances.Where(x => x.StudentId == LoginClass.LoginId).ToList();
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

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReportAttendance.rpt"));
            rd.SetDataSource(PassList);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "RegisteredCoursesList.pdf");

            }
            catch
            {
                throw;
            }
        }

        public ActionResult GenerateReportLeaves()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();

            var parent = db.Parents.Where(x => x.Id == LoginClass.LoginId).First();
            var student = db.Students.Where(x => x.Id == parent.StudentId).First();
            var List = db.LeavesRequests.Where(x => x.StudentId == student.Id).ToList();
            List<LeaveViewModel> PassList = new List<LeaveViewModel>();
            foreach (var i in List)
            {
                LeaveViewModel l = new LeaveViewModel();
                l.Id = i.Id;
                l.Reason = i.Reason;
                l.Date = Convert.ToDateTime(i.Date);
                if (i.Status == 1)
                {
                    l.Status = "Approved";
                }
                else
                {
                    l.Status = "Pending";
                }
                PassList.Add(l);
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReportLeaves.rpt"));
            rd.SetDataSource(PassList);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "LeaveList.pdf");

            }
            catch
            {
                throw;
            }
        }
    }
}
