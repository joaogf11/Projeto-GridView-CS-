using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Pedido
{
    public class PedItensProvider
    {
        public List<PedFatItens> ListPedItens(SqlConnection connection, List<string> pedidosIds)
        {
            List<PedFatItens> itensPedido = new List<PedFatItens>();
            using (SqlCommand commands = new SqlCommand())
            {
                commands.Connection = connection;
                string pedidosFilter = string.Join(",", pedidosIds.Select(NumPedido => $"'{NumPedido}'"));
                commands.CommandText = $"SELECT PedidoItem.cdproduto,qtdproduto, Produto.DsVenda FROM PedidoItem " +
                                       $"INNER JOIN Produto ON Produto.CdProduto = PedidoItem.cdproduto " +
                                       $"WHERE NumPedido IN ({pedidosFilter})";
                SqlDataReader leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var item = new PedFatItens();
                    item.CdProduto = leitor["cdproduto"].ToString();
                    item.QtdProduto = leitor["qtdproduto"].ToString();
                    item.Descricao = leitor["DsVenda"].ToString();
                    itensPedido.Add(item);
                }
                leitor.Close();
                
            }

            return itensPedido;
        }
    }
}
