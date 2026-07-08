using System.ComponentModel.DataAnnotations;

namespace InterfaceAPI.DTOs;

public class AtualizarContatoDto
{
    [Required]
    public string Telefone { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}