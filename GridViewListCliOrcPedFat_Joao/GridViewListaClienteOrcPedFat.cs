using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<OrcPedFat> _orcpedfat = new List<OrcPedFat>();
        private List<Itens> _itens = new List<Itens>();
        private List<Finan> _fin = new List<Finan>();
        private readonly CliListProvider _clienteProvider = new CliListProvider();
        private readonly OrcItensProvider _orcItensProvider = new OrcItensProvider();
        private readonly OrcListProvider _orcListProvider = new OrcListProvider();
        private readonly OrcFinProvider _orcFinProvider = new OrcFinProvider();
        private readonly PedListProvider _pedidosProvider = new PedListProvider();
        private readonly PedItensProvider _pedItensProvider = new PedItensProvider();
        private readonly PedFinProvider _pedFinProvider = new PedFinProvider();
        private readonly FatListProvider _faturamentosProvider = new FatListProvider();
        private readonly FatItensProvider _fatItensProvider = new FatItensProvider();
        private readonly FatFinProvider _fatFinProvider = new FatFinProvider();

        public GridViewListaClienteOrcPedFat()
        {
            InitializeComponent();

            chkOrc.CheckedChanged += (sender, e) =>
            {
                CheckboxStateUpdate(chkOrc);
                btnFiltrar.Enabled = true;
                if (!chkOrc.Checked) btnFiltrar.Enabled = false;
            };

            chkPed.CheckedChanged += (sender, e) =>
            {
                CheckboxStateUpdate(chkPed);
                btnFiltrar.Enabled = true;
                if (!chkPed.Checked) btnFiltrar.Enabled = false;
            };

            chkFat.CheckedChanged += (sender, e) =>
            {
                CheckboxStateUpdate(chkFat);
                btnFiltrar.Enabled = true;
                if (!chkFat.Checked) btnFiltrar.Enabled = false;
            };
            chkCliente.CheckedChanged += (sender, e) =>
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
            chkSts.CheckedChanged += (sender, e) =>
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
            chkData.CheckedChanged += (sender, e) =>
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
            btnFiltrar.Click += (sender, e) => { SetParams(); };

            btnDetalhes.Click += (sender, e) => { SetDetails(); };

            btnLimpar.Click += (sender, e) =>
            {
                DataGridViewsExecuteClear();
                dataGridViewCliente.Enabled = false;
                dataGridViewCliente.DataSource = null;
                dataGridViewCliente.Rows.Clear();
                ClearLabels(label1, label2, label3, label5, label6);
                ClearControls();
                foreach (var chk in groupBox1.Controls.OfType<CheckBox>()) chk.Checked = false;
                foreach (var chk in groupBox2.Controls.OfType<CheckBox>()) chk.Checked = false;
                btnDetalhes.Enabled = false;
                btnLimpar.Enabled = false;
                btnFiltrar.Enabled = false;
            };
        }

        private void LoadClients()
        {
            using (var connectionManager = new SqlConnManager())
            {
                var connection = connectionManager.GetConnection();
                _clientes = _clienteProvider.ListCliente(connection);
                dataGridViewCliente.DataSource = _clientes;
                InitializeDataGridViewCliente();
            }
        }

        private void SetParams()
        {
            using (var connectionManager = new SqlConnManager())
            using (var connection = connectionManager.GetConnection())
            {
                var selectedClientIds = new List<string>();
                var selectedStatus = new List<string>();
                DateTime? dataInicio = null;
                DateTime? dataFim = null;

                if (chkCliente.Checked && _clientes != null)
                    selectedClientIds = _clientes.Where(cliente => cliente.IsSelected)
                        .Select(cliente => cliente.CdCliente).ToList();

                if (chkSts.Checked)
                {
                    if (chkEnc.Checked) selectedStatus.Add("E");
                    if (chkAbt.Checked) selectedStatus.Add("A");
                }

                if (chkData.Checked)
                {
                    dataInicio = dtTimeIni.Value.Date;
                    dataFim = dtTimeFim.Value.Date.AddDays(1).AddTicks(-1);
                }

                if (chkOrc.Checked)
                {
                    _orcpedfat = _orcListProvider.ListOrc(connection, selectedClientIds, selectedStatus, dataInicio,
                        dataFim);
                    dataGridViewFiltros.DataSource = _orcpedfat;
                }

                if (chkPed.Checked)
                {
                    _orcpedfat = _pedidosProvider.ListPedidos(connection, selectedClientIds, selectedStatus, dataInicio,
                        dataFim);
                    dataGridViewFiltros.DataSource = _orcpedfat;
                }

                if (chkFat.Checked)
                {
                    _orcpedfat = _faturamentosProvider.ListFaturamentos(connection, selectedClientIds, selectedStatus,
                        dataInicio, dataFim);
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
                    using (var connectionManager = new SqlConnManager())
                    {
                        var connection = connectionManager.GetConnection();
                        _itens = _orcItensProvider.ListOrcItens(connection, selectedOrcIds);
                        _fin = _orcFinProvider.ListOrcFin(connection, selectedOrcIds);
                        dataGridViewItens.DataSource = _itens;
                        dataGridViewFinan.DataSource = _fin;
                        btnLimpar.Enabled = true;
                        InitializeDataGridViewItens();
                        InitializeDataGridViewFinan();
                    }
            }

            if (_orcpedfat != null && chkPed.Checked)
            {
                var selectedPedIds = _orcpedfat.Where(ped => ped.IsSelected).Select(ped => ped.NumPedido).ToList();

                if (selectedPedIds.Any())
                    using (var connectionManager = new SqlConnManager())
                    {
                        var connection = connectionManager.GetConnection();
                        _itens = _pedItensProvider.ListPedItens(connection, selectedPedIds);
                        _fin = _pedFinProvider.ListPedFin(connection, selectedPedIds);
                        dataGridViewItens.DataSource = _itens;
                        dataGridViewFinan.DataSource = _fin;
                        btnLimpar.Enabled = true;
                        InitializeDataGridViewItens();
                        InitializeDataGridViewFinan();
                    }
            }

            if (_orcpedfat != null && chkFat.Checked)
            {
                var selectedFatIds = _orcpedfat.Where(fat => fat.IsSelected).Select(fat => fat.NumPedido).ToList();

                if (selectedFatIds.Any())
                    using (var connectionManager = new SqlConnManager())
                    {
                        var connection = connectionManager.GetConnection();
                        _itens = _fatItensProvider.ListFatItens(connection, selectedFatIds);
                        _fin = _fatFinProvider.ListFatFin(connection, selectedFatIds);
                        dataGridViewItens.DataSource = _itens;
                        dataGridViewFinan.DataSource = _fin;
                        btnLimpar.Enabled = true;
                        InitializeDataGridViewItens();
                        InitializeDataGridViewFinan();
                    }
            }
        }

        private void InitializeDataGridViewCliente()
        {
            dataGridViewCliente.RowHeadersVisible = false;
            dataGridViewCliente.Columns.Clear();

            var checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                Name = "IsSelected",
                HeaderText = "Selecionado",
                DataPropertyName = "IsSelected",
                DisplayIndex = 0
            };
            dataGridViewCliente.Columns.Add(checkBoxColumn);

            var codigoColumn = new DataGridViewTextBoxColumn
            {
                Name = "CdCliente",
                HeaderText = "Código",
                DataPropertyName = "CdCliente",
                DisplayIndex = 1
            };
            dataGridViewCliente.Columns.Add(codigoColumn);

            var razaoColumn = new DataGridViewTextBoxColumn
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

            var checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                Name = "IsSelected",
                HeaderText = "Selecionado",
                DataPropertyName = "IsSelected",
                DisplayIndex = 0
            };
            dataGridViewFiltros.Columns.Add(checkBoxColumn);
            var numColumn = new DataGridViewTextBoxColumn
            {
                Name = "NumPedido",
                HeaderText = "Código",
                DataPropertyName = "NumPedido",
                DisplayIndex = 1
            };
            dataGridViewFiltros.Columns.Add(numColumn);

            var clienteColumn = new DataGridViewTextBoxColumn
            {
                Name = "Cliente",
                HeaderText = "Cliente",
                DataPropertyName = "Cliente",
                DisplayIndex = 2
            };
            dataGridViewFiltros.Columns.Add(clienteColumn);
            var statusColumn = new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                DisplayIndex = 3
            };
            dataGridViewFiltros.Columns.Add(statusColumn);
            if (chkOrc.Checked)
                label1.Visible = true;
            else if (chkPed.Checked)
                label2.Visible = true;
            else
                label3.Visible = true;
        }

        private void InitializeDataGridViewItens()
        {
            label5.Visible = true;
            dataGridViewItens.Visible = true;
            dataGridViewItens.RowHeadersVisible = false;
            dataGridViewItens.Columns.Clear();

            var codColumn = new DataGridViewTextBoxColumn
            {
                Name = "CdProduto",
                HeaderText = "Código Produto",
                DataPropertyName = "CdProduto",
                DisplayIndex = 0
            };
            dataGridViewItens.Columns.Add(codColumn);

            var descColumn = new DataGridViewTextBoxColumn
            {
                Name = "Descricao",
                HeaderText = "Descrição",
                DataPropertyName = "Descricao",
                DisplayIndex = 1
            };
            dataGridViewItens.Columns.Add(descColumn);
            var clienteColumn = new DataGridViewTextBoxColumn
            {
                Name = "NumPed",
                HeaderText = "Número Pedido",
                DataPropertyName = "NumPed",
                DisplayIndex = 2
            };
            dataGridViewItens.Columns.Add(clienteColumn);
            var qtdColumn = new DataGridViewTextBoxColumn
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

            var valColumn = new DataGridViewTextBoxColumn
            {
                Name = "Valor",
                HeaderText = "Valor Total",
                DataPropertyName = "Valor",
                DisplayIndex = 1
            };
            dataGridViewFinan.Columns.Add(valColumn);

            var dataColumn = new DataGridViewTextBoxColumn
            {
                Name = "DataEmi",
                HeaderText = "Data Emissão",
                DataPropertyName = "DataEmi",
                DisplayIndex = 2
            };
            dataGridViewFinan.Columns.Add(dataColumn);

            var tipoColumn = new DataGridViewTextBoxColumn
            {
                Name = "TipoDoc",
                HeaderText = "Forma de Pagamento",
                DataPropertyName = "TipoDoc",
                DisplayIndex = 3
            };
            dataGridViewFinan.Columns.Add(tipoColumn);
            var clienteColumn = new DataGridViewTextBoxColumn
            {
                Name = "NumPed",
                HeaderText = "Número Pedido",
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
            _itens.Clear();
            _fin.Clear();
        }

        private void ClearLabels(params Label[] labels)
        {
            foreach (var label in labels) label.Visible = false;
        }

        private void CheckboxStateUpdate(CheckBox checkboxAtivo)
        {
            foreach (var chk in groupBox1.Controls.OfType<CheckBox>())
                if (chk != checkboxAtivo)
                    chk.Enabled = !checkboxAtivo.Checked;
        }
    }
}