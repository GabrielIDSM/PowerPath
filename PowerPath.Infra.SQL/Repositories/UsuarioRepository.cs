using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories;
using PowerPath.Infra.SQL.Contexts;
using PowerPath.Infra.SQL.Repositories.Base;

namespace PowerPath.Infra.SQL.Repositories
{
    public class UsuarioRepository(PowerPathContext context) : BaseRepository<Usuario>(context), IUsuarioRepository
    {
        private readonly PowerPathContext _context = context;

        public Usuario? Obter(string? nome)
        {
            return _context.Usuario
                .FirstOrDefault(u => u.Nome.Equals(nome));
        }
    }
}
