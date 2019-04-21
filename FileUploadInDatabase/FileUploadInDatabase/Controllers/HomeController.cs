using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileUploadInDatabase.Models;

namespace FileUploadInDatabase.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(FormCollection form, HttpPostedFileBase postedFile)
        {
            string fileName, fileExtension;
            if (postedFile != null)
            {
                // Get the image file path 
                fileName = Path.GetFileNameWithoutExtension(postedFile.FileName);
                fileExtension = Path.GetExtension(postedFile.FileName);
                // Here the filename is concatinated with DateTime to avoid duplication of filename
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                //String path = HttpContext.Server.MapPath("../Content/Image/");
                form["resume"] = "~/App_Data/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
                // To save image into image folder
                postedFile.SaveAs(fileName);
            }

            if (ModelState.IsValid)
            {
                using (FileUploadDemoEntities db = new FileUploadDemoEntities())
                {
                    Student model = new Student
                    {
                        name = form["name"],
                        email = form["email"],
                        resume = form["resume"]
                    };
                    db.Students.Add(model);
                    db.SaveChanges();
                    ViewBag.Message = "Data Added Successfully";
                }
            }

            var lst = GetAllStudents();
            return View();
        }

        public FileResult ShowFile(int Id)
        {
            FileUploadDemoEntities db = new FileUploadDemoEntities();
            Student cd = db.Students.FirstOrDefault(c => c.Id == Id);
            string pth = cd.resume.ToString();
            string filename = Path.GetFileName(pth);
            return File(pth, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        public ActionResult ShowStudentData()
        {
            var lst = GetAllStudents();
            return View(lst);
        }

        public List<Student> GetAllStudents()
        {
            FileUploadDemoEntities db = new FileUploadDemoEntities();
            List<Student> lst = db.Students.ToList();
            return lst;
        }
    }
}