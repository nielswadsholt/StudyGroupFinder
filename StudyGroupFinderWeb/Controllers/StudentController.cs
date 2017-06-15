using Graph;
using StudyGroupFinder;
using StudyGroupFinderWeb.Models;
using StudyGroupFinderWeb.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyGroupFinderWeb.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View(StudentGraph.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(StudentViewModel studentViewModel)
        {
            if (StudentGraph.Students.Contains(studentViewModel.ToStudent().Name))
            {
                ModelState.AddModelError("Name", "Det angivne navn findes allerede.");
                return View();
            }
            
            StudentGraph.Add(studentViewModel.ToStudentNode());
            return RedirectToAction("Index");
        }

        public ActionResult Details(string name)
        {
            return View(StudentGraph.Get(name));
        }

        public ActionResult Find(string name)
        {
            Predicate<Student> p1 = s => StudentGraph.Students[name].Data.Study == s.Study;
            Predicate<Student> p2 = s => StudentGraph.Students[name].Data.Study == s.Study && s.SeeksGroup;
            Predicate<Student> p3 = (s => 
                s.SeeksGroup && 
                StudentGraph.Students[name].Data.Study == s.Study &&
                (StudentGraph.Students[name].Data.StudyAttributes.Intersect(s.StudyAttributes)).Count() > 0);
            Predicate<Student> p4 = s => (StudentGraph.Students[name].Data.Attributes.Intersect(s.Attributes)).Count() > 0;
            Predicate<Student> p5 = s => (StudentGraph.Students[name].Data.Attributes.Intersect(s.Attributes)).Count() > 2;
            Predicate<Student> p6 = s => s.Attributes.Contains("A");
            // TODO: Add regex predicate

            List<Path> paths = new List<Path>();

            foreach (Predicate<Student> p in new []{ p1, p2, p3, p4, p5, p6 })
            {
                paths.Add(StudentGraph.Students.FindPath(name, p));
            }

            return View(paths);
        }
    }
}