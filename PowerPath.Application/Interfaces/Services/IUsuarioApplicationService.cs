using PowerPath.Application.DTO;

namespace PowerPath.Application.Interfaces.Services
{
    public interface IUsuarioApplicationService
    {
        Resposta<UsuarioDTO> Criar(string? nome, string? senha);

        Resposta<UsuarioDTO> Autenticar(string? nome, string? senha);
    }
}
