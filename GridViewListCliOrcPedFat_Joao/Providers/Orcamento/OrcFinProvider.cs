using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Orcamento
{
    public class OrcFinProvider
    {
        public List<Finan> ListOrcFin(SqlConnection connection, List<string> orcamentosIds)
        {
            var finanOrcamento = new List<Finan>();

            using (var commands = new SqlCommand())
            {
                commands.Connection = connection;
                var orcamentosFilter = string.Join(",", orcamentosIds.Select(numorcamento => $"'{numorcamento}'"));
                commands.CommandText =
                    "SELECT valorr, dtemissao, dsdocquit, SUBSTRING(numlancto, 3, 2) AS 'num' FROM OrcPagarReceber " +
                    "INNER JOIN DocQuitacao ON DocQuitacao.tpdocquit = OrcPagarReceber.tpdocquit " +
                    $"WHERE SUBSTRING(numlancto, 3, 2) IN ({orcamentosFilter})";
                var leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var financeiro = new Finan();
                    financeiro.TipoDoc = leitor["dsdocquit"].ToString();
                    financeiro.Valor = leitor["valorr"].ToString();
                    financeiro.DataEmi = leitor["dtemissao"].ToString();
                    financeiro.NumPed = leitor["num"].ToString();
                    finanOrcamento.Add(financeiro);
                }

                leitor.Close();
            }

            return finanOrcamento;
        }
    }
}