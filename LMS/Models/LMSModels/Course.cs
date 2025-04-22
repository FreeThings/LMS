using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Course
    {
        public Course()
        {
            Classes = new HashSet<Class>();
        }

        public int Number { get; set; }
        public string Name { get; set; } = null!;
        public string Departments { get; set; } = null!;
        public int CourseId { get; set; }

        public virtual Department DepartmentsNavigation { get; set; } = null!;
        public virtual ICollection<Class> Classes { get; set; }
    }
}
