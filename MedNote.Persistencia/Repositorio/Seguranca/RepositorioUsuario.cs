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
    public class RepositorioUsuario : IRepositorioUsuario
    {
        public IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public RepositorioUsuario(IUnidadeTrabalho unidadeTrabalho)
        {
            UnidadeTrabalho = unidadeTrabalho;
        }

        public Usuario ObterPorLogin(string login, string senha = null, bool somenteAtivos = false)
        {
            try
            {
                Usuario usuario = UnidadeTrabalho.ObterTodos<Usuario>().Where(u => (u.Login == login)).FirstOrDefault();

                if (usuario == null || (somenteAtivos && (usuario.Desativado)))
                    return null;

                return usuario;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public object ChecarLoginAD(string login)
        {
            return Globalsys.Util.Tools.ValidarLogin(login);
        }

        public IList<Usuario> ObterPorTipoDeColaborador(int tipoColaborador)
        {
            try
            {
                Usuario usuario = ObterUsuarioLogado();

                IQueryable<Usuario> usuarios = UnidadeTrabalho.ObterTodos<Usuario>()
               .Where(u => (u.Desativado == false && u.Grupo.Codigo == tipoColaborador));
                return usuarios.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Usuario ObterUsuarioLogado()
        {
            try
            {
                string usuarioLogado = System.Web.HttpContext.Current.TryGetValue(c => c.User.Identity.Name);
                var retorno = UnidadeTrabalho.ObterTodos<Usuario>().Where(t => t.Login.Equals(usuarioLogado) && t.Desativado == false).FirstOrDefault();
                return retorno;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<Funcao, IEnumerable<Acao>> ObterFuncaoeAcoesPorUsuarioLogado()
        {
            try
            {

                Usuario usuario = new RepositorioUsuario(UnidadeTrabalho).ObterUsuarioLogado();
               
                var acoesUsuario = UnidadeTrabalho.ObterTodos<PermisaoUsuarioAcao>()
                                    .Where(c => c.Usuario.Codigo == usuario.Codigo
                                            && c.DataDesativacao == null)
                                     .GroupBy(g => g.Acao.Funcao, g => g.Acao, (funcao, acoes) =>
                                            new { Funcao = funcao, Acoes = acoes })
                                                .ToDictionary(group => group.Funcao, group => group.Acoes);

            
                if (usuario.Admin)
                {
                    var acoesGrupo = UnidadeTrabalho.ObterTodos<Acao>()
                              .Where(pga => pga.DataDesativacao == null)
                              .GroupBy(g => g.Funcao, g => g, (funcao, acoes) =>
                                  new { Funcao = funcao, Acoes = acoes })
                                          .ToDictionary(group => group.Funcao, group => group.Acoes);
                    return acoesUsuario.Concat(acoesGrupo.Where(g => !acoesUsuario.Keys.Contains(g.Key))).ToDictionary(s => s.Key, s => s.Value);
                }
                else
                {
                   var acoesGrupo = UnidadeTrabalho.ObterTodos<PermissaoGrupoAcao>()
                               .Where(pga => pga.DataDesativacao == null && pga.Grupo.Codigo == usuario.Grupo.Codigo)
                               .GroupBy(g => g.Acao.Funcao, g => g.Acao, (funcao, acoes) =>
                                   new { Funcao = funcao, Acoes = acoes })
                                           .ToDictionary(group => group.Funcao, group => group.Acoes);
                    return acoesUsuario.Concat(acoesGrupo.Where(g => !acoesUsuario.Keys.Contains(g.Key))).ToDictionary(s => s.Key, s => s.Value);

                }

               
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Object ObterNomeDoUsuarioLogado()
        {
            Usuario usuarioLogado = ObterUsuarioLogado();
            if (!usuarioLogado.EstabelecimentoSaude.IsNull())
            {
                var retorno = new
                {
                    nome = usuarioLogado.TryGetValue(v => v.Nome),
                    usuarioMaster = usuarioLogado.TryGetValue(v => v.Admin),
                    estabelecimento = usuarioLogado.EstabelecimentoSaude != null
                    ? usuarioLogado.EstabelecimentoSaude.Nome : ""
                };

                return retorno;
            } else {
                var retorno = new
                {
                    nome = usuarioLogado.TryGetValue(v => v.Nome),
                    usuarioMaster = usuarioLogado.TryGetValue(v => v.Admin),
                    estabelecimento = ""
                };

                return retorno;
            }
            
        }

    }
}
