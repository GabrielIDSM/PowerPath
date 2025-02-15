using System.Text.RegularExpressions;
using ConsoleTableExt;
using PowerPath.Application.DTO;
using PowerPath.Application.Interfaces.Services;

namespace PowerPath.Application.Services
{
    public class ConsoleApplicationService(IMedidorApplicationService medidorApplicationService,
        IUsuarioApplicationService usuarioApplicationService) : IConsoleApplicationService
    {
        private static readonly List<string> _cabecalhoMedidor = ["Instalação", "Lote", "Operadora", "Fabricante", "Modelo", "Versão"];
        private readonly IMedidorApplicationService _medidorApplicationService = medidorApplicationService;
        private readonly IUsuarioApplicationService _usuarioApplicationService = usuarioApplicationService;

        public void ProcessarComando(string? comando)
        {
            if (string.IsNullOrWhiteSpace(comando) || "?".Equals(comando?.Trim()))
            {
                Ajuda();
                return;
            }

            Resposta<MedidorDTO> respostaComMedidor;
            Resposta<List<MedidorDTO>> respostaComMedidores;
            Resposta<UsuarioDTO> respostaComUsuario;
            ComandoFormatado comandoFormatado = FormatarComando(comando!);
            int argumentosCount = comandoFormatado.Argumentos?.Count ?? 0;

            switch (comandoFormatado.Operacao)
            {
                case 'i':
                    if (argumentosCount == 6)
                    {
                        int? lote = ParaIntOuNulo(comandoFormatado.Argumentos?[1]);
                        int? modelo = ParaIntOuNulo(comandoFormatado.Argumentos?[4]);
                        int? versao = ParaIntOuNulo(comandoFormatado.Argumentos?[5]);
                        respostaComMedidor = _medidorApplicationService.Inserir(comandoFormatado.Argumentos?[0], lote, comandoFormatado.Argumentos?[2],
                            comandoFormatado.Argumentos?[3], modelo, versao);
                        if (respostaComMedidor.IsSucesso)
                        {
                            MedidorDTO medidor = respostaComMedidor.Resultado!;
                            ImprimirMedidor("Medidor inserido", medidor);
                        }
                        else
                        {
                            Console.WriteLine(respostaComMedidor.Mensagem);
                        }
                    }
                    else
                    {
                        Console.WriteLine("O número de argumentos é inválido.");
                    }
                    break;
                case 'd':
                    if (argumentosCount == 2)
                    {
                        int? lote = ParaIntOuNulo(comandoFormatado.Argumentos?[1]);
                        respostaComMedidor = _medidorApplicationService.Excluir(comandoFormatado.Argumentos?[0], lote);
                        if (respostaComMedidor.IsSucesso)
                        {
                            MedidorDTO medidor = respostaComMedidor.Resultado!;
                            ImprimirMedidor("Medidor excluído", medidor);
                        }
                        else
                        {
                            Console.WriteLine(respostaComMedidor.Mensagem);
                        }
                    }
                    else
                    {
                        Console.WriteLine("O número de argumentos é inválido.");
                    }
                    break;
                case 'e':
                    if (argumentosCount == 6)
                    {
                        int? lote = ParaIntOuNulo(comandoFormatado.Argumentos?[1]);
                        int? modelo = ParaIntOuNulo(comandoFormatado.Argumentos?[4]);
                        int? versao = ParaIntOuNulo(comandoFormatado.Argumentos?[5]);
                        respostaComMedidor = _medidorApplicationService.Alterar(comandoFormatado.Argumentos?[0], lote, comandoFormatado.Argumentos?[2],
                            comandoFormatado.Argumentos?[3], modelo, versao);
                        if (respostaComMedidor.IsSucesso)
                        {
                            MedidorDTO medidor = respostaComMedidor.Resultado!;
                            ImprimirMedidor("Medidor alterado", medidor);
                        }
                        else
                        {
                            Console.WriteLine(respostaComMedidor.Mensagem);
                        }
                    }
                    else
                    {
                        Console.WriteLine("O número de argumentos é inválido.");
                    }
                    break;
                case 'm':
                    if (argumentosCount == 1)
                    {
                        respostaComMedidores = _medidorApplicationService.Inserir(comandoFormatado.Argumentos?[0]);
                        if (respostaComMedidores.IsSucesso)
                        {
                            List<MedidorDTO> medidores = respostaComMedidores.Resultado!;
                            ImprimirMedidores("Lista de medidores inseridos", medidores);
                        }
                        else
                        {
                            Console.WriteLine(respostaComMedidores.Mensagem);
                        }
                    }
                    else
                    {
                        Console.WriteLine("O número de argumentos é inválido.");
                    }
                    break;
                case 'c':
                    if (argumentosCount == 2)
                    {
                        int? lote = ParaIntOuNulo(comandoFormatado.Argumentos?[1]);
                        respostaComMedidor = _medidorApplicationService.Consultar(comandoFormatado.Argumentos?[0], lote);
                        if (respostaComMedidor.IsSucesso)
                        {
                            MedidorDTO medidor = respostaComMedidor.Resultado!;
                            ImprimirMedidor("Medidor consultado", medidor);
                        }
                        else
                        {
                            Console.WriteLine(respostaComMedidor.Mensagem);
                        }
                    }
                    else
                    {
                        Console.WriteLine("O número de argumentos é inválido.");
                    }
                    break;
                case 'l':
                    respostaComMedidores = _medidorApplicationService.Consultar();
                    if (respostaComMedidores.IsSucesso)
                    {
                        List<MedidorDTO> medidores = respostaComMedidores.Resultado!;
                        ImprimirMedidores("Lista de medidores", medidores);
                    }
                    else
                    {
                        Console.WriteLine(respostaComMedidores.Mensagem);
                    }
                    break;
                case 'u':

                    if (argumentosCount == 2)
                    {
                        respostaComUsuario = _usuarioApplicationService.Criar(comandoFormatado.Argumentos?[0], comandoFormatado.Argumentos?[1]);
                        if (respostaComUsuario.IsSucesso)
                        {
                            Console.WriteLine("Usuário criado com sucesso");
                        }
                        else
                        {
                            Console.WriteLine(respostaComUsuario.Mensagem);
                        }
                    }
                    else
                    {
                        Console.WriteLine("O número de argumentos é inválido.");
                    }
                    break;
                default:
                    Console.WriteLine($"\"{comando}\" não é reconhecido como um comando.");
                    break;
            }
        }

        private static void Ajuda()
        {
            List<List<object>> tabela =
                [
                    ["i ou I", "Inserir novo registro", "Instalação, Lote, Operadora, Fabricante, Modelo, Versão"],
                    ["d ou D", "Excluir registro", "Instalação, Lote"],
                    ["e ou E", "Alterar registro", "Instalação, Lote, Operadora, Fabricante, Modelo, Versão"],
                    ["m ou M", "Inserção massiva", "Instalação, Lote, Operadora, Fabricante, Modelo, Versão"],
                    ["c ou C", "Consulta simples", "Instalação, Lote"],
                    ["l ou L", "Consulta completa"],
                    ["u ou U", "Cadastrar usuário", "Usuário, Senha"],
                    ["x ou X", "Sair"],
                    ["?", "Ajuda"]
                ];

            ImprimirTabela("Lista de comandos", ["Comando", "Descrição", "Argumentos"], tabela);
            Console.WriteLine("Exemplos:");
            Console.WriteLine("i \"1234 ABCD\" 1 Claro ABC 1 1");
            Console.WriteLine("c 1234ABCD 1");
        }

        private static int? ParaIntOuNulo(string? valor)
        {
            if (int.TryParse(valor, out int numero))
            {
                return numero;
            }
            return null;
        }

        private static void ImprimirMedidores(string titulo, List<MedidorDTO> medidores)
        {
            List<List<object>> tabela = [];

            foreach (MedidorDTO medidor in medidores)
            {
                tabela.Add([medidor.Instalacao, medidor.Lote, medidor.Operadora, medidor.Fabricante, medidor.Modelo, medidor.Versao]);
            }

            ImprimirTabela(titulo, _cabecalhoMedidor, tabela);
        }

        private static void ImprimirMedidor(string titulo, MedidorDTO medidor)
        {
            List<List<object>> tabela = [];

            tabela.Add([medidor.Instalacao, medidor.Lote, medidor.Operadora, medidor.Fabricante, medidor.Modelo, medidor.Versao]);

            ImprimirTabela(titulo, _cabecalhoMedidor, tabela);
        }

        private static void ImprimirTabela(string titulo, List<string> cabecalho, List<List<object>> tabela)
        {
            ConsoleTableBuilder
                .From(tabela)
                .WithTitle(titulo)
                .WithColumn(cabecalho)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine(TableAligntment.Left);
        }

        private static ComandoFormatado FormatarComando(string comando)
        {
            Match match = Regex.Match(comando, @"^\s*\w\s*(.*)");
            if (!match.Success) return new();

            char operacao = match.Groups[0].Value.ToLower()[0];
            string argumentosComoString = match.Groups[1].Value;
            MatchCollection argumentosMatches = Regex.Matches(argumentosComoString, @"[^\s""]+|""[^""]*""");

            List<string> argumentos = [];
            foreach (Match param in argumentosMatches)
            {
                argumentos.Add(param.Value.Trim('"'));
            }

            return new()
            {
                Operacao = operacao,
                Argumentos = argumentos
            };
        }
    }
}
