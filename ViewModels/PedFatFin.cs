using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsGridView.ViewModels
{
    public class PedFatFin
    {
        public string Valor { get; set; }
        public string DataEmi { get; set; }
        public string TipoDoc { get; set; }

        public bool Equals(PedFatFin other)
        {
            return Valor == other.Valor;
        }

    }
}
