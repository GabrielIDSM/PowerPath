namespace PowerPath.Domain.Interfaces.Repositories.Base
{
    public interface IBaseRepository<Entity> where Entity : class
    {
        void Criar(Entity entity);
    }
}
