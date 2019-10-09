using Globalsys.Model;
using System;
using System.Linq;
using Globalsys.Validacao;
using Globalsys;
using MedNote.Infra;
using LinqKit;
using MedNote.Dominio.MedNote;
using MedNote.Infra.Dominio.Seguranca;
using MedNote.Dominio.DTOs;
using MedNote.Repositorios.Seguranca;

namespace MedNote.API.Models.Ative
{
    public class SetorModel : IModel
    {

        private static SetorModel model { get; set; }

        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static SetorModel Instancia
        {
            get
            {
                if (model == null)
                    model = new SetorModel();

                return model;
            }
        }
       
        public SetorDTO Cadastrar(SetorDTO setor)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                contexto.BeginTransaction();

                Setor setorNew = new Setor();
                SetorDTO setorDto = new SetorDTO();

                setorNew = this.ConverteDtoEntidade(setor);

                if (setorNew != null)
                {
                    setorNew.Desativado = false;
                    var tipoSetor = contexto.ObterPorId<TipoSetor>(setor.TipoSetor.Codigo);
                    setorNew.TipoSetor = tipoSetor;
                    setorNew.DataCadastro = DateTime.Now;

                    var estabelecimento = contexto.ObterPorId<EstabelecimentoSaude>(setor.Estabelecimento.Codigo);
                    setorNew.Estabelecimento = estabelecimento;
                    ValidarCampos(setorNew, EstadoObjeto.Novo);
                    contexto.Salvar<Setor>(setorNew);
                    //contexto.Commit();

                    // salvando cada um dos Leitos
                    foreach (var _leito in setor.Leitos)
                    {
                        Leito leito = new Leito();
                        leito.Setor = new Setor { Codigo = setorNew.Codigo };
                        leito.DataCadastro = DateTime.Now;
                        leito.Identificador = _leito.Identificador;
                        leito.Bl_Liberado = true;
                        leito.Desativado = false;
                        leito.DataDesativacao = null;
                        ValidarCamposLeito(leito, EstadoObjeto.Novo);
                        contexto.Salvar<Leito>(leito);
                    }

                    contexto.Commit();

                    setorDto = this.ConverteEntidadeDto(setorNew);
                }

                return setorDto;

            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public IQueryable<SetorDTO> Consultar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                Usuario usuario = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(UnidadeDeTrabalho).ObterUsuarioLogado();

                
                var predicate = PredicateBuilder.New<Setor>();
                predicate = predicate.And(p => p.Desativado == false);
                if (!usuario.Admin)
                    predicate = predicate.And(p => p.Estabelecimento.Cliente.Codigo == codigo);
                
                IQueryable<Setor> query = contexto.ObterTodos<Setor>().Where(predicate);
                var setor = query.Select(x => (this.ConverteEntidadeDto(x)));

                return setor;
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }

