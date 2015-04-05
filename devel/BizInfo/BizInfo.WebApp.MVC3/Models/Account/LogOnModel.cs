using System.ComponentModel.DataAnnotations;

namespace BizInfo.WebApp.MVC3.Models.Account
{
    public class LogOnModel
    {
        [Required]
        [Display(Name = "Jméno")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; }

        [Display(Name = "Zůstat přihlášen")]
        public bool RememberMe { get; set; }
    }
}