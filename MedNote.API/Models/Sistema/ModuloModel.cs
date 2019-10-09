using MedNote.Infra.Dominio.Seguranca;
using MedNote.Infra;
using MedNote.Repositorios.Seguranca;
using Globalsys;
using Globalsys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedNote.API.Models.Sistema
{
    public class ModuloContract
    {
        public virtual Int32 Codigo { get; set; }

        public virtual String Nome { get; set; }

        public virtual String Descricao { get; set; }

        public virtual String Imagem { get; set; }
    }
    public class ModuloModel : IModel
    {
        private static ModuloModel model { get; set; }
        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static ModuloModel Instancia
        {
            get
            {
                if (model == null)
                    model = new ModuloModel();

                return model;
            }
        }

        public IQueryable<ModuloContract> Consultar()
        {


            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            IRepositorioUsuario repPermissao = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(contexto);
            IEnumerable<Funcao> funcoesPermitidas = repPermissao.ObterFuncaoeAcoesPorUsuarioLogado().Select(x => x.Key);
            int[] idsFuncoesPai = funcoesPermitidas.Select(x => x.Pai.Codigo).ToArray();
            List<ModuloContract> list = new List<ModuloContract>();
            foreach (var item in contexto.ObterTodos<Funcao>().Where(x => idsFuncoesPai.Contains(x.Codigo) && x.Tipo == TipoFuncao.Modulo && x.Icone != null).Distinct().ToList())
                list.Add(new ModuloContract { Codigo = item.Codigo, Descricao = item.Descricao, Nome = item.Nome, Imagem = item.Icone });
            
           
           /* list.Add(new ModuloContract { Codigo = 1, Descricao = "Oil & Gas", Nome = "Oil & Gas", Imagem = "../app/img/botao-oil-gas.png" });
            list.Add(new ModuloContract { Codigo = 2, Descricao = "Pontual", Nome = "Pontual", Imagem = "../app/img/botao-pontual.png" });
            list.Add(new ModuloContract { Codigo = 3, Descricao = "VIX On", Nome = "VIX On", Imagem = "../app/img/botao-vixon.png" });
            */
            return list.AsQueryable();
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