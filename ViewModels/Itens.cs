using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsGridView.ViewModels
{
    public class Itens
    {
        public string CdProduto { get; set; }
        public string QtdProduto { get; set; }
        public string Descricao { get; set; }
        public string NumPed { get; set; }

        public bool Equals(Itens other)
        {
            return CdProduto == other.CdProduto;
        }
    }
}
