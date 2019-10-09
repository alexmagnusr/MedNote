using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtiveEngenharia.Dominio
{
    public class RelatorioHistoricoSaldoMateriais
    {
        public String CodigoDescricao { get; set; }
        public DateTime DtInicial { get; set; }
        public DateTime DtFim { get; set; }
        public List<DadosHistoricoSaldoMateriais> baixas = new List<DadosHistoricoSaldoMateriais>();

    }

    public class DadosHistoricoSaldoMateriais
    {
        public string NomeEletricista { get; set; }
        public DateTime DtBaixa { get; set; }
        public decimal QtdBaixa { get; set; }
        public decimal Saldo { get; set; }
    }
}

