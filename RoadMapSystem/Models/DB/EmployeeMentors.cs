using System;
using System.Collections.Generic;

namespace RoadMapSystem.Models.DB
{
    public partial class EmployeeMentors
    {
        public int MentorId { get; set; }
        public int InternId { get; set; }
        public DateTime DataOfMileStone { get; set; }
        public bool? DataChanger { get; set; }

        public Employee Intern { get; set; }
        public Employee Mentor { get; set; }
    }
}
