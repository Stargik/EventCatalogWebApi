
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class UserModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType (DataType.Password)]
        public string Password { get; set; }
        
        public ICollection<string> Roles { get; set; }
    }
}
