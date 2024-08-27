using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Faturamento
{
    public class FatFinProvider
    {
        public List<Finan> ListFatFin(SqlConnection connection, List<string> faturamentosIds)
        {
            var finanFaturamento = new List<Finan>();

            using (var commands = new SqlCommand())
            {
                commands.Connection = connection;
                var faturamentosFilter = string.Join(",", faturamentosIds.Select(NumPedidoo => $"'{NumPedidoo}'"));
                commands.CommandText =
                    "SELECT valorr, dtemissao, dsdocquit,SUBSTRING(numlancto, 3, 2) AS 'num' FROM PagarReceber " +
                    "INNER JOIN DocQuitacao ON DocQuitacao.tpdocquit = PagarReceber.tpdocquit " +
                    $"WHERE SUBSTRING(numlancto, 3, 2) IN ({faturamentosFilter})";
                var leitor = commands.ExecuteReader();

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