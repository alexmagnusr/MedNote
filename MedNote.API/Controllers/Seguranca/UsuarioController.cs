using MedNote.API.Helpers.Globalsys;
using MedNote.API.Models.Seguranca;
using MedNote.Infra.Dominio.Seguranca;
using Globalsys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Globalsys.Extensoes;
using MedNote.Repositorios.Seguranca;
using MedNote.Infra;
using MedNote.Dominio.DTOs;

namespace MedNote.API.Controllers.Seguranca
{
    /// <summary>
    /// Controller usuários
    /// </summary>
    /// 
    [RoutePrefix("api/Usuario")]
    public class UsuarioController : BaseApiController
    {

        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        private Object formatarDados(Usuario data, bool jSon = true)
        {
            var contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            return new
            {
                data.Codigo,
                Nome = data.TryGetValue(v => v.Nome),
                Login = data.TryGetValue(v => v.Login),
                EspecialidadeDesc = data.Especialidade == null ? null : data.Especialidade.Descricao,
                Especialidade = data.Especialidade == null ? 0 : data.Especialidade.Codigo,
                EstabelecimentoDesc = data.EstabelecimentoSaude == null ? null : data.EstabelecimentoSaude.Nome,
                EstabelecimentoSaude = data.EstabelecimentoSaude == null ? 0 : data.EstabelecimentoSaude.Codigo,
                GrupoDesc = data.TryGetValue(v => v.Grupo.Descricao),
                Grupo = data.TryGetValue(v => v.Grupo.Codigo),
                Email = data.TryGetValue(v => v.Email),
                CpfFormatado = data.TryGetValue(v => v.NumDocumento.ToCPFFormat()),
                NumDocumento = data.TryGetValue(v => v.NumDocumento),
                DataDeCadastro = data.DataDeCadastro.ToCultureString(),
                DataDesativacao = data.DataDesativacao.ToString(),
                LoginAd = data.LoginAD,
                Admin = data.TryGetValue(v => data.Admin),
                AdminCliente = data.TryGetValue(v => data.AdminCliente),
                Cliente = data.Cliente == null ? 0 : data.Cliente.Codigo,
                ClienteDesc = data.Cliente == null ? null : data.Cliente.Nome
            };
        }

        public UsuarioController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            UsuarioModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        [Authorize(Roles = "Master,GetUsuario")]
        [Route("Consultar")]
        [HttpGet]
        // GET: api/Usuario
        public Object Consultar(int codigo)
        {
            return Json(UsuarioModel.Instancia.Consultar(codigo).ToList().Select(p => new
            {
                p.Codigo,
                Nome = p.TryGetValue(v => v.Nome),
                Login = p.TryGetValue(v => v.Login),
                Email = p.TryGetValue(v => v.Email),
                EspecialidadeDesc = p.Especialidade == null ? null : p.Especialidade.Descricao,
                Especialidade = p.Especialidade == null ? 0 : p.Especialidade.Codigo,
                EstabelecimentoDesc = p.EstabelecimentoSaude == null ? null : p.EstabelecimentoSaude.Nome,
                Estabelecimento = p.EstabelecimentoSaude == null ? 0 : p.EstabelecimentoSaude.Codigo,
                GrupoDesc = p.TryGetValue(v => v.Grupo.Descricao),
                Grupo = p.TryGetValue(v => v.Grupo.Codigo),
                CpfFormatado = p.TryGetValue(v => v.NumDocumento.ToCPFFormat()),
                NumDocumento = p.TryGetValue(v => v.NumDocumento.ToCPFFormat()),
                DataDeCadastro = p.DataDeCadastro.ToCultureString(),
                DataDesativacao = p.DataDesativacao.ToString(),
                //ClienteDesc = p.Cliente.Nome,
                //Cliente = p.Cliente.Codigo,
                p.LoginAD
            }).OrderBy(x => x.Nome));
        }

        // GET: api/Usuario/5
        [Authorize(Roles = "Master,Get/idUsuario")]
        public Object Get(int id)
        {
            return formatarDados(UsuarioModel.Instancia.Editar(id), false);
        }

  
        [Authorize(Roles = "Master,PostUsuario")]
        [HttpPost]
        [Route("Cadastro")]
        public Object Post([FromBody]UsuarioEmpresa usuarioEmpresa)
        {
            return formatarDados(UsuarioModel.Instancia.Cadastrar(usuarioEmpresa));
        }

        // PUT: api/Usuario/5
        [Authorize(Roles = "Master,PutUsuario")]
        [HttpPost]
        [Route("Atualiza")]
        public Object Put(int id, [FromBody]UsuarioEmpresa usuarioEmpresa)
        {
            return formatarDados(UsuarioModel.Instancia.Atualizar(usuarioEmpresa, id));
        }

