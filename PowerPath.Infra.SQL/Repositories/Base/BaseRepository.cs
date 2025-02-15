using PowerPath.Domain.Interfaces.Repositories.Base;
using PowerPath.Infra.SQL.Contexts;

namespace PowerPath.Infra.SQL.Repositories.Base
{
    public class BaseRepository<Entity>(PowerPathContext context) : IBaseSQLRepository<Entity> where Entity : class
    {
        private readonly PowerPathContext _context = context;

        public void Atualizar(Entity entity)
        {
            _context.Set<Entity>().Update(entity);
        }

        public void Criar(Entity entity)
        {
            _context.Set<Entity>().Add(entity);
        }

        public void Salvar()
        {
            _context.SaveChanges();
        }
    }
}
