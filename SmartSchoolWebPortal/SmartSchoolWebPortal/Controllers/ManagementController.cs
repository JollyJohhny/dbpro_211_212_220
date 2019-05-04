using CrystalDecisions.CrystalReports.Engine;
using SmartSchoolWebPortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace SmartSchoolWebPortal.Controllers
{
    public class ManagementController : Controller
    {
        public static string MailUserName= "zoharasheed2345@gmail.com";
        public static string MailPassword = "JUNAIDTHECODER";

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

        public ActionResult Account(string message)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var admin = db.Managements.First();
            ViewBag.Name = admin.Name;
            ViewBag.Contact = admin.Contact;
            ViewBag.CNIC = admin.NIC;
            ViewBag.Email = admin.Email;

            ViewBag.Message = message;
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

        public ActionResult STudentRequests(string message)
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
            ViewBag.Message = message;

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

            //Sending mail to Student with his/her UserName and Password
            using (MailMessage mailMessage = new MailMessage(MailUserName, student.Email))
            {
                mailMessage.Subject = "Hi!";
                mailMessage.Body = "This mail is from Smart School Web Portal admin! Your User Name is " + collection.UserName + " and Password is " + collection.Password;

                mailMessage.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(MailUserName, MailPassword);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mailMessage);

                }
            }

            string message = "Email Sent to " + student.UserName + " !";
            return RedirectToAction("STudentRequests", "Management", new { Message = message });
        }

        public ActionResult PTudentRequests(string message)
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
            ViewBag.Message = message;

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

            //Sending mail to Parent with his/her UserName and Password
            using (MailMessage mailMessage = new MailMessage(MailUserName, parent.Email))
            {
                mailMessage.Subject = "Hi!";
                mailMessage.Body = "This mail is from Smart School Web Portal admin! Your User Name is " + collection.UserName + " and Password is " + collection.Password;

                mailMessage.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(MailUserName, MailPassword);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mailMessage);

                }
            }


           

            string message = "Email Sent to " + parent.UserName + " !";
            return RedirectToAction("PTudentRequests", "Management", new { Message = message });

        }

        public ActionResult LogOff()
        {
            LoginClass.LoginSession = null;
            LoginClass.LoginId = 0;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult MAddHostels()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MAddHostels(HostelViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            Hostel hostel = new Hostel();
            hostel.Name = collection.HostelName;
            hostel.Location = collection.HostelLocation;
            hostel.Details = collection.HostelDetails;
            hostel.Rent = collection.HostelRent;
            if (collection.Image != null)
            {
                string filename = Path.GetFileNameWithoutExtension(collection.Image.FileName);
                string ext = Path.GetExtension(collection.Image.FileName);
                filename = filename + DateTime.Now.Millisecond.ToString();
                filename = filename + ext;
                string filetodb = "/img/Hostels/" + filename;
                filename = Path.Combine(Server.MapPath("~/img/Hostels"), filename);
                collection.Image.SaveAs(filename);
                collection.ImagePath = filetodb;
            }
            else
            {
                collection.ImagePath = "/img/Hostels/MumtazHall.jpg";
            }
            hostel.ImagePath = collection.ImagePath;
            db.Hostels.Add(hostel);
            db.SaveChanges();
            return View();
        }


        public ActionResult MViewHostels()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var list = db.Hostels.ToList();
            List<HostelViewModel> PassList = new List<HostelViewModel>();
            foreach(var i in list)
            {
                HostelViewModel h = new HostelViewModel();
                h.HostelName = i.Name;
                h.HostelLocation = i.Location;
                h.ImagePath = i.ImagePath;
                h.Id = i.Id;
                h.HostelRent = Convert.ToInt32(i.Rent);
                h.HostelDetails = i.Details;
                PassList.Add(h);
            }
            return View(PassList);
        }


        public ActionResult UpdateHostel(int id)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var hostel = db.Hostels.Where(x => x.Id == id).First();
            HostelViewModel h = new HostelViewModel();
            h.HostelName = hostel.Name;
            h.HostelLocation = hostel.Location;
            h.HostelRent = Convert.ToInt32(hostel.Rent);
            h.HostelDetails = hostel.Details;

            return View(h);
        }

        public ActionResult RegisterCourse()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            List<string> list = new List<string>();
            var students = db.Students.Where(x => x.Status == 1).ToList();
            foreach (var i in students)
            {
                list.Add(i.RegisterationNumber);
            }
            list.Sort();
            ViewBag.list = new SelectList(list);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegisterCourse(RegisteredCourseViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            RegisteredCourse course = new RegisteredCourse();
            course.CourseId = collection.CourseId;
            var student = db.Students.Where(x => x.RegisterationNumber == collection.StudentId).First();
            course.StudentId = student.Id;
            course.RegisterationDate = DateTime.Now;

            db.RegisteredCourses.Add(course);
            db.SaveChanges();
            string message = "Course Registered!";
            return RedirectToAction("Account", "Management", new { Message = message });
        }

        public ActionResult ComplainNotice(int id)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var complain = db.Complaints.Where(x => x.Id == id).First();
            complain.Status = 1;

            db.SaveChanges();
            string message = "Complain Noticed!";
            return RedirectToAction("Account", "Management", new { Message = message });
        }


        public ActionResult ComplaintStatus()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var List = db.Complaints.ToList();
            List<ComplainViewModel> PassList = new List<ComplainViewModel>();
            foreach (var i in List)
            {
                ComplainViewModel c = new ComplainViewModel();
                c.Details = i.Details;
                c.Id = i.Id;
                c.Date = Convert.ToDateTime(i.Date);
                PassList.Add(c);
            }
            return View(PassList);
        }


        public ActionResult LeaveRequest()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var List = db.LeavesRequests.Where(x => x.Status != 1).ToList();
            List<LeaveViewModel> PassList = new List<LeaveViewModel>();
            foreach(var i in List)
            {
                LeaveViewModel l = new LeaveViewModel();
                l.Id = i.Id;
                l.Reason = i.Reason;
                l.Date =Convert.ToDateTime(i.Date);
                l.StudentId =Convert.ToInt32(i.StudentId);
                PassList.Add(l);
            }
            return View(PassList);
        }

        public ActionResult LeaveRequestAccept(int id)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            
            var Request = db.LeavesRequests.Where(x => x.Id == id).First();
            var Student = db.Students.Where(x => x.Id == Request.StudentId).First();
            Request.Status = 1;

            db.SaveChanges();

            
            //Sending mail to Student that his/her Leave Request is Accepted
            using (MailMessage mailMessage = new MailMessage(MailUserName, Student.Email))
            {
                mailMessage.Subject = "Hi!";
                mailMessage.Body = "This mail is from Smart School Web Portal admin! Your Leave Request for Date " + Request.Date + " is acceptted!";

                mailMessage.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(MailUserName, MailPassword);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mailMessage);

                }
            }

            string message = "Email Sent to " + Student.UserName + " !";
            return RedirectToAction("Account", "Management", new { Message = message });


        }

        [HttpPost]
        public ActionResult UpdateHostel(int id, HostelViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var hostel = db.Hostels.Where(x => x.Id == id).First();
            if(collection.HostelName == null)
            {
                collection.HostelName = hostel.Name;
            }
            if(collection.HostelRent == 0)
            {
                collection.HostelRent =Convert.ToInt16(hostel.Rent);
            }
            if(collection.HostelLocation == null)
            {
                collection.HostelLocation = hostel.Location;
            }
            hostel.Name = collection.HostelName;
            hostel.Details = collection.HostelDetails;
            hostel.Rent = collection.HostelRent;
            hostel.Location = collection.HostelLocation;

            db.SaveChanges();
            string message = "Hostel Updated!";
            return RedirectToAction("Account", "Management", new { Message = message });
        }


        public ActionResult AddFee()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            List<string> list = new List<string>();
            var students = db.Students.Where(x => x.Status == 1).ToList();
            foreach (var i in students)
            {
                list.Add(i.RegisterationNumber);
            }
            list.Sort();
            ViewBag.list = new SelectList(list);
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddFee(StudentFeeViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var student = db.Students.Where(x => x.RegisterationNumber == collection.StudentId).First();
            StudentFee fee = new StudentFee();
            fee.StudentId = student.Id;
            fee.Amount = collection.Amount;
            fee.Date = collection.Date;
            fee.Status = "UnPaid";

            db.StudentFees.Add(fee);
            db.SaveChanges();

            string message = "Student fee Added!";
            return RedirectToAction("Account", "Management", new { Message = message });
        }


        public ActionResult ViewFees(int id)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var student = db.Students.Where(x => x.Id == id).First();
            var fees = db.StudentFees.Where(x => x.StudentId == id).ToList();
            List<StudentFeeViewModel> PassList = new List<StudentFeeViewModel>();
            foreach(var i in fees)
            {
                StudentFeeViewModel sf = new StudentFeeViewModel();
                sf.Amount =Convert.ToInt32(i.Amount);
                sf.Date =Convert.ToDateTime(i.Date);
                sf.Status = i.Status;
                sf.Id = i.Id;
                PassList.Add(sf);
            }
            ViewBag.Name = student.Name;
            return View(PassList);
        }

        public ActionResult Enrollment()
        {
            string message = "This section is under Development and not required!";
            return RedirectToAction("Account", "Management", new { Message = message });
        }

        public ActionResult PayFees(int id)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var sf = db.StudentFees.Where(x => x.Id == id).First();
            sf.Status = "Paid";
            db.SaveChanges();
            string message = "Student fee Paid!";
            return RedirectToAction("Account", "Management", new { Message = message });
        }

        public ActionResult DeleteFee(int id)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var fee = db.StudentFees.Where(x => x.Id == id).First();
            db.Entry(fee).State = System.Data.Entity.EntityState.Deleted;

            string message = "Student fee Deleted!";
            return RedirectToAction("Account", "Management", new { Message = message });
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteHostel(int id)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var hostel = db.Hostels.Where(x => x.Id == id).First();
            db.Entry(hostel).State = System.Data.Entity.EntityState.Deleted;
            
            db.SaveChanges();
            string message = "Hostel is Deleted";
            return RedirectToAction("Account", "Management", new { Message = message });
        }

        public ActionResult ViewAllStudent()
        {
            DBSmartSchoolWebPortalEntities111 ent = new DBSmartSchoolWebPortalEntities111();
            var List = ent.Students.ToList();

            List<StudentViewModel> PassList = new List<StudentViewModel>();
            foreach (var i in List)
            {
                StudentViewModel student = new StudentViewModel();
                student.Name = i.Name;
                student.Id = i.Id;
                student.Contact = i.Contact;
                student.RegisterationNumber = i.RegisterationNumber;
                student.Email = i.Email;
                student.UserName = i.UserName;
                student.Password = i.Password;
                PassList.Add(student);
            }

            return View(PassList);
        }

        public ActionResult UpdateStudent(int id)
        {
            DBSmartSchoolWebPortalEntities111 ent = new DBSmartSchoolWebPortalEntities111();
            var student = ent.Students.Where(x => x.Id == id).First();
            StudentViewModel s = new StudentViewModel();
            s.Name = student.Name;
            s.Contact = student.Contact;
            s.Email = student.Email;
            s.RegisterationNumber = student.RegisterationNumber;
            s.UserName = student.UserName;
            s.Password = student.Password;
            return View(s);
        }

        [HttpPost]
        public ActionResult UpdateStudent(int id, StudentViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 ent = new DBSmartSchoolWebPortalEntities111();
            Student student = ent.Students.Where(x => x.Id == id).First();
            if (collection.Name == null)
            {
                collection.Name = student.Name;
            }
            if (collection.Contact == null)
            {
                collection.Contact = student.Contact;
            }
            if (collection.Email == null)
            {
                collection.Email = student.Email;
            }
            if (collection.UserName == null)
            {
                collection.UserName = student.UserName;
            }
            if (collection.Password == null)
            {
                collection.Password = student.Password;
            }
            if (collection.RegisterationNumber == null)
            {
                collection.RegisterationNumber = student.RegisterationNumber;
            }
            student.Name = collection.Name;
            student.Contact = collection.Contact;
            student.Email = collection.Email;
            student.RegisterationNumber = collection.RegisterationNumber;
            student.UserName = student.UserName;
            student.Password = student.Password;

            ent.SaveChanges();

            return RedirectToAction("ViewAllStudent");
        }

        public ActionResult DeleteStudent(int id)
        {
            DBSmartSchoolWebPortalEntities111 ent = new DBSmartSchoolWebPortalEntities111();
            var student = ent.Students.Where(x => x.Id == id).First();
            ent.Entry(student).State = System.Data.Entity.EntityState.Deleted;
            ent.SaveChanges();
            string message = "Student is Deleted";
            return RedirectToAction("Account", "Management", new { Message = message });
            
        }


        public ActionResult HostelRequests()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var requests = db.HostelRequests.Where(x=>x.Status !=1).ToList();
            List<HostelRequestViewModel> list = new List<HostelRequestViewModel>();
            foreach(var i in requests)
            {
                HostelRequestViewModel req = new HostelRequestViewModel();
                var Student = db.Students.Where(x => x.Id == i.StudentId).First();
                var Hostel = db.Hostels.Where(x => x.Id == i.HostelId).First();
                req.HostelName = Hostel.Name;
                req.StudentName = Student.Name;
                req.StudentRegNo = Student.RegisterationNumber;
                req.Id = i.Id;
                list.Add(req);
            }
            return View(list);

        }

        public ActionResult AcceptRequestHostel(int id)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var request = db.HostelRequests.Where(x => x.Id == id).First();
            RegisteredHostel hostelreg = new RegisteredHostel();
            hostelreg.StudentId = request.StudentId;
            hostelreg.HostelId = request.HostelId;
            hostelreg.RegisterationDate = DateTime.Now;
            request.Status = 1;
            var student = db.Students.Where(x => x.Id == request.StudentId).First();
            db.RegisteredHostels.Add(hostelreg);
            db.SaveChanges();

            //Sending mail to Student that his/her Hostel Request is accepted!
            using (MailMessage mailMessage = new MailMessage(MailUserName, student.Email))
            {
                mailMessage.Subject = "Hi!";
                mailMessage.Body = "This mail is from Smart School Web Portal admin! Your Hostel Request is Accepted!";

                mailMessage.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(MailUserName, MailPassword);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mailMessage);

                }
            }

            string message = "Hostel Request Accepted!";
            return RedirectToAction("Account", "Management", new { Message = message });


        }

        public ActionResult ViewNews()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            List<NewsViewModel> PassList = new List<NewsViewModel>();
            var List = db.News.ToList();
            foreach(var i in List)
            {
                NewsViewModel n = new NewsViewModel();
                n.Date =Convert.ToDateTime(i.Date);
                n.Description = i.Description;
                n.Title = i.Title;
                n.Id = i.Id;
                if(i.Status != 1)
                {
                    n.Status = "UnPublished";
                }
                else
                {
                    n.Status = "Published";
                }
                PassList.Add(n);

            }
            return View(PassList);
        }

        public ActionResult AddNews()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddNews(NewsViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            News news = new News();
            news.Date = DateTime.Now;
            news.Description = collection.Description;
            news.Title = collection.Title;
            

            db.News.Add(news);
            db.SaveChanges();
            return RedirectToAction("ViewNews");
        }


        public ActionResult AddEvents()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddEvents(EventViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            Event eve = new Event();
            eve.Desciption = collection.Description;
            eve.Date = collection.Date;
          


            db.Events.Add(eve);
            db.SaveChanges();
            return RedirectToAction("ViewEvents");
        }

        public ActionResult ViewEvents()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var List = db.Events.ToList();
            List<EventViewModel> PassList = new List<EventViewModel>();
            foreach(var i in List)
            {
                EventViewModel e = new EventViewModel();
                e.Description = i.Desciption;
                e.Id = i.Id;
                e.Date =Convert.ToDateTime(i.Date);
                PassList.Add(e);
            }

            return View(PassList);
        }

        public ActionResult DeleteEvent(int id)
        {
            DBSmartSchoolWebPortalEntities111 ent = new DBSmartSchoolWebPortalEntities111();
            var eve = ent.Events.Where(x => x.Id == id).First();
            ent.Entry(eve).State = System.Data.Entity.EntityState.Deleted;
            ent.SaveChanges();

            string message = "Event Deleted!";
            return RedirectToAction("Account", "Management", new { Message = message });
        }

        public ActionResult DeleteNews(int id)
        {
            DBSmartSchoolWebPortalEntities111 ent = new DBSmartSchoolWebPortalEntities111();
            var news = ent.News.Where(x => x.Id == id).First();
            ent.Entry(news).State = System.Data.Entity.EntityState.Deleted;
            ent.SaveChanges();

            string message = "News Deleted!";
            return RedirectToAction("Account", "Management", new { Message = message });
        }

        public ActionResult PublishNews(int id)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var news = db.News.Where(x => x.Id == id).First();
            news.Status = 1;
            db.SaveChanges();

            string message = "News Published!";
            return RedirectToAction("Account", "Management", new { Message = message });

        }

        public ActionResult UnPublishNews(int id)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var news = db.News.Where(x => x.Id == id).First();
            news.Status = 2;
            db.SaveChanges();

            string message = "News UnPublished!";
            return RedirectToAction("Account", "Management", new { Message = message });

        }


        public ActionResult ViewAllParent()
        {
            DBSmartSchoolWebPortalEntities111 ent = new DBSmartSchoolWebPortalEntities111();
            var List = ent.Parents.ToList();

            List<ParentViewModel> PassList = new List<ParentViewModel>();
            foreach (var i in List)
            {
                ParentViewModel parent = new ParentViewModel();
                parent.Name = i.Name;
                parent.Contact = i.Contact;
                parent.Id = i.Id;
                parent.Email = i.Email;
                parent.NIC = i.NIC;
                parent.UserName = i.UserName;
                parent.Password = i.Password;

                PassList.Add(parent);
            }

            return View(PassList);
        }

        public ActionResult UpdateParent(int id)
        {
            DBSmartSchoolWebPortalEntities111 ent = new DBSmartSchoolWebPortalEntities111();
            var parent = ent.Parents.Where(x => x.Id == id).First();
            ParentViewModel p = new ParentViewModel();
            p.Name = parent.Name;
            p.Contact = parent.Contact;
            p.Email = parent.Email;
            p.UserName = parent.UserName;
            p.Password = parent.Password;
            p.NIC = parent.NIC;
            return View(p);
        }

        [HttpPost]
        public ActionResult UpdateParent(int id, ParentViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 ent = new DBSmartSchoolWebPortalEntities111();
            var parent = ent.Parents.Where(x => x.Id == id).First();
            if(collection.Name == null)
            {
                collection.Name = parent.Name;
            }
            if(collection.Contact == null)
            {
                collection.Contact = parent.Contact;
            }
            if(collection.Email == null)
            {
                collection.Email = parent.Email;
            }
            
            if(collection.NIC == null)
            {
                collection.NIC = parent.NIC;
            }
            parent.Name = collection.Name;
            parent.Contact = collection.Contact;
            parent.Email = collection.Email;
            parent.UserName = parent.UserName;
            parent.Password = parent.Password;
            parent.NIC = collection.NIC;

            ent.SaveChanges();

            return RedirectToAction("AllParent");
        }



        public ActionResult DeleteParent(int id)
        {
            DBSmartSchoolWebPortalEntities111 ent = new DBSmartSchoolWebPortalEntities111();
            var parent = ent.Parents.Where(x => x.Id == id).First();
            ent.Entry(parent).State = System.Data.Entity.EntityState.Deleted;
            ent.SaveChanges();
            string message = "Parent is Deleted";
            return RedirectToAction("Account", "Management", new { Message = message });

        }



        // Reports Functions

        public ActionResult GenerateReportAllStudents()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var List = db.Students.Where(x => x.Status == 1).ToList();

            List<StudentViewModel> PassList = new List<StudentViewModel>();
            foreach (var i in List)
            {
                StudentViewModel student = new StudentViewModel();
                student.Name = i.Name;
                student.Id = i.Id;
                student.Contact = i.Contact;
                student.RegisterationNumber = i.RegisterationNumber;
                student.Email = i.Email;
                student.UserName = i.UserName;
                student.Password = i.Password;
                PassList.Add(student);
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReportAllStudents.rpt"));
            rd.SetDataSource(PassList);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "RegisteredStudentsList.pdf");

            }
            catch
            {
                throw;
            }
        }

        public ActionResult GenerateReportAllParents()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var List = db.Parents.Where(x => x.Status == 1).ToList();

            List<ParentViewModel> PassList = new List<ParentViewModel>();
            foreach (var i in List)
            {
                ParentViewModel parent = new ParentViewModel();
                parent.Name = i.Name;
                parent.Contact = i.Contact;
                parent.Id = i.Id;
                parent.Email = i.Email;
                parent.NIC = i.NIC;
                parent.UserName = i.UserName;
                parent.Password = i.Password;

                PassList.Add(parent);
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReportAllParents.rpt"));
            rd.SetDataSource(PassList);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "RegisteredParentsList.pdf");

            }
            catch
            {
                throw;
            }
        }


        public ActionResult GenerateReportHostels()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var list = db.Hostels.ToList();
            List<HostelViewModel> PassList = new List<HostelViewModel>();
            foreach (var i in list)
            {
                HostelViewModel h = new HostelViewModel();
                h.HostelName = i.Name;
                h.HostelLocation = i.Location;
                h.Id = i.Id;
                h.HostelRent = Convert.ToInt32(i.Rent);
                h.HostelDetails = i.Details;
                PassList.Add(h);
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReportAllHostels.rpt"));
            rd.SetDataSource(PassList);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "HostelsList.pdf");

            }
            catch
            {
                throw;
            }
        }

        public ActionResult GenerateReportNews()
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

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReportNews.rpt"));
            rd.SetDataSource(PassList);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "NewsList.pdf");

            }
            catch
            {
                throw;
            }
        }

        public ActionResult GenerateReportEvents()
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

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReportEvents.rpt"));
            rd.SetDataSource(PassList);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "EventsList.pdf");

            }
            catch
            {
                throw;
            }
        }

        public ActionResult GenerateReportComplains()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var List = db.Complaints.ToList();
            List<ComplainViewModel> PassList = new List<ComplainViewModel>();
            foreach (var i in List)
            {
                ComplainViewModel c = new ComplainViewModel();
                c.Details = i.Details;
                c.Id = i.Id;
                c.Date = Convert.ToDateTime(i.Date);
                PassList.Add(c);
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReportComplains.rpt"));
            rd.SetDataSource(PassList);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "ComplaintsList.pdf");

            }
            catch
            {
                throw;
            }
        }

        public ActionResult GenerateReportFees(int id)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var student = db.Students.Where(x => x.Id == id).First();
            var fees = db.StudentFees.Where(x => x.StudentId == id).ToList();
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

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReportFees.rpt"));
            rd.SetDataSource(PassList);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "FeesList.pdf");

            }
            catch
            {
                throw;
            }
        }

        public ActionResult GenerateReportAtt()
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var list = db.StudentAttendances.ToList();
            List<StudentAttendanceViewModel> LIST = new List<StudentAttendanceViewModel>();
            foreach (var i in list)
            {
                StudentAttendanceViewModel s = new StudentAttendanceViewModel();
                var att = db.ClassAttendances.Where(x => x.Id == i.ClassAttendanceId).First();

                s.StudentId = Convert.ToInt32(i.StudentId);
                if (i.Status == 1)
                {
                    s.Status = "Present";
                }
                else if (i.Status == 2)
                {
                    s.Status = "Absent";
                }
                else if (i.Status == 3)
                {
                    s.Status = "Leave";
                }
                else
                {
                    s.Status = "Late";
                }
                s.Date =Convert.ToDateTime(att.Date);
                LIST.Add(s);

            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReportAtt.rpt"));
            rd.SetDataSource(LIST);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "AttendanceList.pdf");

            }
            catch
            {
                throw;
            }
        }

    }
}
