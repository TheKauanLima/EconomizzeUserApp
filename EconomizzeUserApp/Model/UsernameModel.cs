using System.ComponentModel.DataAnnotations;

namespace EconomizzeUserApp.Model
{
    public class UsernameModel
    {
        [Required(ErrorMessage = "Email necessario")]
        [EmailAddress(ErrorMessage = "Tem de ser um email")]
        public string Username { get; set; } = string.Empty;
    }
}
