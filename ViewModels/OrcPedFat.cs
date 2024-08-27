using System;

namespace WindowsFormsGridView.ViewModels
{
    public class OrcPedFat : IEquatable<OrcPedFat>
    {
        public string NumPedido { get; set; }
        public string Cliente { get; set; }
        public bool IsSelected { get; set; }
        public string Status { get; set; }

        public bool Equals(OrcPedFat other)
        {
            return NumPedido == other.NumPedido;
        }
    }
}