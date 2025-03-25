
# 🏦 API Banking

A API Banking é um sistema de caixa de banco que permite o cadastro de contas bancárias e a transferência de valores entre elas. Esta documentação fornece informações detalhadas sobre como rodar localmente a API e também integrar e utilizar a API em seus sistemas.

# 📐 Arquitetura
Esta API segue uma arquitetura em camadas baseada nos princípios do Clean Architecture e DDD (Domain-Driven Design).
O projeto está organizado da seguinte forma:

📦 BankingSystemAPI  
 ┣ 📂 Banking.Application        - Contém os casos de uso e regras de negócio  
 ┣ 📂 Banking.Domain             - Define as entidades, agregados e interfaces do domínio  
 ┣ 📂 Banking.Infrastructure     - Implementa repositórios, serviços externos e banco de dados  
 ┣ 📂 Banking.Shared             - Classes utilitárias e componentes compartilhados  
 ┣ 📂 Banking.Tests              - Testes unitários e de integração  
 ┣ 📂 BankingAPI                 - Camada de apresentação (Controllers, Configurations)  

# ✅ Requisitos
Antes de rodar a API, certifique-se de ter os seguintes requisitos instalados:

✅ .NET 8 SDK

✅ Visual Studio 2022 (17.8 ou superior)

✅ MySQL instalado localmente

✅ DBeaver, MySQL Workbench ou outra ferramenta de sua preferência para gerenciar o banco de dados.

✅ Postman ou outro cliente para testar APIs (opcional

## 📖 Documentação
A API utiliza o Swagger para documentação e, ao executar o projeto, a interface do Swagger será aberta automaticamente.


## 🚀 Rodando o Projeto Localmente

Clonando o repositório

```bash
  git clone https://github.com/JotaTostes/BankingSystemAPI.git
```

Va para o diretório do projeto e execute o arquivo

```bash
 BankingSystemAPI.sln
```

## 🗄️ Configuração do Banco de Dados
1️⃣ Certifique-se de que o MySQÇ está rodando e aceitando conexões locais.

2️⃣ Abra o gerenciador de banco de dados de sua preferência e execute o seguinte comando para criar o banco de dados:
```bash
  CREATE DATABASE Bank;
```

## 🔹Aplicando as Migrações
Para criar as tabelas no banco de dados, execute:
```bash
  dotnet ef database update
```
## ▶️ Executando a API
Para rodar a aplicação, utilize o seguinte comando na raiz do projeto:
```bash
dotnet run
```
Ou, se estiver utilizando Visual Studio, pressione F5 para iniciar a depuração.
pós a inicialização, a API estará disponível em:
```bash
Now listening on: https://localhost:5001
Now listening on: http://localhost:5000
```
Agora você pode acessar https://localhost:5001/index.html para testar a API.

## 🔗 Integração com a API
Abaixo, você encontrará um exemplo de como implementar uma chamada GET em sua API para consumir a API Banking. Detalhes como métodos, tipos de retorno e outras informações estão disponíveis na documentação do Swagger.

🔹 Exemplo de Requisição GET

A API pode ser consumida via HttpClient.

- Base URL: https://localhost:5001/api/[controller]
- Autenticação: Atualmente, a API não requer autenticação JWT. O acesso é controlado fisicamente pela máquina onde a API está sendo executada.

📌 Exemplo de Código C# para Buscar Contas Bancárias
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

## 📌 Contribuindo

Se quiser contribuir com melhorias ou correções, siga estes passos:

- Fork o repositório

- Crie uma branch (git checkout -b feature/nova-feature)

- Faça suas alterações e commite (git commit -m "Descrição da melhoria")

- Faça um push para sua branch (git push origin feature/nova-feature)

- Abra um Pull Request
