using StudentSystem.Data;
using StudentSystem.Data.Repositories;
using StudentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private Func<IStudentSystemData> unitOfWorkFactory;
        private IGenericRepository<Student> studentsRepo;
        private IGenericRepository<Homework> homeworkRepo;

        public HomeController(
            Func<IStudentSystemData> unitOfWorkFactory,
            IGenericRepository<Student> studentRepo,
            IGenericRepository<Homework> homeworkRepo)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.studentsRepo = studentRepo;
            this.homeworkRepo = homeworkRepo;
        }

        public ActionResult Index()
        {
            var students = this.studentsRepo.All.Where(x=>this.homeworkRepo.All.FirstOrDefault().Student.FirstName == (x.FirstName));

            return View(students);
        }

        [HttpPost]
        public ActionResult Add(Student studentToAdd)
        {
            using (var unitOfWork = this.unitOfWorkFactory())
            {
                this.studentsRepo.Add(studentToAdd);
                unitOfWork.Commit();
            }

            return this.Redirect("Details/" + studentToAdd.Id);

        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var student = this.studentsRepo.GetById(id);

            return this.View(student);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return this.View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}