
using System.ComponentModel.DataAnnotations;

namespace EcommerceLibrary.Models;

public class AuthenticationModel
{

    public const string requiredMessage = "this field is required";
    [Required(ErrorMessage = requiredMessage)]
    public string first_name { get; set; }
    [Required(ErrorMessage = requiredMessage)]
    [MaxLength(10 , ErrorMessage = "name must be 4 to 10 characters"), MinLength(4, ErrorMessage = "name must be 4 to 10 characters")]
    public string last_name { get; set; }
    [Required(ErrorMessage = requiredMessage),MinLength(8,ErrorMessage = "password must be from 8 to 16 characters"),MaxLength(16, ErrorMessage = "password must be from 8 to 16 characters")]
    public string password { get; set; }
    [Required (ErrorMessage = requiredMessage)]
    [Phone(ErrorMessage = "Please Enter Valid Phone Number")]
    public string  phone_number { get; set; }
    [Required(ErrorMessage = requiredMessage)]

    [EmailAddress(ErrorMessage = "Please Enter Valid Email")]
    public string email { get; set; }
    [Required(ErrorMessage = requiredMessage)]
    public string city { get; set; }

    public int? role_id { get; set; }

}
