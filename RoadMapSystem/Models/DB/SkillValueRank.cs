using System;
using System.Collections.Generic;

namespace RoadMapSystem.Models.DB
{
    public partial class SkillValueRank
    {
        public int EmployeeRankId { get; set; }
        public int SkillId { get; set; }

        public EmployeeRank EmployeeRank { get; set; }
        public Skill Skill { get; set; }
    }
}
