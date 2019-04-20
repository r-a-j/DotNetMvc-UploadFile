using System;
using System.Collections.Generic;
using System.IO;
//using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileUploadInDatabase.Models;

namespace FileUploadInDatabase.Controllers
{
    public class HomeController : Controller
    {
        FileUploadDemoEntities db = new FileUploadDemoEntities();

        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Index(FormCollection form, HttpPostedFileBase postedFile)
        {



            string path, fileName, fileExtension;

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
                // Pass model into database
                
                using (db)
                {
                    Student model = new Student
                    {
                        name = form["name"].ToString(),
                        email = form["email"].ToString(),
                        resume = form["resume"].ToString()
                    };
                    db.Students.Add(model);
                    db.SaveChanges();
                    ViewBag.Message = "Data Added Successfully";
                }
            }

            // Code for displaying the files on new page.
            List<Student> lst = db.Students.ToList();
            ViewBag.List = lst;

            return View();
        }
    }
}