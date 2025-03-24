using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Entities
{
    public class RegistroDesativacaoConta
    {
        public int Id { get; private set; }
        public string NumeroDocumento { get; private set; } = string.Empty;
        public DateTime DataDesativacao { get; private set; }
        public string UsuarioResponsavel { get; private set; } = string.Empty;

        private RegistroDesativacaoConta() { }

        public RegistroDesativacaoConta(string numeroDocumento, string usuarioResponsavel)
        {
            NumeroDocumento = numeroDocumento;
            DataDesativacao = DateTime.UtcNow;
            UsuarioResponsavel = usuarioResponsavel;
        }
    }
}
