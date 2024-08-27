using System.Collections.Generic;
using System.Data.SqlClient;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers
{
    public class CliListProvider
    {
        public List<Cliente> ListCliente(SqlConnection connection)
        {
            var clientes = new List<Cliente>();

            using (var commands = new SqlCommand())
            {
                commands.Connection = connection;
                commands.CommandText = "SELECT cdcliente, razao FROM Cliente " +
                                       "INNER JOIN Empresa ON Empresa.empresaid = cliente.empresaid";
                var leitor = commands.ExecuteReader();


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