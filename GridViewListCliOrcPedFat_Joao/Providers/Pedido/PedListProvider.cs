using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Pedido
{
    public class PedListProvider
    {
        public List<OrcamentoPedidoFaturamento> ListPedidos(SqlConnection connection, List<string> clienteIds, List<string> status)
        {
            List<OrcamentoPedidoFaturamento> pedidos = new List<OrcamentoPedidoFaturamento>();

            using (SqlCommand commands = new SqlCommand())
            {
                commands.Connection = connection;
                string clientesFilter = string.Join(",", clienteIds.Select(cdcliente => $"'{cdcliente}'"));
                commands.CommandText = $"SELECT NumPedido, DtPedido, cdcliente FROM Pedido " +
                                       $"WHERE DtFaturamento IS NULL AND CdCliente IN ({clientesFilter}) ";
                SqlDataReader leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var pedido = new OrcamentoPedidoFaturamento();
                    pedido.NumPedido = leitor["NumPedido"].ToString();
                    pedido.Cliente = leitor["cdcliente"].ToString();
                    pedidos.Add(pedido);
                }
                leitor.Close();

            }

            return pedidos;
        }
    }
}
