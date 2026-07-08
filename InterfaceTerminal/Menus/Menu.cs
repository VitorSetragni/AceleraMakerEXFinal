using InterfaceTerminal.Models;
using InterfaceTerminal.Services;

namespace InterfaceTerminal.Menus;

public class Menu
{
    private readonly IClienteApiService _clienteApiService;

    public Menu(IClienteApiService clienteApiService)
    {
        _clienteApiService = clienteApiService;
    }

    public async Task ExecutarAsync()
    {
        bool continuar = true;

        while (continuar)
        {
            Console.Clear();
            Console.WriteLine(" ");
            Console.WriteLine(" SISTEMA DE CLIENTES - MENU PRINCIPAL");
            Console.WriteLine(" ");
            Console.WriteLine("1 - Pesquisar cliente");
            Console.WriteLine("2 - Atualizar telefone e e-mail");
            Console.WriteLine("0 - Sair");
            Console.WriteLine(" ");
            Console.Write("Escolha uma opção: ");

            string? opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    await PesquisarClienteAsync();
                    break;

                case "2":
                    await AtualizarClienteAsync();
                    break;

                case "0":
                    continuar = false;
                    break;

                default:
                    Console.WriteLine();
                    Console.WriteLine("Opção inválida.");
                    Pausar();
                    break;
            }
        }
    }

    private async Task PesquisarClienteAsync()
    {
        Console.Clear();
        Console.WriteLine("    PESQUISAR CLIENTE");
        Console.WriteLine();

        int? codigo = LerCodigoCliente();

        if (codigo == null)
        {
            Pausar();
            return;
        }

        var resultado = await _clienteApiService.PesquisarClienteAsync(codigo.Value);

        Console.WriteLine();

        if (!resultado.Sucesso)
        {
            Console.WriteLine(resultado.Mensagem);
            Pausar();
            return;
        }

        ExibirCliente(resultado.Dados!);
        Pausar();
    }

    private async Task AtualizarClienteAsync()
    {
        Console.Clear();
        Console.WriteLine("    ATUALIZAR TELEFONE E E-MAIL DO CLIENTE");
        Console.WriteLine();

        int? codigo = LerCodigoCliente();

        if (codigo == null)
        {
            Pausar();
            return;
        }

        var resultadoPesquisa = await _clienteApiService.PesquisarClienteAsync(codigo.Value);

        Console.WriteLine();

        if (!resultadoPesquisa.Sucesso)
        {
            Console.WriteLine(resultadoPesquisa.Mensagem);
            Pausar();
            return;
        }

        Console.WriteLine("Cliente encontrado.");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Informe os novos dados de contato:");
        Console.WriteLine();

        Console.Write("Digite o novo telefone: ");
        string telefone = Console.ReadLine() ?? "";

        Console.Write("Digite o novo e-mail: ");
        string email = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(telefone))
        {
            Console.WriteLine();
            Console.WriteLine("O telefone não pode ficar vazio.");
            Pausar();
            return;
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine();
            Console.WriteLine("O e-mail não pode ficar vazio.");
            Pausar();
            return;
        }

        var resultadoAtualizacao = await _clienteApiService.AtualizarContatoAsync(
            codigo.Value,
            telefone,
            email
        );

        Console.WriteLine();

        if (!resultadoAtualizacao.Sucesso)
        {
            Console.WriteLine(resultadoAtualizacao.Mensagem);
            Pausar();
            return;
        }

        Console.WriteLine("Contato atualizado com sucesso!");
        Console.WriteLine();

        ExibirCliente(resultadoAtualizacao.Dados!);
        Pausar();
    }

    private static int? LerCodigoCliente()
    {
        Console.Write("Digite o código do cliente: ");
        string? codigoTexto = Console.ReadLine();

        if (!int.TryParse(codigoTexto, out int codigo))
        {
            Console.WriteLine();
            Console.WriteLine("Código inválido. Digite apenas números.");
            return null;
        }

        return codigo;
    }

    private static void ExibirCliente(Cliente cliente)
    {
        Console.WriteLine("Dados do cliente:");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Código:   {cliente.Codigo}");
        Console.WriteLine($"Nome:     {cliente.Nome}");
        Console.WriteLine($"Telefone: {cliente.Telefone}");
        Console.WriteLine($"E-mail:   {cliente.Email}");
        Console.WriteLine("----------------------------------------");
    }

    private static void Pausar()
    {
        Console.WriteLine();
        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadLine();
    }
}