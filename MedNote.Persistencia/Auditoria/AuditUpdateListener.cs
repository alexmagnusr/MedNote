using NHibernate;
using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Persistencia.Auditoria
{
    public class AuditUpdateListener : IPostUpdateEventListener
    {
        private const string _noValueString = "*No Value*";

        private static string getStringValueFromStateArray(object[] stateArray, int position)
        {
            var value = stateArray[position];

            return value == null || value.ToString() == string.Empty
                    ? _noValueString
                    : value.ToString();
        }

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            try
            {
                //if (@event.Entity is MedNote.Infra.Dominio.Sistema.Auditoria || @event.Entity is MedNote.Infra.Dominio.Sistema.LogErro)
                //{
                //    return;
                //}

                //var entityFullName = @event.Entity.GetType().FullName;

                //if (@event.OldState == null)
                //{
                //    throw new ArgumentNullException("Nenhum estado antigo disponível para o tipo de entidade '" + entityFullName +
                //                                    "'.Certifique-se de carregá-lo na Sessão antes de modificá-lo e salvá-lo.");
                //}

                //var dirtyFieldIndexes = @event.Persister.FindDirty(@event.State, @event.OldState, @event.Entity, @event.Session);

                //var session = @event.Session.GetSession(EntityMode.Poco);
                //StringBuilder ocorrencia = new StringBuilder();
                //foreach (var dirtyFieldIndex in dirtyFieldIndexes)
                //{
                //    var oldValue = getStringValueFromStateArray(@event.OldState, dirtyFieldIndex);
                //    var newValue = getStringValueFromStateArray(@event.State, dirtyFieldIndex);

                //    if (oldValue == newValue)
                //    {
                //        continue;
                //    }
                //    ocorrencia.AppendLine("CAMPO: " + @event.Persister.PropertyNames[dirtyFieldIndex] + " VALOR ANTIGO: " + oldValue + " VALOR NOVO: " + newValue);

                //}

                //session.Save(new MedNote.Infra.Dominio.Sistema.Auditoria
                //{
                //    NomeEntidade = @event.Entity.GetType().Name,
                //    NomeCompletoEntidade = entityFullName,
                //    Descricao = ocorrencia.ToString(),
                //    Usuario = Auditoria.Model.AuditModel.GetUserLogged(),
                //    CodigoRegistro = @event.Id.ToString(),
                //    Acao = "ATUALIZADO",
                //    DataDeCadastro = DateTime.Now,
                //    IP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString()
                //});

                //session.Flush();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
