using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Orcamento
{
    public class OrcItensProvider
    {
        public List<OrcItens> ListOrcItens(SqlConnection connection, List<string> orcamentosIds)
        {
            List<OrcItens> itensOrcamento = new List<OrcItens>();

            using (SqlCommand commands = new SqlCommand())
            {
                commands.Connection = connection;
                string orcamentosFilter = string.Join(",", orcamentosIds.Select(numorcamento => $"'{numorcamento}'"));
                commands.CommandText = $"SELECT OrcamentoItem.cdproduto, Produto.DsVenda FROM OrcamentoItem " +
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

            }

            return itensOrcamento;
        }
    }
}
