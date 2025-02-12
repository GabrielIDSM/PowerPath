using PowerPath.Application.DTO;

namespace PowerPath.Application.Interfaces.Services
{
    public interface IMedidorApplicationService
    {
        public Resposta<MedidorDTO> Inserir(string? instalacao, int? lote, string? operadora, string? fabricante, int? modelo, int? versao);

        public Resposta<List<MedidorDTO>> Inserir(string? caminhoArquivo);

        public Resposta<MedidorDTO> Alterar(string? instalacao, int? lote, string? operadora, string? fabricante, int? modelo, int? versao);

        public Resposta<MedidorDTO> Excluir(string? instalacao, int? lote);

        public Resposta<MedidorDTO> Consultar(string? instalacao, int? lote);

        public Resposta<List<MedidorDTO>> Consultar();
    }
}
