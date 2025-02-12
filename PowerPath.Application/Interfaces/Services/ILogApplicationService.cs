using PowerPath.Application.DTO;

namespace PowerPath.Application.Interfaces.Services
{
    public interface ILogApplicationService
    {
        void Criar(string acao, string mensagem);

        Resposta<List<LogDTO>> ListarPorData(int ano, int mes, int dia);
    }
}
