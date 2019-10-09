using Globalsys;
using Globalsys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Globalsys.Validacao;
using Globalsys.Extensoes;
using MedNote.Infra.Dominio.Sistema;
using MedNote.Infra.Dominio.Seguranca;
using MedNote.Infra;
using MedNote.Repositorios.Seguranca;

namespace MedNote.API.Models.Sistema
{
    public class NotificacaoModel : IModel
    {
        private static NotificacaoModel model { get; set; }
        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static NotificacaoModel Instancia
        {
            get
            {
                if (model == null)
                    model = new NotificacaoModel();

                return model;
            }
        }

        public void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = default(EstadoObjeto?))
        {
            throw new NotImplementedException();
        }

        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = default(EstadoObjeto?))
        {
            throw new NotImplementedException();
        }

        public IQueryable<Notificacao> Consultar(bool? visualizado)
        {
            IUnidadeTrabalho unidadeTrabalho = UnidadeDeTrabalho;
            Usuario usuario = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(unidadeTrabalho).ObterUsuarioLogado();
            IQueryable<Notificacao> notificacoes = unidadeTrabalho.ObterTodos<Notificacao>().Where(p => p.Usuario.Codigo == usuario.Codigo);
            if (visualizado.HasValue)
                notificacoes = notificacoes.Where(p => p.Visualizado == (bool)visualizado);

            return notificacoes.OrderByDescending(p => p.DataDeCadastro);
        }

        public int QntNotificacoes(bool? visualizado)
        {
            /*IUnidadeTrabalho unidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            Usuario usuario = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(unidadeTrabalho).ObterUsuarioLogado();
            IQueryable<Notificacao> notificacoes = unidadeTrabalho.ObterTodos<Notificacao>().Where(p => p.Usuario.Codigo == usuario.Codigo);
            if (visualizado.HasValue)
                notificacoes = notificacoes.Where(p => p.Visualizado == (bool)visualizado);
            return notificacoes.Count();*/


            return 0;
        }

        public Notificacao MarcarComoVisualizado(int codigo)
        {
            IUnidadeTrabalho unidadeTrabalho = UnidadeDeTrabalho;
            Usuario usuario = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(unidadeTrabalho).ObterUsuarioLogado();
            unidadeTrabalho.BeginTransaction();
            Notificacao notificacao = unidadeTrabalho.ObterPorId<Notificacao>(codigo);
            notificacao.DataDeVisualizacao = DateTime.Now;
            notificacao.Visualizado = true;
            unidadeTrabalho.Atualizar<Notificacao>(notificacao);
            unidadeTrabalho.Commit();
            return notificacao;
        }
        public Object SetPlayerId(string playerId)
        {
            IUnidadeTrabalho unidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            Usuario usuario = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(unidadeTrabalho).ObterUsuarioLogado();
            if (!usuario.IsEmpty())
            {
                //if (usuario.PlayerId.IsEmpty())
                //{
                    unidadeTrabalho.BeginTransaction();
                    //usuario.PlayerId = playerId;
                    unidadeTrabalho.Atualizar<Usuario>(usuario);
                    unidadeTrabalho.Commit();
                //}
                //else if (!usuario.PlayerId.Trim().Equals(playerId.Trim()))
                //{
                //    unidadeTrabalho.BeginTransaction();
                //    usuario.PlayerId = playerId;
                //    unidadeTrabalho.Atualizar<Usuario>(usuario);
                //    unidadeTrabalho.Commit();
                //}
            }
            return usuario.TryGetValue(v => v.Codigo);
        }
    }
}