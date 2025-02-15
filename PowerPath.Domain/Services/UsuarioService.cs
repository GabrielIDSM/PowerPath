using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Services;

namespace PowerPath.Domain.Services
{
    public class UsuarioService : IUsuarioService
    {
        public Usuario Criar(string? nome, string? senha)
        {
            Validar(nome, senha);
            return new()
            {
                Nome = nome!,
                Senha = senha!
            };
        }

        public Usuario CriptografarSenha(Usuario usuario, string senhaCriptografada)
        {
            usuario.Senha = senhaCriptografada;
            return usuario;
        }

        public void Validar(string? nome, string? senha)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O valor do campo \"Nome\" não pode ser nulo ou vazio.");

            if (nome.Length < 6)
                throw new ArgumentException("O valor do campo \"Nome\" não pode possuir menos de 6 caracteres.");

            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("O valor do campo \"Senha\" não pode ser nulo ou vazio.");

            if (senha.Length < 8)
                throw new ArgumentException("O valor do campo \"Senha\" não pode possuir menos de 8 caracteres.");

            if (!senha.Any(char.IsUpper))
                throw new ArgumentException("Ao menos um caractere do campo \"Senha\" precisa ser uma letra maiúscula.");

            if (!senha.Any(char.IsDigit))
                throw new ArgumentException("Ao menos um caractere do campo \"Senha\" precisa ser um número.");
        }
    }
}
