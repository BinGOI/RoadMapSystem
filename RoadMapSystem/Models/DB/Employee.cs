using System;
using System.Collections.Generic;

namespace RoadMapSystem.Models.DB
{
    public partial class Employee
    {
        public Employee()
        {
            EmployeeMentorsIntern = new HashSet<EmployeeMentors>();
            EmployeeMentorsMentor = new HashSet<EmployeeMentors>();
            EmployeeSkillValue = new HashSet<EmployeeSkillValue>();
        }

        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int EmployeeRoleId { get; set; }
        public int RankId { get; set; }

        public EmployeeRole EmployeeRole { get; set; }
        public EmployeeRank Rank { get; set; }
        public EmployeeAccount EmployeeAccount { get; set; }
        public ICollection<EmployeeMentors> EmployeeMentorsIntern { get; set; }
        public ICollection<EmployeeMentors> EmployeeMentorsMentor { get; set; }
        public ICollection<EmployeeSkillValue> EmployeeSkillValue { get; set; }
    }
}
