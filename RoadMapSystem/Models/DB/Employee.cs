using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Імя")]
        public string Name { get; set; }

        [Display(Name = "Прізвище")]
        public string Surname { get; set; }

        [Display(Name = "По-батькові")]
        public string Patronymic { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Номер телефону")]
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
