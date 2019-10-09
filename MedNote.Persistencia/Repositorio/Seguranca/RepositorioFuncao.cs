using MedNote.Infra.Dominio.Seguranca;
using MedNote.Repositorios.Seguranca;
using Globalsys;
using Globalsys.Exceptions;
using Globalsys.Validacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIX.Persistencia.Repositorio.Seguranca;

namespace MedNote.Persistencia.Repositorio.Seguranca
{
    public class RepositorioFuncao : IRepositorioFuncao
    {
        public IUnidadeTrabalho UnidadeTrabalho { get; set; }
        public RepositorioFuncao(IUnidadeTrabalho unidadeTrabalho)
        {
            UnidadeTrabalho = unidadeTrabalho;
        }
        public IList<Funcao> ObterPorTipo(TipoFuncao tipo)
        {
            Usuario usuario = new RepositorioUsuario(UnidadeTrabalho).ObterUsuarioLogado();

            if (usuario.Admin)
            {
                IList<Funcao> funcao = UnidadeTrabalho.ObterTodos<Funcao>()
            .Where(f => (f.Tipo == tipo && f.DataDesativacao == null))
            .ToList();

                return funcao;
            }
            else
            {
                int[] codigosGrupos = UnidadeTrabalho.ObterTodos<Grupo>().Where(mb => mb.Codigo == usuario.Grupo.Codigo && mb.Desativado == false)
                                    .Select(x => x.Codigo).ToArray();

                int[] idsFuncaoGrupo = UnidadeTrabalho.ObterTodos<PermissaoGrupoAcao>()
                  .Where(pga => codigosGrupos.Contains(pga.Grupo.Codigo) && pga.DataDesativacao == null).ToList().Select(x => x.Acao.Funcao.Codigo).Distinct().ToArray();

                IList<Funcao> funcao = UnidadeTrabalho.ObterTodos<Funcao>()
                .Where(f => f.DataDesativacao == null && idsFuncaoGrupo.Contains(f.Codigo)).Select(x => x.Pai).Distinct()
                .ToList();

                return funcao;
            }


        }

        public IList<Funcao> ObterPaginasPorModulo(int idModulo)
        {
            Usuario usuario = new RepositorioUsuario(UnidadeTrabalho).ObterUsuarioLogado();

            if (usuario.Admin)
            {
                IList<Funcao> funcao = UnidadeTrabalho.ObterTodos<Funcao>()
             .Where(f => (f.Pai.Codigo == idModulo && f.DataDesativacao == null))
             .ToList();

                return funcao;
            }
            else
            {
                int[] codigosGrupos = UnidadeTrabalho.ObterTodos<Grupo>().Where(mb => mb.Codigo == usuario.Grupo.Codigo && mb.Desativado == false)
                                     .Select(gp => gp.Codigo).ToArray();

                int[] idsFuncaoGrupo = UnidadeTrabalho.ObterTodos<PermissaoGrupoAcao>()
                  .Where(pga => codigosGrupos.Contains(pga.Grupo.Codigo) && pga.DataDesativacao == null).ToList().Select(x => x.Acao.Funcao.Codigo).Distinct().ToArray();

                IList<Funcao> funcao = UnidadeTrabalho.ObterTodos<Funcao>()
                .Where(f => f.DataDesativacao == null && idsFuncaoGrupo.Contains(f.Codigo) && f.Pai.Codigo == idModulo)
                .ToList();

                return funcao;

            }
        }

    }
}
