using System.ComponentModel.DataAnnotations;

namespace EconomizzeUserApp.Model
{
    public class UserDetailModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Primeiro Nome é obrigatório.")]
        [StringLength(50, ErrorMessage = "Primeiro Nome não pode ter mais que 50 caracteres.")]
        public string UserFirstName { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Segundo Nome não pode ter mais que 50 caracteres.")]
        public string UserMiddleName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sobrenome é obrigatório.")]
        [StringLength(50, ErrorMessage = "Sobrenome não pode ter mais que 50 caracteres.")]
        public string UserLastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório.")]
        [StringLength(11, ErrorMessage = "CPF deve conter 11 caracteres.")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "RG é obrigatório.")]
        public string Rg { get; set; } = string.Empty;
    }
}
