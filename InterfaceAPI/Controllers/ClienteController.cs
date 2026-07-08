using InterfaceAPI.DTOs;
using InterfaceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAPI.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClietenController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClietenController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet("{codigo:int}")]
    public async Task<ActionResult<ClienteRespostaDto>> ConsultarCliente(int codigo)
    {
        try
        {
            var cliente = await _clienteService.ConsultarClienteAsync(codigo);

            if (cliente == null)
            {
                return NotFound(new
                {
                    mensagem = "Cliente não encontrado."
                });
            }

            return Ok(cliente);
        }
        catch (NotImplementedException erro)
        {
            return StatusCode(501, new
            {
                mensagem = erro.Message
            });
        }
    }

    [HttpPut("{codigo:int}/contato")]
    public async Task<ActionResult<ClienteRespostaDto>> AtualizarContato(
        int codigo,
        [FromBody] AtualizarContatoDto request
    )
    {
        try
        {
            var cliente = await _clienteService.AtualizarContatoAsync(codigo, request);

            if (cliente == null)
            {
                return NotFound(new
                {
                    mensagem = "Cliente não encontrado."
                });
            }

            return Ok(cliente);
        }
        catch (NotImplementedException erro)
        {
            return StatusCode(501, new
            {
                mensagem = erro.Message
            });
        }
    }
}