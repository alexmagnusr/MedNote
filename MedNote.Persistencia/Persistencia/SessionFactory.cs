using MedNote.Persistencia.Auditoria;
using MedNote.Persistencia.Mapeamento.Seguranca;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;
using System;

namespace MedNote.Persistencia.Persistencia
{
    public class SessionFactory
    {
        private static SessionFactory fInstancia;

        public static SessionFactory Instancia
        {
            get
            {
                if (fInstancia == null)
                    fInstancia = new SessionFactory();

                return fInstancia;
            }
        }

        private ISessionFactory ISessionFactory { get; set; }

        public SessionFactory()
        {
            try
            {
                FluentConfiguration config = obterConfiguracaoFluent();
                this.ISessionFactory = config.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private static FluentConfiguration obterConfiguracaoFluent()
        {
            /* return Fluently.Configure()
                            .Database(MsSqlConfiguration
                                         .MsSql2012
                                         .ConnectionString(c => c.FromConnectionStringWithKey("conexao")))*/
            return Fluently.Configure()
                           .Database(MsSqlConfiguration
                                        .MsSql2008
                                        .ConnectionString(c => c.FromConnectionStringWithKey("conexao")))
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UsuarioMap>()).ExposeConfiguration(AddAuditor);
        }

        public static void GerarBanco()
        {
            obterConfiguracaoFluent().ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true)).BuildSessionFactory();
        }
        private static void AddAuditor(Configuration config)
        {
          config.SetListener(ListenerType.PostUpdate, new AuditUpdateListener());
          config.SetListener(ListenerType.PostInsert, new AuditCreateListener());
          config.SetListener(ListenerType.PostDelete, new AuditDeleteListener());

        }
        public ISession ObterSessao()
        {
            return this.ISessionFactory.OpenSession();
        }
    }
}
