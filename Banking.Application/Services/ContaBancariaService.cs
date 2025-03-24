using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using Banking.Domain.Interfaces;
using Banking.Shared.Validations;
using FluentValidation;

namespace Banking.Application.Services
{
    public class ContaBancariaService : IContaBancariaService
    {
        private readonly IContaBancariaRepository _repository;
        private readonly IRegistroDesativacaoRepository _registroDesativacaoRepository;
        private readonly IValidator<CriarContaBancariaDto> _validator;
        public ContaBancariaService(IContaBancariaRepository repository, IRegistroDesativacaoRepository registroDesativacaoRepository,
            IValidator<CriarContaBancariaDto> validator)
        {
            _repository = repository;
            _registroDesativacaoRepository = registroDesativacaoRepository;
            _validator = validator;
        }

        public async Task<IEnumerable<ContaBancaria>> ObterTodasContasAsync() =>
            await _repository.GetAllAsync();

        public async Task<(bool Sucesso, List<string> Erros)> CriarContaAsync(CriarContaBancariaDto criarContaBancaria)
        {
            if (await _repository.DocumentoExisteAsync(criarContaBancaria.Documento))
                return (false, new List<string> { "Já existe uma conta bancária cadastrada para este documento." });

            var conta = new ContaBancaria(
                criarContaBancaria.NomeCliente,
                criarContaBancaria.Documento);

            var resultadoValidacao = await _validator.ValidateAsync(criarContaBancaria);
            if (!resultadoValidacao.IsValid)
            {
                return (false, resultadoValidacao.Errors.Select(e => e.ErrorMessage).ToList());
            }

            await _repository.AddAsync(conta);
            await _repository.SalvarAlteracoesAsync();

            return (true, new List<string>());
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
