using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Orcamento
{
    public class OrcFinProvider
    {
        public List<OrcFin> ListOrcFin(SqlConnection connection,List<string> orcamentosIds)
        {
            List<OrcFin> finanOrcamento = new List<OrcFin>();

            using (SqlCommand commands = new SqlCommand())
            {
                commands.Connection = connection;
                string orcamentosFilter = string.Join(",", orcamentosIds.Select(numorcamento => $"'{numorcamento}'"));
                commands.CommandText = $"SELECT valorr, dtemissao, dsdocquit FROM OrcPagarReceber " +
                                       $"INNER JOIN DocQuitacao ON DocQuitacao.tpdocquit = OrcPagarReceber.tpdocquit " +
                                       $"WHERE SUBSTRING(numlancto, 3, 2) IN ({orcamentosFilter})";
                SqlDataReader leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var financeiro = new OrcFin();
                    financeiro.TipoDoc = leitor["dsdocquit"].ToString();
                    financeiro.Valor = leitor["valorr"].ToString();
                    financeiro.DataEmi = leitor["dtemissao"].ToString();
                    finanOrcamento.Add(financeiro);
                }
                leitor.Close();

            }

            return finanOrcamento;
        }
    }
}
