using Globalsys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Globalsys.Validacao;
using Globalsys;
using Globalsys.Extensoes;
using Globalsys.Exceptions;
using MedNote.Infra;
using MedNote.Infra.Dominio.Seguranca;
using MedNote.Dominio.DTOs;
using MedNote.Dominio.MedNote;

namespace MedNote.API.Models.Ative
{
    public class ClienteModel : IModel
    {

        private static ClienteModel model { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static ClienteModel Instancia
        {
            get
            {
                if (model == null)
                    model = new ClienteModel();

                return model;
            }
        }

        public ClienteDTO Cadastrar(ClienteDTO cliente)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

            try
            {
                contexto.BeginTransaction();
                Cliente clienteNew = new Cliente();
                clienteNew.Nome = cliente.Nome;
                clienteNew.NumDocumento = cliente.NumDocumento;
                clienteNew.DataInicioContrato = cliente.DataInicioContrato;
                clienteNew.DataTerminoContrato = cliente.DataTerminoContrato;
                clienteNew.DiaVencimento = cliente.DiaVencimento.ToString();
                clienteNew.Email = cliente.Email;
                clienteNew.NomeGestorContrato = cliente.NomeGestorContrato;
                clienteNew.ValorContrato = cliente.ValorContrato;
                clienteNew.Telefone = cliente.Telefone;
                clienteNew.DataDeCadastro = DateTime.Now;
                clienteNew.Desativado = false;

                ValidarCampos(clienteNew, EstadoObjeto.Novo);
                contexto.Salvar<Cliente>(clienteNew);
                

                //var ultimoCadastro = contexto.ObterTodos<Cliente>().Where(x => x.Nome == clienteNew.Nome).FirstOrDefault();
                for (int i = 0; i < cliente.Estabelecimento.Count; i++)
                {
                    EstabelecimentoSaude estabalecimento = new EstabelecimentoSaude();
                    estabalecimento.Cliente = new Cliente { Codigo = clienteNew.Codigo };
                    estabalecimento.DataDeCadastro = DateTime.Now;
                    estabalecimento.Nome = cliente.Estabelecimento[i].Nome;
                    estabalecimento.Telefone = cliente.Estabelecimento[i].Telefone;
                    estabalecimento.Email = cliente.Estabelecimento[i].Email;
                    contexto.Salvar<EstabelecimentoSaude>(estabalecimento);
                }

                

                ClienteDTO retorno = new ClienteDTO();
                retorno.Codigo = clienteNew.Codigo;
                retorno.DataDeCadastro = clienteNew.DataDeCadastro;
                retorno.DataDesativacao = clienteNew.DataDesativacao;
                retorno.DataInicioContrato = clienteNew.DataInicioContrato;
                retorno.DataTerminoContrato = clienteNew.DataTerminoContrato;
                retorno.Desativado = clienteNew.Desativado;
                retorno.DiaVencimento = clienteNew.DiaVencimento;
                retorno.Email = clienteNew.Email;
                retorno.Nome = clienteNew.Nome;
                retorno.NomeGestorContrato = clienteNew.NomeGestorContrato;
                retorno.ValorContrato = clienteNew.ValorContrato;
                retorno.NumDocumento = clienteNew.NumDocumento;
                retorno.Telefone = clienteNew.Telefone;
                retorno.Estabelecimento = contexto.ObterTodos<EstabelecimentoSaude>().Where(y => y.Cliente.Codigo == cliente.Codigo && y.Desativado == false).ToList();

                contexto.Commit();

                return retorno;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }

        }

        public IList<ClienteDTO> Consultar()
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                IQueryable<Cliente> query = contexto.ObterTodos<Cliente>().Where(x => x.Desativado == false).OrderBy(c => c.Nome);
                var clientes = query.ToList();
                var listaClientes = (from cliente in query
                                          
                                         select new ClienteDTO()
                                         {
                                             Codigo = cliente.Codigo,
                                             DataDeCadastro = cliente.DataDeCadastro,
                                             DataDesativacao = cliente.DataDesativacao,
                                             DataInicioContrato = cliente.DataInicioContrato,
                                             DataTerminoContrato = cliente.DataTerminoContrato,
                                             Desativado = cliente.Desativado,
                                             DiaVencimento = cliente.DiaVencimento,
                                             Email = cliente.Email,
                                             Nome = cliente.Nome,
                                             NomeGestorContrato = cliente.NomeGestorContrato,
                                             ValorContrato = cliente.ValorContrato,
                                             NumDocumento = cliente.NumDocumento,
                                             Telefone = cliente.Telefone,
                                             Estabelecimento = null
                                            
                                           }).ToList();
                
