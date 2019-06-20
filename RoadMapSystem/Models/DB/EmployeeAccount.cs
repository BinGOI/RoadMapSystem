using System;
using System.Collections.Generic;

namespace RoadMapSystem.Models.DB
{
    public partial class EmployeeAccount
    {
        public int EmployeeAccountId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public Employee EmployeeAccountNavigation { get; set; }
    }
}
