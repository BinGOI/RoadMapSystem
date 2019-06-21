using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoadMapSystem.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не вказаний Логін працівника")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не вказаний Пароль працівника")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
