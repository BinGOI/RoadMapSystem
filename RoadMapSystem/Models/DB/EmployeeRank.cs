using System;
using System.Collections.Generic;

namespace RoadMapSystem.Models.DB
{
    public partial class EmployeeRank
    {
        public EmployeeRank()
        {
            Employee = new HashSet<Employee>();
            SkillValueRank = new HashSet<SkillValueRank>();
        }

        public int EmployeeRankId { get; set; }
        public string EmployeeRankTitle { get; set; }

        public ICollection<Employee> Employee { get; set; }
        public ICollection<SkillValueRank> SkillValueRank { get; set; }
    }
}
