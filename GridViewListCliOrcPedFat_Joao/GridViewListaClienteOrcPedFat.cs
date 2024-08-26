using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private List<OrcamentoPedidoFaturamento> _orcamentos;
        private OrcListProvider _orcListProvider;
        private List<Itens> _orcItens;
        private OrcItensProvider _orcItensProvider;
        private List<Finan> _orcFin;
        private OrcFinProvider _orcFinProvider;
        private List<OrcamentoPedidoFaturamento> _pedidos;
        private PedListProvider _pedidosProvider;
        private List<Itens> _pedItens;
        private PedItensProvider _pedItensProvider;
        private List<Finan> _pedFin;
        private PedFinProvider _pedFinProvider;
        private List<OrcamentoPedidoFaturamento> _faturamentos;
        private FatListProvider _faturamentosProvider;
        private List<Itens> _fatItens;
        private FatItensProvider _fatItensProvider;
        private List<Finan> _fatFin;
        private FatFinProvider _fatFinProvider;

        public GridViewListaClienteOrcPedFat()
        {
            InitializeComponent();
            SetComponents();
            InitializeLists();

            chkOrc.CheckedChanged += (object sender, System.EventArgs e) =>
            {
                CheckboxStateUpdate(chkOrc);
                btnFiltrar.Visible = true;
                if (!chkOrc.Checked)
                {
                    btnFiltrar.Visible = false;
                }
            };

            chkPed.CheckedChanged += (object sender, System.EventArgs e) =>
            {
                CheckboxStateUpdate(chkPed);
                btnFiltrar.Visible = true;
                if (!chkPed.Checked)
                {
                    btnFiltrar.Visible = false;
                }
            };

            chkFat.CheckedChanged += (object sender, System.EventArgs e) =>
            {
                CheckboxStateUpdate(chkFat);
                btnFiltrar.Visible = true;
                if (!chkFat.Checked)
                {
                    btnFiltrar.Visible = false;
                }
            };

            btnCarregar.Click += (object sender, System.EventArgs e) => { LoadClients(); };

            btnFiltrar.Click += (object sender, System.EventArgs e) => { SetParams(); };

            btnDetalhes.Click += async (object sender, System.EventArgs e) => { SetDetails(); };

            btnLimpar.Click += (object sender, System.EventArgs e) =>
            {
                DataGridViewsExecuteClear();
                ClearLabels(label1, label2, label3, label5, label6);
                ClearControls();
                foreach (Control control in this.Controls)
                {
                    if (control is CheckBox checkBox)
                    {
                        checkBox.Checked = false;
                    }
                    else if (control is Button button)
                    {
                        if (button.Name == "btnDetalhes" || button.Name == "btnLimpar" || button.Name == "btnFiltrar")
                        {
                            button.Visible = false;
                        }
                    }
                }
            };
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

        private void InitializeLists()
        {
            _clientes = new List<Cliente>();
            _orcamentos = new List<OrcamentoPedidoFaturamento>();
            _orcItens = new List<Itens>();
            _orcFin = new List<Finan>();
            _pedidos = new List<OrcamentoPedidoFaturamento>();
            _pedItens = new List<Itens>();
            _pedFin = new List<Finan>();
            _faturamentos = new List<OrcamentoPedidoFaturamento>();
            _fatItens = new List<Itens>();
            _fatFin = new List<Finan>();
        }

        private void LoadClients()
        {
            using (var connectionManager = new SqlConnManager())
            {
                SqlConnection connection = connectionManager.GetConnection();
                _clientes = _clienteProvider.ListCliente(connection);
                dataGridViewCliente.DataSource = _clientes;
                InitializeDataGridViewCliente();
                chkOrc.Enabled = true;
                chkPed.Enabled = true;
                chkFat.Enabled = true;
            }
        }

        private void SetParams()
        {
            if (_clientes == null || !_clientes.Any()) return;

            List<string> selectedClientIds = _clientes.Where(cliente => cliente.IsSelected)
                .Select(cliente => cliente.CdCliente).ToList();


            if (!selectedClientIds.Any()) return;

            using (var connectionManager = new SqlConnManager())
            using (SqlConnection connection = connectionManager.GetConnection())
            {
                if (chkOrc.Checked)
                {
                    _orcamentos = _orcListProvider.ListOrc(connection, selectedClientIds);
                    dataGridViewFiltros.DataSource = _orcamentos;
                }

                if (chkPed.Checked)
                {
                    _pedidos = _pedidosProvider.ListPedidos(connection, selectedClientIds);
                    dataGridViewFiltros.DataSource = _pedidos;
                }

                if (chkFat.Checked)
                {
                    _faturamentos = _faturamentosProvider.ListFaturamentos(connection, selectedClientIds);
                    dataGridViewFiltros.DataSource = _faturamentos;
                }

                btnDetalhes.Visible = true;
                InitializeDataGridViewFiltros();
            }
        }

        private void SetDetails()
        {
            if (_orcamentos != null && chkOrc.Checked)
            {
                var selectedOrcIds = _orcamentos.Where(orc => orc.IsSelected).Select(orc => orc.NumPedido).ToList();

                if (selectedOrcIds.Any())
                {
                    using (var connectionManager = new SqlConnManager())
                    {
                        SqlConnection connection = connectionManager.GetConnection();
                        _orcItens = _orcItensProvider.ListOrcItens(connection, selectedOrcIds);
                        _orcFin = _orcFinProvider.ListOrcFin(connection, selectedOrcIds);
                        dataGridViewItens.DataSource = _orcItens;
                        dataGridViewFinan.DataSource = _orcFin;
                        btnLimpar.Visible = true;
                        InitializeDataGridViewItens();
                        InitializeDataGridViewFinan();
                    }
                }
            }

            if (_pedidos != null && chkPed.Checked)
            {
                var selectedPedIds = _pedidos.Where(ped => ped.IsSelected).Select(ped => ped.NumPedido).ToList();

                if (selectedPedIds.Any())
                {
                    using (var connectionManager = new SqlConnManager())
                    {
                        SqlConnection connection = connectionManager.GetConnection();
                        _pedItens = _pedItensProvider.ListPedItens(connection, selectedPedIds);
                        _pedFin = _pedFinProvider.ListPedFin(connection, selectedPedIds);
                        dataGridViewItens.DataSource = _pedItens;
                        dataGridViewFinan.DataSource = _pedFin;
                        btnLimpar.Visible = true;
                        InitializeDataGridViewItens();
                        InitializeDataGridViewFinan();
                    }
                }
            }

            if (_faturamentos != null && chkFat.Checked)
            {
                var selectedFatIds = _faturamentos.Where(fat => fat.IsSelected).Select(fat => fat.NumPedido).ToList();

                if (selectedFatIds.Any())
                {
                    using (var connectionManager = new SqlConnManager())
                    {
                        SqlConnection connection = connectionManager.GetConnection();
                        _fatItens = _fatItensProvider.ListFatItens(connection, selectedFatIds);
                        _fatFin = _fatFinProvider.ListFatFin(connection, selectedFatIds);
                        dataGridViewItens.DataSource = _fatItens;
                        dataGridViewFinan.DataSource = _fatFin;
                        btnLimpar.Visible = true;
                        InitializeDataGridViewItens();
                        InitializeDataGridViewFinan();
                    }
                }
            }
        }

        private void InitializeDataGridViewCliente()
        {
            dataGridViewCliente.RowHeadersVisible = false;
            dataGridViewCliente.Columns.Clear();

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                Name = "IsSelected",
                HeaderText = "Selecionado",
                DataPropertyName = "IsSelected",
                DisplayIndex = 0
            };
            dataGridViewCliente.Columns.Add(checkBoxColumn);

            DataGridViewTextBoxColumn codigoColumn = new DataGridViewTextBoxColumn
            {
                Name = "CdCliente",
                HeaderText = "Código",
                DataPropertyName = "CdCliente",
                DisplayIndex = 1
            };
            dataGridViewCliente.Columns.Add(codigoColumn);

            DataGridViewTextBoxColumn razaoColumn = new DataGridViewTextBoxColumn
            {
                Name = "Razao",
                HeaderText = "Razão",
                DataPropertyName = "Razao",
                DisplayIndex = 2
            };
            dataGridViewCliente.Columns.Add(razaoColumn);
        }

        private void InitializeDataGridViewFiltros()
        {
            dataGridViewFiltros.Visible = true;
            dataGridViewFiltros.RowHeadersVisible = false;
            dataGridViewFiltros.Columns.Clear();

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                Name = "IsSelected",
                HeaderText = "Selecionado",
                DataPropertyName = "IsSelected",
                DisplayIndex = 0
            };
            dataGridViewFiltros.Columns.Add(checkBoxColumn);
            DataGridViewTextBoxColumn numColumn = new DataGridViewTextBoxColumn
            {
                Name = "NumPedido",
                HeaderText = "Código",
                DataPropertyName = "NumPedido",
                DisplayIndex = 1
            };
            dataGridViewFiltros.Columns.Add(numColumn);

            DataGridViewTextBoxColumn clienteColumn = new DataGridViewTextBoxColumn
            {
                Name = "Cliente",
                HeaderText = "Cliente",
                DataPropertyName = "Cliente",
                DisplayIndex = 2
            };
            dataGridViewFiltros.Columns.Add(clienteColumn);
            if (chkOrc.Checked)
            {
                label1.Visible = true;
            }
            else if (chkPed.Checked)
            {
                label2.Visible = true;
            }
            else
            {
                label3.Visible = true;
            }
        }

        private void InitializeDataGridViewItens()
        {
            label5.Visible = true;
            dataGridViewItens.Visible = true;
            dataGridViewItens.RowHeadersVisible = false;
            dataGridViewItens.Columns.Clear();

            DataGridViewTextBoxColumn codColumn = new DataGridViewTextBoxColumn
            {
                Name = "CdProduto",
                HeaderText = "Código Produto",
                DataPropertyName = "CdProduto",
                DisplayIndex = 0
            };
            dataGridViewItens.Columns.Add(codColumn);

            DataGridViewTextBoxColumn descColumn = new DataGridViewTextBoxColumn
            {
                Name = "Descricao",
                HeaderText = "Descricao",
                DataPropertyName = "Descricao",
                DisplayIndex = 1
            };
            dataGridViewItens.Columns.Add(descColumn);
            DataGridViewTextBoxColumn clienteColumn = new DataGridViewTextBoxColumn
            {
                Name = "NumPed",
                HeaderText = "Número",
                DataPropertyName = "NumPed",
                DisplayIndex = 2
            };
            dataGridViewItens.Columns.Add(clienteColumn);
        }

        private void InitializeDataGridViewFinan()
        {
            label6.Visible = true;
            dataGridViewFinan.Visible = true;
            dataGridViewFinan.RowHeadersVisible = false;
            dataGridViewFinan.Columns.Clear();

            DataGridViewTextBoxColumn valColumn = new DataGridViewTextBoxColumn
            {
                Name = "Valor",
                HeaderText = "Valor Total",
                DataPropertyName = "Valor",
                DisplayIndex = 1
            };
            dataGridViewFinan.Columns.Add(valColumn);

            DataGridViewTextBoxColumn dataColumn = new DataGridViewTextBoxColumn
            {
                Name = "DataEmi",
                HeaderText = "Descricao",
                DataPropertyName = "DataEmi",
                DisplayIndex = 2
            };
            dataGridViewFinan.Columns.Add(dataColumn);

            DataGridViewTextBoxColumn tipoColumn = new DataGridViewTextBoxColumn
            {
                Name = "TipoDoc",
                HeaderText = "Forma de Pagamento",
                DataPropertyName = "TipoDoc",
                DisplayIndex = 3
            };
            dataGridViewFinan.Columns.Add(tipoColumn);
            DataGridViewTextBoxColumn clienteColumn = new DataGridViewTextBoxColumn
            {
                Name = "NumPed",
                HeaderText = "Número",
                DataPropertyName = "NumPed",
                DisplayIndex = 4
            };
            dataGridViewFinan.Columns.Add(clienteColumn);
        }

        private void DataGridViewsExecuteClear()
        {
            DataGridViewCleanData(dataGridViewFiltros);
            DataGridViewCleanData(dataGridViewItens);
            DataGridViewCleanData(dataGridViewFinan);
        }

        private void DataGridViewCleanData(DataGridView dataGridView)
        {
            dataGridView.DataSource = null;
            dataGridView.Rows.Clear();
            dataGridView.Visible = false;
        }

        private void ClearControls()
        {
            _pedidos.Clear();
            _pedItens.Clear();
            _pedFin.Clear();
            _faturamentos.Clear();
            _fatItens.Clear();
            _fatFin.Clear();
            _orcFin.Clear();
            _orcItens.Clear();
            _orcamentos.Clear();
        }

        private void ClearLabels(params Label[] labels)
        {
            foreach (var label in labels)
            {
                label.Visible = false;
            }
        }

        private void CheckboxStateUpdate(CheckBox checkboxAtivo)
        {
            foreach (var chk in new[] { chkOrc, chkPed, chkFat })
            {
                if (chk != checkboxAtivo)
                {
                    chk.Enabled = !checkboxAtivo.Checked;
                }
            }
        }
    }
}