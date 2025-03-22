using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Entities
{
    public class Transacao
    {
        public int Id { get; private set; }
        public int ContaOrigemId { get; private set; }
        public int ContaDestinoId { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataTransacao { get; private set; }

        private Transacao() { }

        public Transacao(int contaOrigemId, int contaDestinoId, decimal valor)
        {
            ContaOrigemId = contaOrigemId;
            ContaDestinoId = contaDestinoId;
            Valor = valor;
            DataTransacao = DateTime.UtcNow;
        }
    }
}
