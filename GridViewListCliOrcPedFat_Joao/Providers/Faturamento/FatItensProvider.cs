using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Faturamento
{
    public class FatItensProvider
    {
        public List<PedFatItens> ListFatItens(SqlConnection connection, List<string> faturamentosIds)
        {
            List<PedFatItens> itensFaturamento = new List<PedFatItens>();
            using (SqlCommand commands = new SqlCommand())
            {
                commands.Connection = connection;
                string faturamentosFilter = string.Join(",", faturamentosIds.Select(NumPedidoo => $"'{NumPedidoo}'"));
                commands.CommandText = $"SELECT PedidoItem.cdproduto,qtdproduto, Produto.DsVenda FROM PedidoItem " +
                                       $"INNER JOIN Produto ON Produto.CdProduto = PedidoItem.cdproduto " +
                                       $"WHERE NumPedido IN ({faturamentosFilter})";
                SqlDataReader leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var item = new PedFatItens();
                    item.CdProduto = leitor["cdproduto"].ToString();
                    item.QtdProduto = leitor["qtdproduto"].ToString();
                    item.Descricao = leitor["DsVenda"].ToString();
                    itensFaturamento.Add(item);
                }
                leitor.Close();

            }

            return itensFaturamento;
        }
    }
}
