using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
[assembly: InternalsVisibleTo( "LMSControllerTests" )]
namespace LMS_CustomIdentity.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : Controller
    {

        private readonly LMSContext db;

        public ProfessorController(LMSContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Students(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Class(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Categories(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            return View();
        }

        public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            ViewData["uid"] = uid;
            return View();
        }

        /*******Begin code to modify********/


        /// <summary>
        /// Returns a JSON array of all the students in a class.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "dob" - date of birth
        /// "grade" - the student's grade in this class
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
        {
            
            // var students = from c in db.Classes 
            //     join co in db.Courses on c.Course equals co.CourseId
            //     where co.Departments == subject && co.Number == num
            //     && c.Season == season && c.Year == year
            //     from e in c.Enrolled
            //     select new
            //     {
            //         fname = e.Student.FName,
            //         lname = e.Student.LName,
            //         uid = e.Student.UId,
            //         dob = e.Student.Dob,
            //         grade = e.Grade
            //     };
            
            var students = from e in db.Enrolleds
                join c in db.Classes on e.Class equals c.ClassId
                join co in db.Courses on c.Course equals co.CourseId
                join s in db.Students on e.Student equals s.UserId
                where co.Departments == subject
                      && co.Number == num
                      && c.Season == season
                      && c.Year == year
                select new
                {
                    fname = s.FirstName,
                    lname = s.LastName,
                    uid = s.UserId,
                    dob = s.DateOfBirth,
                    grade = e.Grade
                };

            return Json(students.ToArray());
        }



        /// <summary>
        /// Returns a JSON array with all the assignments in an assignment category for a class.
        /// If the "category" parameter is null, return all assignments in the class.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The assignment category name.
        /// "due" - The due DateTime
        /// "submissions" - The number of submissions to the assignment
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class, 
        /// or null to return assignments from all categories</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
        {
            
            var query = from a in db.Assignments
                join ac in db.AssignmentCategories on a.Category equals ac.CategoryId
                join c in db.Classes on ac.Class equals c.ClassId
                join co in db.Courses on c.Course equals co.CourseId
                where co.Departments == subject
                      && co.Number == num
                      && c.Season == season
                      && c.Year == year
                      && (category == null || ac.Name == category)
                select new
                {
                    aname = a.Name,
                    cname = ac.Name,
                    due = a.DueDate,
                    submissions = db.Submissions.Count(s => s.Assignment == a.AssignmentId)
                };

            return Json(query.ToArray());
            
            return Json(null);
        }


        /// <summary>
        /// Returns a JSON array of the assignment categories for a certain class.
        /// Each object in the array should have the folling fields:
        /// "name" - The category name
        /// "weight" - The category weight
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
        {
            var categories = from ac in db.AssignmentCategories
                join c in db.Classes on ac.Class equals c.ClassId
                join co in db.Courses on c.Course equals co.CourseId
                where co.Departments == subject
                      && co.Number == num
                      && c.Season == season
                      && c.Year == year
                select new
                {
                    name = ac.Name,
                    weight = ac.Weight
                };

            return Json(categories.ToArray());
        }

        /// <summary>
        /// Creates a new assignment category for the specified class.
        /// If a category of the given class with the given name already exists, return success = false.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The new category name</param>
        /// <param name="catweight">The new category weight</param>
        /// <returns>A JSON object containing {success = true/false} </returns>
        public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
        {
            var classObj = (from c in db.Classes
                join co in db.Courses on c.Course equals co.CourseId
                where co.Departments == subject
                      && co.Number == num
                      && c.Season == season
                      && c.Year == year
                select c).FirstOrDefault();

            if (classObj == null) return Json(new { success = false });

            bool exists = db.AssignmentCategories
                .Any(ac => ac.Class == classObj.ClassId && ac.Name == category);

            if (exists) return Json(new { success = false });

            db.AssignmentCategories.Add(new AssignmentCategory
            {
                Class = classObj.ClassId,
                Name = category,
                Weight = catweight
            });

            db.SaveChanges();
            return Json(new { success = true });
            
            return Json(new { success = false });
        }

        /// <summary>
        /// Creates a new assignment for the given class and category.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="asgpoints">The max point value for the new assignment</param>
        /// <param name="asgdue">The due DateTime for the new assignment</param>
        /// <param name="asgcontents">The contents of the new assignment</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
        {
            
            var categoryObj = (from ac in db.AssignmentCategories
                join c in db.Classes on ac.Class equals c.ClassId
                join co in db.Courses on c.Course equals co.CourseId
                where co.Departments == subject
                      && co.Number == num
                      && c.Season == season
                      && c.Year == year
                      && ac.Name == category
                select ac).FirstOrDefault();

            if (categoryObj == null) return Json(new { success = false });

            bool exists = db.Assignments
                .Any(a => a.Category == categoryObj.CategoryId && a.Name == asgname);

            if (exists) return Json(new { success = false });

            db.Assignments.Add(new Assignment
            {
                Category = categoryObj.CategoryId,
                Name = asgname,
                Instructions = asgcontents,
                DueDate = asgdue,
                MaxValue = asgpoints
            });

            db.SaveChanges();

            var classObj = (from c in db.Classes
                join co in db.Courses on c.Course equals co.CourseId
                where co.Departments == subject
                      && co.Number == num
                      && c.Season == season
                      && c.Year == year
                select c).First();
            
            var classId = classObj.ClassId;

            db.SaveChanges();
            
            var students = (from e in db.Enrolleds
                join c in db.Classes on e.Class equals c.ClassId
                    where c.ClassId == classId
                select e.Student).ToList();
            
            foreach (var student in students)
            {
                UpdateStudentGrade(student, categoryObj.Class);
            }
            
            return Json(new { success = true });
        }


        /// <summary>
        /// Gets a JSON array of all the submissions to a certain assignment.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "time" - DateTime of the submission
        /// "score" - The score given to the submission
        /// 
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
        {
            var submissions = from s in db.Submissions
                join a in db.Assignments on s.Assignment equals a.AssignmentId
                join ac in db.AssignmentCategories on a.Category equals ac.CategoryId
                join c in db.Classes on ac.Class equals c.ClassId
                join co in db.Courses on c.Course equals co.CourseId
                join stu in db.Students on s.Student equals stu.UserId
                where co.Departments == subject
                      && co.Number == num
                      && c.Season == season
                      && c.Year == year
                      && ac.Name == category
                      && a.Name == asgname
                select new
                {
                    fname = stu.FirstName,
                    lname = stu.LastName,
                    uid = stu.UserId,
                    time = s.Time,
                    score = s.Score
                };

            return Json(submissions.ToArray());
            
            return Json(null);
        }


        /// <summary>
        /// Set the score of an assignment submission
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <param name="uid">The uid of the student who's submission is being graded</param>
        /// <param name="score">The new score for the submission</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
        {
            
            var submission = (from s in db.Submissions
                join a in db.Assignments on s.Assignment equals a.AssignmentId
                join ac in db.AssignmentCategories on a.Category equals ac.CategoryId
                join c in db.Classes on ac.Class equals c.ClassId
                join co in db.Courses on c.Course equals co.CourseId
                where co.Departments == subject
                      && co.Number == num
                      && c.Season == season
                      && c.Year == year
                      && ac.Name == category
                      && a.Name == asgname
                      && s.Student == uid
                select s).FirstOrDefault();

            if (submission == null) return Json(new { success = false });

            submission.Score = score;
            db.SaveChanges();
            
            var classObj = (from c in db.Classes
                join co in db.Courses on c.Course equals co.CourseId
                where co.Departments == subject
                      && co.Number == num
                      && c.Season == season
                      && c.Year == year
                select c).First();
            
            var classId = classObj.ClassId;

            db.SaveChanges();
            
            UpdateStudentGrade(uid, classId);
            
            return Json(new { success = true });
            
            return Json(new { success = false });
        }


        /// <summary>
        /// Returns a JSON array of the classes taught by the specified professor
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester in which the class is taught
        /// "year" - The year part of the semester in which the class is taught
        /// </summary>
        /// <param name="uid">The professor's uid</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {            
            
            var classes = from c in db.Classes
                join co in db.Courses on c.Course equals co.CourseId
                where c.Professor == uid
                select new
                {
                    subject = co.Departments,
                    number = co.Number,
                    name = co.Name,
                    season = c.Season,
                    year = c.Year
                };

            return Json(classes.ToArray());
            
            return Json(null);
        }

        
        
        /// <summary>
        /// This method calculates the grade for a student in a class.
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="classId"></param>
            private void UpdateStudentGrade(string uid, int classId)
            {
                
                var categories = (from ac in db.AssignmentCategories
                    where ac.Class == classId
                    select new
                    {
                        ac.CategoryId,
                        ac.Weight
                    }).ToList();

                double totalWeightedGrade = 0.0;
                
                foreach(var category in categories)
                {
                    var assignments = (from a in db.Assignments
                        join s in db.Submissions on a.AssignmentId equals s.Assignment
                        where s.Student == uid && a.Category == category.CategoryId
                        select new
                        {
                            s.Score,
                            a.MaxValue
                        }).ToList();
                    
                    if (!assignments.Any()) continue;
                    foreach(var assignment in assignments)
                    {
                        totalWeightedGrade += (assignment.Score / (double)assignment.MaxValue) * category.Weight;
                    }
                }
        
                double totalPercentage = totalWeightedGrade;
                string letter;
                if (totalPercentage >= 93) letter = "A";
                else if (totalPercentage >= 90) letter = "A-";
                else if (totalPercentage >= 87) letter = "B+";
                else if (totalPercentage >= 83) letter = "B";
                else if (totalPercentage >= 80) letter = "B-";
                else if (totalPercentage >= 77) letter = "C+";
                else if (totalPercentage >= 73) letter = "C";
                else if (totalPercentage >= 70) letter = "C-";
                else if (totalPercentage >= 67) letter = "D+";
                else if (totalPercentage >= 63) letter = "D";
                else if (totalPercentage >= 60) letter = "D-";
                else letter = "E";
                
                var enrolled = db.Enrolleds.First(e => e.Student == uid && e.Class == classId);
                enrolled.Grade = letter;
                db.SaveChanges();
            }
        
        
        /*******End code to modify********/
    }
}

