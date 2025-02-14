using PowerPath.Application.DTO;

namespace PowerPath.Application.Interfaces.Services
{
    public interface IMedidorApplicationService
    {
        Resposta<MedidorDTO> Inserir(string? instalacao, int? lote, string? operadora, string? fabricante, int? modelo, int? versao);

        Resposta<List<MedidorDTO>> Inserir(string? caminhoArquivo);

        Resposta<MedidorDTO> Alterar(string? instalacao, int? lote, string? operadora, string? fabricante, int? modelo, int? versao);

        Resposta<MedidorDTO> Excluir(string? instalacao, int? lote);

        Resposta<MedidorDTO> Consultar(string? instalacao, int? lote);

        Resposta<List<MedidorDTO>> Consultar(bool incluirExcluidos = false);

        Resposta<List<string>> ListarOperadoras();
    }
}
