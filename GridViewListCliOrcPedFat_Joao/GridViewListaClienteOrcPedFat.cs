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
        private List<Cliente> _clientes = new List<Cliente>();
        private List<OrcamentoPedidoFaturamento> _orcpedfat = new List<OrcamentoPedidoFaturamento>();
        private List<Itens> _Itens = new List<Itens>();
        private List<Finan> _Fin = new List<Finan>();
        private CliListProvider _clienteProvider = new CliListProvider();
        private OrcItensProvider _orcItensProvider = new OrcItensProvider();
        private OrcListProvider _orcListProvider = new OrcListProvider();
        private OrcFinProvider _orcFinProvider = new OrcFinProvider();
        private PedListProvider _pedidosProvider = new PedListProvider();
        private PedItensProvider _pedItensProvider = new PedItensProvider();
        private PedFinProvider _pedFinProvider = new PedFinProvider();
        private FatListProvider _faturamentosProvider = new FatListProvider();
        private FatItensProvider _fatItensProvider = new FatItensProvider();
        private FatFinProvider _fatFinProvider = new FatFinProvider();

        public GridViewListaClienteOrcPedFat()
        {
            InitializeComponent();

            chkOrc.CheckedChanged += (object sender, System.EventArgs e) =>
            {
                CheckboxStateUpdate(chkOrc);
                btnFiltrar.Enabled = true;
                if (!chkOrc.Checked) { btnFiltrar.Enabled = false; }
            };

            chkPed.CheckedChanged += (object sender, System.EventArgs e) =>
            {
                CheckboxStateUpdate(chkPed);
                btnFiltrar.Enabled = true;
                if (!chkPed.Checked) { btnFiltrar.Enabled = false; }
            };

            chkFat.CheckedChanged += (object sender, System.EventArgs e) =>
            {
                CheckboxStateUpdate(chkFat);
                btnFiltrar.Enabled = true;
                if (!chkFat.Checked) { btnFiltrar.Enabled = false; }
            };
            chkCliente.CheckedChanged += (object sender, System.EventArgs e) =>
            {
                dataGridViewCliente.Enabled = true;
                LoadClients();
                chkSts.Enabled = false;
                chkData.Enabled = false;
                if (!chkCliente.Checked)
                {
                    dataGridViewCliente.Enabled = false;
                    dataGridViewCliente.DataSource = null;
                    dataGridViewCliente.Rows.Clear();
                    chkSts.Enabled = true;
                    chkData.Enabled = true;
                    chkCliente.Enabled = true;
                }
                
            };
            chkSts.CheckedChanged += (object sender, System.EventArgs e) =>
            {
                chkEnc.Enabled = true;
                chkAbt.Enabled = true;
                chkCliente.Enabled = false;
                chkData.Enabled = false;
                if (!chkSts.Checked)
                {
                    chkEnc.Enabled = false;
                    chkAbt.Enabled = false;
                    chkEnc.Checked = false;
                    chkAbt.Checked = false;
                    chkCliente.Enabled = true;
                    chkData.Enabled = true;
                }
            };
            chkData.CheckedChanged += (object sender, System.EventArgs e) =>
            {
                dtTimeIni.Enabled = true;
                dtTimeFim.Enabled = true;
                chkCliente.Enabled = false;
                chkSts.Enabled = false;
                if (!chkData.Checked)
                {
                    dtTimeFim.Enabled = false;
                    dtTimeFim.Value = DateTime.Today;
                    dtTimeIni.Enabled = false;
                    dtTimeIni.Value = DateTime.Today;
                    chkCliente.Enabled = true;
                    chkSts.Enabled = true;
                }
            };
            btnFiltrar.Click += (object sender, System.EventArgs e) => { SetParams(); };

            btnDetalhes.Click += (object sender, System.EventArgs e) => { SetDetails(); };

            btnLimpar.Click += (object sender, System.EventArgs e) =>
            {
                DataGridViewsExecuteClear();
                dataGridViewCliente.Enabled = false;
                dataGridViewCliente.DataSource = null;
                dataGridViewCliente.Rows.Clear();
                ClearLabels(label1, label2, label3, label5, label6);
                ClearControls();
                foreach (CheckBox chk in groupBox1.Controls.OfType<CheckBox>())
                {
                    chk.Checked = false;
                }
                foreach (CheckBox chk in groupBox2.Controls.OfType<CheckBox>())
                {
                    chk.Checked = false;
                }
                btnDetalhes.Enabled = false;
                btnLimpar.Enabled = false;
                btnFiltrar.Enabled = false;
            };
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
            using (var connectionManager = new SqlConnManager())
            using (SqlConnection connection = connectionManager.GetConnection())
            {
                List<string> selectedClientIds = new List<string>();
                List<string> selectedStatus = new List<string>();
                DateTime? dataInicio = null;
                DateTime? dataFim = null;

                if (chkCliente.Checked && _clientes != null)
                {
                    selectedClientIds = _clientes.Where(cliente => cliente.IsSelected)
                        .Select(cliente => cliente.CdCliente).ToList();
                }

                if (chkSts.Checked)
                {
                    if (chkEnc.Checked)
                    {
                        selectedStatus.Add("E");
                    }
                    if (chkAbt.Checked)
                    {
                        selectedStatus.Add("A");
                    }
                }
                if (chkData.Checked)
                {
                    dataInicio = dtTimeIni.Value.Date;
                    dataFim = dtTimeFim.Value.Date.AddDays(1).AddTicks(-1);
                }

                if (chkOrc.Checked)
                {
                    _orcpedfat = _orcListProvider.ListOrc(connection, selectedClientIds, selectedStatus, dataInicio,dataFim);
                    dataGridViewFiltros.DataSource = _orcpedfat;
                }

                if (chkPed.Checked)
                {
                    _orcpedfat = _pedidosProvider.ListPedidos(connection, selectedClientIds, selectedStatus, dataInicio, dataFim);
                    dataGridViewFiltros.DataSource = _orcpedfat;
                }

                if (chkFat.Checked)
                {
                    _orcpedfat = _faturamentosProvider.ListFaturamentos(connection, selectedClientIds, selectedStatus, dataInicio, dataFim);
                    dataGridViewFiltros.DataSource = _orcpedfat;
                }
                btnDetalhes.Enabled = true;
                InitializeDataGridViewFiltros();
            }
        }

        private void SetDetails()
        {
            if (_orcpedfat != null && chkOrc.Checked)
            {
                var selectedOrcIds = _orcpedfat.Where(orc => orc.IsSelected).Select(orc => orc.NumPedido).ToList();

                if (selectedOrcIds.Any())
                {
                    using (var connectionManager = new SqlConnManager())
                    {
                        SqlConnection connection = connectionManager.GetConnection();
                        _Itens = _orcItensProvider.ListOrcItens(connection, selectedOrcIds);
                        _Fin = _orcFinProvider.ListOrcFin(connection, selectedOrcIds);
                        dataGridViewItens.DataSource = _Itens;
                        dataGridViewFinan.DataSource = _Fin;
                        btnLimpar.Enabled = true;
                        InitializeDataGridViewItens();
                        InitializeDataGridViewFinan();
                    }
                }
            }

            if (_orcpedfat != null && chkPed.Checked)
            {
                var selectedPedIds = _orcpedfat.Where(ped => ped.IsSelected).Select(ped => ped.NumPedido).ToList();

                if (selectedPedIds.Any())
                {
                    using (var connectionManager = new SqlConnManager())
                    {
                        SqlConnection connection = connectionManager.GetConnection();
                        _Itens = _pedItensProvider.ListPedItens(connection, selectedPedIds);
                        _Fin = _pedFinProvider.ListPedFin(connection, selectedPedIds);
                        dataGridViewItens.DataSource = _Itens;
                        dataGridViewFinan.DataSource = _Fin;
                        btnLimpar.Enabled = true;
                        InitializeDataGridViewItens();
                        InitializeDataGridViewFinan();
                    }
                }
            }

            if (_orcpedfat != null && chkFat.Checked)
            {
                var selectedFatIds = _orcpedfat.Where(fat => fat.IsSelected).Select(fat => fat.NumPedido).ToList();

                if (selectedFatIds.Any())
                {
                    using (var connectionManager = new SqlConnManager())
                    {
                        SqlConnection connection = connectionManager.GetConnection();
                        _Itens = _fatItensProvider.ListFatItens(connection, selectedFatIds);
                        _Fin = _fatFinProvider.ListFatFin(connection, selectedFatIds);
                        dataGridViewItens.DataSource = _Itens;
                        dataGridViewFinan.DataSource = _Fin;
                        btnLimpar.Enabled = true;
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
            if (chkOrc.Checked) { label1.Visible = true; }
            else if (chkPed.Checked) { label2.Visible = true; }
            else { label3.Visible = true; }
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
            DataGridViewTextBoxColumn qtdColumn = new DataGridViewTextBoxColumn
            {
                Name = "QtdProduto",
                HeaderText = "Quantidade",
                DataPropertyName = "QtdProduto",
                DisplayIndex = 3
            };
            dataGridViewItens.Columns.Add(qtdColumn);
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
            _orcpedfat.Clear();
            _Itens.Clear();
            _Fin.Clear();
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
            foreach (var chk in groupBox1.Controls.OfType<CheckBox>())
            {
                if (chk != checkboxAtivo)
                {
                    chk.Enabled = !checkboxAtivo.Checked;
                }
            }

        }
    }
}
