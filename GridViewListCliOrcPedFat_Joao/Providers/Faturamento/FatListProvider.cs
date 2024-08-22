using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Faturamento
{
    public class FatListProvider
    {
        public List<PedidoFaturamento> ListFaturamentos(SqlConnection connection, List<string> clienteIds)
        {
            List<PedidoFaturamento> faturamentos = new List<PedidoFaturamento>();

            using (SqlCommand commands = new SqlCommand())
            {
                commands.Connection = connection;
                string clientesFilter = string.Join(",", clienteIds.Select(cdcliente => $"'{cdcliente}'"));
                commands.CommandText = $"SELECT NumPedido, DtPedido, DtFaturamento FROM Pedido " +
                                       $"WHERE DtFaturamento IS NOT NULL AND CdCliente IN ({clientesFilter}) ";
                SqlDataReader leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var faturamento = new PedidoFaturamento();
                    faturamento.NumPedido = leitor["NumPedido"].ToString();
                    faturamento.DataPedido = leitor["DtPedido"].ToString();
                    faturamento.DataFaturamento = leitor["DtFaturamento"].ToString();
                    faturamentos.Add(faturamento);
                }
                leitor.Close();

            }

            return faturamentos;
        }
    }
}
