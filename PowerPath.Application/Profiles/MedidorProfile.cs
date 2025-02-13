using AutoMapper;
using PowerPath.Application.DTO;
using PowerPath.Domain.Entities;

namespace PowerPath.Application.Profiles
{
    public class MedidorProfile : Profile
    {
        public MedidorProfile()
        {
            CreateMap<Medidor, MedidorDTO>();
            CreateMap<MedidorDTO, Medidor>();
        }
    }
}
