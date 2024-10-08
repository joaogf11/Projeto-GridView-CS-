﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Orcamento
{
    public class OrcItensProvider
    {
        public List<Itens> ListOrcItens(SqlConnection connection, List<string> orcamentosIds)
        {
            var itensOrcamento = new List<Itens>();

            using (var commands = new SqlCommand())
            {
                commands.Connection = connection;
                var orcamentosFilter = string.Join(",", orcamentosIds.Select(numorcamento => $"'{numorcamento}'"));
                commands.CommandText =
                    "SELECT OrcamentoItem.cdproduto,qtdproduto,Produto.DsVenda, numorcamento FROM OrcamentoItem " +
                    "INNER JOIN Produto ON Produto.CdProduto = OrcamentoItem.cdproduto " +
                    $"WHERE numorcamento IN ({orcamentosFilter})";
                var leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var item = new Itens();
                    item.CdProduto = leitor["cdproduto"].ToString();
                    item.Descricao = leitor["DsVenda"].ToString();
                    item.QtdProduto = leitor["qtdproduto"].ToString();
                    item.NumPed = leitor["numorcamento"].ToString();
                    itensOrcamento.Add(item);
                }

                leitor.Close();
            }

            return itensOrcamento;
        }
    }
}