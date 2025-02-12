using AutoMapper;
using PowerPath.Application.DTO;
using PowerPath.Application.Interfaces.Services;
using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories;
using PowerPath.Domain.Interfaces.Services;

namespace PowerPath.Application.Services
{
    public class LogApplicationService(ILogRepository logRepository,
        ILogService logService,
        IMapper mapper) : ILogApplicationService
    {
        private readonly ILogRepository _logRepository = logRepository;
        private readonly ILogService _logService = logService;
        private readonly IMapper _mapper = mapper;

        public void Criar(string acao, string mensagem)
        {
            Log log = _logService.Criar(acao, mensagem);
            _logRepository.Criar(log);
        }

        public List<LogDTO> ListarPorData(int ano, int mes, int dia)
        {
            List<Log> logs = _logRepository.ListarPorData(ano, mes, dia);
            return _mapper.Map<List<LogDTO>>(logs);
        }
    }
}
