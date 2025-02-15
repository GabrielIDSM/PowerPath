using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Facades.Repositories;
using PowerPath.Domain.Interfaces.Repositories.Medidores;

namespace PowerPath.Application.Facades
{
    public class MedidorRepositoryFacade : IMedidorRepositoryFacade
    {
        private readonly IMedidorFileRepository _medidorFileRepository;
        private readonly IMedidorSQLRepository _medidorSQLRepository;

        public MedidorRepositoryFacade(IMedidorFileRepository medidorFileRepository,
            IMedidorSQLRepository medidorSQLRepository)
        {
            _medidorFileRepository = medidorFileRepository;
            _medidorSQLRepository = medidorSQLRepository;
        }

        public void Atualizar(List<Medidor> medidores)
        {
            _medidorFileRepository.Atualizar(medidores);
            _medidorSQLRepository.Atualizar(medidores);
        }

        public void Atualizar(Medidor medidor)
        {
            _medidorFileRepository.Atualizar(medidor);
            _medidorSQLRepository.Atualizar(medidor);
        }

        public void Criar(List<Medidor> medidores)
        {
            _medidorFileRepository.Criar(medidores);
            _medidorSQLRepository.Criar(medidores);
        }

        public void Criar(Medidor medidor)
        {
            _medidorFileRepository.Criar(medidor);
            _medidorSQLRepository.Criar(medidor);
        }

        public List<Medidor> Listar(bool incluirExcluidos = false)
        {
            return _medidorSQLRepository.Listar(incluirExcluidos);
        }

        public Medidor? Obter(string instalacao, int lote)
        {
            return _medidorSQLRepository.Obter(instalacao, lote);
        }

        public void Salvar()
        {
            _medidorSQLRepository.Salvar();
        }
    }
}
