using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers
{
    public class OrcItensProvider
    {
        public List<OrcItens> ListOrcItens(List<string> orcamentosIds)
        {
            string connectionString = "Server=NBJOAO;" +
                                      "Database=DBDEV;" +
                                      "User ID=sa;" +
                                      "Password=dp;";
            List<OrcItens> itensOrcamento = new List<OrcItens>();

            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand commands = new SqlCommand())
                    {
                        commands.Connection = connection;
                        string orcamentosFilter = string.Join(",", orcamentosIds.Select(numorcamento => $"'{numorcamento}'"));
                        commands.CommandText = $"SELECT OrcamentoItem.cdproduto,Produto.DsVenda FROM OrcamentoItem " +
                                               $"INNER JOIN Produto ON Produto.CdProduto = OrcamentoItem.cdproduto " +
                                               $"WHERE numorcamento IN ({orcamentosFilter})";
                        SqlDataReader leitor = commands.ExecuteReader();
                        
                        while (leitor.Read())
                        {
                            var item = new OrcItens();
                            item.CdProduto = leitor["cdproduto"].ToString();
                            item.Descricao = leitor["DsVenda"].ToString();
                            itensOrcamento.Add(item);

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

            return itensOrcamento;
        }
    }
}
