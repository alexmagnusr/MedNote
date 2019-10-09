using Globalsys.Model;
using System;
using System.Linq;
using Globalsys.Validacao;
using Globalsys;
using MedNote.Infra;
using MedNote.Infra.Dominio.Seguranca;
using LinqKit;
using MedNote.Dominio.MedNote;

namespace MedNote.API.Models.Ative
{
    public class TipoSetorModel : IModel
    {

        private static TipoSetorModel model { get; set; }

        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static TipoSetorModel Instancia
        {
            get
            {
                if (model == null)
                    model = new TipoSetorModel();

                return model;
            }
        }

        public TipoSetor Cadastrar(TipoSetor tipoSetor)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                TipoSetor tipoSetorNew = new TipoSetor();
                tipoSetorNew.Descricao = tipoSetor.Descricao;
                tipoSetorNew.Desativado = tipoSetor.Desativado;
                if(tipoSetor.Estabelecimento != null)
                {
                    var estabelecimento = contexto.ObterPorId<EstabelecimentoSaude>(tipoSetor.Estabelecimento.Codigo);
                    tipoSetorNew.Estabelecimento = estabelecimento;
                }
                
                tipoSetorNew.DataCadastro = DateTime.Now;

                ValidarCampos(tipoSetorNew, EstadoObjeto.Novo);
                contexto.Salvar<TipoSetor>(tipoSetorNew);

                return tipoSetorNew;

            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public IQueryable<TipoSetor> Consultar(bool inativos)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

                //Usuario usuario = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(contexto).ObterUsuarioLogado();

                var predicate = PredicateBuilder.New<TipoSetor>();
                if (inativos)
                {
                    predicate = predicate.And(p => !p.DataDesativacao.HasValue);
                }
                else
                {
                    predicate = predicate.And(p => !p.Desativado);
                }
                    
                //if (!usuario.bl_login_ad)
                //{
                //predicate = predicate.And(p => p.Estabelecimento.Cliente.Codigo == codigo);
                //}

                IQueryable<TipoSetor> query = contexto.ObterTodos<TipoSetor>().Where(predicate);

                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public TipoSetor Atualizar(TipoSetor tipoSetor, int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                contexto.BeginTransaction();

                var tipoSetorOld = contexto.ObterPorId<TipoSetor>(codigo);
                tipoSetorOld.Descricao = tipoSetor.Descricao;
                tipoSetorOld.Desativado = tipoSetor.Desativado;
                if (tipoSetor.Estabelecimento != null)
                {
                    var estabelecimento = contexto.ObterPorId<EstabelecimentoSaude>(tipoSetor.Estabelecimento.Codigo);
                    tipoSetorOld.Estabelecimento = estabelecimento;
                }

                contexto.Atualizar(tipoSetorOld);
                contexto.Commit();
                return tipoSetorOld;

            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }

        }

        public TipoSetor Editar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                return contexto.ObterPorId<TipoSetor>(codigo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TipoSetor Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                TipoSetor tipoSetor = contexto.ObterPorId<TipoSetor>(codigo);
                tipoSetor.DataDesativacao = DateTime.Now;
                tipoSetor.Desativado = true;
                contexto.BeginTransaction();
                contexto.Atualizar(tipoSetor);
                contexto.Commit();

                return tipoSetor;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }

        }

        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            TipoSetor item = (TipoSetor)objeto;
            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                case EstadoObjeto.Alterado:

                    break;

            }
        }

        public void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = null)
        {

        }
    }
}