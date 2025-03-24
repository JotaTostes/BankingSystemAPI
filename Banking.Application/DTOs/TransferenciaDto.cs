namespace Banking.Application.DTOs
{
    public class TransferenciaDto
    {
        public string NumeroDocumentoOrigem { get; set; } = string.Empty;
        public string NumeroDocumentoDestino { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}
