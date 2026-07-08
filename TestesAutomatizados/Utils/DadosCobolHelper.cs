using System.Text;

namespace TestesAutomatizados.Utils;

public static class DadosCobolHelper
{
    private static readonly Encoding Utf8SemBom = new UTF8Encoding(false);

    public static void PrepararAmbiente()
    {
        string pastaRaiz = ObterPastaRaizProjeto();
        string pastaCobol = Path.Combine(pastaRaiz, "Cobol");

        string executavelCobol = Path.Combine(pastaCobol, "cliente");

        if (!File.Exists(executavelCobol))
        {
            throw new InvalidOperationException(
                "Executável COBOL não encontrado. Rode ./scripts/compilar.sh dentro da pasta Cobol antes dos testes."
            );
        }

        string pastaDados = Path.Combine(pastaCobol, "dados");
        string pastaIntegracao = Path.Combine(pastaCobol, "integracao");

        Directory.CreateDirectory(pastaDados);
        Directory.CreateDirectory(pastaIntegracao);

        string arquivoClientes = Path.Combine(pastaDados, "clientes.dat");

        var linhas = new List<string>
        {
            FormatarCliente(1, "Ana Souza", "31999990001", "ana.souza@email.com"),
            FormatarCliente(2, "Bruno Lima", "31999990002", "bruno.lima@email.com"),
            FormatarCliente(3, "Carla Mendes", "31999990003", "carla.mendes@email.com")
        };

        File.WriteAllLines(arquivoClientes, linhas, Utf8SemBom);

        File.Delete(Path.Combine(pastaIntegracao, "requisicao.txt"));
        File.Delete(Path.Combine(pastaIntegracao, "resposta.txt"));
    }

    private static string FormatarCliente(
        int codigo,
        string nome,
        string telefone,
        string email
    )
    {
        return codigo.ToString("D5")
            + AjustarTexto(nome, 40)
            + AjustarTexto(telefone, 15)
            + AjustarTexto(email, 50);
    }

    private static string AjustarTexto(string valor, int tamanho)
    {
        valor = valor.Trim();

        if (valor.Length > tamanho)
        {
            return valor.Substring(0, tamanho);
        }

        return valor.PadRight(tamanho);
    }

    private static string ObterPastaRaizProjeto()
    {
        var pastaAtual = new DirectoryInfo(AppContext.BaseDirectory);

        while (pastaAtual != null)
        {
            bool encontrouCobol = Directory.Exists(
                Path.Combine(pastaAtual.FullName, "Cobol")
            );

            bool encontrouApi = Directory.Exists(
                Path.Combine(pastaAtual.FullName, "InterfaceAPI")
            );

            if (encontrouCobol && encontrouApi)
            {
                return pastaAtual.FullName;
            }

            pastaAtual = pastaAtual.Parent;
        }

        throw new InvalidOperationException(
            "Não foi possível localizar a pasta raiz do projeto."
        );
    }
}