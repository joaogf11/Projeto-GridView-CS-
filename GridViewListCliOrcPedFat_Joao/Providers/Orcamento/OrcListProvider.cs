using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Orcamento
{
    public class OrcListProvider
    {
        public List<ViewModels.Orcamento> ListOrc(SqlConnection connection,List<string> clienteIds)
        {
            
            List<ViewModels.Orcamento> orcamentos = new List<ViewModels.Orcamento>();
           
                    using (SqlCommand commands = new SqlCommand())
                    {
                        commands.Connection = connection;
                        string clientesFilter = string.Join(",", clienteIds.Select(cdcliente => $"'{cdcliente}'"));
                        commands.CommandText = $"SELECT numorcamento FROM orcamento WHERE cdcliente IN ({clientesFilter})";
                        SqlDataReader leitor = commands.ExecuteReader();


                        while (leitor.Read())
                        {
                            var orcamento = new ViewModels.Orcamento();
                            orcamento.NumOrcamento = leitor["numorcamento"].ToString();
                            orcamentos.Add(orcamento);

                        }
                        leitor.Close();

            }
            return orcamentos;
        }
    }
}
