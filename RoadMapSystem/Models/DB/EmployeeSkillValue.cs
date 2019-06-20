using System;
using System.Collections.Generic;

namespace RoadMapSystem.Models.DB
{
    public partial class EmployeeSkillValue
    {
        public EmployeeSkillValue()
        {
            MileStone = new HashSet<MileStone>();
        }

        public int EmployeeSkillValueId { get; set; }
        public int EmployeeId { get; set; }
        public int SkillId { get; set; }

        public Employee Employee { get; set; }
        public Skill Skill { get; set; }
        public ICollection<MileStone> MileStone { get; set; }
    }
}
