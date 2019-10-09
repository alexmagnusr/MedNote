using MedNote.Persistencia.Auditoria.Model;
using NHibernate;
using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MedNote.Infra.Dominio.Seguranca;
using MedNote.Infra.Dominio.Sistema;

namespace MedNote.Persistencia.Auditoria
{
    public class AuditCreateListener : IPostInsertEventListener
    {
        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }
        public void OnPostInsert(PostInsertEvent @event)
        {
            try
            {
                //if (@event.Entity is MedNote.Infra.Dominio.Sistema.Auditoria || @event.Entity is LogErro)
                //{
                //    return;
                //}
                //var session = @event.Session.GetSession(EntityMode.Poco);
                //var entityFullName = @event.Entity.GetType().FullName;
                //StringBuilder propiedades = new StringBuilder();
                //foreach (var item in GetProperties(@event.Entity))
                //    propiedades.AppendLine("CAMPO: " + item.Name + " VALOR: " + item.GetValue(@event.Entity, null));
                //session.Save(new MedNote.Infra.Dominio.Sistema.Auditoria
                //{
                //    NomeEntidade = @event.Entity.GetType().Name,
                //    NomeCompletoEntidade = entityFullName,
                //    Descricao = propiedades.ToString(),
                //    Usuario = AuditModel.GetUserLogged(),
                //    CodigoRegistro = @event.Id.ToString(),
                //    Acao = "CRIADO",
                //    DataDeCadastro = DateTime.Now,
                //    IP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString()
                //});
                //session.Flush();
            }
            catch (Exception ex)
            {

            }

        }
    }
}
