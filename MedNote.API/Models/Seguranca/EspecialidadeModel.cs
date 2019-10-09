using Globalsys.Model;
using System;
using System.Linq;
using Globalsys.Validacao;
using Globalsys;
using MedNote.Infra;
using MedNote.Infra.Dominio.Seguranca;
using LinqKit;
using MedNote.Repositorios.Seguranca;

namespace MedNote.API.Models.Ative
{
    public class EspecialidadeModel : IModel
    {

        private static EspecialidadeModel model { get; set; }

        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static EspecialidadeModel Instancia
        {
            get
            {
                if (model == null)
                    model = new EspecialidadeModel();

                return model;
            }
        }

        public Especialidade Cadastrar(Especialidade especialidade)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                Especialidade especialidadeNew = new Especialidade();
                especialidadeNew.Descricao = especialidade.Descricao;
                especialidadeNew.Medica = especialidade.Medica;
                especialidadeNew.Desativado = false;
                var cliente = contexto.ObterPorId<Cliente>(especialidade.Cliente.Codigo);
                especialidadeNew.Cliente = cliente;
                especialidadeNew.DataCadastro = DateTime.Now;

                ValidarCampos(especialidadeNew, EstadoObjeto.Novo);
                contexto.Salvar<Especialidade>(especialidadeNew);

                return especialidadeNew;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<Especialidade> Consultar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

                //Usuario usuario = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(contexto).ObterUsuarioLogado();

                var predicate = PredicateBuilder.New<Especialidade>();
                predicate = predicate.And(p => p.Desativado == false);
                //if (!usuario.bl_login_ad)
                //{
                    //predicate = predicate.And(p => p.Cliente.Codigo == codigo);
                //}
       
                IQueryable<Especialidade> query = contexto.ObterTodos<Especialidade>().Where(predicate);

                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }

        public Especialidade Atualizar(Especialidade especialidade, int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                contexto.BeginTransaction();

                var especialidadeOld = contexto.ObterPorId<Especialidade>(codigo);
                especialidadeOld.Descricao = especialidade.Descricao;
                especialidadeOld.Medica = especialidade.Medica;
                var cliente = contexto.ObterPorId<Cliente>(especialidade.Cliente.Codigo);
                especialidadeOld.Cliente = cliente;
                
                contexto.Atualizar(especialidadeOld);
                contexto.Commit();
                return especialidadeOld;
                
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
          
        }

        public Especialidade Editar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                return contexto.ObterPorId<Especialidade>(codigo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public Especialidade Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                Especialidade especialidade = contexto.ObterPorId<Especialidade>(codigo);
                especialidade.DataDesativacao = DateTime.Now;
                especialidade.Desativado = true;
                contexto.BeginTransaction();
                contexto.Atualizar(especialidade);
                contexto.Commit();

                return especialidade;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
         
        }

        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            Especialidade item = (Especialidade)objeto;
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