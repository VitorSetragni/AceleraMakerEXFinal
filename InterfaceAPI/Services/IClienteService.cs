using InterfaceAPI.DTOs;

namespace InterfaceAPI.Services;

public interface IClienteService
{
    Task<ClienteRespostaDto?> ConsultarClienteAsync(int codigo);
    Task<ClienteRespostaDto?> AtualizarContatoAsync(
        int codigo,
        AtualizarContatoDto request
    );
}