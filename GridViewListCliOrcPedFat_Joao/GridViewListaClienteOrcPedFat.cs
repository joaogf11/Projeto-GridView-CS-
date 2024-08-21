using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers;
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
        public GridViewListaClienteOrcPedFat()
        {
            InitializeComponent();
            SetComponents();
            InitializeListClientes();


           
            btnCarregar.Click += (object sender, System.EventArgs e) =>
            {

                _clientes = _clienteProvider.ListCliente();
                dataGridView1.DataSource = _clientes;
                InitializeDataGridView1();

                
            };

            btnFiltrar.Click += (object sender, System.EventArgs e) =>
            {
                if(_clientes != null && _clientes.Any()){
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
                        _orcamentos = _orcListProvider.ListOrc(selectedClientIds);
                        dataGridView2.DataSource = _orcamentos;
                        InitializeDataGridView2();
                    }
                }
            };

            btnDetalhes.Click += (object sender, System.EventArgs e) =>
            {
                if (_orcamentos != null && _orcamentos.Any())
                {
                    List<string> selectedOrcIds = new List<string>();

                    foreach (var orc in _orcamentos)
                    {
                        if (orc.IsSelected)
                        {
                            selectedOrcIds.Add(orc.NumPedido);
                        }
                    }

                    
                    if (selectedOrcIds.Any())
                    {
                        _orcItens = _orcItensProvider.ListOrcItens(selectedOrcIds);
                        dataGridView3.DataSource = _orcItens;
                        _orcFin = _orcFinProvider.ListOrcFin(selectedOrcIds);
                        dataGridView4.DataSource = _orcFin;
                        InitializeDataGridView3();
                        InitializeDataGridView4();
                    }
                }

            };
            
           btnLimpar.Click += (object sender, System.EventArgs e) =>
            {
                _clientes.Clear();
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                _orcamentos.Clear();
                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();
                _orcItens.Clear();
                dataGridView3.DataSource = null;
                dataGridView3.Rows.Clear();
                _orcFin.Clear();
                dataGridView4.DataSource = null;
                dataGridView4.Rows.Clear();
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
                Name = "NumPedido",
                HeaderText = "Código",
                DataPropertyName = "NumPedido",
                DisplayIndex = 1
            };
            dataGridView2.Columns.Add(numColumn);
        }
        private void InitializeDataGridView3()
        {
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
        private void InitializeDataGridView4()
        {
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


        private void SetComponents()
        {
            _clienteProvider = new CliListProvider();
            _orcListProvider = new OrcListProvider();
            _orcItensProvider = new OrcItensProvider();
            _orcFinProvider = new OrcFinProvider();
        }
        private void InitializeListClientes()
        {

            _clientes = new List<Cliente>();
            _orcamentos = new List<Orcamento>();
            _orcItens = new List<OrcItens>();
            _orcFin = new List<OrcFin>();
        }

    }
}
