using System.Globalization;
using Microsoft.Extensions.Configuration;
using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories.Medidores;

namespace PowerPath.Infra.Files.Repositories
{
    public class MedidorRepository : IMedidorFileRepository
    {
        private const string SEPARADOR = ";";
        private const string CABECALHO = "Instalacao;Lote;Operadora;Fabricante;Modelo;Versao;Criacao;Alteracao;Excluido";
        private readonly string _caminhoArquivo;

        public MedidorRepository(IConfiguration configuration)
        {
            _caminhoArquivo = configuration["Database:FilePath"]!;

            if (!File.Exists(_caminhoArquivo))
                File.WriteAllText(_caminhoArquivo, $"{CABECALHO}\n");
        }

        public void Atualizar(Medidor medidor)
        {
            List<Medidor> medidores = Listar(true);
            int index = medidores.FindIndex(r => r.Instalacao == medidor.Instalacao && r.Lote == medidor.Lote);
            medidores[index] = medidor;
            Salvar(medidores);
        }

        public void Atualizar(List<Medidor> medidores)
        {
            List<Medidor> todosMedidores = Listar(true);
            medidores.ForEach(m =>
            {
                int index = todosMedidores.FindIndex(r => r.Instalacao == m.Instalacao && r.Lote == m.Lote);
                todosMedidores[index] = m;
            });
            Salvar(todosMedidores);
        }

        public void Criar(Medidor medidor)
        {
            AnexarAoArquivo(medidor);
        }

        public void Criar(List<Medidor> medidores)
        {
            using var sw = File.AppendText(_caminhoArquivo);
            medidores.ForEach(m => sw.WriteLine(ParaLinha(m)));
        }

        public List<Medidor> Listar(bool incluirExcluidos = false)
        {
            return File.ReadAllLines(_caminhoArquivo)
                .Skip(1)
                .Select(ParaObjeto)
                .Where(m => incluirExcluidos || m.Excluido == 0)
                .ToList();
        }

        public Medidor? Obter(string instalacao, int lote)
        {
            return Listar(true).FirstOrDefault(m => m.Instalacao == instalacao && m.Lote == lote);
        }

        public Medidor ParaObjeto(string linha)
        {
            string[]? colunas = linha.Split(SEPARADOR);
            return new Medidor
            {
                Instalacao = colunas[0],
                Lote = int.Parse(colunas[1]),
                Operadora = colunas[2],
                Fabricante = colunas[3],
                Modelo = int.Parse(colunas[4]),
                Versao = int.Parse(colunas[5]),
                Criacao = DateTime.Parse(colunas[6], CultureInfo.InvariantCulture),
                Alteracao = string.IsNullOrEmpty(colunas[7]) ? null : DateTime.Parse(colunas[7], CultureInfo.InvariantCulture),
                Excluido = ulong.Parse(colunas[8])
            };
        }

        public string ParaLinha(Medidor medidor)
        {
            return string.Join(SEPARADOR, [
                medidor.Instalacao,
                medidor.Lote.ToString(),
                medidor.Operadora,
                medidor.Fabricante,
                medidor.Modelo.ToString(),
                medidor.Versao.ToString(),
                medidor.Criacao.ToString("o"),
                medidor.Alteracao?.ToString("o") ?? "",
                medidor.Excluido.ToString()
            ]);
        }

        public void AnexarAoArquivo(Medidor medidor)
        {
            using var sw = File.AppendText(_caminhoArquivo);
            sw.WriteLine(ParaLinha(medidor));
        }

        private void Salvar(List<Medidor> medidores)
        {
            File.WriteAllLines(_caminhoArquivo, new[] { $"{CABECALHO}" }
                .Concat(medidores.Select(ParaLinha)));
        }
    }
}
