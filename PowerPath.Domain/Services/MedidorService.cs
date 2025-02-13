using PowerPath.Domain.Entities;
using PowerPath.Domain.Enums;
using PowerPath.Domain.Extensions;
using PowerPath.Domain.Interfaces.Services;

namespace PowerPath.Domain.Services
{
    public class MedidorService : IMedidorService
    {
        public void Atualizar(Medidor medidor, string? operadora, string? fabricante, int? modelo, int? versao)
        {
            Validar(medidor.Instalacao, medidor.Lote, operadora, fabricante, modelo, versao);

            medidor.Operadora = operadora!;
            medidor.Fabricante = fabricante!;
            medidor.Modelo = modelo!.Value;
            medidor.Versao = versao!.Value;
            medidor.Alteracao = DateTime.Now;
            medidor.Excluido = 0;
        }

        public Medidor Criar(string? instalacao, int? lote, string? operadora, string? fabricante, int? modelo, int? versao)
        {
            Validar(instalacao, lote, operadora, fabricante, modelo, versao);

            return new()
            {
                Instalacao = instalacao!,
                Lote = lote!.Value,
                Operadora = operadora!,
                Fabricante = fabricante!,
                Modelo = modelo!.Value,
                Versao = versao!.Value,
                Criacao = DateTime.Now,
                Excluido = 0
            };
        }

        public void Excluir(Medidor medidor)
        {
            if (IsExcluido(medidor))
                throw new ArgumentException("O medidor já foi excluído anteriormente.", nameof(medidor));

            medidor.Excluido = 1;
        }

        public bool IsAtivo(Medidor? medidor)
        {
            return medidor is not null && medidor.Excluido == 0;
        }

        public bool IsExcluido(Medidor? medidor)
        {
            return medidor is not null && medidor.Excluido == 1;
        }

        public List<string> ListarOperadoras()
        {
            return EnumExtensions.GetDescriptions<Operadora>();
        }

        public void Validar(string? instalacao, int? lote)
        {
            if (string.IsNullOrWhiteSpace(instalacao))
                throw new ArgumentException("O valor do campo \"Instalação\" não pode ser nulo ou vazio.", nameof(instalacao));

            if (lote is null || lote < 1 || lote > 10)
                throw new ArgumentException("O valor do campo \"Lote\" precisa ser um inteiro de 1 a 10.", nameof(lote));
        }

        public void Validar(string? instalacao, int? lote, string? operadora, string? fabricante, int? modelo, int? versao)
        {
            Validar(instalacao, lote);

            List<string> operadoras = ListarOperadoras();

            if (string.IsNullOrWhiteSpace(operadora) || !operadoras.Contains(operadora!))
                throw new ArgumentException($"O valor do campo \"Operadora\" precisa ser um dos valores a seguir: {string.Join(", ", operadoras)}.", nameof(operadora));

            if (string.IsNullOrWhiteSpace(fabricante))
                throw new ArgumentException("O valor do campo \"Fabricante\" não pode ser nulo ou vazio.", nameof(fabricante));

            if (modelo is null)
                throw new ArgumentException("O valor do campo \"Modelo\" não pode ser nulo.", nameof(modelo));

            if (versao is null)
                throw new ArgumentException("O valor do campo \"Versão\" não pode ser nulo.", nameof(versao));
        }
    }
}
