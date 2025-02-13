using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories;
using PowerPath.Infra.SQL.Contexts;

namespace PowerPath.Infra.SQL.Repositories
{
    public class MedidorRepository(PowerPathContext context) : IMedidorSQLRepository
    {
        private readonly PowerPathContext _context = context;

        public void Atualizar(Medidor medidor)
        {
            _context.Medidor.Update(medidor);
        }

        public void Atualizar(List<Medidor> medidores)
        {
            _context.Medidor.UpdateRange(medidores);
        }

        public void Criar(Medidor medidor)
        {
            _context.Medidor.Add(medidor);
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

        public void Salvar()
        {
            _context.SaveChanges();
        }
    }
}
