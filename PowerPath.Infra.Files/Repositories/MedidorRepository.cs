using System.Globalization;
using Microsoft.Extensions.Configuration;
using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Repositories;

namespace PowerPath.Infra.Files.Repositories
{
    public class MedidorRepository : IMedidorFileRepository
    {
        private const string SEPARADOR = ";";
        private const string CABECALHO = "Instalacao;Lote;Operadora;Fabricante;Modelo;Versao;Criacao;Alteracao;Excluido";
        private readonly string _filePath;

        public MedidorRepository(IConfiguration configuration)
        {
            _filePath = configuration["Database:FilePath"]!;

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, $"{CABECALHO}\n");
        }

        public void Atualizar(Medidor medidor)
        {
            List<Medidor> medidores = Listar(true);
            int index = medidores.FindIndex(r => r.Instalacao == medidor.Instalacao && r.Lote == medidor.Lote);
            medidores[index] = medidor;
            Salvar(medidores);
        }

        public void Criar(Medidor medidor)
        {
            AnexarAoArquivo(medidor);
        }

        public List<Medidor> Listar(bool incluirExcluidos = false)
        {
            return File.ReadAllLines(_filePath)
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
            string[]? parts = linha.Split(SEPARADOR);
            return new Medidor
            {
                Instalacao = parts[0],
                Lote = int.Parse(parts[1]),
                Operadora = parts[2],
                Fabricante = parts[3],
                Modelo = int.Parse(parts[4]),
                Versao = int.Parse(parts[5]),
                Criacao = DateTime.Parse(parts[6], CultureInfo.InvariantCulture),
                Alteracao = string.IsNullOrEmpty(parts[7]) ? null : DateTime.Parse(parts[7], CultureInfo.InvariantCulture),
                Excluido = ulong.Parse(parts[8])
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
            using var sw = File.AppendText(_filePath);
            sw.WriteLine(ParaLinha(medidor));
        }

        private void Salvar(List<Medidor> medidores)
        {
            File.WriteAllLines(_filePath, new[] { $"{CABECALHO}" }
                .Concat(medidores.Select(ParaLinha)));
        }
    }
}
