// =============================================================
// ClientesPage.xaml.cs - Lógica da Página de Clientes
// =============================================================
// Este arquivo contém toda a lógica da página de clientes:
//   - Carregar lista de clientes do banco
//   - Adicionar novo cliente
//   - Editar cliente selecionado
//   - Excluir cliente
//   - Buscar clientes por nome
// =============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ProjetoWPF.Models;
using ProjetoWPF.Services;

namespace ProjetoWPF
{
    /// <summary>
    /// Página para gerenciamento de clientes.
    /// Permite cadastrar, editar, excluir e buscar clientes.
    /// </summary>
    public partial class ClientesPage : Page
    {
        // Referência ao serviço de banco de dados
        private readonly DatabaseService _dbService;

        // Lista de clientes carregados
        private List<Cliente> _todosClientes;

        // ID do cliente sendo editado (0 = novo cliente)
        private int _clienteIdEdicao = 0;

        public ClientesPage()
        {
            InitializeComponent();

            // Obtém a instância do serviço de banco
            _dbService = DatabaseService.Instance;
            _todosClientes = new List<Cliente>();

            // Carrega a lista de clientes ao abrir a página
            CarregarClientes();
        }

        /// <summary>
        /// Carrega todos os clientes do banco de dados.
        /// </summary>
        private void CarregarClientes()
        {
            try
            {
                // Busca todos os clientes
                _todosClientes = _dbService.BuscarTodosClientes();

                // Exibe na tabela
                dgClientes.ItemsSource = _todosClientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar clientes: {ex.Message}",
                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Filtra a lista de clientes conforme texto digitado.
        /// </summary>
        private void TxtBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            string busca = txtBusca.Text.ToLower();

            if (string.IsNullOrWhiteSpace(busca))
            {
                // Se busca vazia, mostra todos
                dgClientes.ItemsSource = _todosClientes;
            }
            else
            {
                // Filtra por nome, telefone ou CPF
                var filtrados = _todosClientes.Where(c =>
                    c.Nome.ToLower().Contains(busca) ||
                    c.Telefone.ToLower().Contains(busca) ||
                    c.CPF.ToLower().Contains(busca)
                ).ToList();

                dgClientes.ItemsSource = filtrados;
            }
        }

        /// <summary>
        /// Atualiza a lista de clientes.
        /// </summary>
        private void BtnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            CarregarClientes();
            LimparFormulario();
        }

        /// <summary>
        /// Quando um cliente é selecionado na tabela, preenche o formulário.
        /// </summary>
        private void DgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Verifica se há cliente selecionado
            if (dgClientes.SelectedItem is Cliente cliente)
            {
                // Preenche o formulário com os dados do cliente
                _clienteIdEdicao = cliente.Id;
                txtNome.Text = cliente.Nome;
                txtEndereco.Text = cliente.Endereco;
                txtNumero.Text = cliente.Numero;
                txtBairro.Text = cliente.Bairro;
                txtCidade.Text = cliente.Cidade;
                txtTelefone.Text = cliente.Telefone;
                txtRG.Text = cliente.RG;
                txtCPF.Text = cliente.CPF;

                // Muda o texto do botão para indicar edição
                btnSalvar.Content = "💾 Atualizar";
            }
        }

        /// <summary>
        /// Limpa o formulário e prepara para novo cadastro.
        /// </summary>
        private void BtnLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparFormulario();
        }

        /// <summary>
        /// Limpa todos os campos do formulário.
        /// </summary>
        private void LimparFormulario()
        {
            _clienteIdEdicao = 0;
            txtNome.Text = "";
            txtEndereco.Text = "";
            txtNumero.Text = "";
            txtBairro.Text = "";
            txtCidade.Text = "";
            txtTelefone.Text = "";
            txtRG.Text = "";
            txtCPF.Text = "";

            // Reseta o texto do botão
            btnSalvar.Content = "💾 Salvar";

            // Remove seleção da tabela
            dgClientes.SelectedItem = null;
        }

        /// <summary>
        /// Salva ou atualiza o cliente no banco.
        /// </summary>
        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Valida se o nome foi preenchido
                if (string.IsNullOrWhiteSpace(txtNome.Text))
                {
                    MessageBox.Show("Por favor, preencha o nome do cliente.",
                        "Campo Obrigatório", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtNome.Focus();
                    return;
                }

                // Cria o objeto cliente
                var cliente = new Cliente
                {
                    Id = _clienteIdEdicao,
                    Nome = txtNome.Text,
                    Endereco = txtEndereco.Text,
                    Numero = txtNumero.Text,
                    Bairro = txtBairro.Text,
                    Cidade = txtCidade.Text,
                    Telefone = txtTelefone.Text,
                    RG = txtRG.Text,
                    CPF = txtCPF.Text
                };

                if (_clienteIdEdicao == 0)
                {
                    // Novo cliente - insere no banco
                    int id = _dbService.SalvarCliente(cliente);
                    MessageBox.Show($"Cliente cadastrado com sucesso! ID: {id}",
                        "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Cliente existente - atualiza
                    _dbService.AtualizarCliente(cliente);
                    MessageBox.Show("Cliente atualizado com sucesso!",
                        "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Limpa formulário e recarrega lista
                LimparFormulario();
                CarregarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar cliente: {ex.Message}",
                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
