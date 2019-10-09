using Globalsys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Globalsys.Extensoes;
using MedNote.Infra.Dominio.Sistema;

namespace MedNote.API.Models.Sistema
{
    public class AuditoriaModel
    {
        private static AuditoriaModel auditoriaModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static AuditoriaModel Instancia
        {
            get
            {
                if (auditoriaModel == null)
                    auditoriaModel = new AuditoriaModel();

                return auditoriaModel;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public IQueryable<Auditoria> Consultar(string tipo)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;

            IQueryable<Auditoria> query = contexto.ObterTodos<Auditoria>();
            if (!tipo.IsEmpty())
            {
                switch (tipo.ToUpper())
                {
                    case "CRIADO":
                        query = query.Where(p => p.Acao.ToUpper().Equals(tipo.Trim().ToUpper()));
                        break;
                    case "ATUALIZADO":
                        query = query.Where(p => p.Acao.ToUpper().Equals(tipo.Trim().ToUpper()));
                        break;
                    case "DELETADO":
                        query = query.Where(p => p.Acao.ToUpper().Equals(tipo.Trim().ToUpper()));
                        break;
                }
            }

            return query;
        }
    }
}