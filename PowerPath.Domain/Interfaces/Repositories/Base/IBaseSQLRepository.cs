namespace PowerPath.Domain.Interfaces.Repositories.Base
{
    public interface IBaseSQLRepository<Entity> : IBaseRepository<Entity> where Entity : class
    {
        void Atualizar(Entity entity);

        void Salvar();
    }
}
