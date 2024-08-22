using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsGridView.ViewModels
{
    public class Orcamento
    {
        public string NumOrcamento { get; set; }

        public bool IsSelected { get; set; }

        public bool Equals(Orcamento other)
        {
            return NumOrcamento == other.NumOrcamento;
        }
    }
}
