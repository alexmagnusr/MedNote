using Globalsys.Model;
using System;
using System.Linq;
using Globalsys.Validacao;
using Globalsys;
using MedNote.Infra;
using MedNote.Infra.Dominio.Seguranca;
using MedNote.Dominio.MedNote;
using LinqKit;
using Globalsys.Exceptions;

namespace MedNote.API.Models.Seguranca
{
    public class SetorUsuarioModel : IModel
    {

        private static SetorUsuarioModel model { get; set; }

        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static SetorUsuarioModel Instancia
        {
            get
            {
                if (model == null)
                    model = new SetorUsuarioModel();

                return model;
            }
        }

        public SetorUsuario Cadastrar(SetorUsuario setorUsuario)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                SetorUsuario setorUsuarioNew = new SetorUsuario();
                setorUsuarioNew.Desativado = false;
                var setor = contexto.ObterPorId<Setor>(setorUsuario.Setor.Codigo);
                setorUsuarioNew.Setor = setor;
                var usuario = contexto.ObterPorId<Usuario>(setorUsuario.Usuario.Codigo);
                setorUsuarioNew.Usuario = usuario;
                setorUsuarioNew.DataDeCadastro = DateTime.Now;

                ValidarCampos(setorUsuarioNew, EstadoObjeto.Novo);
                ValidarRegras(setorUsuarioNew, EstadoObjeto.Novo);
                contexto.Salvar(setorUsuarioNew);

                return setorUsuarioNew;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<SetorUsuario> Consultar(int codigoUsuario)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

                var predicate = PredicateBuilder.New<SetorUsuario>();
                predicate = predicate.And(p => p.Desativado == false && p.Usuario.Codigo == codigoUsuario);

                IQueryable<SetorUsuario> query = contexto.ObterTodos<SetorUsuario>().Where(predicate);

                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public SetorUsuario Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                SetorUsuario setorUsuario = contexto.ObterPorId<SetorUsuario>(codigo);
                setorUsuario.DataDesativacao = DateTime.Now;
                setorUsuario.Desativado = true;
                contexto.BeginTransaction();
                contexto.Atualizar(setorUsuario);
                contexto.Commit();

                return setorUsuario;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }

        }

        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            SetorUsuario item = (SetorUsuario)objeto;
            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                case EstadoObjeto.Alterado:

                    break;

            }
        }

        public void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            SetorUsuario item = (SetorUsuario)objeto;
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                    if (contexto.ObterTodos<SetorUsuario>().Where(t => t.Setor.Codigo == item.Setor.Codigo && t.Usuario.Codigo == item.Usuario.Codigo && !t.Desativado).Any())
                        throw new CoreException("Este setor já está adicionado para o usuário.");
                    break;

            }
        }
    }
}