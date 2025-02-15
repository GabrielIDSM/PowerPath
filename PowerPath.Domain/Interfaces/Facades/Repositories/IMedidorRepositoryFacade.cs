using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories.Base;
using PowerPath.Domain.Interfaces.Repositories.Medidores.Base;

namespace PowerPath.Domain.Interfaces.Facades.Repositories
{
    public interface IMedidorRepositoryFacade : IBaseMedidorRepository, IBaseSQLRepository<Medidor>
    {
    }
}
