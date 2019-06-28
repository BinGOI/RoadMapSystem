using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadMapSystem.ViewModels
{
    public class RegisterModel
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string PhoneNumber { get; set; }
        public int EmployeeRoleId { get; set; }
        public int RankId { get; set; }

        public int? MentorId { get; set; }

        public Dictionary<string, int> SkillsValue { get; set; }

    }
}
