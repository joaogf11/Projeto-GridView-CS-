﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsGridView.ViewModels
{
    public class PedFatItens
    {
        public string CdProduto { get; set; }
        public string QtdProduto {get; set; }
        public string Descricao { get; set; }

        public bool Equals(PedFatItens other)
        {
            return CdProduto == other.CdProduto;
        }
    }
}
