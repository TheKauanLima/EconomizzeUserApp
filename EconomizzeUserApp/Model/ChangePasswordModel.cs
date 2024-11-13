using System.ComponentModel.DataAnnotations;

namespace EconomizzeUserApp.Model
{
    public class ChangePasswordModel
    {
        public int UserId { get; set; }
        public Guid UserUniqueId { get; set; }

        [Required(ErrorMessage = "Senha necessaria")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Comfirmacao necessaria")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Comfirmacao necessaria, senhas diferentes")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha necessaria")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;
    }
}
