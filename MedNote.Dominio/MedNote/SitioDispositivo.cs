﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Dominio.MedNote
{
    public class SitioDispositivo
    {
        public virtual int Codigo { get; set; }
        public virtual string Descricao { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual TipoDispositivo TipoDispositivo { get; set; }
    }
}