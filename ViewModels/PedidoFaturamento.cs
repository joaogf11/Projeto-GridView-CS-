using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsGridView.ViewModels
{
    public class PedidoFaturamento
    {
        public string NumPedido { get; set; }
        public string DataPedido { get; set; }
        public string DataFaturamento { get; set; }

        public bool IsSelected { get; set; }

        public bool Equals(PedidoFaturamento other)
        {
            return NumPedido == other.NumPedido;
        }
    }
}
