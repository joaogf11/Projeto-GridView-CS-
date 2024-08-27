using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Pedido
{
    public class PedFinProvider
    {
        public List<Finan> ListPedFin(SqlConnection connection, List<string> pedidosIds)
        {
            var finanPedido = new List<Finan>();

            using (var commands = new SqlCommand())
            {
                commands.Connection = connection;
                var pedidosFilter = string.Join(",", pedidosIds.Select(NumPedido => $"'{NumPedido}'"));
                commands.CommandText =
                    "SELECT valorr, dtemissao, dsdocquit,SUBSTRING(numlancto, 3, 2) AS 'num' FROM PagarReceber " +
                    "INNER JOIN DocQuitacao ON DocQuitacao.tpdocquit = PagarReceber.tpdocquit " +
                    $"WHERE SUBSTRING(numlancto, 3, 2) IN ({pedidosFilter})";
                var leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var financeiro = new Finan();
                    financeiro.TipoDoc = leitor["dsdocquit"].ToString();
                    financeiro.Valor = leitor["valorr"].ToString();
                    financeiro.DataEmi = leitor["dtemissao"].ToString();
                    financeiro.NumPed = leitor["num"].ToString();
                    finanPedido.Add(financeiro);
                }

                leitor.Close();
            }

            return finanPedido;
        }
    }
}