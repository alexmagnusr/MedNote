using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Globalsys.Exceptions;
using Globalsys.Extensoes;
using NHibernate.Event;
using System.Reflection;
using NHibernate;

namespace MedNote.Persistencia.Auditoria
{
    public class AuditDeleteListener : IPostDeleteEventListener
    {
        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }
        public void OnPostDelete(PostDeleteEvent @event)
        {
            try
            {
                if (@event.Entity is MedNote.Infra.Dominio.Sistema.Auditoria || @event.Entity is MedNote.Infra.Dominio.Sistema.LogErro)
                {
                    return;
                }
                var session = @event.Session.GetSession(EntityMode.Poco);
                var entityFullName = @event.Entity.GetType().FullName;
                StringBuilder propiedades = new StringBuilder();
                foreach (var item in GetProperties(@event.Entity))
                    propiedades.AppendLine("CAMPO: " + item.Name + " VALOR: " + item.GetValue(@event.Entity, null));
                session.Save(new MedNote.Infra.Dominio.Sistema.Auditoria
                {
                    NomeEntidade = @event.Entity.GetType().Name,
                    NomeCompletoEntidade = entityFullName,
                    Descricao = propiedades.ToString(),
                    Usuario = Auditoria.Model.AuditModel.GetUserLogged(),
                    CodigoRegistro = @event.Id.ToString(),
                    Acao = "DELETADO",
                    DataDeCadastro = DateTime.Now,
                    IP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString()
                });
                session.Flush();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
