using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsGridView.ViewModels
{
    public class Cliente 
    { 
        public string CdCliente { get; set; }
        public string Razao { get; set; }

        public bool IsSelected { get; set; }

        public bool Equals(Cliente other)
        {
            return CdCliente == other.CdCliente;
        }

        
    }
}
