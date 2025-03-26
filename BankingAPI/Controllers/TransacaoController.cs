using Asp.Versioning;
using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TransacaoController : ControllerBase
    {
        private readonly ITransacaoService _service;

        public TransacaoController(ITransacaoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Realiza uma transferência entre contas
        /// </summary>
        /// <param name="transferenciaDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Transferir([FromBody] TransferenciaDto transferenciaDto)
        {
            if (transferenciaDto.Valor <= 0)
                return BadRequest("O valor da transferência deve ser maior que zero.");

            var (sucesso, erros) = await _service.TransferirAsync(transferenciaDto);

            if (!sucesso)
            {
                return BadRequest(erros);
            }

            return Ok("Transferência concluída com sucesso.");
        }
    }
}
