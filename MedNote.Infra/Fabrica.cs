using MedNote.Persistencia.Persistencia;
using Globalsys;
using Globalsys.Persistence;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Infra
{
    public class Fabrica : IFabrica
    {
        private static Fabrica fabrica { get; set; }

        //Singleton
        public static Fabrica Instancia
        {
            get
            {
                if (fabrica == null)
                    fabrica = new Fabrica();

                return fabrica;
            }
        }

        public virtual StandardKernel Kernel { get; set; }

        public Fabrica()
        {
            ModuloSistema modulo = new ModuloSistema();
            Kernel = new StandardKernel(modulo);
        }

        public T Obter<T>()
        {
            if (typeof(T) is IUnidadeTrabalho)
            {
                IUnidadeTrabalho unidadeTrabalho = Kernel.Get<IUnidadeTrabalho>();
                unidadeTrabalho.Fabrica = Instancia;

                return (T)unidadeTrabalho;
            }
            else
                return Kernel.Get<T>();
        }

        public T ObterRepositorio<T>(object unidadeTrabalho)
        {
            return Kernel.Get<T>(new ConstructorArgument("unidadeTrabalho", unidadeTrabalho));
        }
    }
}
