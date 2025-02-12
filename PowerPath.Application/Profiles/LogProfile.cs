using System.Globalization;
using AutoMapper;
using PowerPath.Application.DTO;
using PowerPath.Domain.Entities;

namespace PowerPath.Application.Profiles
{
    public class LogProfile : Profile
    {
        public LogProfile()
        {
            CreateMap<Log, LogDTO>()
                .ForMember(d => d.DataHora, opt => opt.MapFrom(o => o.DataHora.ToString("dd/MM/yyyy hh:mm:ss")));

            CreateMap<LogDTO, Log>()
                .ForMember(d => d.DataHora, opt => opt.MapFrom(o => string.IsNullOrEmpty(o.DataHora) ?
                    DateTime.ParseExact(o.DataHora!, "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture) :
                    (DateTime?)null));
        }
    }
}
