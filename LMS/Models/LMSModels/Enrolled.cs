using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Enrolled
    {
        public string Student { get; set; } = null!;
        public int Class { get; set; }
        public string Grade { get; set; } = null!;

        public virtual Class ClassNavigation { get; set; } = null!;
        public virtual Student StudentNavigation { get; set; } = null!;
    }
}
