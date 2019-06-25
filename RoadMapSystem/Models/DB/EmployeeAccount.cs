using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoadMapSystem.Models.DB
{
    public partial class EmployeeAccount
    {
        public int EmployeeAccountId { get; set; }
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public Employee EmployeeAccountNavigation { get; set; }
    }
}
