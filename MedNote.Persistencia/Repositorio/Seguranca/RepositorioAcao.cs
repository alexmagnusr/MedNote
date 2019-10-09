using MedNote.Infra.Dominio.Seguranca;
using MedNote.Repositorios.Seguranca;
using Globalsys;
using Globalsys.Exceptions;
using Globalsys.Validacao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedNote.Persistencia.Repositorio.Seguranca
{
    public class RepositorioAcao : IRepositorioAcao
    {
        public IUnidadeTrabalho UnidadeTrabalho { get; set; }
        public RepositorioAcao(IUnidadeTrabalho unidadeTrabalho)
        {
            UnidadeTrabalho = unidadeTrabalho;
        }
        public IList<Acao> ObterAcoesPorPagina(int pagina)
        {
                IList<Acao> Acao = UnidadeTrabalho.ObterTodos<Acao>()             
             .Where(a => a.Funcao.Codigo == pagina && a.DataDesativacao == null)
             .ToList();
          
            return Acao;
        }
        
    }
}
