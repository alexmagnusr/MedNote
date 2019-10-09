using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Globalsys.MEF.Contratos;

namespace Globalsys.MEF
{
    public sealed class ModuloManager
    {
        private static ModuloManager _instance;
        public static ModuloManager Instance()
        {
            if (_instance == null)
                _instance = new ModuloManager();

            return _instance;
        }


        private ModuloManager() { }

        private AggregateCatalog Catalog { get; set; }
        private CompositionContainer Container { get; set; }

        IEnumerable<Lazy<IPortal>> _portal;
        [ImportMany(typeof(IPortal))]
        public IEnumerable<Lazy<IPortal>> Portal
        {
            get
            {
                if (_portal == null)
                    _portal = new List<Lazy<IPortal>>();

                return _portal;
            }
            set
            {
                _portal = value;
            }
        }
     
        public void Initialize(string pluginPath)
        {
            this.Catalog = new AggregateCatalog(
                    new AssemblyCatalog(typeof(ModuloManager).Assembly)
                    , new DirectoryCatalog(pluginPath, "*.dll")
                );
            this.Container = new CompositionContainer(this.Catalog);
            this.Container.ComposeParts(this);
        }
    }
}
