// =============================================================
// VestidoControl.xaml.cs - Lógica do Formulário de Notas
// =============================================================
// Este arquivo contém toda a lógica do formulário:
//   - Carregar próximo número de nota
//   - Calcular valor restante automaticamente
//   - Salvar nota no banco de dados
//   - Gerar PDF da nota
//   - Limpar formulário
// =============================================================

using System;
using System.Windows;
using System.Windows.Controls;
using ProjetoWPF.Models;
using ProjetoWPF.Services;

namespace ProjetoWPF
{
    /// <summary>
    /// Controle para criar e gerenciar notas fiscais de vestidos/ternos.
    /// </summary>
    public partial class VestidoControl : UserControl
    {
        // Referências aos serviços (banco de dados e PDF)
        private readonly DatabaseService _dbService;
        private readonly PdfService _pdfService;

        public VestidoControl()
        {
            InitializeComponent();

            // Obtém as instâncias dos serviços
            _dbService = DatabaseService.Instance;
            _pdfService = PdfService.Instance;

            // Carrega o próximo número de nota disponível
            CarregarProximoNumeroNota();

            // Preenche a data atual no campo de contagem
            txtDataContagem.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Busca e exibe o próximo número de nota disponível.
        /// </summary>
        private void CarregarProximoNumeroNota()
        {
            try
            {
                txtNumeroNota.Text = _dbService.GerarProximoNumeroNota();
            }
            catch (Exception ex)
            {
                // Se der erro, usa um número padrão
                txtNumeroNota.Text = "010001";
                MessageBox.Show($"Aviso: {ex.Message}", "Aviso", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Calcula automaticamente o valor restante (Valor - Sinal).
        /// Chamado sempre que os campos Valor ou Sinal são alterados.
        /// </summary>
        private void CalcularRestante(object sender, TextChangedEventArgs e)
        {
            try
            {
                // Tenta converter os valores digitados
                decimal valor = 0;
                decimal sinal = 0;

                // Remove caracteres não numéricos e converte
                string valorTexto = txtValor.Text.Replace(".", ",");
                string sinalTexto = txtSinal.Text.Replace(".", ",");

                decimal.TryParse(valorTexto, out valor);
                decimal.TryParse(sinalTexto, out sinal);

                // Calcula e exibe o restante
                decimal restante = valor - sinal;
                txtRestante.Text = restante.ToString("N2");
            }
            catch
            {
                // Em caso de erro, mostra zero
                txtRestante.Text = "0,00";
            }
        }

        /// <summary>
        /// Limpa todos os campos do formulário.
        /// </summary>
        private void BtnLimpar_Click(object sender, RoutedEventArgs e)
        {
            // Pergunta ao usuário se realmente quer limpar
            var resultado = MessageBox.Show(
                "Tem certeza que deseja limpar todos os campos?",
                "Confirmar",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                LimparFormulario();
            }
        }

        /// <summary>
        /// Limpa todos os campos e carrega novo número de nota.
        /// </summary>
        private void LimparFormulario()
        {
            // Limpa os campos de texto
            txtNome.Text = "";
            txtEndereco.Text = "";
            txtNumeroEnd.Text = "";
            txtBairro.Text = "";
            txtCidade.Text = "";
            txtTelefone.Text = "";
            txtRG.Text = "";
            txtCPF.Text = "";
            txtDataEvento.Text = "";
            txtProva.Text = "";
            txtRetirar.Text = "";
            txtDevolucao.Text = "";
            txtDescricao.Text = "";
            txtValor.Text = "";
            txtSinal.Text = "";
            txtRestante.Text = "0,00";
            txtAtendente.Text = "";
            txtLocatario.Text = "";

            // Desmarca os checkboxes
            chkGravata.IsChecked = false;
            chkSapato.IsChecked = false;
            chkClutch.IsChecked = false;
            chkEstola.IsChecked = false;
            chkCamisa.IsChecked = false;
            chkColete.IsChecked = false;

            // Atualiza a data
            txtDataContagem.Text = DateTime.Now.ToString("dd/MM/yyyy");

            // Carrega novo número de nota
            CarregarProximoNumeroNota();
        }

        /// <summary>
        /// Salva a nota no banco de dados.
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

                // Cria o objeto Nota com os dados do formulário
                var nota = CriarNotaDoFormulario();

                // Salva no banco de dados
                int idNota = _dbService.SalvarNota(nota);

                // Mostra mensagem de sucesso
                MessageBox.Show($"Nota {nota.NumeroNota} salva com sucesso!\nID: {idNota}",
                    "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                // Pergunta se quer gerar o PDF
                var gerarPdf = MessageBox.Show(
                    "Deseja gerar o PDF da nota agora?",
                    "Gerar PDF",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (gerarPdf == MessageBoxResult.Yes)
                {
                    GerarPdfDaNota(nota);
                }

                // Limpa o formulário para nova nota
                LimparFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar a nota: {ex.Message}",
                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Gera o PDF da nota atual (sem salvar no banco).
        /// </summary>
        private void BtnGerarPdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Valida se o nome foi preenchido
                if (string.IsNullOrWhiteSpace(txtNome.Text))
                {
                    MessageBox.Show("Por favor, preencha pelo menos o nome do cliente.",
                        "Campo Obrigatório", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtNome.Focus();
                    return;
                }

                // Cria o objeto Nota com os dados do formulário
                var nota = CriarNotaDoFormulario();

                // Gera o PDF
                GerarPdfDaNota(nota);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar PDF: {ex.Message}",
                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Gera o PDF e abre no visualizador padrão.
        /// </summary>
        private void GerarPdfDaNota(Nota nota)
        {
            // Gera o PDF
            string caminhoArquivo = _pdfService.GerarPdf(nota);

            // Mostra mensagem com o caminho
            MessageBox.Show($"PDF gerado com sucesso!\n\nArquivo salvo em:\n{caminhoArquivo}",
                "PDF Gerado", MessageBoxButton.OK, MessageBoxImage.Information);

            // Abre o PDF
            _pdfService.AbrirPdf(caminhoArquivo);
        }

        /// <summary>
        /// Cria um objeto Nota com todos os dados do formulário.
        /// </summary>
        private Nota CriarNotaDoFormulario()
        {
            // Converte os valores numéricos
            decimal valor = 0;
            decimal sinal = 0;

            decimal.TryParse(txtValor.Text.Replace(".", ","), out valor);
            decimal.TryParse(txtSinal.Text.Replace(".", ","), out sinal);

            // Cria e retorna o objeto Nota
            return new Nota
            {
                NumeroNota = txtNumeroNota.Text,
                ClienteNome = txtNome.Text,
                ClienteEndereco = txtEndereco.Text,
                ClienteNumero = txtNumeroEnd.Text,
                ClienteBairro = txtBairro.Text,
                ClienteCidade = txtCidade.Text,
                ClienteTelefone = txtTelefone.Text,
                ClienteRG = txtRG.Text,
                ClienteCPF = txtCPF.Text,
                DataEvento = txtDataEvento.Text,
                DataProva = txtProva.Text,
                DataRetirar = txtRetirar.Text,
                DataDevolucao = txtDevolucao.Text,
                DescricaoProdutos = txtDescricao.Text,
                Valor = valor,
                Sinal = sinal,
                Restante = valor - sinal,
                Gravata = chkGravata.IsChecked ?? false,
                Sapato = chkSapato.IsChecked ?? false,
                Clutch = chkClutch.IsChecked ?? false,
                Estola = chkEstola.IsChecked ?? false,
                Camisa = chkCamisa.IsChecked ?? false,
                Colete = chkColete.IsChecked ?? false,
                DataContagem = txtDataContagem.Text,
                Atendente = txtAtendente.Text,
                Locatario = txtLocatario.Text,
                DataCriacao = DateTime.Now
            };
        }
    }
}
