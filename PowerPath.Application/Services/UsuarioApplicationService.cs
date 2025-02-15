using AutoMapper;
using PowerPath.Application.DTO;
using PowerPath.Application.Interfaces.Services;
using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories;
using PowerPath.Domain.Interfaces.Security;
using PowerPath.Domain.Interfaces.Services;

namespace PowerPath.Application.Services
{
    public class UsuarioApplicationService(ILogApplicationService logAppService,
        ISenhaSecurity senhaSecurity,
        IUsuarioRepository usuarioRepository,
        IUsuarioService usuarioService,
        IMapper mapper) : IUsuarioApplicationService
    {
        private readonly ILogApplicationService _logAppService = logAppService;
        private readonly ISenhaSecurity _senhaSecurity = senhaSecurity;
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
        private readonly IUsuarioService _usuarioService = usuarioService;
        private readonly IMapper _mapper = mapper;

        public Resposta<UsuarioDTO> Autenticar(string? nome, string? senha)
        {
            try
            {
                _logAppService.Criar("Autenticação", $"Tentativa de autenticação com Usuário: \"{nome}\".");
                _usuarioService.Validar(nome, senha);

                Usuario? usuario = _usuarioRepository.Obter(nome);
                if (usuario is not null && _senhaSecurity.SenhaIsSenhaCriptografada(senha!, usuario.Senha))
                {
                    _logAppService.Criar("Autenticação", $"Sessão iniciada com Usuário: \"{nome}\".");
                    return Resposta<UsuarioDTO>.Sucesso(_mapper.Map<UsuarioDTO>(usuario));
                }
                else
                {
                    return Resposta<UsuarioDTO>.Erro("Usuário e/ou senha incorretos.");
                }
            }
            catch (Exception e)
            {
                return Resposta<UsuarioDTO>.Erro(e.Message);
            }
        }

        public Resposta<UsuarioDTO> Criar(string? nome, string? senha)
        {
            try
            {
                _logAppService.Criar("Criação de usuário", $"Tentativa de criação do Usuário: \"{nome}\".");
                _usuarioService.Validar(nome, senha);

                Usuario? usuarioExistente = _usuarioRepository.Obter(nome);
                if (usuarioExistente is null)
                {
                    Usuario? usuario = _usuarioService.Criar(nome, senha);
                    string senhaCriptografada = _senhaSecurity.GerarSenhaCriptografada(senha!);
                    _usuarioService.CriptografarSenha(usuario, senhaCriptografada);
                    _usuarioRepository.Criar(usuario);
                    _usuarioRepository.Salvar();

                    _logAppService.Criar("Criação de usuário", $"Usuário: \"{nome}\" criado.");
                    return Resposta<UsuarioDTO>.Sucesso(_mapper.Map<UsuarioDTO>(usuario));
                }
                else
                {
                    return Resposta<UsuarioDTO>.Erro("Já existe um Usuário: \"{nome}\".");
                }
            }
            catch (Exception e)
            {
                return Resposta<UsuarioDTO>.Erro(e.Message);
            }
        }
    }
}
