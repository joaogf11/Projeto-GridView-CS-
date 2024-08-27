using System;

namespace WindowsFormsGridView.ViewModels
{
    public class Itens : IEquatable<Itens>
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