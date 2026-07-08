using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TestesAutomatizados.Configuracao;

public class TesteApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        string pastaRaiz = ObterPastaRaizProjeto();
        string pastaApi = Path.Combine(pastaRaiz, "InterfaceAPI");

        builder.UseContentRoot(pastaApi);
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