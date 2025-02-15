using PowerPath.Domain.Entities;

namespace PowerPath.Domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        Usuario Criar(string? nome, string? senha);

        Usuario CriptografarSenha(Usuario usuario, string senhaCriptografada);

        void Validar(string? nome, string? senha);
    }
}
