using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Pedido
{
    public class PedFinProvider
    {
        public List<PedFatFin> ListPedFin(SqlConnection connection, List<string> pedidosIds)
        {
            List<PedFatFin> finanPedido = new List<PedFatFin>();

            using (SqlCommand commands = new SqlCommand())
            {
                commands.Connection = connection;
                string pedidosFilter = string.Join(",", pedidosIds.Select(NumPedido => $"'{NumPedido}'"));
                commands.CommandText = $"SELECT valorr, dtemissao, dsdocquit FROM PagarReceber " +
                                       $"INNER JOIN DocQuitacao ON DocQuitacao.tpdocquit = PagarReceber.tpdocquit " +
                                       $"WHERE SUBSTRING(numlancto, 3, 2) IN ({pedidosFilter})";
                SqlDataReader leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var financeiro = new PedFatFin();
                    financeiro.TipoDoc = leitor["dsdocquit"].ToString();
                    financeiro.Valor = leitor["valorr"].ToString();
                    financeiro.DataEmi = leitor["dtemissao"].ToString();
                    finanPedido.Add(financeiro);
                }
                leitor.Close();

            }

            return finanPedido;
        }
    }
}
