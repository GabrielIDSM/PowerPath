﻿using AutoMapper;
using PowerPath.Application.DTO;
using PowerPath.Application.Interfaces.Services;
using PowerPath.Domain.Entities;
using PowerPath.Domain.Interfaces.Facades.Repositories;
using PowerPath.Domain.Interfaces.Services;

namespace PowerPath.Application.Services
{
    public class MedidorApplicationService(IMedidorRepositoryFacade medidorRepository,
        IMedidorService medidorService,
        IMapper mapper) : IMedidorApplicationService
    {
        private const string SEPARADOR = ";";
        private readonly IMedidorRepositoryFacade _medidorRepository = medidorRepository;
        private readonly IMedidorService _medidorService = medidorService;
        private readonly IMapper _mapper = mapper;

        public Resposta<MedidorDTO> Alterar(string? instalacao, int? lote, string? operadora, string? fabricante, int? modelo, int? versao)
        {
            try
            {
                _medidorService.Validar(instalacao, lote);
                Medidor? medidor = _medidorRepository.Obter(instalacao!, lote!.Value);

                if (medidor is not null && medidor.Excluido == 0)
                {
                    _medidorService.Atualizar(medidor, operadora, fabricante, modelo, versao);
                    _medidorRepository.Atualizar(medidor);
                    _medidorRepository.Salvar();
                    return Resposta<MedidorDTO>.Sucesso(_mapper.Map<MedidorDTO>(medidor));
                }
                else
                {
                    return Resposta<MedidorDTO>.Erro($"Medidor não encontrado para Instalação: \"{instalacao}\" e Lote: \"{lote}\".");
                }
            }
            catch (Exception e)
            {
                return Resposta<MedidorDTO>.Erro(e.Message);
            }
        }

        public Resposta<MedidorDTO> Consultar(string? instalacao, int? lote)
        {
            try
            {
                _medidorService.Validar(instalacao, lote);
                Medidor? medidor = _medidorRepository.Obter(instalacao!, lote!.Value);

                if (medidor is not null && medidor.Excluido == 0)
                {
                    return Resposta<MedidorDTO>.Sucesso(_mapper.Map<MedidorDTO>(medidor));
                }
                else
                {
                    return Resposta<MedidorDTO>.Erro($"Medidor não encontrado para Instalação: \"{instalacao}\" e Lote: \"{lote}\".");
                }
            }
            catch (Exception e)
            {
                return Resposta<MedidorDTO>.Erro(e.Message);
            }
        }

        public Resposta<List<MedidorDTO>> Consultar()
        {
            try
            {
                return Resposta<List<MedidorDTO>>.Sucesso(_mapper.Map<List<MedidorDTO>>(_medidorRepository.Listar()));
            }
            catch (Exception e)
            {
                return Resposta<List<MedidorDTO>>.Erro(e.Message);
            }
        }

        public Resposta<MedidorDTO> Excluir(string? instalacao, int? lote)
        {
            try
            {
                _medidorService.Validar(instalacao, lote);
                Medidor? medidor = _medidorRepository.Obter(instalacao!, lote!.Value);

                if (medidor is not null && medidor.Excluido == 0)
                {
                    _medidorService.Excluir(medidor);
                    _medidorRepository.Atualizar(medidor);
                    _medidorRepository.Salvar();
                    return Resposta<MedidorDTO>.Sucesso(_mapper.Map<MedidorDTO>(medidor));
                }
                else
                {
                    return Resposta<MedidorDTO>.Erro($"Medidor não encontrado para Instalação: \"{instalacao}\" e Lote: \"{lote}\".");
                }
            }
            catch (Exception e)
            {
                return Resposta<MedidorDTO>.Erro(e.Message);
            }
        }

        public Resposta<MedidorDTO> Inserir(string? instalacao, int? lote, string? operadora, string? fabricante, int? modelo, int? versao)
        {
            try
            {
                _medidorService.Validar(instalacao, lote);
                Medidor? medidor = _medidorRepository.Obter(instalacao!, lote!.Value);

                if (medidor is null)
                {
                    medidor = _medidorService.Criar(instalacao, lote, operadora, fabricante, modelo, versao);
                    _medidorRepository.Criar(medidor);
                    _medidorRepository.Salvar();
                }
                else if (medidor is not null && medidor.Excluido == 1)
                {
                    _medidorService.Atualizar(medidor, operadora, fabricante, modelo, versao);
                    _medidorRepository.Atualizar(medidor);
                    _medidorRepository.Salvar();
                }
                else
                {
                    return Resposta<MedidorDTO>.Erro($"Medidor não encontrado para Instalação: \"{instalacao}\" e Lote: \"{lote}\".");
                }

                return Resposta<MedidorDTO>.Sucesso(_mapper.Map<MedidorDTO>(medidor));
            }
            catch (Exception e)
            {
                return Resposta<MedidorDTO>.Erro(e.Message);
            }
        }

        public Resposta<List<MedidorDTO>> Inserir(string? caminhoArquivo)
        {
            try
            {
                if (!File.Exists(caminhoArquivo))
                    return Resposta<List<MedidorDTO>>.Erro($"Arquivo não encontrado em: \"{caminhoArquivo}\".");

                List<Medidor> medidores = File.ReadAllLines(caminhoArquivo)
                    .Select(ParaObjeto)
                    .ToList();
                List<Medidor> medidoresCadastrados = _medidorRepository.Listar(true);
                List<Medidor> medidoresParaCriar = [];
                List<Medidor> medidoresParaAtualizar = [];

                foreach (Medidor medidor in medidores)
                {
                    _medidorService.Validar(medidor.Instalacao, medidor.Lote);

                    Medidor? medidorCadastrado = medidoresCadastrados.FirstOrDefault(
                        m =>
                            m.Instalacao.Equals(medidor.Instalacao) &&
                            m.Lote == medidor.Lote
                        );

                    if (medidorCadastrado is null)
                    {
                        Medidor medidorParaCriar = _medidorService.Criar(medidor.Instalacao, medidor.Lote, medidor.Operadora, medidor.Fabricante, medidor.Modelo, medidor.Versao);
                        medidoresParaCriar.Add(medidorParaCriar);
                    }
                    else if (medidorCadastrado is not null && medidorCadastrado.Excluido == 1)
                    {
                        _medidorService.Atualizar(medidorCadastrado, medidor.Operadora, medidor.Fabricante, medidor.Modelo, medidor.Versao);
                        medidoresParaAtualizar.Add(medidorCadastrado);
                    }
                    else
                    {
                        return Resposta<List<MedidorDTO>>.Erro($"Medidor não encontrado para Instalação: \"{medidor.Instalacao}\" e Lote: \"{medidor.Lote}\".");
                    }
                }

                _medidorRepository.Criar(medidoresParaCriar);
                _medidorRepository.Atualizar(medidoresParaAtualizar);
                _medidorRepository.Salvar();

                return Resposta<List<MedidorDTO>>.Sucesso(_mapper.Map<List<MedidorDTO>>(_medidorRepository.Listar()));
            }
            catch (Exception e)
            {
                return Resposta<List<MedidorDTO>>.Erro(e.Message);
            }
        }

        private Medidor ParaObjeto(string linha)
        {
            string[]? colunas = linha.Split(SEPARADOR);
            return new Medidor
            {
                Instalacao = colunas[0],
                Lote = int.Parse(colunas[1]),
                Operadora = colunas[2],
                Fabricante = colunas[3],
                Modelo = int.Parse(colunas[4]),
                Versao = int.Parse(colunas[5])
            };
        }
    }
}
