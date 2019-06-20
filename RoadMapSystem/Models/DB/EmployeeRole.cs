using System;
using System.Collections.Generic;

namespace RoadMapSystem.Models.DB
{
    public partial class EmployeeRole
    {
        public EmployeeRole()
        {
            Employee = new HashSet<Employee>();
        }

        public int EmployeeRoleId { get; set; }
        public string EmployeeRoleName { get; set; }

        public ICollection<Employee> Employee { get; set; }
    }
}
