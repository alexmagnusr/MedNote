using MedNote.Infra.Dominio.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MedNote.Repositorios.Seguranca
{
    public interface IRepositorioGrupo
    {        
        Grupo ObterPorLogin(string login, string senha = null, bool somenteAtivos = false);

        void Salvar(Grupo grupo);   
    }
}
