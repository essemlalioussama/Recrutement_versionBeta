using Microsoft.AspNet.Identity;
using Recrutement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var list = db.Categories.ToList();
            return View(list);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Details(int JobId)
        {
            var job = db.Jobs.Find(JobId);
            if (job == null)
            {
                return HttpNotFound();
            }
            Session["JobId"] = JobId;
            return View(job);
        }

        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Apply(string Message, HttpPostedFileBase upload)
        {
            var UserId = User.Identity.GetUserId();
            var JobId = (int)Session["JobId"];

            var check = db.ApplyForJobs.Where(a => a.JobId == JobId && a.UserId == UserId).ToList();

            if(check.Count<1)
            {
                var job = new ApplyForJob();

                string path = Path.Combine(Server.MapPath("~/Uploads/CV"), upload.FileName);
                upload.SaveAs(path);
                job.CV= upload.FileName;
                job.UserId = UserId;
                job.JobId = JobId;
                job.Message = Message;
                job.Date = DateTime.Now;

                db.ApplyForJobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            else
            {
                ViewBag.Result = "Vous avez déja postulé à cet emploi";
            }

            

            return View();
        }

        [Authorize]
        public ActionResult GetJobsByUser()
        {
            var UserId = User .Identity.GetUserId();
            var Jobs = db.ApplyForJobs.Where(a => a.UserId == UserId).ToList();

            return View(Jobs);
        }

        [Authorize]
        public ActionResult DetailOfJobs(int id)
        {
            var jobs = db.ApplyForJobs.Find(id);
            if (jobs == null)
            {
                return HttpNotFound();
            }
            
            return View(jobs);
        }

        [Authorize]
        public ActionResult GetOffreByCompany()
        {
            var UserId = User.Identity.GetUserId();

            var Jobs = from post in db.ApplyForJobs
                       join job in db.Jobs
                       on post.JobId equals job.Id
                       where job.User.Id == UserId
                       select post;

            var groupe = from job in Jobs
                         group job by job.job.JobTitle
                         into gr
                         select new JobsViewModel
                         {
                             JobTitle = gr.Key,
                             Elements = gr
                         };


            return View(groupe.ToList());

        }


        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string searchName)
        {
            var result = db.Jobs.Where(a => a.JobTitle.Contains(searchName) 
            || a.JobContent.Contains(searchName)
            || a.Category.CategoryName.Contains(searchName)
            || a.Category.CategoryDescription.Contains(searchName));
            return View(result.ToList());
        }




        // GET
        public ActionResult Edit(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST
        [HttpPost]
        public ActionResult Edit(ApplyForJob job)
        {
            if (ModelState.IsValid)
            {
                job.Date = DateTime.Now;
                db.Entry(job).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetJobsByUser");
            }
            return View(job);
        }

        public ActionResult Delete(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(ApplyForJob job)
        {

                // TODO: Add delete logic here

                db.ApplyForJobs.Remove(db.ApplyForJobs.Find(job.Id));
                db.SaveChanges();
                return RedirectToAction("GetJobsByUser");

        }

        public ActionResult Contact()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            var mail = new MailMessage();
            var logInfo = new NetworkCredential("oussiesso@gmail.com", "password");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("oussiesso@gmail.com"));
            mail.Subject = contact.Objet;
            mail.IsBodyHtml = true;
            mail.Body ="Nom de l'expéditeur :"+contact.Name+"<br>"+
                "Email de l'expéditeur :" + contact.Email + "<br>"+
                "Objet de l'e-mail :" + contact.Objet + "<br>"+
                "corps de l'e-mail : <strong>" + contact.Message + "<strong><br>";
            var smtpClient = new SmtpClient("smtp.gmail.com",587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = logInfo;
            smtpClient.Send(mail);
            return RedirectToAction("index", "Home");
        }
    }
}