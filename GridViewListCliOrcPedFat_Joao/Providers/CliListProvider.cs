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
        
        public List<Cliente> ListCliente(SqlConnection connection)
        {
            
            List<Cliente> clientes = new List<Cliente>();

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

            }
            return clientes;
        }
    }
}

