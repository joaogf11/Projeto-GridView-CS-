using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Faturamento
{
    public class FatItensProvider
    {
        public List<Itens> ListFatItens(SqlConnection connection, List<string> faturamentosIds)
        {
            List<Itens> itensFaturamento = new List<Itens>();
            using (SqlCommand commands = new SqlCommand())
            {
                commands.Connection = connection;
                string faturamentosFilter = string.Join(",", faturamentosIds.Select(NumPedidoo => $"'{NumPedidoo}'"));
                commands.CommandText = $"SELECT PedidoItem.cdproduto,qtdproduto, Produto.DsVenda,NumPedido FROM PedidoItem " +
                                       $"INNER JOIN Produto ON Produto.CdProduto = PedidoItem.cdproduto " +
                                       $"WHERE NumPedido IN ({faturamentosFilter})";
                SqlDataReader leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var item = new Itens();
                    item.CdProduto = leitor["cdproduto"].ToString();
                    item.QtdProduto = leitor["qtdproduto"].ToString();
                    item.Descricao = leitor["DsVenda"].ToString();
                    item.NumPed = leitor["NumPedido"].ToString();
                    itensFaturamento.Add(item);
                }
                leitor.Close();

            }

            return itensFaturamento;
        }
    }
}
