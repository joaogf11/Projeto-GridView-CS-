using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Orcamento
{
    public class OrcListProvider
    {
        public List<OrcamentoPedidoFaturamento> ListOrc(SqlConnection connection,List<string> clienteIds)
        {
            
            List<OrcamentoPedidoFaturamento> orcamentos = new List<OrcamentoPedidoFaturamento>();
           
                    using (SqlCommand commands = new SqlCommand())
                    {
                        commands.Connection = connection;
                        string clientesFilter = string.Join(",", clienteIds.Select(cdcliente => $"'{cdcliente}'"));
                        commands.CommandText = $"SELECT numorcamento,cdcliente FROM orcamento WHERE cdcliente IN ({clientesFilter})";
                        SqlDataReader leitor = commands.ExecuteReader();


                        while (leitor.Read())
                        {
                            var orcamento = new OrcamentoPedidoFaturamento();
                            orcamento.NumPedido = leitor["numorcamento"].ToString();
                            orcamento.Cliente = leitor["cdcliente"].ToString();
                            orcamentos.Add(orcamento);
                    }
                        leitor.Close();

            }
            return orcamentos;
        }
    }
}
