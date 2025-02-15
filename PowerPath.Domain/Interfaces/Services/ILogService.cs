using PowerPath.Domain.Entities;

namespace PowerPath.Domain.Interfaces.Services
{
    public interface ILogService
    {
        Log Criar(string? acao, string? mensagem);
    }
}
