using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Orcamento
{
    public class OrcListProvider
    {
        public List<OrcamentoPedidoFaturamento> ListOrc(SqlConnection connection, List<string> clienteIds,
            List<string> status, DateTime? dataInicio, DateTime? dataFim)
        {
            List<OrcamentoPedidoFaturamento> orcamentos = new List<OrcamentoPedidoFaturamento>();

            using (SqlCommand commands = new SqlCommand())
            {
                var query = "SELECT numorcamento,cdcliente FROM orcamento WHERE";
                commands.Connection = connection;
                if (clienteIds != null && clienteIds.Count > 0)
                {
                    string clientesFilter = string.Join(",", clienteIds.Select(cdcliente => $"'{cdcliente}'"));
                    query += $" cdcliente IN ({clientesFilter})";
                }

                if (status != null && status.Count > 0)
                {
                    var mappedStatus = status.Select(s =>
                    {
                        if (s == "A") return "'P'";
                        if (s == "E") return "'F'";
                        return null;
                    }).Where(s => s != null).ToList();

                    if (mappedStatus.Count > 0)
                    {
                        string statusFilter = string.Join(",", mappedStatus);
                        query += $" storcamento IN ({statusFilter})";
                    }
                }

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    query += " dtorcamento BETWEEN @dataInicio AND @dataFim";
                    commands.Parameters.AddWithValue("@dataInicio", dataInicio.Value);
                    commands.Parameters.AddWithValue("@dataFim", dataFim.Value);
                }

                commands.CommandText = query;
                SqlDataReader leitor = commands.ExecuteReader();


                while (leitor.Read())
                {
                    var orcamento = new OrcamentoPedidoFaturamento();
                    orcamento.NumPedido = leitor["numorcamento"].ToString();
                    orcamento.Cliente = leitor["cdcliente"].ToString();
                    orcamentos.Add(orcamento);
                }

                leitor.Close();
            }

            return orcamentos;
        }
    }
}