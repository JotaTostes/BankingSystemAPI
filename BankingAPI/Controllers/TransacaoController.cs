using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransacaoController : ControllerBase
    {
        private readonly ITransacaoService _service;

        public TransacaoController(ITransacaoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Transferir([FromBody] TransferenciaDto transferenciaDto)
        {
            if (transferenciaDto.Valor <= 0)
                return BadRequest("O valor da transferência deve ser maior que zero");

            var resultado = await _service.TransferirAsync(transferenciaDto);

            if (!resultado)
                return BadRequest("Transferência falhou. Verifique se ambas as contas estão ativas e se a conta de origem tem saldo suficiente.");

            return Ok("Transferência concluída com sucesso");
        }
    }
}
