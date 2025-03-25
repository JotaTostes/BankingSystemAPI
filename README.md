
# API Banking

A API Banking é um sistema de caixa de banco que permite o cadastro de contas bancárias e a transferência de valores entre elas. Esta documentação fornece informações detalhadas sobre como rodar localmente a API e também integrar e utilizar a API em seus sistemas.


## Requisitos
Antes de iniciar a execução do projeto, certifique-se de ter os seguintes requisitos instalados:

- .NET 8 SDK

- Visual Studio 2022 (17.8 ou superior)

- SQL Server instalado localmente

- SQL Server Management Studio (SSMS) para gerenciar o banco de dados

- Postman ou outro cliente para testar APIs (opcional)
## Documentação
A API utiliza o Swagger para documentação e, ao executar o projeto, a interface do Swagger será aberta automaticamente.


## Rodando Local

Clone o projeto

```bash
  git clone https://github.com/JotaTostes/BankingSystemAPI.git
```

Va para o diretório do projeto e execute o arquivo

```bash
 BankingSystemAPI.sln
```




## Configuração do Banco de Dados
Certifique-se de que o SQL Server está rodando e aceitando conexões locais.

Abra o SQL Server Management Studio (SSMS) e execute o seguinte comando para criar o banco de dados:
```bash
  CREATE DATABASE Bank;
```

## Aplicando as Migrações
Execute o código para aplicar as migrações e criar as tabelas
```bash
  dotnet ef database update
```
## Executando o Projeto
Para rodar a aplicação, utilize o seguinte comando na raiz do projeto:
```bash
dotnet run
```
Ou, se estiver utilizando Visual Studio, pressione F5 para iniciar a depuração.
O terminal irá abrir mostrará a URL onde a API está rodando, por exemplo:
```bash
Now listening on: https://localhost:5001
Now listening on: http://localhost:5000
```
Agora você pode acessar https://localhost:5001/index.html para testar a API.
## Integração com API
Abaixo, você encontrará um exemplo de como implementar uma chamada GET em sua API para consumir a API Banking. Detalhes como métodos, tipos de retorno e outras informações estão disponíveis na documentação do Swagger.

- Utilize HttpClient para configurar requisições para a API 
- URL https://localhost:5001/api/[controller]
- Atualmente, nossa API não requer autenticação JWT. A segurança do sistema será garantida por meio da restrição física de acesso à máquina onde ele será executado.

```c#
public class ContaBancaria
{
    public int Id { get; set; }
    public string NomeCliente { get; set; }
    public string Documento { get; set; }
    public double Saldo { get; set; }
    public DateTime DataAbertura { get; set; }
    public bool Ativa { get; set; }
}

public class Program
{
    static async Task Main()
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://localhost:5001/");
        
        try
        {
            var contas = await httpClient.GetFromJsonAsync<List<ContaBancaria>>("api/ContaBancaria");
            
            foreach (var conta in contas)
            {
                Console.WriteLine($"ID: {conta.Id}, Nome: {conta.NomeCliente}, Saldo: {conta.Saldo:C}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter contas: {ex.Message}");
        }
    }
}
```
