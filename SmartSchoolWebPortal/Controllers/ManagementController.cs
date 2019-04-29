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

        [HttpPost]
        public ActionResult UpdateHostel(int id, HostelViewModel collection)
        {
            DBSmartSchoolWebPortalEntities111 db = new DBSmartSchoolWebPortalEntities111();
            var hostel = db.Hostels.Where(x => x.Id == id).First();
            hostel.Name = collection.HostelName;
            hostel.Details = collection.HostelDetails;
            hostel.Rent = collection.HostelRent;
            hostel.Location = collection.HostelLocation;

            db.SaveChanges();
            string message = "Hostel Updated!";
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
        public ActionResult UpdateStudent(int id, StudentViewModel model)
        {
            DBSmartSchoolWebPortalEntities111 ent = new DBSmartSchoolWebPortalEntities111();
            Student student = ent.Students.Where(x => x.Id == id).First();
            student.Name = model.Name;
            student.Contact = model.Contact;
            student.Email = model.Email;
            student.RegisterationNumber = model.RegisterationNumber;
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




    }
}
