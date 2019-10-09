using MedNote.Infra.Dominio.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Repositorios.Seguranca
{
    public interface IRepositorioUsuario
    {
        /// <summary>
        /// Busca Usuário pelo Login, Senha e permite filtrar somente usuários ativos
        /// </summary>
        /// <param name="login">Login do usuário</param>
        /// <param name="senha">Senha do usuário</param>
        /// <param name="somenteAtivos"><c>true</c> para buscar somentes usuários ativos, ou <c>false</c> para não realizar filtrar(default)</param>
        /// <returns>Usuário</returns>
        Usuario ObterPorLogin(string login, string senha = null, bool somenteAtivos = false);

        IList<Usuario> ObterPorTipoDeColaborador(int tipoColaborador);

        /// <summary>
        /// Checa se o login existe no AD
        /// </summary>
        /// <param name="login">Login do usuário no AD</param>
        /// <returns>Email do usuario encontrado</returns>
        object ChecarLoginAD(string login);

        Usuario ObterUsuarioLogado();

        Object ObterNomeDoUsuarioLogado();

        Dictionary<Funcao, IEnumerable<Acao>> ObterFuncaoeAcoesPorUsuarioLogado();

    }
}