                return listaClientes;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public ClienteDTO Atualizar(ClienteDTO cliente, int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

            try
            {
                contexto.BeginTransaction();
                Cliente ClienteA = contexto.ObterPorId<Cliente>(codigo);
                ClienteA.Nome = cliente.Nome;
                ClienteA.NumDocumento = cliente.NumDocumento;
                ClienteA.DataInicioContrato = cliente.DataInicioContrato;
                ClienteA.DataTerminoContrato = cliente.DataTerminoContrato;
                ClienteA.DiaVencimento = cliente.DiaVencimento;
                ClienteA.Email = cliente.Email;
                ClienteA.NomeGestorContrato = cliente.NomeGestorContrato;
                ClienteA.ValorContrato = cliente.ValorContrato;
                ClienteA.Telefone = cliente.Telefone;
                //var lista = contexto.ObterTodos<EstabelecimentoSaude>().Where(x => x.Cliente.Codigo == codigo && x.Desativado == false).ToList();

                for (int i = 0; i < cliente.Estabelecimento.Count; i++)
                {

                    if (!cliente.Estabelecimento[i].Editar)
                    {
                        if (cliente.Estabelecimento[i].Codigo == 0)
                        {
                            EstabelecimentoSaude estabalecimento = new EstabelecimentoSaude();
                            estabalecimento.Cliente = ClienteA;
                            estabalecimento.DataDeCadastro = DateTime.Now;
                            estabalecimento.Nome = cliente.Estabelecimento[i].Nome;
                            estabalecimento.Telefone = cliente.Estabelecimento[i].Telefone;
                            estabalecimento.Email = cliente.Estabelecimento[i].Email;
                            contexto.Salvar<EstabelecimentoSaude>(estabalecimento);
                        }
                       
                    }
                    else if (cliente.Estabelecimento[i].Editar)
                    {
                        if (cliente.Estabelecimento[i].Codigo != 0)
                        {
                            EstabelecimentoSaude estabalecimento = contexto.ObterPorId<EstabelecimentoSaude>(cliente.Estabelecimento[i].Codigo);
                            estabalecimento.Nome = cliente.Estabelecimento[i].Nome;
                            estabalecimento.Telefone = cliente.Estabelecimento[i].Telefone;
                            estabalecimento.Email = cliente.Estabelecimento[i].Email;
                            contexto.Atualizar<EstabelecimentoSaude>(estabalecimento);
                        }
                        else
                        {
                            EstabelecimentoSaude estabalecimento = new EstabelecimentoSaude();
                            estabalecimento.Cliente = ClienteA;
                            estabalecimento.DataDeCadastro = DateTime.Now;
                            estabalecimento.Nome = cliente.Estabelecimento[i].Nome;
                            estabalecimento.Telefone = cliente.Estabelecimento[i].Telefone;
                            estabalecimento.Email = cliente.Estabelecimento[i].Email;
                            contexto.Salvar<EstabelecimentoSaude>(estabalecimento);
                        }
                    }

                }


                ValidarCampos(ClienteA, EstadoObjeto.Alterado);

                contexto.BeginTransaction();
                contexto.Atualizar<Cliente>(ClienteA);
                contexto.Commit();

                ClienteDTO clienteDTO = new ClienteDTO();
                clienteDTO = cliente;
                return clienteDTO;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }



        }

        public ClienteDTO Editar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                var cliente = contexto.ObterPorId<Cliente>(codigo);

                ClienteDTO retorno = new ClienteDTO();

                retorno.Codigo = cliente.Codigo;
                retorno.DataDeCadastro = cliente.DataDeCadastro;
                retorno.DataDesativacao = cliente.DataDesativacao;
                retorno.DataInicioContrato = cliente.DataInicioContrato;
                retorno.DataTerminoContrato = cliente.DataTerminoContrato;
                retorno.Desativado = cliente.Desativado;
                retorno.DiaVencimento = cliente.DiaVencimento;
                retorno.Email = cliente.Email;
                retorno.Nome = cliente.Nome;
                retorno.NomeGestorContrato = cliente.NomeGestorContrato;
                retorno.ValorContrato = cliente.ValorContrato;
                retorno.NumDocumento = cliente.NumDocumento;
                retorno.Telefone = cliente.Telefone;

                return retorno;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ClienteDTO Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                Cliente cliente = contexto.ObterPorId<Cliente>(codigo);
                cliente.DataDesativacao = DateTime.Now;
                cliente.Desativado = true;
                contexto.BeginTransaction();
                contexto.Atualizar(cliente);

                var estabelecimentos = contexto.ObterTodos<EstabelecimentoSaude>().Where(x => x.Cliente.Codigo == codigo).ToList();
                for (int i = 0; i < estabelecimentos.Count; i++)
                {
                    var est = contexto.ObterPorId<EstabelecimentoSaude>(estabelecimentos[i].Codigo);
                    est.DataDesativacao = DateTime.Now;
                    est.Desativado = true;
                    contexto.Atualizar<EstabelecimentoSaude>(est);
                    contexto.Commit();
                }


                //contexto.Commit();

                ClienteDTO retorno = new ClienteDTO();

                retorno.Codigo = cliente.Codigo;
                retorno.DataDeCadastro = cliente.DataDeCadastro;
                retorno.DataDesativacao = cliente.DataDesativacao;
                retorno.DataInicioContrato = cliente.DataInicioContrato;
                retorno.DataTerminoContrato = cliente.DataTerminoContrato;
                retorno.Desativado = cliente.Desativado;
                retorno.DiaVencimento = cliente.DiaVencimento;
                retorno.Email = cliente.Email;
                retorno.Nome = cliente.Nome;
                retorno.NomeGestorContrato = cliente.NomeGestorContrato;
                retorno.ValorContrato = cliente.ValorContrato;
                retorno.NumDocumento = cliente.NumDocumento;
                retorno.Telefone = cliente.Telefone;
                retorno.Estabelecimento = contexto.ObterTodos<EstabelecimentoSaude>().Where(y => y.Cliente.Codigo == cliente.Codigo && y.Desativado == false).ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }


        }
        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            Cliente item = (Cliente)objeto;
            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                case EstadoObjeto.Alterado:

                    if (!item.NumDocumento.IsValidCNPJ())
                        throw new CoreException("CNPJ informado inválido.");

                    break;

            }
        }

        public void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = null)
        {

        }
    }
}