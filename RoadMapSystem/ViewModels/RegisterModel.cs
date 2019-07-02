using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoadMapSystem.ViewModels
{
    public class RegisterModel
    {
        [Display(Name = "Логін")]
        public string Login { get; set; }
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Електронна адреса")]
        public string Email { get; set; }

        [Display(Name = "Імя")]
        public string Name { get; set; }

        [Display(Name = "Прізвище")]
        public string Surname { get; set; }

        [Display(Name = "По-батькові")]
        public string Patronymic { get; set; }

        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Посада")]
        public int EmployeeRoleId { get; set; }


        [Display(Name = "Технічний рівень")]
        public int RankId { get; set; }

        public int? MentorId { get; set; }

        public Dictionary<string, int> SkillsValue { get; set; }

    }
}
