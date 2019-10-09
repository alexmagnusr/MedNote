using MedNote.Infra.Dominio.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globalsys.Validacao;

namespace MedNote.Repositorios.Seguranca
{
    public interface IRepositorioCliente
    {

        Cliente Cadastrar(Cliente Cliente);
        IQueryable<Cliente> Consultar();
        Cliente Atualizar(Cliente Cliente, int codigo);
        Cliente Editar(int codigo);
        Cliente Deletar(int codigo);
        void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null);
        void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = null);

    }
}
