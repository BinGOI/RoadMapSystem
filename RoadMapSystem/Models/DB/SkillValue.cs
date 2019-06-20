using System;
using System.Collections.Generic;

namespace RoadMapSystem.Models.DB
{
    public partial class SkillValue
    {
        public SkillValue()
        {
            Skill = new HashSet<Skill>();
        }

        public int SkillValueId { get; set; }
        public string SkillValueTitle { get; set; }

        public ICollection<Skill> Skill { get; set; }
    }
}
