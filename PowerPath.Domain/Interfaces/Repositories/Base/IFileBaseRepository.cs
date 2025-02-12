using PowerPath.Domain.Entities;

namespace PowerPath.Domain.Interfaces.Repositories.Base
{
    public interface IFileBaseRepository<Entity> where Entity : class
    {
        public Entity ParaObjeto(string linha);

        public string ParaLinha(Entity entity);

        void AnexarAoArquivo(Entity entity);
    }
}
