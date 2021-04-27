using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WareHouseSys.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "必須填寫帳號欄位。")]
        [Display(Name = "帳號")]
        public string ID { get; set; }

        [Required(ErrorMessage = "必須填寫密碼欄位。")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Display(Name = "記住我?")]
        public bool RememberMe { get; set; }
    }

   

}
