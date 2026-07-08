namespace InterfaceAPI.DTOs;

public class ClienteRespostaDto
{
    public int Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}