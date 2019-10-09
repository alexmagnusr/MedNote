using MedNote.Infra.Dominio.Seguranca;
using MedNote.Persistencia.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globalsys.Exceptions;
using Globalsys.Extensoes;

namespace MedNote.Persistencia.Auditoria.Model
{
    public class AuditModel
    {
        public static Usuario GetUserLogged()
        {
            UnidadeTrabalho contexto = new UnidadeTrabalho();
            string usuarioLogado = System.Web.HttpContext.Current.TryGetValue(c => c.User.Identity.Name);
            return contexto.ObterTodos<Usuario>().Where(t => t.Login.ToLower().Equals(usuarioLogado.ToLower())).FirstOrDefault();
        }
    }
}
