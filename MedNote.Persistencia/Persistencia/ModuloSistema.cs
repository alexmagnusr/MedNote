using MedNote.Persistencia.Repositorio.Seguranca;
using MedNote.Repositorios.Seguranca;
using Globalsys;
using Globalsys.Repositories;
using Ninject.Modules;
using VIX.Persistencia.Repositorio.Seguranca;

namespace MedNote.Persistencia.Persistencia
{
    
    public class ModuloSistema : NinjectModule
    {
        public override void Load()
        {
            base.Bind<IUnidadeTrabalho>().To<UnidadeTrabalho>();
            base.Bind<IRepositorioGrupo>().To<RepositorioGrupo>();
            base.Bind<IRepositorioUsuario>().To<RepositorioUsuario>();
            base.Bind<IRepositorioCliente>().To<RepositorioCliente>();
            base.Bind<IRepositorioEspecialidade>().To<RepositorioEspecialidade>();
            base.Bind<IRepositorioFuncao>().To<RepositorioFuncao>();
            base.Bind<IRepositorioAcao>().To<RepositorioAcao>();
            base.Bind<IRepositorioPermissao>().To<RepositorioPermissao>();
        }
    }
}
