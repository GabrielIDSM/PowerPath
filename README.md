# Power Path
Power Path é um sistema de automatização de cadastros de medidores de energia

## Console
Os seguintes comandos estão disponíveis na aplicação de console
| Comando    | Descrição               | Argumentos                                              |
|------------|-------------------------|---------------------------------------------------------|
| `i` ou `I` | Inserir novo registro   | Instalação, Lote, Operadora, Fabricante, Modelo, Versão |
| `d` ou `D` | Excluir registro        | Instalação, Lote                                        |
| `e` ou `E` | Alterar registro        | Instalação, Lote, Operadora, Fabricante, Modelo, Versão |
| `m` ou `M` | Inserção massiva        | Instalação, Lote, Operadora, Fabricante, Modelo, Versão |
| `c` ou `C` | Consulta simples        | Instalação, Lote                                        |
| `l` ou `L` | Consulta completa       |                                                         |
| `u` ou `U` | Cadastrar usuário       | Usuário, Senha                                          |
| `x` ou `X` | Sair                    |                                                         |
| `?`        | Ajuda                   |                                                         |

## Autenticação
A autenticação da API e Web é via Bearer Token utilizando JWT, para cadastrar as credenciais é necessário utilizar a aplicação de console

## Base de dados
O sistema utiliza um servidor MySQL Community 8.0.41. As credenciais usadas no ambiente de desenvolvimento estão no arquivo **appsettings.json**. O script de criação da base e das tabelas utilizadas pode ser encontrado na pasta **Scripts** no projeto **PowerPath.Infra.SQL**, mas seu conteúdo está disponível a seguir
~~~~sql
CREATE DATABASE PowerPath;
USE PowerPath;

CREATE TABLE Medidor (
    Instalação VARCHAR(10) NOT NULL,
    Lote INT NOT NULL,
    Operadora VARCHAR(5) NOT NULL,
    Fabricante VARCHAR(15) NOT NULL,
    Modelo INT NOT NULL,
    Versão INT NOT NULL,
	Criacao DATETIME2 NOT NULL,
	Alteracao DATETIME2 NULL,
    Excluido BIT NOT NULL DEFAULT 0,
    PRIMARY KEY (Instalacao, Lote)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_bin;

CREATE TABLE Usuario (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    Senha VARCHAR(255) NOT NULL
) CHARACTER SET utf8mb4 COLLATE utf8mb4_bin;
~~~~

## Variáveis de ambiente
| Variável | Descrição |
|-|-|
| JWT_SECRET_KEY | Chave secreta usada para geração do JWT |
