using System.Diagnostics;
using System.Text;
using InterfaceAPI.Models;

namespace InterfaceAPI.Gateways;

public class CobolClienteGateway : ICobolClienteGateway
{
    private readonly string _pastaCobol;
    private readonly string _executavelCobol;
    private readonly string _arquivoRequisicao;
    private readonly string _arquivoResposta;
    private readonly SemaphoreSlim _semaforo = new(1, 1);
    private static readonly Encoding Utf8SemBom = new UTF8Encoding(false);

    public CobolClienteGateway(IWebHostEnvironment ambiente)
    {
        var pastaApi = ambiente.ContentRootPath;

        var pastaPrincipal = Directory.GetParent(pastaApi)?.FullName
            ?? throw new InvalidOperationException("Não foi possível localizar a pasta principal do projeto.");

        _pastaCobol = Path.Combine(pastaPrincipal, "Cobol");
        _executavelCobol = Path.Combine(_pastaCobol, "cliente");
        _arquivoRequisicao = Path.Combine(_pastaCobol, "integracao", "requisicao.txt");
        _arquivoResposta = Path.Combine(_pastaCobol, "integracao", "resposta.txt");
    }

    public async Task<Cliente?> ConsultarClienteAsync(int codigo)
    {
        await _semaforo.WaitAsync();

        try
        {
            EscreverRequisicao('C', codigo, "", "");

            await ExecutarCobolAsync();

            return LerResposta();
        }
        finally
        {
            _semaforo.Release();
        }
    }

    public async Task<Cliente?> AtualizarContatoAsync(
        int codigo,
        string telefone,
        string email
    )
    {
        await _semaforo.WaitAsync();

        try
        {
            EscreverRequisicao('A', codigo, telefone, email);

            await ExecutarCobolAsync();

            return LerResposta();
        }
        finally
        {
            _semaforo.Release();
        }
    }

    private void EscreverRequisicao(
        char operacao,
        int codigo,
        string telefone,
        string email
    )
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_arquivoRequisicao)!);

        string linha =
            operacao +
            codigo.ToString("D5") +
            AjustarTexto(telefone, 15) +
            AjustarTexto(email, 50);

        File.WriteAllText(_arquivoRequisicao, linha + Environment.NewLine, Utf8SemBom);
    }

    private async Task ExecutarCobolAsync()
    {
        if (!File.Exists(_executavelCobol))
        {
            throw new InvalidOperationException(
                "Executável COBOL não encontrado. Compile o COBOL antes de rodar a API."
            );
        }

        var processo = new Process();

        processo.StartInfo = new ProcessStartInfo
        {
            FileName = _executavelCobol,
            WorkingDirectory = _pastaCobol,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        processo.Start();

        string saida = await processo.StandardOutput.ReadToEndAsync();
        string erro = await processo.StandardError.ReadToEndAsync();

        await processo.WaitForExitAsync();

        if (processo.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Erro ao executar o programa COBOL. Saída: {saida} Erro: {erro}"
            );
        }
    }

    private Cliente? LerResposta()
    {
        if (!File.Exists(_arquivoResposta))
        {
            throw new InvalidOperationException(
                "Arquivo de resposta do COBOL não foi gerado."
            );
        }

        string linha = File.ReadAllText(_arquivoResposta, Utf8SemBom)
            .TrimEnd('\r', '\n');

        if (linha.Length < 2)
        {
            throw new InvalidOperationException(
                "Resposta do COBOL está vazia ou inválida."
            );
        }

        string status = linha.Substring(0, 2);

        if (status == "NF")
        {
            return null;
        }

        if (status != "OK")
        {
            throw new InvalidOperationException(
                $"O programa COBOL retornou erro no processamento. Status recebido: {status}"
            );
        }

        linha = linha.PadRight(112);

        string codigoTexto = linha.Substring(2, 5);

        if (!int.TryParse(codigoTexto, out int codigo))
        {
            throw new InvalidOperationException(
                $"Código retornado pelo COBOL está inválido: {codigoTexto}"
            );
        }

        return new Cliente
        {
            Codigo = codigo,
            Nome = linha.Substring(7, 40).Trim(),
            Telefone = linha.Substring(47, 15).Trim(),
            Email = linha.Substring(62, 50).Trim()
        };
    }

    private static string AjustarTexto(string valor, int tamanho)
    {
        valor = valor
            .Replace("\r", "")
            .Replace("\n", "")
            .Trim();

        if (valor.Length > tamanho)
        {
            return valor.Substring(0, tamanho);
        }

        return valor.PadRight(tamanho);
    }
}