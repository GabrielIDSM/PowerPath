using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Services;

namespace PowerPath.Domain.Services
{
    public class LogService : ILogService
    {
        public Log Criar(string? acao, string? mensagem)
        {
            if (string.IsNullOrWhiteSpace(acao))
                throw new ArgumentException("O valor do campo \"Ação\" não pode ser nulo ou vazio.");

            if (string.IsNullOrWhiteSpace(mensagem))
                throw new ArgumentException("O valor do campo \"Mensagem\" não pode ser nulo ou vazio.");

            return new()
            {
                Acao = acao,
                Mensagem = mensagem,
                DataHora = DateTime.Now
            };
        }
    }
}
