using System.Globalization;
using Microsoft.Extensions.Configuration;
using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories;

namespace PowerPath.Infra.Files.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly string _caminhoArquivo;
        private readonly string _caminhoRaiz;

        public LogRepository(IConfiguration configuration)
        {
            _caminhoRaiz = configuration["Log:RootPath"]!;
            _caminhoArquivo = $"{_caminhoRaiz}_{DateTime.Now:yyyyMMdd}.txt";

            if (!File.Exists(_caminhoArquivo))
                File.WriteAllText(_caminhoArquivo, "");
        }

        public void Criar(Log log)
        {
            AnexarAoArquivo(log);
        }

        public List<Log> ListarPorData(int ano, int mes, int dia)
        {
            DateTime dateTime = new(ano, mes, dia);
            string caminhoArquivo = $"{_caminhoRaiz}_{dateTime:yyyyMMdd}.txt";

            if (!File.Exists(caminhoArquivo))
                throw new ArgumentException($"Não existem registros em {dateTime:dd/MM/yyyy}.");

            return File.ReadAllLines(caminhoArquivo)
                .Select(ParaObjeto)
                .ToList();
        }

        public void AnexarAoArquivo(Log log)
        {
            using var sw = File.AppendText(_caminhoArquivo);
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
