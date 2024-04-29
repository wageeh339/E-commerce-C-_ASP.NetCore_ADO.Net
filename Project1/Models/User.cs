using System.ComponentModel.DataAnnotations;

namespace Project1.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
