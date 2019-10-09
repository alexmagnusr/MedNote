using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalsys.Model
{
    public class ModelBase
    {
        private static ModelBase usuarioModel { get; set; }
        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }
        /// <summary>
        /// Singleton.
        /// </summary>
        ///
        /// <value>
        /// Retorna o(a) Instancia.
        /// </value>
        public static ModelBase Instancia
        {
            get
            {
                if (usuarioModel == null)
                    usuarioModel = new ModelBase();

                return usuarioModel;
            }
        }

    }
}
