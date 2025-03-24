using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class ContaBancariaDto
    {
        public string NomeCliente { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public DateTime DataAbertura { get; set; }
        public bool Ativo { get; set; }
    }
}
