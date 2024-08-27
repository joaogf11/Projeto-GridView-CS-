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
            var itensPedido = new List<Itens>();
            using (var commands = new SqlCommand())
            {
                commands.Connection = connection;
                var pedidosFilter = string.Join(",", pedidosIds.Select(nummovimento => $"'{nummovimento}'"));
                commands.CommandText =
                    "SELECT pendenciavendaitem.cdproduto,qtdsolicitada, Produto.DsVenda, nummovimento FROM pendenciavendaitem " +
                    "INNER JOIN Produto ON Produto.CdProduto = pendenciavendaitem.cdproduto " +
                    $"WHERE nummovimento IN ({pedidosFilter})";
                var leitor = commands.ExecuteReader();

                while (leitor.Read())
                {
                    var item = new Itens();
                    item.CdProduto = leitor["cdproduto"].ToString();
                    item.QtdProduto = leitor["qtdsolicitada"].ToString();
                    item.NumPed = leitor["nummovimento"].ToString();
                    item.Descricao = leitor["DsVenda"].ToString();
                    itensPedido.Add(item);
                }

                leitor.Close();
            }

            return itensPedido;
        }
    }
}