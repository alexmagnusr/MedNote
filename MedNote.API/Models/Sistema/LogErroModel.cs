using Globalsys;
using Globalsys.Model;
using MedNote.Infra.Dominio.Sistema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedNote.API.Models.Sistema
{
    public class LogErroModel : IModel
    {
        private static LogErroModel model { get; set; }
        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }
        /// <summary>
        /// Singleton.
        /// </summary>
        ///
        /// <value>
        /// Retorna o(a) Instancia.
        /// </value>
        public static LogErroModel Instancia
        {
            get
            {
                if (model == null)
                    model = new LogErroModel();

                return model;
            }
        }

        public void Cadastrar(LogErro logErro)
        {
            try
            {
                IUnidadeTrabalho contexto = Infra.Fabrica.Instancia.Obter<Globalsys.IUnidadeTrabalho>();
                contexto.Salvar<LogErro>(logErro);
            }
            catch (Exception ex)
            {
            }
        }

        public IQueryable<LogErro> Consultar()
        {
            IUnidadeTrabalho contexto = Infra.Fabrica.Instancia.Obter<Globalsys.IUnidadeTrabalho>();

            IQueryable<LogErro> query = contexto.ObterTodos<LogErro>();
            return query.OrderByDescending(p => p.DataErro);
        }


        public void ValidarRegras(object objeto, Globalsys.Validacao.EstadoObjeto? estadoObjeto = null)
        {
            throw new NotImplementedException();
        }

        public void ValidarCampos(object objeto, Globalsys.Validacao.EstadoObjeto? estadoObjeto = null)
        {
            throw new NotImplementedException();
        }
    }
}