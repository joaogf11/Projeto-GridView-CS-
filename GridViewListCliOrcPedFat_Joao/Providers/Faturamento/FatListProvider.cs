using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Faturamento
{
    public class FatListProvider
    {
        public List<OrcamentoPedidoFaturamento> ListFaturamentos(SqlConnection connection, List<string> clienteIds)
        {
            List<OrcamentoPedidoFaturamento> faturamentos = new List<OrcamentoPedidoFaturamento>();

            using (SqlCommand commands = new SqlCommand())
            {
                commands.Connection = connection;
                string clientesFilter = string.Join(",", clienteIds.Select(cdcliente => $"'{cdcliente}'"));
                commands.CommandText = $"SELECT NumPedido, DtPedido,cdcliente, DtFaturamento FROM Pedido " +
                                       $"WHERE DtFaturamento IS NOT NULL AND CdCliente IN ({clientesFilter}) ";
                SqlDataReader leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var faturamento = new OrcamentoPedidoFaturamento();
                    faturamento.NumPedido = leitor["NumPedido"].ToString();
                    faturamento.Cliente = leitor["cdcliente"].ToString();
                    faturamentos.Add(faturamento);
                }
                leitor.Close();

            }

            return faturamentos;
        }
    }
}
