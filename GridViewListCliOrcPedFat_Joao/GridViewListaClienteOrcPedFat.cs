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
            InitializeLists();
            chkOrc.CheckedChanged += (object sender, System.EventArgs e) => { AtualizarEstadoCheckBoxes(chkOrc); };

            chkPed.CheckedChanged += (object sender, System.EventArgs e) => { AtualizarEstadoCheckBoxes(chkPed); };

            chkFat.CheckedChanged += (object sender, System.EventArgs e) => { AtualizarEstadoCheckBoxes(chkFat); };

            btnCarregar.Click += (object sender, System.EventArgs e) => { LoadClients(); };

            btnFiltrar.Click += (object sender, System.EventArgs e) =>
            {
                btnDetalhes.Visible = true;
                SetParams();
            };
            btnDetalhes.Click += async (object sender, System.EventArgs e) =>
            {
                btnLimpar.Visible = true;
                SetDetails();
            };
            btnLimpar.Click += (object sender, System.EventArgs e) =>
            {
                LimparDataGridViews();
                LimparLabels(label1, label2, label3, label5, label6);
                LimparControles();
                foreach (Control control in this.Controls)
                {
                    if (control is CheckBox checkBox) { checkBox.Checked = false; }
                    else if (control is Button button)
                    {
                        if (button.Name == "btnDetalhes" || button.Name == "btnLimpar" || button.Name == "btnFiltrar") { button.Visible = false; }
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

        private void LoadClients()
        {
            using (var connectionManager = new SqlConnManager())
            {
                SqlConnection connection = connectionManager.GetConnection();
                _clientes = _clienteProvider.ListCliente(connection);
                dataGridViewCliente.DataSource = _clientes;
                InitializeDataGridViewCliente();
            }
        }

        private void SetParams()
        {
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
                            dataGridViewFiltros.DataSource = _orcamentos;
                            InitializeDataGridViewFiltros();
                        }
                    }

                    if (chkPed.Checked)
                    {
                        using (var connectionManager = new SqlConnManager())
                        {
                            SqlConnection connection = connectionManager.GetConnection();
                            _pedidos = _pedidosProvider.ListPedidos(connection, selectedClientIds);
                            dataGridViewFiltros.DataSource = _pedidos;
                            InitializeDataGridViewFiltros();
                        }
                    }

                    if (chkFat.Checked)
                    {
                        using (var connectionManager = new SqlConnManager())
                        {
                            SqlConnection connection = connectionManager.GetConnection();
                            _faturamentos = _faturamentosProvider.ListFaturamentos(connection, selectedClientIds);
                            dataGridViewFiltros.DataSource = _faturamentos;
                            InitializeDataGridViewFiltros();
                        }
                    }
                }
            }
        }

        private async void SetDetails()
        {
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
                            dataGridViewItens.DataSource = _orcItens;
                            dataGridViewFinan.DataSource = _orcFin;
                            InitializeDataGridViewItens();
                            InitializeDataGridViewFinan();
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
                            dataGridViewItens.DataSource = _pedItens;
                            dataGridViewFinan.DataSource = _pedFin;
                            InitializeDataGridViewItens();
                            InitializeDataGridViewFinan();
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
                            dataGridViewItens.DataSource = _fatItens;
                            dataGridViewFinan.DataSource = _fatFin;
                            InitializeDataGridViewItens();
                            InitializeDataGridViewFinan();
                        }
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
            if (chkOrc.Checked)
            {
                label1.Visible = true;
                DataGridViewTextBoxColumn numColumn = new DataGridViewTextBoxColumn
                {
                    Name = "NumOrcamento",
                    HeaderText = "Código",
                    DataPropertyName = "NumOrcamento",
                    DisplayIndex = 1
                };
                dataGridViewFiltros.Columns.Add(numColumn);
            }
            else
            {
                if (chkPed.Checked)
                {
                    label2.Visible = true;
                }
                else
                {
                    label3.Visible = true;
                }

                DataGridViewTextBoxColumn numColumn = new DataGridViewTextBoxColumn
                {
                    Name = "NumPedido",
                    HeaderText = "Código",
                    DataPropertyName = "NumPedido",
                    DisplayIndex = 1
                };
                dataGridViewFiltros.Columns.Add(numColumn);
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
                DisplayIndex = 2
            };
            dataGridViewFinan.Columns.Add(tipoColumn);
        }

        private void LimparDataGridViews()
        {
            LimparDataGridView(dataGridViewFiltros);
            LimparDataGridView(dataGridViewItens);
            LimparDataGridView(dataGridViewFinan);
        }

        private void LimparDataGridView(DataGridView dataGridView)
        {
            dataGridView.DataSource = null;
            dataGridView.Rows.Clear();
            dataGridView.Visible = false;
        }

        private void LimparControles()
        {
            var campos = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                                            .Where(f => f.Name.StartsWith("_") 
                                                        && f.FieldType.IsGenericType 
                                                        && f.FieldType.GetGenericTypeDefinition() == typeof(List<>));

            foreach (var campo in campos)
            {
                var lista = campo.GetValue(this) as System.Collections.IList;
                lista?.Clear();
            }
        }

        private void LimparLabels(params Label[] labels)
        {
            foreach (var label in labels)
            {
                label.Visible = false;
            }
        }

        private void AtualizarEstadoCheckBoxes(CheckBox checkboxAtivo)
        {
            btnFiltrar.Visible = true;
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