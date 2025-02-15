using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories.Base;

namespace PowerPath.Domain.Interfaces.Repositories.Medidores.Base
{
    public interface IBaseMedidorRepository : IBaseRepository<Medidor>
    {
        void Atualizar(List<Medidor> medidores);

        void Criar(List<Medidor> medidores);

        List<Medidor> Listar(bool incluirExcluidos = false);

        Medidor? Obter(string instalacao, int lote);
    }
}
