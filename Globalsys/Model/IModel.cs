using Globalsys.Validacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalsys.Model
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Criar regras de negocio 
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="estadoObjeto"></param>
        void ValidarRegras(Object objeto, EstadoObjeto? estadoObjeto = null);

        /// <summary>
        /// Validar capos do objeto
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="estadoObjeto"></param>
        void ValidarCampos(Object objeto, EstadoObjeto? estadoObjeto = null);
    }
}
