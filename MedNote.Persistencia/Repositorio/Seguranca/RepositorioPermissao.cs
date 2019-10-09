using MedNote.Infra.Dominio.Seguranca;
using Globalsys;
using Globalsys.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIX.Persistencia.Repositorio.Seguranca;

namespace MedNote.Persistencia.Repositorio.Seguranca
{
    public class RepositorioPermissao : IRepositorioPermissao
    {
        public IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public RepositorioPermissao(IUnidadeTrabalho unidadeTrabalho)
        {
            UnidadeTrabalho = unidadeTrabalho;
        }

        public bool PossuiPermissao(string controller, string action)
        {
            RepositorioUsuario repUsuario = new RepositorioUsuario(UnidadeTrabalho);

            Usuario usuario = repUsuario.ObterUsuarioLogado();
            //int[] codigosGrupos = UnidadeTrabalho.ObterTodos<Membro>().Where(mb => mb.Usuario.Codigo == usuario.Codigo && mb. == null)
            //                            .Select(gp => gp.Grupo).Select(g => g.Codigo).ToArray();
            //int[] idsAcaoUsuario = UnidadeTrabalho.ObterTodos<PermisaoUsuarioAcao>()
            //    .Where(p => p.Usuario.Codigo == usuario.Codigo && p.Desativado == false).ToList().Select(x => x.Acao.Codigo).ToArray();
            int[] idsAcaoGrupo = UnidadeTrabalho.ObterTodos<PermissaoGrupoAcao>()
                 .Where(pga => /*codigosGrupos.Contains(pga.Grupo.Codigo) &&*/ pga.Desativado == false).ToList().Select(x => x.Acao.Codigo).ToArray();


            return UnidadeTrabalho.ObterTodos<PermisaoUsuarioAcao>()
                    .Where(p => p.Usuario.Codigo == usuario.Codigo && p.Desativado == false)
                        .Any(x => x.Acao.Funcao.Nome.Equals(controller)
                            && x.Acao.Nome.Equals(action))
                ||
                UnidadeTrabalho.ObterTodos<PermissaoGrupoAcao>()
                 .Where(pga => /*codigosGrupos.Contains(pga.Grupo.Codigo) &&*/ pga.Desativado == false)
                    .Any(x => x.Acao.Funcao.Nome.Equals(controller)
                        && x.Acao.Nome.Equals(action))

                ||
                UnidadeTrabalho.ObterTodos<Recurso>()
                .Where(x =>
                    x.AcaoSecundaria.Nome.Equals(action) &&
                    x.AcaoSecundaria.Funcao.Nome.Equals(controller) &&
                    ((idsAcaoGrupo.Contains(x.AcaoPrincipal.Codigo)))
                    //((idsAcaoGrupo.Contains(x.AcaoPrincipal.Codigo) || (idsAcaoUsuario.Contains(x.AcaoPrincipal.Codigo))))
                    ).Any();


        }
    }
}
