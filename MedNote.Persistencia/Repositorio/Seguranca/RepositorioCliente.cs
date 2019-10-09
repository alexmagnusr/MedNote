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
    public class RepositorioCliente : IRepositorioCliente
    {
        public IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public RepositorioCliente(IUnidadeTrabalho unidadeTrabalho)
        {
            UnidadeTrabalho = unidadeTrabalho;
        }

        
        public Cliente Cadastrar(Cliente Cliente)
        {
            IUnidadeTrabalho contexto = UnidadeTrabalho;

            ValidarCampos(Cliente, EstadoObjeto.Novo);

            Cliente.DataDeCadastro = DateTime.Now;
            contexto.Salvar<Cliente>(Cliente);


            return Cliente;
        }

        public IQueryable<Cliente> Consultar()
        {
            IUnidadeTrabalho contexto = UnidadeTrabalho.Fabrica.Obter<IUnidadeTrabalho>();
            IQueryable<Cliente> query = contexto.ObterTodos<Cliente>().Where(x => x.Desativado == false);

            //Usuario usuario = UnidadeTrabalho.Fabrica.ObterRepositorio<IRepositorioCliente>(contexto).ObterUsuarioLogado();
            //int[] ClientesUsuario = usuario.Contratos.Select(x => x.Contrato.Cliente.Codigo).Distinct().ToArray();

            //if (usuario.Colaborador.Tipo != TipoColaborador.Administrador)
            //    query = query.Where(x => ClientesUsuario.Contains(x.Codigo));

            return query;
        }
        public Cliente Atualizar(Cliente Cliente, int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeTrabalho;
            Cliente ClienteA = contexto.ObterPorId<Cliente>(codigo);
            ValidarCampos(Cliente, EstadoObjeto.Novo);
            //ClienteA.Bairro = Cliente.Bairro;
            //ClienteA.Cidade = Cliente.Cidade;
            ClienteA.NumDocumento = Cliente.NumDocumento;
            //ClienteA.Logradouro = Cliente.Logradouro;
            ClienteA.Nome = Cliente.Nome;
            //ClienteA.Numero = Cliente.Numero;
            //ClienteA.InscricaoEstatudal = Cliente.InscricaoEstatudal;
            //ClienteA.Referencia = Cliente.Referencia;
            //ClienteA.Telefone = Cliente.Telefone;
            //ClienteA.Matriz = Cliente.Matriz;
            //ClienteA.Cep = Cliente.Cep;

            contexto.BeginTransaction();
            contexto.Atualizar<Cliente>(ClienteA);
            contexto.Commit();

            return contexto.ObterPorId<Cliente>(codigo);
        }

        public Cliente Editar(int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeTrabalho;
            return contexto.ObterPorId<Cliente>(codigo);
        }

        public Cliente Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeTrabalho;
            Cliente Cliente = contexto.ObterPorId<Cliente>(codigo);
            Cliente.DataDesativacao = DateTime.Today;
            contexto.BeginTransaction();
            contexto.Atualizar(Cliente);
            contexto.Commit();

            return Cliente;
        }
        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            Cliente item = (Cliente)objeto;
            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                case EstadoObjeto.Alterado:

                    if (!item.NumDocumento.IsValidCNPJ())
                        throw new CoreException("CNPJ informado já inválido inválido.");

                    break;

            }
        }

        public void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = null)
        {

        }

    }
}
