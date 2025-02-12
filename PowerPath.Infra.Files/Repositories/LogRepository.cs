using System.Globalization;
using Microsoft.Extensions.Configuration;
using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories;

namespace PowerPath.Infra.Files.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly string _filePath;
        private readonly string _rootPath;

        public LogRepository(IConfiguration configuration)
        {
            _rootPath = configuration["Log:RootPath"]!;
            _filePath = $"{_rootPath}_{DateTime.Now:yyyyMMdd}.txt";

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "");
        }

        public void Criar(Log log)
        {
            AnexarAoArquivo(log);
        }

        public List<Log> ListarPorData(int ano, int mes, int dia)
        {
            DateTime dateTime = new(ano, mes, dia);
            string filePath = $"{_rootPath}_{dateTime:yyyyMMdd}.txt";

            return File.ReadAllLines(filePath)
                .Select(ParaObjeto)
                .ToList();
        }

        public void AnexarAoArquivo(Log log)
        {
            using var sw = File.AppendText(_filePath);
            sw.WriteLine(ParaLinha(log));
        }

        public Log ParaObjeto(string linha)
        {
            string[] partes = linha.Split([" - "], StringSplitOptions.None);

            Log log = new()
            {
                DataHora = DateTime.ParseExact(partes[0], "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture),
                Acao = partes[1],
                Mensagem = partes[2]
            };

            return log;
        }

        public string ParaLinha(Log log)
        {
            return $"{log.DataHora:dd/MM/yyyy hh:mm:ss} - {log.Acao} - {log.Mensagem}";
        }
    }
}
