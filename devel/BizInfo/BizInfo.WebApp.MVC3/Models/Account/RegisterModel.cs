using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BizInfo.WebApp.MVC3.Models.Account
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Jméno")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musí mít alespoň {2} znaků..", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Zopakujte heslo")]
        [Compare("Password", ErrorMessage = "Hesla se neshodují.")]
        public string ConfirmPassword { get; set; }
    }
}