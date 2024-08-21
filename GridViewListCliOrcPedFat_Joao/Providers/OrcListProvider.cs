using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers
{
    public class OrcListProvider
    {
        public List<Orcamento> ListOrc(List<string> clienteIds)
        {
            string connectionString = "Server=NBJOAO;" +
                                      "Database=DBDEV;" +
                                      "User ID=sa;" +
                                      "Password=dp;";
            List<Orcamento> orcamentos = new List<Orcamento>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand commands = new SqlCommand())
                    {
                        commands.Connection = connection;
                        string clientesFilter = string.Join(",", clienteIds.Select(cdcliente => $"'{cdcliente}'"));
                        commands.CommandText = $"SELECT numorcamento FROM orcamento WHERE cdcliente IN ({clientesFilter})";
                        SqlDataReader leitor = commands.ExecuteReader();


                        while (leitor.Read())
                        {
                            var orcamento = new Orcamento();
                            orcamento.NumPedido = leitor["numorcamento"].ToString();
                            orcamentos.Add(orcamento);

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
            return orcamentos;
        }
    }
}
