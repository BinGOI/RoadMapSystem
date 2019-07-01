using RoadMapSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadMapSystem.ViewModels
{
    public class CompareModel
    {
        public Employee employeeProfile { get; set; }

        public EmployeeRank compRank { get; set; }
    }
}
