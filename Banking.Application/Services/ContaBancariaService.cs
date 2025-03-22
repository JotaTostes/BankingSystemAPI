using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using Banking.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Banking.Application.Services
{
    public class ContaBancariaService : IContaBancariaService
    {
        private readonly IContaBancariaRepository _repository;
        private readonly IRegistroDesativacaoRepository _registroDesativacaoRepository;
        public ContaBancariaService(IContaBancariaRepository repository, IRegistroDesativacaoRepository registroDesativacaoRepository)
        {
            _repository = repository;
            _registroDesativacaoRepository = registroDesativacaoRepository;
        }

        public async Task<IEnumerable<ContaBancaria>> ObterTodasContasAsync() =>
            await _repository.GetAllAsync();

        public async Task<bool> CriarContaAsync(CriarContaBancariaDto criarContaBancaria)
        {
            if (await _repository.DocumentoExisteAsync(criarContaBancaria.Documento))
                return false;

            var conta = new ContaBancaria(
                criarContaBancaria.NomeCliente,
                criarContaBancaria.Documento);

            await _repository.AddAsync(conta);
            await _repository.SalvarAlteracoesAsync();

            return true;
        }
        public async Task<IEnumerable<ContaBancariaDto>> BuscarContasAsync(string nome, string documento)
        {
            var contas = await _repository.BuscarContasAsync(nome, documento);

            return contas.Select(a => new ContaBancariaDto
            {
                NomeCliente = a.NomeCliente,
                Documento = a.Documento,
                Saldo = a.Saldo,
                DataAbertura = a.DataAbertura,
                Ativo = a.Ativa
            });
        }

        public async Task<bool> DesativarContaAsync(string documento, string usuarioResponsavel)
        {
            var conta = await _repository.GetByDocumentoAsync(documento);

            if (conta == null || !conta.Ativa)
                return false;

            conta.Desativar();
            await _repository.UpdateAsync(conta);

            var registro = new RegistroDesativacaoConta(documento, usuarioResponsavel);
            await _registroDesativacaoRepository.AdicionarAsync(registro);

            await _repository.SalvarAlteracoesAsync();
            await _registroDesativacaoRepository.SalvarAlteracoesAsync();

            return true;
        }
    }
}
