using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories.Base;
using PowerPath.Domain.Interfaces.Repositories.Medidores.Base;

namespace PowerPath.Domain.Interfaces.Repositories.Medidores
{
    public interface IMedidorFileRepository : IBaseMedidorRepository, IBaseFileRepository<Medidor>
    {
        void Atualizar(Medidor medidor);
    }
}
