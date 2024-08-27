using System;

namespace WindowsFormsGridView.ViewModels
{
    public class Finan : IEquatable<Finan>
    {
        public string Valor { get; set; }
        public string DataEmi { get; set; }
        public string TipoDoc { get; set; }
        public string NumPed { get; set; }

        public bool Equals(Finan other)
        {
            return Valor == other.Valor;
        }
    }
}