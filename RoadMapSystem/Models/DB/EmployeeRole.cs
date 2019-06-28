using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoadMapSystem.Models.DB
{
    public partial class EmployeeRole
    {
        public EmployeeRole()
        {
            Employee = new HashSet<Employee>();
        }

        public int EmployeeRoleId { get; set; }

        [Display(Name = "Посада")]
        public string EmployeeRoleName { get; set; }

        public ICollection<Employee> Employee { get; set; }
    }
}
