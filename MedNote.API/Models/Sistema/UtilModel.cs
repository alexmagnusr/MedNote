using MedNote.API.Helpers.Globalsys;
using Globalsys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Globalsys.Validacao;
using MedNote.Infra.Dominio.Sistema;
using Globalsys;
using MedNote.Infra;

namespace MedNote.API.Models.Sistema
{
    public class UtilModel : IModel
    {

        private static UtilModel model { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static UtilModel Instancia
        {
            get
            {
                if (model == null)
                    model = new UtilModel();

                return model;
            }
        }
        public  SuccessResult ConsultarCEP(string cep)
        {
            SuccessResult result = new SuccessResult();
            try
            {
                var ws = new WSCorreios.AtendeClienteClient();
                var resposta = ws.consultaCEP(cep);
                result.Success = true;
                result.Data = resposta;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }
        

        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            throw new NotImplementedException();
        }

        public void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            throw new NotImplementedException();
        }
    }
}