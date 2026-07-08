# AceleraMakerExFinal

## Objetivo

Este projeto implementa uma solução para modernização de um sistema legado de cadastro de clientes da Cooperativa Financeira Alfa.

A solução permite:

- pesquisar cliente pelo código;
- exibir código, nome, telefone e e-mail;
- atualizar telefone e e-mail;
- informar quando o cliente não existe;
- persistir as alterações no arquivo de dados utilizado pelo COBOL.

## Arquitetura

A arquitetura foi dividida em três partes principais:

```text
InterfaceTerminal -> InterfaceAPI -> COBOL -> Dados
```

- **InterfaceTerminal**: aplicação .NET Console usada pelo usuário.
- **InterfaceAPI**: API REST ASP.NET Core que expõe os serviços de cliente.
- **COBOL**: componente legado responsável por consultar e atualizar o arquivo de clientes.
- **Dados**: arquivo de dados com registros de tamanho fixo.

## Estrutura do Projeto

```text
AceleraMakerExFinal/
├── InterfaceAPI/
│   ├── Controllers/
│   ├── DTOs/
│   ├── Gateways/
│   ├── Models/
│   ├── Services/
│   └── Program.cs
│
├── InterfaceTerminal/
│   ├── Configuracoes/
│   ├── DTOs/
│   ├── Menus/
│   ├── Models/
│   ├── Services/
│   ├── TratamentoErros/
│   └── Program.cs
│
├── Cobol/
│   ├── src/
│   ├── copybooks/
│   ├── dados/
│   └── integracao/
│
├── TestesAutomatizados/
│   ├── Configuracao/
│   ├── Utils/
│   └── ClientesApiTests.cs
│
├── docs/
└── evidencias/
```

## Tecnologias Utilizadas

- .NET
- ASP.NET Core Web API
- Aplicação Console .NET
- GnuCOBOL
- Swagger
- xUnit
- Arquivos de layout fixo

## Como Compilar o COBOL

Na raiz do projeto, execute:

```bash
cd Cobol
cobc -x -free -I copybooks -o cliente src/cliente.cob
```

Esse comando gera o executável:

```text
Cobol/cliente
```

## Como Rodar a API

Em um terminal, execute:

```bash
cd InterfaceAPI
dotnet run
```

A API deve ficar disponível em:

```text
http://localhost:5040
```

O Swagger pode ser acessado em:

```text
http://localhost:5040/swagger
```

## Como Rodar a Interface Terminal

Em outro terminal, execute:

```bash
cd InterfaceTerminal
dotnet run
```

O usuário poderá escolher entre:

```text
1 - Pesquisar cliente
2 - Atualizar telefone e e-mail
0 - Sair
```

## Endpoints da API

### Pesquisar cliente

```http
GET /api/clientes/{codigo}
```

### Atualizar telefone e e-mail

```http
PUT /api/clientes/{codigo}/contato
```

Exemplo de corpo da requisição:

```json
{
  "telefone": "31988887777",
  "email": "novogmail@gmail.com"
}
```

## Como Rodar os Testes Automatizados

Antes dos testes, compile o COBOL

Depois, na raiz do projeto:

```bash
dotnet test TestesAutomatizados/TestesAutomatizados.csproj
```

Resultado esperado:

```text
Passed: 4
```

Os testes cobrem:

- pesquisa de cliente existente;
- pesquisa de cliente inexistente;
- atualização de contato de cliente existente;
- verificação de persistência da atualização;
- tentativa de atualização de cliente inexistente.

## Observações

O Swagger foi mantido como apoio para documentação e teste manual da API. A interface principal de uso do sistema é a aplicação de terminal.
