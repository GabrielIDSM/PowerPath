using System.Text;
using ConsoleTableExt;
using PowerPath.Application.Interfaces.Services;

namespace PowerPath.Application.Services
{
    public class ConsoleApplicationService : IConsoleApplicationService
    {
        public string ProcessarComando(string? comando)
        {
            if (string.IsNullOrWhiteSpace(comando) || "?".Equals(comando?.Trim()))
                Ajuda();

            throw new NotImplementedException();
        }

        static void Ajuda()
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

            Console.WriteLine("PowerPath é um sistema de automatização e otimização de registros de medidores de energia. Os seguintes comandos são suportados:");
            Console.WriteLine();
            ImprimirTabela("Lista de comandos", ["Comando", "Descrição", "Argumentos"], tabela);
        }

        static void ImprimirTabela(string titulo, List<string> cabecalho, List<List<object>> tabela)
        {
            ConsoleTableBuilder
                .From(tabela)
                .WithTitle(titulo)
                .WithColumn(cabecalho)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine(TableAligntment.Left);
        }
    }
}
