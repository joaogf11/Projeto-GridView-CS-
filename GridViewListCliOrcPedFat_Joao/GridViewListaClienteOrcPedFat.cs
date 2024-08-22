using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers;
using WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Faturamento;
using WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Orcamento;
using WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers.Pedido;
using WindowsFormsGridView.ViewModels;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao
{
    public partial class GridViewListaClienteOrcPedFat : Form
    {
        private List<Cliente> _clientes;
        private CliListProvider _clienteProvider;

        private List<Orcamento> _orcamentos;
        private OrcListProvider _orcListProvider;

        private List<OrcItens> _orcItens;
        private OrcItensProvider _orcItensProvider;

        private List<OrcFin> _orcFin;
        private OrcFinProvider _orcFinProvider;

        private List<PedidoFaturamento> _pedidos;
        private PedListProvider _pedidosProvider;

        private List<PedFatItens> _pedItens;
        private PedItensProvider _pedItensProvider;

        private List<PedFatFin> _pedFin;
        private PedFinProvider _pedFinProvider;

        private List<PedidoFaturamento> _faturamentos;
        private FatListProvider _faturamentosProvider;

        private List<PedFatItens> _fatItens;
        private FatItensProvider _fatItensProvider;

        private List<PedFatFin> _fatFin;
        private FatFinProvider _fatFinProvider;

        public GridViewListaClienteOrcPedFat()
        {
            InitializeComponent();
            SetComponents();
            InitializeListClientes();

            chkOrc.CheckedChanged += (object sender, System.EventArgs e) =>
            {
                btnFiltrar.Visible = true;
                if (chkOrc.Checked)
                {
                    chkPed.Enabled = false;
                    chkFat.Enabled = false;
                }
                else
                {
                    chkPed.Enabled = true;
                    chkFat.Enabled = true;
                }
            };
            chkPed.CheckedChanged += (object sender, System.EventArgs e) =>
            {
                btnFiltrar.Visible = true;
                if (chkPed.Checked)
                {
                    chkOrc.Enabled = false;
                    chkFat.Enabled = false;
                }
                else
                {
                    chkOrc.Enabled = true;
                    chkFat.Enabled = true;
                }
            };
            chkFat.CheckedChanged += (object sender, System.EventArgs e) =>
            {
                btnFiltrar.Visible = true;
                if (chkFat.Checked)
                {
                    chkOrc.Enabled = false;
                    chkPed.Enabled = false;
                }
                else
                {
                    chkOrc.Enabled = true;
                    chkPed.Enabled = true;
                }
            };

            btnCarregar.Click += (object sender, System.EventArgs e) =>
            {
                using (var connectionManager = new SqlConnManager())
                {
                    SqlConnection connection = connectionManager.GetConnection();
                    _clientes = _clienteProvider.ListCliente(connection);
                    dataGridView1.DataSource = _clientes;
                    InitializeDataGridView1();
                }
            };


            btnFiltrar.Click += (object sender, System.EventArgs e) =>
            {
                btnDetalhes.Visible = true;
                
                if (_clientes != null && _clientes.Any())
                {
                    List<string> selectedClientIds = new List<string>();
                    foreach (var cliente in _clientes)
                    {
                        if (cliente.IsSelected)
                        {
                            selectedClientIds.Add(cliente.CdCliente);
                        }
                    }

                    if (selectedClientIds.Any())
                    {
                        if (chkOrc.Checked)
                        {
                            using (var connectionManager = new SqlConnManager())
                            {
                                SqlConnection connection = connectionManager.GetConnection();
                                _orcamentos = _orcListProvider.ListOrc(connection, selectedClientIds);
                                dataGridView2.DataSource = _orcamentos;
                                InitializeDataGridView2();
                            }
                        }

                        if (chkPed.Checked)
                        {
                            using (var connectionManager = new SqlConnManager())
                            {
                                SqlConnection connection = connectionManager.GetConnection();
                                _pedidos = _pedidosProvider.ListPedidos(connection, selectedClientIds);
                                dataGridView5.DataSource = _pedidos;
                                InitializeDataGridView5();
                            }
                        }

                        if (chkFat.Checked)
                        {
                            using (var connectionManager = new SqlConnManager())
                            {
                                SqlConnection connection = connectionManager.GetConnection();
                                _faturamentos = _faturamentosProvider.ListFaturamentos(connection, selectedClientIds);
                                dataGridView8.DataSource = _faturamentos;
                                InitializeDataGridView8();
                            }
                        }
                    }
                }
            };


            btnDetalhes.Click += async (object sender, System.EventArgs e) =>
            {
                btnLimpar.Visible = true;
                if (chkOrc.Checked)
                {
                    if (_orcamentos != null && _orcamentos.Any())
                    {
                        List<string> selectedOrcIds = new List<string>();
                        foreach (var orc in _orcamentos)
                        {
                            if (orc.IsSelected)
                            {
                                selectedOrcIds.Add(orc.NumOrcamento);
                            }
                        }


                        if (selectedOrcIds.Any())
                        {
                            using (var connectionManager = new SqlConnManager())
                            {
                                SqlConnection connection = connectionManager.GetConnection();
                                var taskOrcItens = Task.Run(() =>
                                    _orcItensProvider.ListOrcItens(connection, selectedOrcIds));
                                var taskOrcFin = Task.Run(() =>
                                    _orcFinProvider.ListOrcFin(connection, selectedOrcIds));
                                await Task.WhenAll(taskOrcItens, taskOrcFin);
                                _orcItens = taskOrcItens.Result;
                                _orcFin = taskOrcFin.Result;
                                dataGridView3.DataSource = _orcItens;
                                dataGridView4.DataSource = _orcFin;
                                InitializeDataGridView3();
                                InitializeDataGridView4();
                            }
                        }
                    }
                }

                if (chkPed.Checked)
                {
                    if (_pedidos != null && _pedidos.Any())
                    {
                        List<string> selectedPedIds = new List<string>();
                        foreach (var ped in _pedidos)
                        {
                            if (ped.IsSelected)
                            {
                                selectedPedIds.Add(ped.NumPedido);
                            }
                        }


                        if (selectedPedIds.Any())
                        {
                            using (var connectionManager = new SqlConnManager())
                            {
                                Console.WriteLine(selectedPedIds.ToString());
                                SqlConnection connection = connectionManager.GetConnection();
                                var taskPedItens = Task.Run(() =>
                                    _pedItensProvider.ListPedItens(connection, selectedPedIds));
                                var taskPedFin = Task.Run(() =>
                                    _pedFinProvider.ListPedFin(connection, selectedPedIds));
                                await Task.WhenAll(taskPedItens, taskPedFin);
                                _pedItens = taskPedItens.Result;
                                _pedFin = taskPedFin.Result;
                                dataGridView6.DataSource = _pedItens;
                                dataGridView7.DataSource = _pedFin;
                                InitializeDataGridView6();
                                InitializeDataGridView7();
                            }
                        }
                    }
                }

                if (chkFat.Checked)
                {
                    if (_faturamentos != null && _faturamentos.Any())
                    {
                        List<string> selectedFatIds = new List<string>();
                        foreach (var fat in _faturamentos)
                        {
                            if (fat.IsSelected)
                            {
                                selectedFatIds.Add(fat.NumPedido);
                            }
                        }


                        if (selectedFatIds.Any())
                        {
                            using (var connectionManager = new SqlConnManager())
                            {
                                SqlConnection connection = connectionManager.GetConnection();
                                var taskFatItens = Task.Run(() =>
                                    _fatItensProvider.ListFatItens(connection, selectedFatIds));
                                var taskFatFin = Task.Run(() =>
                                    _fatFinProvider.ListFatFin(connection, selectedFatIds));
                                await Task.WhenAll(taskFatItens, taskFatFin);
                                _fatItens = taskFatItens.Result;
                                _fatFin = taskFatFin.Result;
                                dataGridView9.DataSource = _fatItens;
                                dataGridView10.DataSource = _fatFin;
                                InitializeDataGridView9();
                                InitializeDataGridView10();
                            }
                        }
                    }
                }
            };


            btnLimpar.Click += (object sender, System.EventArgs e) =>
            {
                _orcamentos.Clear();
                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();
                dataGridView2.Visible = false;
                _orcItens.Clear();
                dataGridView3.DataSource = null;
                dataGridView3.Rows.Clear();
                dataGridView3.Visible = false;
                _orcFin.Clear();
                dataGridView4.DataSource = null;
                dataGridView4.Rows.Clear();
                dataGridView4.Visible = false;
                _pedidos.Clear();
                dataGridView5.DataSource = null;
                dataGridView5.Rows.Clear();
                dataGridView5.Visible = false;
                _pedItens.Clear();
                dataGridView6.DataSource = null;
                dataGridView6.Rows.Clear();
                dataGridView6.Visible = false;
                _pedFin.Clear();
                dataGridView7.DataSource = null;
                dataGridView7.Rows.Clear();
                dataGridView7.Visible = false;
                _faturamentos.Clear();
                dataGridView8.DataSource = null;
                dataGridView8.Rows.Clear();
                dataGridView8.Visible = false;
                _fatItens.Clear();
                dataGridView9.DataSource = null;
                dataGridView9.Rows.Clear();
                dataGridView9.Visible = false;
                _fatFin.Clear();
                dataGridView10.DataSource = null;
                dataGridView10.Rows.Clear();
                dataGridView10.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                chkOrc.Checked = false;
                chkPed.Checked = false;
                chkFat.Checked = false;
                btnDetalhes.Visible = false;
                btnLimpar.Visible = false;
            };
        }

        private void InitializeDataGridView1()
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns.Clear();

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                Name = "IsSelected",
                HeaderText = "Selecionado",
                DataPropertyName = "IsSelected",
                DisplayIndex = 0
            };
            dataGridView1.Columns.Add(checkBoxColumn);

            DataGridViewTextBoxColumn codigoColumn = new DataGridViewTextBoxColumn
            {
                Name = "CdCliente",
                HeaderText = "Código",
                DataPropertyName = "CdCliente",
                DisplayIndex = 1
            };
            dataGridView1.Columns.Add(codigoColumn);

            DataGridViewTextBoxColumn razaoColumn = new DataGridViewTextBoxColumn
            {
                Name = "Razao",
                HeaderText = "Razão",
                DataPropertyName = "Razao",
                DisplayIndex = 2
            };
            dataGridView1.Columns.Add(razaoColumn);
        }

        private void InitializeDataGridView2()
        {
            if (chkOrc.Checked)
            {
                label1.Visible = true;
                dataGridView2.Visible = true;
                dataGridView2.RowHeadersVisible = false;
                dataGridView2.Columns.Clear();

                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
                {
                    Name = "IsSelected",
                    HeaderText = "Selecionado",
                    DataPropertyName = "IsSelected",
                    DisplayIndex = 0
                };
                dataGridView2.Columns.Add(checkBoxColumn);

                DataGridViewTextBoxColumn numColumn = new DataGridViewTextBoxColumn
                {
                    Name = "NumOrcamento",
                    HeaderText = "Código",
                    DataPropertyName = "NumOrcamento",
                    DisplayIndex = 1
                };
                dataGridView2.Columns.Add(numColumn);
            }
        }

        private void InitializeDataGridView3()
        {
            if (chkOrc.Checked)
            {
                label5.Visible = true;
                dataGridView3.Visible = true;
                dataGridView3.RowHeadersVisible = false;
                dataGridView3.Columns.Clear();

                DataGridViewTextBoxColumn codColumn = new DataGridViewTextBoxColumn
                {
                    Name = "CdProduto",
                    HeaderText = "Código Produto",
                    DataPropertyName = "CdProduto",
                    DisplayIndex = 0
                };
                dataGridView3.Columns.Add(codColumn);

                DataGridViewTextBoxColumn descColumn = new DataGridViewTextBoxColumn
                {
                    Name = "Descricao",
                    HeaderText = "Descricao",
                    DataPropertyName = "Descricao",
                    DisplayIndex = 1
                };
                dataGridView3.Columns.Add(descColumn);
            }
        }

        private void InitializeDataGridView4()
        {
            if (chkOrc.Checked)
            {
                label6.Visible = true;
                dataGridView4.Visible = true;
                dataGridView4.RowHeadersVisible = false;
                dataGridView4.Columns.Clear();

                DataGridViewTextBoxColumn valColumn = new DataGridViewTextBoxColumn
                {
                    Name = "Valor",
                    HeaderText = "Valor Total",
                    DataPropertyName = "Valor",
                    DisplayIndex = 1
                };
                dataGridView4.Columns.Add(valColumn);

                DataGridViewTextBoxColumn dataColumn = new DataGridViewTextBoxColumn
                {
                    Name = "DataEmi",
                    HeaderText = "Descricao",
                    DataPropertyName = "DataEmi",
                    DisplayIndex = 2
                };
                dataGridView4.Columns.Add(dataColumn);

                DataGridViewTextBoxColumn tipoColumn = new DataGridViewTextBoxColumn
                {
                    Name = "TipoDoc",
                    HeaderText = "Forma de Pagamento",
                    DataPropertyName = "TipoDoc",
                    DisplayIndex = 2
                };
                dataGridView4.Columns.Add(tipoColumn);
            }
        }

        private void InitializeDataGridView5()
        {
            if (chkPed.Checked)
            {
                label2.Visible = true;
                dataGridView5.Visible = true;
                dataGridView5.RowHeadersVisible = false;
                dataGridView5.Columns.Clear();

                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
                {
                    Name = "IsSelected",
                    HeaderText = "Selecionado",
                    DataPropertyName = "IsSelected",
                    DisplayIndex = 0
                };
                dataGridView5.Columns.Add(checkBoxColumn);

                DataGridViewTextBoxColumn numColumn = new DataGridViewTextBoxColumn
                {
                    Name = "NumPedido",
                    HeaderText = "Código",
                    DataPropertyName = "NumPedido",
                    DisplayIndex = 1
                };
                dataGridView5.Columns.Add(numColumn);
            }
        }

        private void InitializeDataGridView6()
        {
            if (chkPed.Checked)
            {
                label5.Visible = true;
                dataGridView6.Visible = true;
                dataGridView6.RowHeadersVisible = false;
                dataGridView6.Columns.Clear();

                DataGridViewTextBoxColumn codColumn = new DataGridViewTextBoxColumn
                {
                    Name = "CdProd",
                    HeaderText = "Código Produto",
                    DataPropertyName = "CdProd",
                    DisplayIndex = 0
                };
                dataGridView6.Columns.Add(codColumn);

                DataGridViewTextBoxColumn descColumn = new DataGridViewTextBoxColumn
                {
                    Name = "Descricao",
                    HeaderText = "Descricao",
                    DataPropertyName = "Descricao",
                    DisplayIndex = 1
                };
                dataGridView6.Columns.Add(descColumn);
            }
        }

        private void InitializeDataGridView7()
        {
            if (chkPed.Checked)
            {
                label6.Visible = true;
                dataGridView7.Visible = true;
                dataGridView7.RowHeadersVisible = false;
                dataGridView7.Columns.Clear();

                DataGridViewTextBoxColumn valColumn = new DataGridViewTextBoxColumn
                {
                    Name = "Valor",
                    HeaderText = "Valor Total",
                    DataPropertyName = "Valor",
                    DisplayIndex = 1
                };
                dataGridView7.Columns.Add(valColumn);

                DataGridViewTextBoxColumn dataColumn = new DataGridViewTextBoxColumn
                {
                    Name = "DataEmi",
                    HeaderText = "Descricao",
                    DataPropertyName = "DataEmi",
                    DisplayIndex = 2
                };
                dataGridView7.Columns.Add(dataColumn);

                DataGridViewTextBoxColumn tipoColumn = new DataGridViewTextBoxColumn
                {
                    Name = "TipoDoc",
                    HeaderText = "Forma de Pagamento",
                    DataPropertyName = "TipoDoc",
                    DisplayIndex = 2
                };
                dataGridView7.Columns.Add(tipoColumn);
            }
        }

        private void InitializeDataGridView8()
        {
            if (chkFat.Checked)
            {
                label3.Visible = true;
                dataGridView8.Visible = true;
                dataGridView8.RowHeadersVisible = false;
                dataGridView8.Columns.Clear();

                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
                {
                    Name = "IsSelected",
                    HeaderText = "Selecionado",
                    DataPropertyName = "IsSelected",
                    DisplayIndex = 0
                };
                dataGridView8.Columns.Add(checkBoxColumn);

                DataGridViewTextBoxColumn numColumn = new DataGridViewTextBoxColumn
                {
                    Name = "NumPedido",
                    HeaderText = "Código",
                    DataPropertyName = "NumPedido",
                    DisplayIndex = 1
                };
                dataGridView8.Columns.Add(numColumn);
            }
        }

        private void InitializeDataGridView9()
        {
            if (chkFat.Checked)
            {
                label5.Visible = true;
                dataGridView9.Visible = true;
                dataGridView9.RowHeadersVisible = false;
                dataGridView9.Columns.Clear();

                DataGridViewTextBoxColumn codColumn = new DataGridViewTextBoxColumn
                {
                    Name = "CdProd",
                    HeaderText = "Código Produto",
                    DataPropertyName = "CdProd",
                    DisplayIndex = 0
                };
                dataGridView9.Columns.Add(codColumn);

                DataGridViewTextBoxColumn descColumn = new DataGridViewTextBoxColumn
                {
                    Name = "Descricao",
                    HeaderText = "Descricao",
                    DataPropertyName = "Descricao",
                    DisplayIndex = 1
                };
                dataGridView9.Columns.Add(descColumn);
            }
        }

        private void InitializeDataGridView10()
        {
            if (chkFat.Checked)
            {
                label6.Visible = true;
                dataGridView10.Visible = true;
                dataGridView10.RowHeadersVisible = false;
                dataGridView10.Columns.Clear();

                DataGridViewTextBoxColumn valColumn = new DataGridViewTextBoxColumn
                {
                    Name = "Valor",
                    HeaderText = "Valor Total",
                    DataPropertyName = "Valor",
                    DisplayIndex = 1
                };
                dataGridView10.Columns.Add(valColumn);

                DataGridViewTextBoxColumn dataColumn = new DataGridViewTextBoxColumn
                {
                    Name = "DataEmi",
                    HeaderText = "Descricao",
                    DataPropertyName = "DataEmi",
                    DisplayIndex = 2
                };
                dataGridView10.Columns.Add(dataColumn);

                DataGridViewTextBoxColumn tipoColumn = new DataGridViewTextBoxColumn
                {
                    Name = "TipoDoc",
                    HeaderText = "Forma de Pagamento",
                    DataPropertyName = "TipoDoc",
                    DisplayIndex = 2
                };
                dataGridView10.Columns.Add(tipoColumn);
            }
        }

        private void SetComponents()
        {
            _clienteProvider = new CliListProvider();
            _orcListProvider = new OrcListProvider();
            _orcItensProvider = new OrcItensProvider();
            _orcFinProvider = new OrcFinProvider();
            _pedidosProvider = new PedListProvider();
            _pedItensProvider = new PedItensProvider();
            _pedFinProvider = new PedFinProvider();
            _faturamentosProvider = new FatListProvider();
            _fatItensProvider = new FatItensProvider();
            _fatFinProvider = new FatFinProvider();
        }

        private void InitializeListClientes()
        {
            _clientes = new List<Cliente>();
            _orcamentos = new List<Orcamento>();
            _orcItens = new List<OrcItens>();
            _orcFin = new List<OrcFin>();
            _pedidos = new List<PedidoFaturamento>();
            _pedItens = new List<PedFatItens>();
            _pedFin = new List<PedFatFin>();
            _faturamentos = new List<PedidoFaturamento>();
            _fatItens = new List<PedFatItens>();
            _fatFin = new List<PedFatFin>();
        }
    }
}