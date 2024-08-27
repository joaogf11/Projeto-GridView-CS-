using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Pedido
{
    public class PedListProvider
    {
        public List<OrcPedFat> ListPedidos(SqlConnection connection, List<string> clienteIds, List<string> status, DateTime? dataInicio, DateTime? dataFim)
        {
            List<OrcPedFat> pedidos = new List<OrcPedFat>();

            using (SqlCommand commands = new SqlCommand())
            {
                commands.Connection = connection;
                var query = "SELECT nummovimento, cdcliente FROM pendenciavenda1 WHERE";
                if (clienteIds != null && clienteIds.Count > 0)
                {
                    string clientesFilter = string.Join(",", clienteIds.Select(cdcliente => $"'{cdcliente}'"));
                    query += $" CdCliente IN ({clientesFilter})";
                }

                if (status != null && status.Count > 0)
                {
                    string statusFilter = string.Join(",", status.Select(s => $"'{s}'"));
                    query += $" stpendencia IN ({statusFilter})";
                }

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    query += " dtinicio BETWEEN @dataInicio AND @dataFim";
                    commands.Parameters.AddWithValue("@dataInicio", dataInicio.Value);
                    commands.Parameters.AddWithValue("@dataFim", dataFim.Value);
                }

                commands.CommandText = query;
                SqlDataReader leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var pedido = new OrcPedFat();
                    pedido.NumPedido = leitor["nummovimento"].ToString();
                    pedido.Cliente = leitor["cdcliente"].ToString();
                    pedidos.Add(pedido);
                }
                leitor.Close();

            }

            return pedidos;
        }
    }
}
