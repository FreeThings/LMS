﻿using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Assignment
    {
        public Assignment()
        {
            Submissions = new HashSet<Submission>();
        }

        public string Name { get; set; } = null!;
        public int MaxValue { get; set; }
        public string Instructions { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public int Category { get; set; }
        public int AssignmentId { get; set; }

        public virtual AssignmentCategory CategoryNavigation { get; set; } = null!;
        public virtual ICollection<Submission> Submissions { get; set; }
    }
}