        public IQueryable<SetorDTO> ConsultarPorEstabelecimento(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

                var predicate = PredicateBuilder.New<Setor>();
                predicate = predicate.And(p => p.Desativado == false && p.Estabelecimento.Codigo == codigo);
                

                IQueryable<Setor> query = contexto.ObterTodos<Setor>().Where(predicate);
                var setor = query.Select(x => (this.ConverteEntidadeDto(x)));

                return setor;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public SetorDTO Atualizar(SetorDTO setor, int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                contexto.BeginTransaction();

                var setorOld = contexto.ObterPorId<Setor>(codigo);
                setorOld.Nome = setor.Nome;

                var tipoSetor = contexto.ObterPorId<TipoSetor>(setor.TipoSetor.Codigo);
                setorOld.TipoSetor = tipoSetor;
                var estabelecimento = contexto.ObterPorId<EstabelecimentoSaude>(setor.Estabelecimento.Codigo);
                setorOld.Estabelecimento = estabelecimento;

                contexto.Atualizar(setorOld);
           
                for (int i = 0; i < setor.Leitos.Count; i++)
                {

                    if (!setor.Leitos[i].Editar)
                    {
                        if (setor.Leitos[i].Codigo == 0)
                        {
                            Leito leito = new Leito();
                            leito.Setor = setorOld;
                            leito.DataCadastro = DateTime.Now;
                            leito.Identificador = setor.Leitos[i].Identificador;
                            //A liberação do leito é sempre feita na Admissão/Alta do Paciente
                            //leito.Bl_Liberado = setor.Leitos[i].Bl_Liberado;
                            leito.Desativado = false;
                            leito.DataDesativacao = null;
                            ValidarCamposLeito(leito, EstadoObjeto.Novo);
                            contexto.Salvar<Leito>(leito);
                        }

                    }
                    else if (setor.Leitos[i].Editar)
                    {
                        if (setor.Leitos[i].Codigo != 0)
                        {
                            Leito leito = contexto.ObterPorId<Leito>(setor.Leitos[i].Codigo);
                            leito.Identificador = setor.Leitos[i].Identificador;
                            //A liberação do leito é sempre feita na Admissão/Alta do Paciente
                            //leito.Bl_Liberado = setor.Leitos[i].Bl_Liberado;
                            ValidarCamposLeito(leito, EstadoObjeto.Alterado);
                            contexto.Atualizar<Leito>(leito);
                        }
                        else
                        {
                            Leito leito = new Leito();
                            leito.Setor = setorOld;
                            leito.Bl_Liberado = true;
                            leito.Identificador = setor.Leitos[i].Identificador;
                            leito.DataCadastro = DateTime.Now;
                            leito.Desativado = false;
                            ValidarCamposLeito(leito, EstadoObjeto.Novo);
                            contexto.Salvar<Leito>(leito);
                        }
                    }
                   
                }


                contexto.Commit();
                var setorDto = this.ConverteEntidadeDto(setorOld);

                return setorDto;
                
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
          
        }

        public SetorDTO Editar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                var setor = contexto.ObterPorId<Setor>(codigo);
                var setorDto = this.ConverteEntidadeDto(setor);
                return setorDto;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public SetorDTO Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                Setor setor = contexto.ObterPorId<Setor>(codigo);
                setor.DataDesativacao = DateTime.Now;
                setor.Desativado = true;
                contexto.BeginTransaction();
                contexto.Atualizar(setor);
                contexto.Commit();

                var setorDto = this.ConverteEntidadeDto(setor);
                
                return setorDto;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
         
        }

        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            Setor item = (Setor)objeto;
            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                case EstadoObjeto.Alterado:

                    break;

            }
        }

        public void ValidarCamposLeito(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            Leito item = (Leito)objeto;
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;

            switch ((EstadoObjeto)estadoObjeto)
            {
                //case EstadoObjeto.Novo:
                    //if (contexto.ObterTodos<Leito>().Any(l => l.Setor.Codigo == item.Setor.Codigo && l.Identificador.Equals(item.Identificador) && l.DataDesativacao == null))
                        //throw new Globalsys.Exceptions.CoreException("Já existe um leito com o Identificador \"" + item.Identificador + "\" cadastrado no sistema para este setor.");
                    //break;
                case EstadoObjeto.Alterado:
                    if (contexto.ObterTodos<Leito>().Any(l => l.Setor.Codigo == item.Setor.Codigo && l.Codigo != item.Codigo && l.Identificador.Equals(item.Identificador) && l.DataDesativacao == null))
                        throw new Globalsys.Exceptions.CoreException("Já existe um leito com o Identificador \"" + item.Identificador + "\" cadastrado no sistema para este setor.");

                    break;

            }
        }

        public void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = null)
        {

        }

        private SetorDTO ConverteEntidadeDto(Setor entidade)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

                SetorDTO retorno = new SetorDTO();
                retorno.Codigo = entidade.Codigo;
                retorno.DataCadastro = entidade.DataCadastro;
                retorno.DataDesativacao = entidade.DataDesativacao;
                retorno.Desativado = entidade.Desativado;
                retorno.Estabelecimento = entidade.Estabelecimento;
                retorno.Nome = entidade.Nome;
                retorno.TipoSetor = entidade.TipoSetor;
                retorno.QtdLeitos = entidade.QtdLeito;
                retorno.QtdLeitoInicio = 0;
                retorno.QtdLeitoFim = 0;
                retorno.Leitos = contexto.ObterTodos<Leito>().Where(x => x.Setor.Codigo == retorno.Codigo && x.Desativado == false).ToList();

                return retorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private Setor ConverteDtoEntidade(SetorDTO dto)
        {
            try
            {
                IUnidadeTrabalho contexto = UnidadeDeTrabalho;

                Setor retorno = new Setor();

                retorno.Codigo = dto.Codigo;
                retorno.DataCadastro = dto.DataCadastro;
                retorno.DataDesativacao = dto.DataDesativacao;
                retorno.Desativado = dto.Desativado;
                retorno.Estabelecimento = dto.Estabelecimento;
                retorno.Nome = dto.Nome;
                retorno.TipoSetor = dto.TipoSetor;
                retorno.QtdLeito = contexto.ObterTodos<Leito>().Where(x => x.Setor.Codigo == retorno.Codigo && x.Desativado == false).Count();
                return retorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}