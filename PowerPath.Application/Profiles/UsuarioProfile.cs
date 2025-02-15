using AutoMapper;
using PowerPath.Application.DTO;
using PowerPath.Domain.Entities;

namespace PowerPath.Application.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<UsuarioDTO, Usuario>();
        }
    }
}
