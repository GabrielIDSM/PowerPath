using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories.Medidores;
using PowerPath.Infra.SQL.Contexts;
using PowerPath.Infra.SQL.Repositories.Base;

namespace PowerPath.Infra.SQL.Repositories
{
    public class MedidorRepository(PowerPathContext context) : BaseRepository<Medidor>(context), IMedidorSQLRepository
    {
        private readonly PowerPathContext _context = context;

        public void Atualizar(List<Medidor> medidores)
        {
            _context.Medidor.UpdateRange(medidores);
        }

        public void Criar(List<Medidor> medidores)
        {
            _context.Medidor.AddRange(medidores);
        }

        public List<Medidor> Listar(bool incluirExcluidos = false)
        {
            return [.. _context.Medidor.Where(m => m.Excluido == 0 || incluirExcluidos)];
        }

        public Medidor? Obter(string instalacao, int lote)
        {
            return _context.Medidor
                .Where(m => m.Instalacao.Equals(instalacao) && m.Lote == lote)
                .FirstOrDefault();
        }
    }
}
