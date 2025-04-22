using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Submission
    {
        public string Student { get; set; } = null!;
        public int Assignment { get; set; }
        public DateTime Time { get; set; }
        public string Contents { get; set; } = null!;
        public int Score { get; set; }

        public virtual Assignment AssignmentNavigation { get; set; } = null!;
        public virtual Student StudentNavigation { get; set; } = null!;
    }
}
