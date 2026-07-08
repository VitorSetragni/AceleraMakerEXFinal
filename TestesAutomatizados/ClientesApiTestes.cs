using System.Net;
using System.Net.Http.Json;
using TestesAutomatizados.Configuracao;
using TestesAutomatizados.Utils;

namespace TestesAutomatizados;

public class ClientesApiTestes : IClassFixture<TesteApiFactory>
{
    private readonly HttpClient _clienteHttp;

    public ClientesApiTestes(TesteApiFactory factory)
    {
        _clienteHttp = factory.CreateClient();
    }

    [Fact]
    public async Task PesquisarClienteExistente_DeveRetornarDadosDoCliente()
    {
        DadosCobolHelper.PrepararAmbiente();

        var resposta = await _clienteHttp.GetAsync("/api/clientes/1");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        var cliente = await resposta.Content.ReadFromJsonAsync<ClienteRespostaTeste>();

        Assert.NotNull(cliente);
        Assert.Equal(1, cliente!.Codigo);
        Assert.Equal("Ana Souza", cliente.Nome);
        Assert.Equal("31999990001", cliente.Telefone);
        Assert.Equal("ana.souza@email.com", cliente.Email);
    }

    [Fact]
    public async Task PesquisarClienteInexistente_DeveRetornarNotFound()
    {
        DadosCobolHelper.PrepararAmbiente();

        var resposta = await _clienteHttp.GetAsync("/api/clientes/999");

        Assert.Equal(HttpStatusCode.NotFound, resposta.StatusCode);
    }

    [Fact]
    public async Task AtualizarContatoClienteExistente_DeveAlterarTelefoneEEmail()
    {
        DadosCobolHelper.PrepararAmbiente();

        var dadosAtualizacao = new AtualizarContatoTeste
        {
            Telefone = "31988887777",
            Email = "ana.novo@email.com"
        };

        var respostaAtualizacao = await _clienteHttp.PutAsJsonAsync(
            "/api/clientes/1/contato",
            dadosAtualizacao
        );

        Assert.Equal(HttpStatusCode.OK, respostaAtualizacao.StatusCode);

        var clienteAtualizado =
            await respostaAtualizacao.Content.ReadFromJsonAsync<ClienteRespostaTeste>();

        Assert.NotNull(clienteAtualizado);
        Assert.Equal(1, clienteAtualizado!.Codigo);
        Assert.Equal("Ana Souza", clienteAtualizado.Nome);
        Assert.Equal("31988887777", clienteAtualizado.Telefone);
        Assert.Equal("ana.novo@email.com", clienteAtualizado.Email);

        var respostaConsulta = await _clienteHttp.GetAsync("/api/clientes/1");

        Assert.Equal(HttpStatusCode.OK, respostaConsulta.StatusCode);

        var clienteConsultado =
            await respostaConsulta.Content.ReadFromJsonAsync<ClienteRespostaTeste>();

        Assert.NotNull(clienteConsultado);
        Assert.Equal("31988887777", clienteConsultado!.Telefone);
        Assert.Equal("ana.novo@email.com", clienteConsultado.Email);
    }

    [Fact]
    public async Task AtualizarContatoClienteInexistente_DeveRetornarNotFound()
    {
        DadosCobolHelper.PrepararAmbiente();

        var dadosAtualizacao = new AtualizarContatoTeste
        {
            Telefone = "31977776666",
            Email = "inexistente@email.com"
        };

        var resposta = await _clienteHttp.PutAsJsonAsync(
            "/api/clientes/999/contato",
            dadosAtualizacao
        );

        Assert.Equal(HttpStatusCode.NotFound, resposta.StatusCode);
    }

    private class ClienteRespostaTeste
    {
        public int Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    private class AtualizarContatoTeste
    {
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}