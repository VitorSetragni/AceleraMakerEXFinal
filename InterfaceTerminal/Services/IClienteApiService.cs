using InterfaceTerminal.Models;
using InterfaceTerminal.TratamentoErros;

namespace InterfaceTerminal.Services;

public interface IClienteApiService
{
    Task<TratamentoErrosApi<Cliente>> PesquisarClienteAsync(int codigo);

    Task<TratamentoErrosApi<Cliente>> AtualizarContatoAsync(
        int codigo,
        string telefone,
        string email
    );
}