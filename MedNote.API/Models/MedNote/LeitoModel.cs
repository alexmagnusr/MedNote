using Globalsys.Model;
using System;
using System.Linq;
using Globalsys.Validacao;
using Globalsys;
using MedNote.Infra;
using LinqKit;
using MedNote.Dominio.MedNote;

namespace MedNote.API.Models.MedNote
{
    public class LeitoModel : IModel
    {

        private static LeitoModel model { get; set; }

        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static LeitoModel Instancia
        {
            get
            {
                if (model == null)
                    model = new LeitoModel();

                return model;
            }
        }

        public Leito Cadastrar(Leito leito)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                Leito leitoNew = new Leito();
                leitoNew.Bl_Liberado = true;
                leitoNew.Identificador = leito.Identificador;
                leitoNew.Desativado = false;
                var setor = contexto.ObterPorId<Setor>(leito.Setor.Codigo);
                leitoNew.Setor = setor;
                leitoNew.DataCadastro = DateTime.Now;

                ValidarCampos(leitoNew, EstadoObjeto.Novo);
                contexto.Salvar<Leito>(leitoNew);

                return leitoNew;

            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public IQueryable<Leito> Consultar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                var predicate = PredicateBuilder.New<Leito>();
                predicate = predicate.And(p => p.Desativado == false);
                predicate = predicate.And(p => p.Setor.Estabelecimento.Cliente.Codigo == codigo);
                IQueryable<Leito> query = contexto.ObterTodos<Leito>().Where(predicate);

                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public Leito Atualizar(Leito leito, int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                contexto.BeginTransaction();

                var leitoOld = contexto.ObterPorId<Leito>(codigo);
                leitoOld.Bl_Liberado = leito.Bl_Liberado;
                var setor = contexto.ObterPorId<Setor>(leito.Setor.Codigo);
                leitoOld.Setor = setor;


                ValidarCampos(leitoOld, EstadoObjeto.Alterado);

                contexto.Atualizar(leitoOld);
                contexto.Commit();
                return leitoOld;

            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }

        }

        public Leito Editar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                return contexto.ObterPorId<Leito>(codigo);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public Leito Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                Leito leito = contexto.ObterPorId<Leito>(codigo);
                leito.DataDesativacao = DateTime.Now;
                leito.Desativado = true;
                contexto.BeginTransaction();
                contexto.Atualizar(leito);
                contexto.Commit();

                return leito;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }

        }

        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            Leito item = (Leito)objeto;
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;

            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                    if (contexto.ObterTodos<Leito>().Any(l => l.Setor.Codigo == item.Setor.Codigo && l.Identificador.Equals(item.Identificador) && l.DataDesativacao == null))
                        throw new Globalsys.Exceptions.CoreException("Já existe um leito com o Identificador \"" + item.Identificador + "\" cadastrado no sistema para este setor.");
                    break;
                case EstadoObjeto.Alterado:
                    if (contexto.ObterTodos<Leito>().Any(l => l.Setor.Codigo == item.Setor.Codigo && l.Codigo != item.Codigo && l.Identificador.Equals(item.Identificador) && l.DataDesativacao == null))
                        throw new Globalsys.Exceptions.CoreException("Já existe um leito com o Identificador \"" + item.Identificador + "\" cadastrado no sistema para este setor.");

                    break;

            }
        }

        public void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = null)
        {

        }
    }
}