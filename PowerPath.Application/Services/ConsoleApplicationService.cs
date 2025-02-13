using System.Text.RegularExpressions;
using ConsoleTableExt;
using PowerPath.Application.DTO;
using PowerPath.Application.Interfaces.Services;

namespace PowerPath.Application.Services
{
    public class ConsoleApplicationService(IMedidorApplicationService medidorApplicationService) : IConsoleApplicationService
    {

        private readonly IMedidorApplicationService _medidorApplicationService = medidorApplicationService;

        public void ProcessarComando(string? comando)
        {
            if (string.IsNullOrWhiteSpace(comando) || "?".Equals(comando?.Trim()))
                Ajuda();

            ComandoFormatado comandoFormatado = FormatarComando(comando!);

            switch (comandoFormatado.Operacao)
            {
                case 'i':
                    break;
                case 'd':
                    break;
                case 'e':
                    break;
                case 'm':
                    break;
                case 'c':
                    break;
                case 'l':
                    Resposta<List<MedidorDTO>> resposta = _medidorApplicationService.Consultar();
                    if (resposta.IsSucesso)
                    {
                        List<MedidorDTO> medidores = resposta.Resultado!;
                        List<List<object>> tabela = [];

                        foreach (MedidorDTO medidor in medidores)
                        {
                            tabela.Add([medidor.Instalacao, medidor.Lote, medidor.Operadora, medidor.Fabricante,
                                medidor.Modelo, medidor.Fabricante, medidor.Versao]);
                        }

                        ImprimirTabela("Lista de comandos", ["Instalação", "Lote", "Operadora", "Fabricante", "Modelo", "Versão"], tabela);
                    }
                    else
                    {
                        Console.WriteLine(resposta.Mensagem);
                    }
                    break;
                default:
                    Console.WriteLine($"\"{comando}\" não é reconhecido como um comando, confira os comandos válidos a seguir:");
                    Ajuda();
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

            char operacao = match.Groups[1].Value.ToLower()[0];
            string argumentosComoString = match.Groups[2].Value;
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
