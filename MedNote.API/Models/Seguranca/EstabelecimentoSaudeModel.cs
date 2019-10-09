using Globalsys.Model;
using System;
using System.Linq;
using Globalsys.Validacao;
using Globalsys;
using MedNote.Infra;
using MedNote.Infra.Dominio.Seguranca;
using LinqKit;

namespace MedNote.API.Models.Ative
{
    public class EstabelecimentoSaudeModel : IModel
    {

        private static EstabelecimentoSaudeModel model { get; set; }

        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static EstabelecimentoSaudeModel Instancia
        {
            get
            {
                if (model == null)
                    model = new EstabelecimentoSaudeModel();

                return model;
            }
        }

        public EstabelecimentoSaude Cadastrar(EstabelecimentoSaude estabelecimento)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                EstabelecimentoSaude estabelecimentoNew = new EstabelecimentoSaude();
                estabelecimentoNew.Nome = estabelecimento.Nome;
                estabelecimentoNew.Telefone = estabelecimento.Telefone;
                estabelecimentoNew.Email = estabelecimento.Email;
                estabelecimentoNew.Desativado = false;
                var cliente = contexto.ObterPorId<Cliente>(estabelecimento.Cliente.Codigo);
                estabelecimentoNew.Cliente = cliente;
                estabelecimentoNew.DataDeCadastro = DateTime.Now;

                ValidarCampos(estabelecimentoNew, EstadoObjeto.Novo);
                contexto.Salvar<EstabelecimentoSaude>(estabelecimentoNew);

                return estabelecimentoNew;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<EstabelecimentoSaude> Consultar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
               
                var predicate = PredicateBuilder.New<EstabelecimentoSaude>();
                predicate = predicate.And(p => p.Desativado == false);
                predicate = predicate.And(p => p.Cliente.Codigo == codigo);
            
                IQueryable<EstabelecimentoSaude> query = contexto.ObterTodos<EstabelecimentoSaude>().Where(predicate);

                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }

        public IQueryable<EstabelecimentoSaude> ObterEstabelecimentoPorcliente(int codigoCliente)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                IQueryable<EstabelecimentoSaude> query = contexto.ObterTodos<EstabelecimentoSaude>().Where(x => x.Desativado == false && x.Cliente.Codigo == codigoCliente);

                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public EstabelecimentoSaude Atualizar(EstabelecimentoSaude estabelecimento, int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                contexto.BeginTransaction();

                var especialidadeOld = contexto.ObterPorId<EstabelecimentoSaude>(codigo);
                especialidadeOld.Nome = estabelecimento.Nome;
                especialidadeOld.Telefone = estabelecimento.Telefone;
                especialidadeOld.Email = estabelecimento.Email;
                var cliente = contexto.ObterPorId<Cliente>(estabelecimento.Cliente.Codigo);
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

        public EstabelecimentoSaude Editar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                return contexto.ObterPorId<EstabelecimentoSaude>(codigo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public EstabelecimentoSaude Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                EstabelecimentoSaude estabelecimento = contexto.ObterPorId<EstabelecimentoSaude>(codigo);
                estabelecimento.DataDesativacao = DateTime.Now;
                estabelecimento.Desativado = true;
                contexto.BeginTransaction();
                contexto.Atualizar(estabelecimento);
                contexto.Commit();

                return estabelecimento;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
         
        }

        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            EstabelecimentoSaude item = (EstabelecimentoSaude)objeto;
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