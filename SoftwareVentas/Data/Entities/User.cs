using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SoftwareVentas.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Documento")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]

        public string Document { get; set; } = null!;

        [Display(Name = "Nombres")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]

        public string FirstName { get; set; } = null!;

        [Display(Name = "Apellidoo")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]

        public string LastName { get; set; } = null!;

        public string FullName => $"{FirstName} {LastName}";
        
        public int RoleId { get; set; }

        public Role Role { get; set; }

    }

}