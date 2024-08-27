﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Faturamento
{
    public class FatListProvider
    {
        public List<OrcamentoPedidoFaturamento> ListFaturamentos(SqlConnection connection, List<string> clienteIds,
            List<string> status, DateTime? dataInicio, DateTime? dataFim)
        {
            List<OrcamentoPedidoFaturamento> faturamentos = new List<OrcamentoPedidoFaturamento>();

            using (SqlCommand commands = new SqlCommand())
            {
                commands.Connection = connection;
                var query =
                    "SELECT NumPedido, DtPedido,cdcliente, DtFaturamento FROM Pedido WHERE";
                if (clienteIds != null && clienteIds.Count > 0)
                {
                    string clientesFilter = string.Join(",", clienteIds.Select(cdcliente => $"'{cdcliente}'"));
                    query += $" CdCliente IN ({clientesFilter})";
                }

                if (status != null && status.Count > 0)
                {
                    string statusFilter = string.Join(",", status.Select(s => $"'{s}'"));
                    query += $" StPedido IN ({statusFilter})";
                }

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    query += " DtFaturamento BETWEEN @dataInicio AND @dataFim";
                    commands.Parameters.AddWithValue("@dataInicio", dataInicio.Value);
                    commands.Parameters.AddWithValue("@dataFim", dataFim.Value);
                }

                commands.CommandText = query;
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