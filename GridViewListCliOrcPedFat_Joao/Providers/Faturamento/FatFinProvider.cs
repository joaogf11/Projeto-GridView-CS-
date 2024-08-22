using System;
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
        public List<PedFatFin> ListFatFin(SqlConnection connection, List<string> faturamentosIds)
        {
            List<PedFatFin> finanFaturamento = new List<PedFatFin>();

            using (SqlCommand commands = new SqlCommand())
            {
                commands.Connection = connection;
                string faturamentosFilter = string.Join(",", faturamentosIds.Select(NumPedidoo => $"'{NumPedidoo}'"));
                commands.CommandText = $"SELECT valorr, dtemissao, dsdocquit FROM PagarReceber " +
                                       $"INNER JOIN DocQuitacao ON DocQuitacao.tpdocquit = PagarReceber.tpdocquit " +
                                       $"WHERE SUBSTRING(numlancto, 3, 2) IN ({faturamentosFilter})";
                SqlDataReader leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var financeiro = new PedFatFin();
                    financeiro.TipoDoc = leitor["dsdocquit"].ToString();
                    financeiro.Valor = leitor["valorr"].ToString();
                    financeiro.DataEmi = leitor["dtemissao"].ToString();
                    finanFaturamento.Add(financeiro);
                }
                leitor.Close();

            }

            return finanFaturamento;
        }
    }
}
