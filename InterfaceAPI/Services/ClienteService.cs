using InterfaceAPI.DTOs;
using InterfaceAPI.Gateways;
using InterfaceAPI.Models;

namespace InterfaceAPI.Services;

public class ClienteService : IClienteService
{
    private readonly ICobolClienteGateway _cobolGateway;

    public ClienteService(ICobolClienteGateway cobolGateway)
    {
        _cobolGateway = cobolGateway;
    }

    public async Task<ClienteRespostaDto?> ConsultarClienteAsync(int codigo)
    {
        var cliente = await _cobolGateway.ConsultarClienteAsync(codigo);

        if (cliente == null)
        {
            return null;
        }

        return ConverterParaDto(cliente);
    }

    public async Task<ClienteRespostaDto?> AtualizarContatoAsync(
        int codigo,
        AtualizarContatoDto request
    )
    {
        var cliente = await _cobolGateway.AtualizarContatoAsync(
            codigo,
            request.Telefone,
            request.Email
        );

        if (cliente == null)
        {
            return null;
        }

        return ConverterParaDto(cliente);
    }

    private static ClienteRespostaDto ConverterParaDto(Cliente cliente)
    {
        return new ClienteRespostaDto
        {
            Codigo = cliente.Codigo,
            Nome = cliente.Nome,
            Telefone = cliente.Telefone,
            Email = cliente.Email
        };
    }
}