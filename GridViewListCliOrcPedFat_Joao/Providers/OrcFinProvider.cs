using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers
{
    public class OrcFinProvider
    {
        public List<OrcFin> ListOrcFin(List<string> orcamentosIds)
        { string connectionString = "Server=NBJOAO;" +
                                      "Database=DBDEV;" +
                                      "User ID=sa;" +
                                      "Password=dp;";
            List<OrcFin> finanOrcamento = new List<OrcFin>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand commands = new SqlCommand())
                    {
                        commands.Connection = connection;
                        string orcamentosFilter = string.Join(",", orcamentosIds.Select(numorcamento => $"'{numorcamento}'"));
                        commands.CommandText = $"SELECT valorr, dtemissao,dsdocquit FROM OrcPagarReceber " +
                                               $"INNER JOIN DocQuitacao ON DocQuitacao.tpdocquit = OrcPagarReceber.tpdocquit " +
                                               $"WHERE SUBSTRING(numlancto,3,2) IN ({orcamentosFilter})";
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
                        connection.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return finanOrcamento;
        }
    }
}
