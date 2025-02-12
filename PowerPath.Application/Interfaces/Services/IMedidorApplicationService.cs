using PowerPath.Application.DTO;

namespace PowerPath.Application.Interfaces.Services
{
    public interface IMedidorApplicationService
    {
        public OperacaoResultadoDTO<MedidorDTO> Inserir(string? instalacao, int? lote, string? operadora, string? fabricante, int? modelo, int? versao);

        public OperacaoResultadoDTO<List<MedidorDTO>> Inserir(string? caminhoArquivo);

        public OperacaoResultadoDTO<MedidorDTO> Alterar(string? instalacao, int? lote, string? operadora, string? fabricante, int? modelo, int? versao);

        public OperacaoResultadoDTO<MedidorDTO> Excluir(string? instalacao, int? lote);

        public OperacaoResultadoDTO<MedidorDTO> Consultar(string? instalacao, int? lote);

        public OperacaoResultadoDTO<List<MedidorDTO>> Consultar();
    }
}
