using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Dominio.DTOs
{
    public class UsuarioTO
    {
        public virtual int Codigo { get; set; }
        public virtual string Email { get; set; }
        public virtual string Cpf { get; set; }
        public virtual string SenhaAntiga { get; set; }
        public virtual string SenhaNova { get; set; }
        public virtual string ConfirmacaoSenha { get; set; }
    }
}
