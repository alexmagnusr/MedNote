using MedNote.Infra;
using MedNote.Repositorios.Seguranca;
using Globalsys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MedNote.Infra.Dominio.Seguranca;

namespace MedNote.API.Models.Sistema
{
    public static class MenuModel
    {

        public static List<Menu> LoadMenu(int? codigo)
        {


            List<Menu> data = new List<Menu>();
            List<Menu> dataResult = new List<Menu>();
            Menu menuHeading = new Menu();
            menuHeading.text = "Módulos";
            menuHeading.heading = true;
            dataResult.Add(menuHeading);
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            IQueryable<Funcao> funcoes = contexto.ObterTodos<Funcao>();
            IRepositorioUsuario repPermissao = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(contexto);
            foreach (KeyValuePair<Funcao, IEnumerable<Acao>> permisao in repPermissao.ObterFuncaoeAcoesPorUsuarioLogado())
            {

                var moduloDaFuncao = funcoes.Where(p => p.Tipo == TipoFuncao.Modulo).FirstOrDefault();

                if (permisao.Key.Pai != null)
                {
                    moduloDaFuncao = funcoes.Where(p => p.Tipo == TipoFuncao.Modulo && p.Codigo == permisao.Key.Pai.Codigo).FirstOrDefault();
                }

                if (!dataResult.Any(x => x.text.Equals(moduloDaFuncao.Descricao)))
                {
                    if (!String.IsNullOrEmpty(permisao.Key.Ref))
                    {
                        Menu pai = new Menu();
                        pai.text = moduloDaFuncao.Descricao;
                        pai.sref = "#";
                        pai.icon = "icon-folder";

                        Menu filha = new Menu();
                        filha.text = permisao.Key.Descricao;
                        filha.sref = permisao.Key.Ref;
                        pai.submenu.Add(filha);
                        dataResult.Add(pai);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(permisao.Key.Ref))
                    {
                        Menu paiSelecionado = dataResult.Where(x => x.text.Equals(moduloDaFuncao.Descricao)).FirstOrDefault();
                        Menu filha = new Menu();
                        filha.text = permisao.Key.Descricao;
                        filha.sref = permisao.Key.Ref;
                        paiSelecionado.submenu.Add(filha);
                    }
                }
            }

            return dataResult;
        }

    }
}