using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BizInfo.WebApp.MVC3.Models.Account
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Současné heslo")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musí mít alespoň {2} znaků.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nové heslo")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Zopakujte nové heslo")]
        [Compare("NewPassword", ErrorMessage = "Hesla se neshodují.")]
        public string ConfirmPassword { get; set; }
    }
}