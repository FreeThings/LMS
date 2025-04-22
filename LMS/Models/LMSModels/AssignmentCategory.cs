using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class AssignmentCategory
    {
        public AssignmentCategory()
        {
            Assignments = new HashSet<Assignment>();
        }

        public string Name { get; set; } = null!;
        public int Weight { get; set; }
        public int Class { get; set; }
        public int CategoryId { get; set; }

        public virtual Class ClassNavigation { get; set; } = null!;
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}
