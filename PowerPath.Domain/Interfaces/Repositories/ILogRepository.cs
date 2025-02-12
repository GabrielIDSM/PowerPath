using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories.Base;

namespace PowerPath.Domain.Interfaces.Repositories
{
    public interface ILogRepository : IFileBaseRepository<Log>
    {
        void Criar(Log log);

        List<Log> ListarPorData(int ano, int mes, int dia);
    }
}
