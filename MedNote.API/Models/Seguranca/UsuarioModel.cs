using MedNote.Infra.Dominio.Seguranca;
using MedNote.Infra;
using Globalsys;
using Globalsys.Exceptions;
using Globalsys.Validacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Globalsys.Extensoes;
using MedNote.Repositorios.Seguranca;
using Globalsys.Model;
using MedNote.API.Helpers.Globalsys;
using MedNote.Dominio.DTOs;
using LinqKit;
using NHibernate.Util;
using System.Security.Claims;

namespace MedNote.API.Models.Seguranca
{

    public class UsuarioModel : IModel
    {
        private static UsuarioModel usuarioModel { get; set; }

        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static UsuarioModel Instancia
        {
            get
            {
                if (usuarioModel == null)
                    usuarioModel = new UsuarioModel();

                return usuarioModel;
            }
        }
        
        public Object ObterNomeDoUsuarioLogado()
        {
            IUnidadeTrabalho unidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            return Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(unidadeTrabalho).ObterNomeDoUsuarioLogado();
        }

        public Usuario ObterUsuarioLogado()
        {
            IUnidadeTrabalho unidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            return Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(unidadeTrabalho).ObterUsuarioLogado();
        }

        public Usuario Login(string login, string senha)
        {
            //Chamar unidade de trabalho desta forma pois esse metodo será chamado de outros lugares
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            senha = Util.Helper.GetHash(senha);
            return contexto.ObterTodos<Usuario>().Where(t => t.Login.Equals(login) && t.Senha.Equals(senha)).FirstOrDefault();
        }
        public Usuario UsuarioLogado()
        {
            //Chamar unidade de trabalho desta forma pois esse metodo será chamado de outros lugares
            IUnidadeTrabalho unidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            return Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(unidadeTrabalho).ObterUsuarioLogado();
        }

