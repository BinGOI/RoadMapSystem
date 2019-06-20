using System;
using System.Collections.Generic;

namespace RoadMapSystem.Models.DB
{
    public partial class MileStone
    {
        public MileStone()
        {
            Comment = new HashSet<Comment>();
        }

        public int MileStoneId { get; set; }
        public int EmployeeSkillValueId { get; set; }
        public DateTime? Date { get; set; }

        public EmployeeSkillValue EmployeeSkillValue { get; set; }
        public ICollection<Comment> Comment { get; set; }
    }
}
