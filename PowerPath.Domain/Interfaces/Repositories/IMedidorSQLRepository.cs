using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories.Base;

namespace PowerPath.Domain.Interfaces.Repositories
{
    public interface IMedidorSQLRepository : IMedidorBaseRepository
    {
        void Salvar();
    }
}
