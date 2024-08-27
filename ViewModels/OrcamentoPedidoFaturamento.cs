using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsGridView.ViewModels
{
    public class OrcamentoPedidoFaturamento : IEquatable<OrcamentoPedidoFaturamento>
    {
        public string NumPedido { get; set; }
        public string Cliente { get; set; }
        public bool IsSelected { get; set; }
        public bool Equals(OrcamentoPedidoFaturamento other)
        {
            return NumPedido == other.NumPedido;
        }
    }
}
