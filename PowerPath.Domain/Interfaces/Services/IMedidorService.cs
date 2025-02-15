using PowerPath.Domain.Entities;

namespace PowerPath.Domain.Interfaces.Services
{
    public interface IMedidorService
    {
        void Atualizar(Medidor medidor, string? operadora, string? fabricante, int? modelo, int? versao);

        Medidor Criar(string? instalacao, int? lote, string? operadora, string? fabricante, int? modelo, int? versao);

        void Excluir(Medidor medidor);

        List<string> ListarOperadoras();

        void Validar(string? instalacao, int? lote);

        void Validar(string? instalacao, int? lote, string? operadora, string? fabricante, int? modelo, int? versao);

        bool IsAtivo(Medidor? medidor);

        bool IsExcluido(Medidor? medidor);
    }
}
