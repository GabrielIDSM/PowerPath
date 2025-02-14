using System.Globalization;
using AutoMapper;
using PowerPath.Application.DTO;
using PowerPath.Domain.Entities;

namespace PowerPath.Application.Profiles
{
    public class MedidorProfile : Profile
    {
        public MedidorProfile()
        {
            CreateMap<Medidor, MedidorDTO>()
                .ForMember(d => d.Excluido, opt => opt.MapFrom(o => o.Excluido == 1))
                .ForMember(d => d.Criacao, opt => opt.MapFrom(o => o.Criacao.ToString("dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture)))
                .ForMember(d => d.Alteracao, opt => opt.MapFrom(o => o.Alteracao != null ?
                    o.Alteracao!.Value.ToString("dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture) :
                    string.Empty));

            CreateMap<MedidorDTO, Medidor>()
                .ForMember(d => d.Excluido, opt => opt.MapFrom(o => o.Excluido.HasValue && o.Excluido.Value ? 1 : 0))
                .ForMember(d => d.Criacao, opt => opt.MapFrom(o => string.IsNullOrEmpty(o.Criacao) ?
                    DateTime.ParseExact(o.Criacao!, "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture) :
                    (DateTime?)null))
                .ForMember(d => d.Alteracao, opt => opt.MapFrom(o => string.IsNullOrEmpty(o.Alteracao) ?
                    DateTime.ParseExact(o.Alteracao!, "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture) :
                    (DateTime?)null));
        }
    }
}
