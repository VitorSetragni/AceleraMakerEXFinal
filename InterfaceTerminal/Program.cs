using InterfaceTerminal.ApiConfiguracao;
using InterfaceTerminal.Menus;
using InterfaceTerminal.Services;

using var httpClient = new HttpClient
{
    BaseAddress = new Uri(ApiConfiguracao.UrlBase)
};

IClienteApiService clienteApiService = new ClienteApiService(httpClient);

var menu = new Menu(clienteApiService);

await menu.ExecutarAsync();