        public Usuario UsuariPossuiLogin(string login)
        {
            try
            {
                //Chamar unidade de trabalho desta forma pois esse metodo será chamado de outros lugares
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                return contexto.ObterTodos<Usuario>().Where(t => t.Login.Equals(login) && t.DataDesativacao == null).FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<Usuario> Consultar(int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            Usuario usuario = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(contexto).ObterUsuarioLogado();

            var predicate = PredicateBuilder.New<Usuario>();
            predicate = predicate.And(p => p.Desativado == false);
            if (!usuario.Admin)
            {
                if (!usuario.Admin && !usuario.EstabelecimentoSaude.IsNull())
                {
                    predicate = predicate.And(p => p.EstabelecimentoSaude.Codigo == usuario.EstabelecimentoSaude.Codigo && p.Cliente.Codigo == usuario.Cliente.Codigo);
                }
                else
                {
                    predicate = predicate.And(p => p.Cliente.Codigo == codigo);
                }
            }
           
            IQueryable<Usuario> query = contexto.ObterTodos<Usuario>().Where(predicate);

            return query;
        }

        public Usuario Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>(); ;
            Usuario usuario = contexto.ObterPorId<Usuario>(codigo);
            usuario.DataDesativacao = DateTime.Today;
            usuario.Desativado = true;
            //ColaboradorModel.Instancia.UnidadeDeTrabalho = UnidadeDeTrabalho;
            //ColaboradorModel.Instancia.Deletar(usuario.Codigo);
            ValidarRegras(usuario, EstadoObjeto.Removido);
            contexto.BeginTransaction();
            contexto.Atualizar(usuario);
            contexto.Commit();

            return usuario;
        }

        public Usuario Editar(int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            return contexto.ObterPorId<Usuario>(codigo);
        }

        public Usuario Atualizar(UsuarioEmpresa usuarioEmpresa, int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                contexto.BeginTransaction();
                Usuario usuarioA = contexto.ObterPorId<Usuario>(codigo);
                //Usuario usuario = new Usuario();
                usuarioA.Login = usuarioEmpresa.Login;
                usuarioA.Codigo = usuarioEmpresa.Codigo;
                usuarioA.Nome = usuarioEmpresa.Nome;
                usuarioA.Email = usuarioEmpresa.Email;
                var grupo = contexto.ObterPorId<Grupo>(usuarioEmpresa.Grupo);
                usuarioA.Grupo = grupo;
                var cliente = contexto.ObterPorId<Cliente>(usuarioEmpresa.Cliente);
                usuarioA.Cliente = cliente;
                var especialidade = contexto.ObterPorId<Especialidade>(usuarioEmpresa.Especialidade);
                usuarioA.Especialidade = especialidade;
                var estabelecimento = contexto.ObterPorId<EstabelecimentoSaude>(usuarioEmpresa.EstabelecimentoSaude);
                usuarioA.EstabelecimentoSaude = estabelecimento;
                usuarioA.NumDocumento = usuarioEmpresa.NumDocumento;
                usuarioA.LoginAD = usuarioEmpresa.LoginAd;
                usuarioA.Admin = usuarioEmpresa.Admin;
                usuarioA.AdminCliente = usuarioEmpresa.AdminCliente;

                ValidarCampos(usuarioA);
                ValidarRegras(usuarioA, EstadoObjeto.Alterado);
                contexto.BeginTransaction();
                contexto.Atualizar(usuarioA);
                contexto.Commit();
                return usuarioA;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public SuccessResult RecuperarSenha(string email)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            var result = new SuccessResult();
            try
            {
                contexto.BeginTransaction();

                var usuario = contexto.ObterTodos<Usuario>().Where(t => t.Email == email).FirstOrDefault();
                if (usuario == null)
                {
                    result.Success = false;
                    result.Message = "Usuário não encontrado.";
                }
                else
                {
                    string senha = Util.Helper.gerarSenhaCPF(usuario.NumDocumento);
                    usuario.Senha = Util.Helper.GetHash(senha);
                    Dictionary<string, string> valoresSubs = new Dictionary<string, string>();
                    valoresSubs.Add("$$NovaSenha$$", "Sua nova senha: " + senha);
                    Globalsys.Util.Email.Enviar("[MedNote] - Recuperação de senha.",
                                                                HttpContext.Current.Server.MapPath("/Content/templates/esqueciSenha.htm"),
                                                                valoresSubs,
                                                                string.Empty,
                                                                null,
                                                                System.Web.HttpContext.Current.Server.MapPath("/Content/images/logo2.png"),
                                                                null,
                                                                email);

                    contexto.Commit();
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                result.Success = false;
                throw ex;
            }

            return result;
        }

        public Usuario Cadastrar(UsuarioEmpresa usuarioColaborador)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

            Usuario usuario = new Usuario();
            try
            {
                contexto.BeginTransaction();
               
                if (!usuarioColaborador.Admin)
                {
                    var cliente = contexto.ObterPorId<Cliente>(usuarioColaborador.Cliente);
                    usuario.Cliente = cliente;

                    var estabelecimento = contexto.ObterPorId<EstabelecimentoSaude>(usuarioColaborador.EstabelecimentoSaude);
                    usuario.EstabelecimentoSaude = estabelecimento;

                    var especialidade = contexto.ObterPorId<Especialidade>(usuarioColaborador.Especialidade);
                    usuario.Especialidade = especialidade;
                }
                var grupo = contexto.ObterPorId<Grupo>(usuarioColaborador.Grupo);
                usuario.Grupo = grupo;
                usuario.Nome = usuarioColaborador.Nome;
                usuario.NumDocumento = usuarioColaborador.NumDocumento;
                usuario.DataDeCadastro = DateTime.Now;
                usuario.Email = usuarioColaborador.Email;
                usuario.Login = usuarioColaborador.Login;
                usuario.LoginAD = usuarioColaborador.LoginAd;
                usuario.DataDeCadastro = DateTime.Now;
                usuario.Desativado = false;
                usuario.Admin = usuarioColaborador.Admin;
                usuario.AdminCliente = usuarioColaborador.AdminCliente;

                ValidarCampos(usuario);
                ValidarRegras(usuario, EstadoObjeto.Novo);

                usuario.Senha = Util.Helper.GetHash(Util.Helper.gerarSenhaCPF(usuarioColaborador.NumDocumento));

                contexto.Salvar<Usuario>(usuario);

                if (!usuario.LoginAD)
                {
                    Dictionary<string, string> valoresSubs = new Dictionary<string, string>();
                    valoresSubs.Add("$$Login$$", usuario.Login);
                    valoresSubs.Add("$$Senha$$", "Sua senha é composta dos 4 primeiros + 2 últimos dígitos do seu cpf");
                    valoresSubs.Add("$$Url$$", "");
                    Globalsys.Util.Email.Enviar("[MedNote] - Cadastro de novo usuário para acesso.",
                                                                HttpContext.Current.Server.MapPath("/Content/templates/novaContaDeUsuario.htm"),
                                                                valoresSubs,
                                                                string.Empty,
                                                                null,
                                                                System.Web.HttpContext.Current.Server.MapPath("/Content/images/logo2.png"),
                                                                null,
                                                                usuario.Email);
                }
                contexto.Commit();

                return usuario;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public List<Grupo> ObterGruposUsuario(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = UnidadeDeTrabalho;
                return contexto.ObterTodos<Grupo>().Where(x => x.Codigo == codigo && x.Desativado == false).ToList();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<PermissaoGrupoAcao> ObterPermissoesGrupoUsuario(int codGrupo)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            return contexto.ObterTodos<PermissaoGrupoAcao>().Where(x => x.Grupo.Codigo == codGrupo && x.DataDesativacao == null).ToList();
        }

        public void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            Usuario usuario = (Usuario)objeto;
            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                    if (contexto.ObterTodos<Usuario>().Any(t => t.Login.Equals(usuario.Login) && t.DataDesativacao == null))
                        throw new CoreException("Login já cadastrado no sistema.");
                    if (contexto.ObterTodos<Usuario>().Any(t => t.Email.Equals(usuario.Email) && t.DataDesativacao == null))
                        throw new CoreException("Email já cadastrado no sistema.");
                    if (!usuario.Admin)
                    {
                        if (!usuario.AdminCliente)
                        {
                            if (usuario.Cliente.IsNull())
                                throw new CoreException("O Cliente é obrigatório para este tipo de Usuário.");
                            if (usuario.EstabelecimentoSaude.IsNull())
                                throw new CoreException("O Estabelecimento de saude é obrigatório para este tipo de Usuário.");
                            if (usuario.Especialidade.IsNull())
                                throw new CoreException("A Especialidade é obrigatória para este tipo de Usuário.");
                            //Criar validação para ter no minimo um setor de usuario para este usuario - SetorUsuario
                        } else {
                            if (usuario.Cliente.IsNull())
                                throw new CoreException("O Cliente é obrigatório para este tipo de Usuário.");
                        } 
                    }
                    break;
                case EstadoObjeto.Alterado:
                    if (contexto.ObterTodos<Usuario>().Any(t => t.Login.Equals(usuario.Login) && t.Codigo != usuario.Codigo && t.DataDesativacao == null))
                        throw new CoreException("Login já cadastrado no sistema.");
                    if (contexto.ObterTodos<Usuario>().Any(t => t.Email.Equals(usuario.Email) && t.Codigo != usuario.Codigo && t.DataDesativacao == null))
                        throw new CoreException("Email já cadastrado no sistema.");
                    if (!usuario.Admin)
                    {
                        if (!usuario.AdminCliente)
                        {
                            if (usuario.Cliente.IsNull())
                                throw new CoreException("O Cliente é obrigatório para este tipo de Usuário.");
                            if (usuario.EstabelecimentoSaude.IsNull())
                                throw new CoreException("O Estabelecimento de saude é obrigatório para este tipo de Usuário.");
                            if (usuario.Especialidade.IsNull())
                                throw new CoreException("A Especialidade é obrigatória para este tipo de Usuário.");
                            //Criar validação para ter no minimo um setor de usuario para este usuario - SetorUsuario
                        }
                        else
                        {
                            if (usuario.Cliente.IsNull())
                                throw new CoreException("O Cliente é obrigatório para este tipo de Usuário.");
                        }
                    }
                    break;


                case EstadoObjeto.Removido:
                    if (contexto.ObterPorId<Usuario>(usuario.Codigo).Admin)
                        throw new CoreException("Você não tem permissão para remover usuários administradores.");
                    break;
            }

        }

        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            try
            {
                Usuario usuario = (Usuario)objeto;

                if (usuario.NumDocumento.IsEmpty())
                    throw new CoreException("O CPF é um campo obrigatório.");
                if (usuario.Login.IsEmpty())
                    throw new CoreException("O Login é um campo obrigatório.");
                if (usuario.Email.IsEmpty())
                    throw new CoreException("O Email é um campo obrigatório.");
                if (usuario.Nome.IsEmpty())
                    throw new CoreException("O Nome é um campo obrigatório.");
                if (!usuario.NumDocumento.ClearCPFFormat().IsValidCPF())
                    throw new CoreException("CPF inválido! Favor verificar.");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public UsuarioTO AlterarSenha(UsuarioTO usuario)
        {
            try
            {
                if (!usuario.SenhaNova.Equals(usuario.ConfirmacaoSenha))
                {
                    throw new CoreException("Confirmação de senha não confere com a nova senha.");
                }
                Usuario usu;
                IUnidadeTrabalho contexto = UnidadeDeTrabalho;
                if (String.IsNullOrEmpty(usuario.Cpf))
                    usu = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(UnidadeDeTrabalho).ObterUsuarioLogado();
                else
                    usu = contexto.ObterTodos<Usuario>().Where(x => x.Senha.Equals(Util.Helper.GetHash(usuario.SenhaAntiga)) && x.NumDocumento == usuario.Cpf.ClearCPFFormat()).FirstOrDefault();

                if (usu != null)
                {
                    contexto.BeginTransaction();
                    usu.Senha = Util.Helper.GetHash(usuario.SenhaNova);
                    contexto.Atualizar<Usuario>(usu);
                    contexto.Commit();
                }
                else
                {
                    throw new CoreException("Dados incorretos. Favor verificar a senha antiga.");
                }

                return usuario;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool EsqueciSenha(UsuarioTO usuario)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                string novaSenha = Globalsys.Util.Tools.GerarSenhaAleatoria();
                Usuario usu = contexto.ObterTodos<Usuario>().Where(x => x.NumDocumento == usuario.Cpf && x.Email == usuario.Email).FirstOrDefault();

                if (usu != null)
                {
                    //if (usu.LoginAd)
                    //    throw new CoreException("Você utiliza o Active directory para autenticar, procure o administrador para redefinir sua senha.");

                    contexto.BeginTransaction();
                    usu.Senha = Util.Helper.GetHash(novaSenha);
                    contexto.Atualizar<Usuario>(usu);
                    contexto.Commit();
                    Dictionary<string, string> valoresSubs = new Dictionary<string, string>();
                    valoresSubs.Add("$$NovaSenha$$", novaSenha);
                    Globalsys.Util.Email.Enviar("[MedNote] - Nova senha de acesso",
                                                                HttpContext.Current.Server.MapPath("/Content/templates/esqueciSenha.htm"),
                                                                valoresSubs,
                                                                string.Empty,
                                                                null,
                                                                System.Web.HttpContext.Current.Server.MapPath("/Content/images/logo2.png"),
                                                                null,
                                                                usu.TryGetValue(v => v.Email));
                }
                else
                {
                    throw new CoreException("Informações incorretas.");
                }

                return true;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Usuario> LoadMenuClientes()
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                List<Usuario> retorno = new List<Usuario>();

                Usuario usuarioLogado = this.ObterUsuarioLogado();

                if (usuarioLogado != null)
                {
                    var predicate = PredicateBuilder.New<Usuario>();
                    predicate = predicate.And(p => p.Desativado == false
                                                && p.Codigo == usuarioLogado.Codigo);
                    if (!usuarioLogado.Admin)
                    {
                        predicate = predicate.And(p => p.Cliente.Codigo == usuarioLogado.Cliente.Codigo);
                    }

                    retorno = contexto.ObterTodos<Usuario>().Where(predicate).ToList();
                }

                return retorno;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ClienteDTO ObterClienteUsuario(string login)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            IQueryable<Usuario> usuarios = contexto.ObterTodos<Usuario>().Where(x => x.Login == login && !x.Desativado);

            if (usuarios.Any() && !usuarios.FirstOrDefault().Cliente.IsNull()) {
                var cliente = usuarios.FirstOrDefault().Cliente;
                return new ClienteDTO()
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
                    NumDocumento = cliente.NumDocumento,
                    Telefone = cliente.Telefone,
                    Estabelecimento = null
                };
            }
            else
                return new ClienteDTO();

            
        }

        public IEnumerable<Claim> ObterPermissoesUsuario(string login)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            var usuario = contexto.ObterTodos<Usuario>().Where(t => t.Login.Equals(login) && t.DataDesativacao == null).FirstOrDefault();
            var acoes = contexto.ObterTodos<PermissaoGrupoAcao>()
                .Where(x => x.Grupo.Codigo == usuario.Grupo.Codigo)
                .Select(x => x.Acao).ToList();

            List<Claim> claims = new List<Claim>();
            acoes.ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x.Ref)));

            if (usuario.TryGetValue(v => v.Admin))
                claims.Add(new Claim(ClaimTypes.Role, "Master"));

            return claims;
        }
    }
}