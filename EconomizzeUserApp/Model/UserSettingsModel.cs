using System.ComponentModel.DataAnnotations;

public class UserSettingsModel
{
    [Required]
    public string Theme { get; set; } = "Light";
}
