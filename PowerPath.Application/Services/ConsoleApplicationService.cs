using System.Text.RegularExpressions;
using ConsoleTableExt;
using PowerPath.Application.DTO;
using PowerPath.Application.Interfaces.Services;

namespace PowerPath.Application.Services
{
    public class ConsoleApplicationService(IMedidorApplicationService medidorApplicationService) : IConsoleApplicationService
    {
        private static readonly List<string> _cabecalhoMedidor = ["Instalação", "Lote", "Operadora", "Fabricante", "Modelo", "Versão"];
        private readonly IMedidorApplicationService _medidorApplicationService = medidorApplicationService;

        public void ProcessarComando(string? comando)
        {
            if (string.IsNullOrWhiteSpace(comando) || "?".Equals(comando?.Trim()))
                Ajuda();

            Resposta<MedidorDTO> resposta;
            Resposta<List<MedidorDTO>> respostaComLista;
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
                        resposta = _medidorApplicationService.Inserir(comandoFormatado.Argumentos?[0], lote, comandoFormatado.Argumentos?[2],
                            comandoFormatado.Argumentos?[3], modelo, versao);
                        if (resposta.IsSucesso)
                        {
                            MedidorDTO medidor = resposta.Resultado!;
                            ImprimirMedidor("Medidor inserido", medidor);
                        }
                        else
                        {
                            Console.WriteLine(resposta.Mensagem);
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
                        resposta = _medidorApplicationService.Excluir(comandoFormatado.Argumentos?[0], lote);
                        if (resposta.IsSucesso)
                        {
                            MedidorDTO medidor = resposta.Resultado!;
                            ImprimirMedidor("Medidor excluído", medidor);
                        }
                        else
                        {
                            Console.WriteLine(resposta.Mensagem);
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
                        resposta = _medidorApplicationService.Alterar(comandoFormatado.Argumentos?[0], lote, comandoFormatado.Argumentos?[2],
                            comandoFormatado.Argumentos?[3], modelo, versao);
                        if (resposta.IsSucesso)
                        {
                            MedidorDTO medidor = resposta.Resultado!;
                            ImprimirMedidor("Medidor alterado", medidor);
                        }
                        else
                        {
                            Console.WriteLine(resposta.Mensagem);
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
                        respostaComLista = _medidorApplicationService.Inserir(comandoFormatado.Argumentos?[0]);
                        if (respostaComLista.IsSucesso)
                        {
                            List<MedidorDTO> medidores = respostaComLista.Resultado!;
                            ImprimirMedidores("Lista de medidores inseridos", medidores);
                        }
                        else
                        {
                            Console.WriteLine(respostaComLista.Mensagem);
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
                        resposta = _medidorApplicationService.Consultar(comandoFormatado.Argumentos?[0], lote);
                        if (resposta.IsSucesso)
                        {
                            MedidorDTO medidor = resposta.Resultado!;
                            ImprimirMedidor("Medidor consultado", medidor);
                        }
                        else
                        {
                            Console.WriteLine(resposta.Mensagem);
                        }
                    }
                    else
                    {
                        Console.WriteLine("O número de argumentos é inválido.");
                    }
                    break;
                case 'l':
                    respostaComLista = _medidorApplicationService.Consultar();
                    if (respostaComLista.IsSucesso)
                    {
                        List<MedidorDTO> medidores = respostaComLista.Resultado!;
                        ImprimirMedidores("Lista de medidores", medidores);
                    }
                    else
                    {
                        Console.WriteLine(respostaComLista.Mensagem);
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
                    ["x ou X", "Sair"],
                    ["?", "Ajuda"]
                ];

            ImprimirTabela("Lista de comandos", ["Comando", "Descrição", "Argumentos"], tabela);
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
                tabela.Add([medidor.Instalacao, medidor.Lote, medidor.Operadora, medidor.Fabricante,
                                medidor.Modelo, medidor.Fabricante, medidor.Versao]);
            }

            ImprimirTabela(titulo, _cabecalhoMedidor, tabela);
        }

        private static void ImprimirMedidor(string titulo, MedidorDTO medidor)
        {
            List<List<object>> tabela = [];

            tabela.Add([medidor.Instalacao, medidor.Lote, medidor.Operadora, medidor.Fabricante,
                medidor.Modelo, medidor.Fabricante, medidor.Versao]);

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
