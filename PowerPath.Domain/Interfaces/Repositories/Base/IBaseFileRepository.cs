namespace PowerPath.Domain.Interfaces.Repositories.Base
{
    public interface IBaseFileRepository<Entity> where Entity : class
    {
        public Entity ParaObjeto(string linha);

        public string ParaLinha(Entity entity);

        void AnexarAoArquivo(Entity entity);
    }
}
