using Globalsys;
using Globalsys.Mvc;
using Globalsys.Exceptions;
using Globalsys.Validacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedNote.Repositorios.Seguranca;
using MedNote.Infra.Dominio.Seguranca;

namespace MedNote.Persistencia.Repositorio.Seguranca
{
    public class RepositorioGrupo : IRepositorioGrupo
    {
        public IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public Grupo ObterPorLogin(string login, string senha = null, bool somenteAtivos = false)
        {
            throw new NotImplementedException();
        }
        
        public void Salvar(Grupo grupo)
        {
            UnidadeTrabalho.Salvar<Grupo>(grupo);

            var i = 1;      
        }
    }
}
