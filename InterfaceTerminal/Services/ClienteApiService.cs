using System.Net;
using System.Net.Http.Json;
using InterfaceTerminal.DTOs;
using InterfaceTerminal.Models;
using InterfaceTerminal.TratamentoErros;

namespace InterfaceTerminal.Services;

public class ClienteApiService : IClienteApiService
{
    private readonly HttpClient _httpClient;

    public ClienteApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TratamentoErrosApi<Cliente>> PesquisarClienteAsync(int codigo)
    {
        try
        {
            var resposta = await _httpClient.GetAsync($"/api/clientes/{codigo}");

            if (resposta.StatusCode == HttpStatusCode.NotFound)
            {
                return TratamentoErrosApi<Cliente>.Falha(
                    "Cliente não encontrado.",
                    naoEncontrado: true
                );
            }

            if (!resposta.IsSuccessStatusCode)
            {
                string detalheErro = await resposta.Content.ReadAsStringAsync();

                return TratamentoErrosApi<Cliente>.Falha(
                    $"Erro ao pesquisar cliente. Status HTTP: {(int)resposta.StatusCode}\n{detalheErro}"
                );
            }

            var cliente = await resposta.Content.ReadFromJsonAsync<Cliente>();

            if (cliente == null)
            {
                return TratamentoErrosApi<Cliente>.Falha(
                    "A API respondeu, mas os dados do cliente não puderam ser lidos."
                );
            }

            return TratamentoErrosApi<Cliente>.Ok(cliente);
        }
        catch (HttpRequestException)
        {
            return TratamentoErrosApi<Cliente>.Falha(
                "Não foi possível conectar na API. Verifique se a InterfaceAPI está rodando."
            );
        }
    }

    public async Task<TratamentoErrosApi<Cliente>> AtualizarContatoAsync(
        int codigo,
        string telefone,
        string email
    )
    {
        try
        {
            var request = new AtualizarContatoDto
            {
                Telefone = telefone,
                Email = email
            };

            var resposta = await _httpClient.PutAsJsonAsync(
                $"/api/clientes/{codigo}/contato",
                request
            );

            if (resposta.StatusCode == HttpStatusCode.NotFound)
            {
                return TratamentoErrosApi<Cliente>.Falha(
                    "Cliente não encontrado.",
                    naoEncontrado: true
                );
            }

            if (!resposta.IsSuccessStatusCode)
            {
                string detalheErro = await resposta.Content.ReadAsStringAsync();

                return TratamentoErrosApi<Cliente>.Falha(
                    $"Erro ao atualizar cliente. Status HTTP: {(int)resposta.StatusCode}\n{detalheErro}"
                );
            }

            var cliente = await resposta.Content.ReadFromJsonAsync<Cliente>();

            if (cliente == null)
            {
                return TratamentoErrosApi<Cliente>.Falha(
                    "A API respondeu, mas os dados atualizados não puderam ser lidos."
                );
            }

            return TratamentoErrosApi<Cliente>.Ok(cliente);
        }
        catch (HttpRequestException)
        {
            return TratamentoErrosApi<Cliente>.Falha(
                "Não foi possível conectar na API. Verifique se a InterfaceAPI está rodando."
            );
        }
    }
}