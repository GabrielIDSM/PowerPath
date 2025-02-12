using PowerPath.Domain.Entities;

namespace PowerPath.Domain.Interfaces.Repositories.Base
{
    public interface IMedidorBaseRepository
    {
        void Atualizar(Medidor medidor);

        void Criar(Medidor medidor);

        List<Medidor> Listar(bool incluirExcluidos = false);

        Medidor? Obter(string instalacao, int lote);
    }
}
