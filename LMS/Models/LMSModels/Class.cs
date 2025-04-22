using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Class
    {
        public Class()
        {
            AssignmentCategories = new HashSet<AssignmentCategory>();
            Enrolleds = new HashSet<Enrolled>();
        }

        public string Season { get; set; } = null!;
        public string Location { get; set; } = null!;
        public DateTime EndTime { get; set; }
        public DateTime StartTime { get; set; }
        public int Course { get; set; }
        public string Professor { get; set; } = null!;
        public int ClassId { get; set; }
        public int Year { get; set; }

        public virtual Course CourseNavigation { get; set; } = null!;
        public virtual Professor ProfessorNavigation { get; set; } = null!;
        public virtual ICollection<AssignmentCategory> AssignmentCategories { get; set; }
        public virtual ICollection<Enrolled> Enrolleds { get; set; }
    }
}
