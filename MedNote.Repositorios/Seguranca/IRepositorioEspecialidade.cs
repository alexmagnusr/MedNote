using MedNote.Infra.Dominio.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globalsys.Validacao;

namespace MedNote.Repositorios.Seguranca
{
    public interface IRepositorioEspecialidade
    {

        Especialidade Cadastrar(Especialidade especialidade);
        IQueryable<Especialidade> Consultar();
        Especialidade Atualizar(Especialidade especialidade, int codigo);
        Especialidade Editar(int codigo);
        Especialidade Deletar(int codigo);
        void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null);
        void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = null);

    }
}
