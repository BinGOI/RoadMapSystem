using System;
using System.Collections.Generic;

namespace RoadMapSystem.Models.DB
{
    public partial class Skill
    {
        public Skill()
        {
            EmployeeSkillValue = new HashSet<EmployeeSkillValue>();
            SkillValueRank = new HashSet<SkillValueRank>();
        }

        public int IdSkill { get; set; }
        public string SkillTitle { get; set; }
        public int SkillValueId { get; set; }
        public string DescriptionOfSkill { get; set; }

        public SkillValue SkillValue { get; set; }
        public ICollection<EmployeeSkillValue> EmployeeSkillValue { get; set; }
        public ICollection<SkillValueRank> SkillValueRank { get; set; }
    }
}
