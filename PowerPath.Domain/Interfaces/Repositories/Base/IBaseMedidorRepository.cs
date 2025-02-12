using PowerPath.Domain.Entities;

namespace PowerPath.Domain.Interfaces.Repositories.Base
{
    public interface IBaseMedidorRepository : IBaseRepository<Medidor>
    {
        void Atualizar(Medidor medidor);

        void Atualizar(List<Medidor> medidores);

        void Criar(List<Medidor> medidores);

        List<Medidor> Listar(bool incluirExcluidos = false);

        Medidor? Obter(string instalacao, int lote);
    }
}
