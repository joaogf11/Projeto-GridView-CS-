using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers
{
    public class CliListProvider
    {
        
        public List<Cliente> ListCliente()
        {
            string connectionString = "Server=NBJOAO;" +
                                      "Database=DBDEV;" +
                                      "User ID=sa;" +
                                      "Password=dp;";

            List<Cliente> clientes = new List<Cliente>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand commands = new SqlCommand())
                    {
                        commands.Connection = connection;
                        commands.CommandText = "SELECT cdcliente, razao FROM Cliente " +
                                               "INNER JOIN Empresa ON Empresa.empresaid = cliente.empresaid";
                        SqlDataReader leitor = commands.ExecuteReader();
                        
                        
                        while (leitor.Read())
                        {
                            var cliente = new Cliente();
                            cliente.CdCliente = leitor["cdcliente"].ToString();
                            cliente.Razao = leitor["razao"].ToString();
                            clientes.Add(cliente);
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

                return clientes;
            }
        }
        
    }
}
