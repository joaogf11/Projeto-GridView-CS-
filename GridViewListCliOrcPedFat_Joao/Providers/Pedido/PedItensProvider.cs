using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Pedido
{
    public class PedItensProvider
    {
        public List<Itens> ListPedItens(SqlConnection connection, List<string> pedidosIds)
        {
            List<Itens> itensPedido = new List<Itens>();
            using (SqlCommand commands = new SqlCommand())
            {
                commands.Connection = connection;
                string pedidosFilter = string.Join(",", pedidosIds.Select(NumPedido => $"'{NumPedido}'"));
                commands.CommandText = $"SELECT PedidoItem.cdproduto,qtdproduto, Produto.DsVenda, NumPedido FROM PedidoItem " +
                                       $"INNER JOIN Produto ON Produto.CdProduto = PedidoItem.cdproduto " +
                                       $"WHERE NumPedido IN ({pedidosFilter})";
                SqlDataReader leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var item = new Itens();
                    item.CdProduto = leitor["cdproduto"].ToString();
                    item.QtdProduto = leitor["qtdproduto"].ToString();
                    item.NumPed = leitor["NumPedido"].ToString();
                    item.Descricao = leitor["DsVenda"].ToString();
                    itensPedido.Add(item);
                }
                leitor.Close();
                
            }

            return itensPedido;
        }
    }
}
