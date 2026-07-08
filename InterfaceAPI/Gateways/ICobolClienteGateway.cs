using InterfaceAPI.Models;

namespace InterfaceAPI.Gateways;

public interface ICobolClienteGateway
{
    Task<Cliente?> ConsultarClienteAsync(int codigo);
    Task<Cliente?> AtualizarContatoAsync(int codigo, string telefone, string email);
}