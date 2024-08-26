﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Faturamento
{
    public class FatFinProvider
    {
        public List<Finan> ListFatFin(SqlConnection connection, List<string> faturamentosIds)
        {
            List<Finan> finanFaturamento = new List<Finan>();

            using (SqlCommand commands = new SqlCommand())
            {
                commands.Connection = connection;
                string faturamentosFilter = string.Join(",", faturamentosIds.Select(NumPedidoo => $"'{NumPedidoo}'"));
                commands.CommandText = $"SELECT valorr, dtemissao, dsdocquit,SUBSTRING(numlancto, 3, 2) AS 'num' FROM PagarReceber " +
                                       $"INNER JOIN DocQuitacao ON DocQuitacao.tpdocquit = PagarReceber.tpdocquit " +
                                       $"WHERE SUBSTRING(numlancto, 3, 2) IN ({faturamentosFilter})";
                SqlDataReader leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var financeiro = new Finan();
                    financeiro.TipoDoc = leitor["dsdocquit"].ToString();
                    financeiro.Valor = leitor["valorr"].ToString();
                    financeiro.DataEmi = leitor["dtemissao"].ToString();
                    financeiro.NumPed = leitor["num"].ToString();
                    finanFaturamento.Add(financeiro);
                }
                leitor.Close();

            }

            return finanFaturamento;
        }
    }
}
