using System;

namespace WindowsFormsGridView.ViewModels
{
    public class Cliente : IEquatable<Cliente>
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