using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Entities
{
    public class ContaBancaria
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public DateTime DataAbertura { get; set; }
        public bool Ativa { get; set; } = true;

        private ContaBancaria() { }

        public ContaBancaria(string nomeCliente, string numeroDocumento)
        {
            NomeCliente = nomeCliente;
            Documento = numeroDocumento;
            Saldo = 1000;
            DataAbertura = DateTime.UtcNow;
            Ativa = true;
        }

        public void Desativar()
        {
            Ativa = false;
        }

        public bool PodeSacar(decimal valor)
        {
            return Ativa && Saldo >= valor;
        }

        public void Sacar(decimal valor)
        {
            if (!PodeSacar(valor))
                throw new InvalidOperationException("Não é possível sacar: saldo insuficiente ou conta inativa");

            Saldo -= valor;
        }

        public void Depositar(decimal valor)
        {
            if (!Ativa)
                throw new InvalidOperationException("Não é possível depositar em uma conta inativa");

            Saldo += valor;
        }
    }
}
