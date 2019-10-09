using Globalsys;
using Globalsys.Exceptions;
using Globalsys.Validacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globalsys.Extensoes;
using MedNote.Repositorios.Seguranca;
using MedNote.Infra.Dominio.Seguranca;

namespace VIX.Persistencia.Repositorio.Seguranca
{
    public class RepositorioEspecialidade : IRepositorioEspecialidade
    {
        public IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public RepositorioEspecialidade(IUnidadeTrabalho unidadeTrabalho)
        {
            UnidadeTrabalho = unidadeTrabalho;
        }
        
        public Especialidade Cadastrar(Especialidade Especialidade)
        {
            IUnidadeTrabalho contexto = UnidadeTrabalho;

            ValidarCampos(Especialidade, EstadoObjeto.Novo);

            Especialidade.DataCadastro = DateTime.Now;
            contexto.Salvar<Especialidade>(Especialidade);


            return Especialidade;
        }

        public IQueryable<Especialidade> Consultar()
        {
            IUnidadeTrabalho contexto = UnidadeTrabalho.Fabrica.Obter<IUnidadeTrabalho>();
            IQueryable<Especialidade> query = contexto.ObterTodos<Especialidade>().Where(x => x.Desativado == false);
            return query;
        }

        public Especialidade Atualizar(Especialidade Especialidade, int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeTrabalho;
            Especialidade EspecialidadeA = contexto.ObterPorId<Especialidade>(codigo);
            ValidarCampos(Especialidade, EstadoObjeto.Novo);
            EspecialidadeA.Cliente = Especialidade.Cliente;
            EspecialidadeA.Descricao = Especialidade.Descricao;
            EspecialidadeA.Medica = Especialidade.Medica;
            contexto.BeginTransaction();
            contexto.Atualizar<Especialidade>(EspecialidadeA);
            contexto.Commit();

            return contexto.ObterPorId<Especialidade>(codigo);
        }

        public Especialidade Editar(int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeTrabalho;
            return contexto.ObterPorId<Especialidade>(codigo);
        }

        public Especialidade Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeTrabalho;
            Especialidade Especialidade = contexto.ObterPorId<Especialidade>(codigo);
            Especialidade.DataDesativacao = DateTime.Today;
            contexto.BeginTransaction();
            contexto.Atualizar(Especialidade);
            contexto.Commit();

            return Especialidade;
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