        // DELETE: api/Usuario/5
        [Authorize(Roles = "Master,DeleteUsuario")]
        [HttpPost]
        [Route("Delete")]
        public Object Delete(int id)
        {
            return Json(formatarDados(UsuarioModel.Instancia.Deletar(id)));
        }

        [HttpGet]
        [Route("ObterNomeDoUsuarioLogado")]
        public Object ObterNomeDoUsuarioLogado()
        {
            return Json(UsuarioModel.Instancia.ObterNomeDoUsuarioLogado());
        }

        [Authorize(Roles = "Master,Get/idUsuario,GetUsuario")]
        [HttpGet]
        [Route("ConsultarUsuarioPorTipoColaborador/{str}")]
        public Object ConsultarUsuarioPorTipoColaborador(int str)
        {
            IRepositorioUsuario repUsuario = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(UnidadeTrabalho);

            return Json(repUsuario.ObterPorTipoDeColaborador(str).Select(p => new
            {
                p.Codigo,
                Nome = p.TryGetValue(v => v.Nome),
                Login = p.TryGetValue(v => v.Login),
                Email = p.TryGetValue(v => v.Email),
                CpfFormatado = p.TryGetValue(v => v.NumDocumento.ToCPFFormat()),
                Cpf = p.TryGetValue(v => v.NumDocumento.ToCPFFormat()),
                DataDeCadastro = p.DataDeCadastro.ToString(),
                DataDesativacao = p.DataDesativacao.ToString()

            }).OrderBy(x => x.Nome));
        }



        [Authorize(Roles = "Master,Get/idUsuario,GetUsuario")]
        [HttpGet]
        [Route("Detalhe/{codUsuario}")]
        public Object Detalhe(int codUsuario)
        {
            var usuarioLogado = UnidadeTrabalho.ObterPorId<Usuario>(codUsuario);
            List<Grupo> lista = UsuarioModel.Instancia.ObterGruposUsuario(usuarioLogado.Grupo.Codigo);
            Usuario usu = usuarioLogado;

            if (usu == null) usu = UsuarioModel.Instancia.Editar(codUsuario);

            var retorno = new List<Object>();
            foreach (var item in lista)
            {
                var modulos = UsuarioModel.Instancia.ObterPermissoesGrupoUsuario(item.Codigo);
                foreach (var m in modulos)
                {
                    retorno.Add(
                    new
                    {
                        Grupo = item.Descricao,
                        Modulo = m.Acao.Funcao.Pai.Nome,
                        Pagina = m.Acao.Funcao.Nome,
                    });
                }
            }
            return new
            {
                Nome = usu.Nome,
                Cpf = usu.NumDocumento.ToCPFFormat(),
                Email = usu.Email,
                Login = usu.Login,
                ListaDetalhe = retorno.Distinct()
            };
        }

        [Authorize(Roles = "Master,Get/idUsuario,GetUsuario")]
        [HttpGet]
        [Route("MeusDados")]
        public Object MeusDados()
        {
            Usuario usuario = UsuarioModel.Instancia.ObterUsuarioLogado();
            return Detalhe(usuario.Codigo);
        }

        [HttpGet]
        [Authorize(Roles = "Master,Get/idUsuario,GetUsuario")]
        [Route("ObterUsuarioLogado")]
        public Object ObterUsuarioLogado()
        {
            Usuario usuario = UsuarioModel.Instancia.ObterUsuarioLogado();
            return new
            {
                Codigo = usuario.Codigo,
                Login = usuario.Login,
                Cpf = usuario.NumDocumento.ToCPFFormat(),
                Email = usuario.Email,
                LoginAd = usuario.LoginAD,
                Nome = usuario.TryGetValue(v => v.Nome)
            };

        }

        [Authorize(Roles = "Master,Get/idUsuario")]
        [Route("TrocarSenha")]
        public Object TrocarSenha([FromBody]UsuarioTO usuario)
        {
            return Json(UsuarioModel.Instancia.AlterarSenha(usuario));
        }


        private static Object getCliente(int codigo)
        {
            var Cliente_selecionada = Fabrica.Instancia.Obter<IUnidadeTrabalho>().ObterTodos<Cliente>().Where(p => p.Codigo == codigo).FirstOrDefault();
            return new { Cliente_selecionada.Nome, Cliente_selecionada.Codigo };
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("RecuperarSenha")]
        public Object RecuperarSenha(string  email)
        {
            return Json(UsuarioModel.Instancia.RecuperarSenha(email));
        }

        [HttpGet]
        [Authorize]
        [Route("cliente")]
        public Object ObterClienteUsuario() => Json(UsuarioModel.Instancia.ObterClienteUsuario(User.Identity.Name));
    }
}
