
# API Banking

Api do Sistema de Caixa de Banco, permitindo o cadastro de contas e a transferência de valores.


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
