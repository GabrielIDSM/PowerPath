using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories.Base;

namespace PowerPath.Domain.Interfaces.Repositories
{
    public interface ILogRepository : IBaseRepository<Log>, IBaseFileRepository<Log>
    {
        List<Log> ListarPorData(int ano, int mes, int dia);
    }
}
