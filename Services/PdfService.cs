// =============================================================
// PdfService - Serviço para Geração de PDF
// =============================================================
// Este arquivo gera o PDF da nota fiscal no formato da nota física.
// Usa a biblioteca itext7 para criar o documento.
// O PDF é salvo na pasta "Notas" dentro da pasta do executável.
// =============================================================

using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using ProjetoWPF.Models;

namespace ProjetoWPF.Services
{
    /// <summary>
    /// Serviço responsável por gerar os PDFs das notas fiscais.
    /// O layout é baseado na nota física da Black Team.
    /// </summary>
    public class PdfService
    {
        // Instância única do serviço (Singleton)
        private static PdfService? _instance;
        public static PdfService Instance => _instance ??= new PdfService();

        // Pasta onde os PDFs serão salvos
        private readonly string _pastaNotas;

        private PdfService()
        {
            // Cria a pasta "Notas" se não existir
            _pastaNotas = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Notas");
            if (!Directory.Exists(_pastaNotas))
            {
                Directory.CreateDirectory(_pastaNotas);
            }
        }

        /// <summary>
        /// Gera o PDF de uma nota fiscal.
        /// </summary>
        /// <param name="nota">Dados da nota</param>
        /// <returns>Caminho do arquivo PDF gerado</returns>
        public string GerarPdf(Nota nota)
        {
            // Nome do arquivo: NumeroNota_NomeCliente_Data.pdf
            string nomeArquivo = $"{nota.NumeroNota}_{nota.ClienteNome.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string caminhoCompleto = Path.Combine(_pastaNotas, nomeArquivo);

            // Cria o documento PDF
            using var writer = new PdfWriter(caminhoCompleto);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf);

            // Configura as fontes
            var fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var fontTitulo = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLDITALIC);

            // ========== CABEÇALHO ==========
            
            // Título "Black Team"
            var titulo = new Paragraph("Black Team")
                .SetFont(fontTitulo)
                .SetFontSize(28)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(0);
            document.Add(titulo);

            // Subtítulo
            var subtitulo = new Paragraph("Ternos e Vestidos para festas")
                .SetFont(fontNormal)
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER);
            document.Add(subtitulo);

            // Telefones
            var telefones = new Paragraph("(31) 2524-3199 / 9 9341-3966")
                .SetFont(fontNormal)
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER);
            document.Add(telefones);

            // Instagram
            var instagram = new Paragraph("@@blackteam.vestidos")
                .SetFont(fontNormal)
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(5);
            document.Add(instagram);

            // Número da Nota (canto direito)
            var numeroNota = new Paragraph($"Nº {nota.NumeroNota}")
                .SetFont(fontBold)
                .SetFontSize(12)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontColor(ColorConstants.RED)
                .SetMarginBottom(10);
            document.Add(numeroNota);

            // ========== DADOS DO CLIENTE ==========
            
            // Tabela com os dados do cliente
            var tabelaCliente = new Table(2).UseAllAvailableWidth();
            tabelaCliente.SetMarginBottom(10);

            // Função auxiliar para adicionar linha na tabela
            void AdicionarLinha(string label, string valor, bool ocupaTudo = false)
            {
                var cellLabel = new Cell()
                    .Add(new Paragraph(label).SetFont(fontBold).SetFontSize(9))
                    .SetBorder(new SolidBorder(0.5f))
                    .SetPadding(3);

                if (ocupaTudo)
                {
                    var cellValor = new Cell()
                        .Add(new Paragraph(valor).SetFont(fontNormal).SetFontSize(9))
                        .SetBorder(new SolidBorder(0.5f))
                        .SetPadding(3);
                    tabelaCliente.AddCell(cellLabel);
                    tabelaCliente.AddCell(cellValor);
                }
                else
                {
                    var cellValor = new Cell()
                        .Add(new Paragraph(valor).SetFont(fontNormal).SetFontSize(9))
                        .SetBorder(new SolidBorder(0.5f))
                        .SetPadding(3);
                    tabelaCliente.AddCell(cellLabel);
                    tabelaCliente.AddCell(cellValor);
                }
            }

            // Nome completo (ocupa linha inteira)
            var cellNomeLabel = new Cell(1, 1)
                .Add(new Paragraph("Nome:").SetFont(fontBold).SetFontSize(9))
                .SetBorder(new SolidBorder(0.5f))
                .SetPadding(3)
                .SetWidth(60);
            var cellNomeValor = new Cell(1, 1)
                .Add(new Paragraph(nota.ClienteNome).SetFont(fontNormal).SetFontSize(9))
                .SetBorder(new SolidBorder(0.5f))
                .SetPadding(3);
            tabelaCliente.AddCell(cellNomeLabel);
            tabelaCliente.AddCell(cellNomeValor);

            AdicionarLinha("Endereço:", $"{nota.ClienteEndereco}, Nº {nota.ClienteNumero}");
            AdicionarLinha("Bairro:", nota.ClienteBairro);
            AdicionarLinha("Cidade:", nota.ClienteCidade);
            AdicionarLinha("Telefone:", nota.ClienteTelefone);
            AdicionarLinha("RG:", nota.ClienteRG);
            AdicionarLinha("CPF:", nota.ClienteCPF);

            document.Add(tabelaCliente);

            // ========== DATAS ==========
            
            var tabelaDatas = new Table(4).UseAllAvailableWidth();
            tabelaDatas.SetMarginBottom(10);

            void AdicionarCelulaData(string label, string valor)
            {
                tabelaDatas.AddCell(new Cell()
                    .Add(new Paragraph(label).SetFont(fontBold).SetFontSize(8))
                    .SetBorder(new SolidBorder(0.5f))
                    .SetPadding(2));
                tabelaDatas.AddCell(new Cell()
                    .Add(new Paragraph(valor).SetFont(fontNormal).SetFontSize(8))
                    .SetBorder(new SolidBorder(0.5f))
                    .SetPadding(2));
            }

            AdicionarCelulaData("Data do Evento:", nota.DataEvento);
            AdicionarCelulaData("Retirar:", nota.DataRetirar);
            AdicionarCelulaData("Prova:", nota.DataProva);
            AdicionarCelulaData("Devolução:", nota.DataDevolucao);

            document.Add(tabelaDatas);

            // ========== DESCRIÇÃO DOS PRODUTOS ==========
            
            var labelDescricao = new Paragraph("Descrição dos produtos:")
                .SetFont(fontBold)
                .SetFontSize(9)
                .SetMarginBottom(2);
            document.Add(labelDescricao);

            var descricaoBox = new Paragraph(nota.DescricaoProdutos)
                .SetFont(fontNormal)
                .SetFontSize(9)
                .SetBorder(new SolidBorder(0.5f))
                .SetPadding(5)
                .SetMinHeight(80)
                .SetMarginBottom(10);
            document.Add(descricaoBox);

            // ========== VALORES ==========
            
            var tabelaValores = new Table(3).UseAllAvailableWidth();
            tabelaValores.SetMarginBottom(10);

            void AdicionarValor(string label, decimal valor)
            {
                tabelaValores.AddCell(new Cell()
                    .Add(new Paragraph(label).SetFont(fontBold).SetFontSize(9))
                    .SetBorder(new SolidBorder(0.5f))
                    .SetPadding(3));
                tabelaValores.AddCell(new Cell()
                    .Add(new Paragraph($"R$ {valor:N2}").SetFont(fontNormal).SetFontSize(9))
                    .SetBorder(new SolidBorder(0.5f))
                    .SetPadding(3));
            }

            tabelaValores.AddCell(new Cell().Add(new Paragraph("Valor:").SetFont(fontBold).SetFontSize(9)).SetBorder(new SolidBorder(0.5f)).SetPadding(3));
            tabelaValores.AddCell(new Cell().Add(new Paragraph($"R$ {nota.Valor:N2}").SetFont(fontNormal).SetFontSize(9)).SetBorder(new SolidBorder(0.5f)).SetPadding(3));
            tabelaValores.AddCell(new Cell().Add(new Paragraph($"Sinal: R$ {nota.Sinal:N2}").SetFont(fontNormal).SetFontSize(9)).SetBorder(new SolidBorder(0.5f)).SetPadding(3));

            tabelaValores.AddCell(new Cell(1, 2).Add(new Paragraph($"Restante: R$ {nota.Restante:N2}").SetFont(fontBold).SetFontSize(9)).SetBorder(new SolidBorder(0.5f)).SetPadding(3));
            tabelaValores.AddCell(new Cell().SetBorder(Border.NO_BORDER));

            document.Add(tabelaValores);

            // ========== ACESSÓRIOS ==========
            
            var labelAcessorios = new Paragraph("Acessórios:")
                .SetFont(fontBold)
                .SetFontSize(9)
                .SetMarginBottom(2);
            document.Add(labelAcessorios);

            // Monta a linha de acessórios com checkboxes
            string MontarCheckbox(bool marcado, string nome)
            {
                return marcado ? $"[X] {nome}  " : $"[ ] {nome}  ";
            }

            var acessorios = new Paragraph()
                .SetFont(fontNormal)
                .SetFontSize(9)
                .Add(MontarCheckbox(nota.Gravata, "Gravata"))
                .Add(MontarCheckbox(nota.Sapato, "Sapato"))
                .Add(MontarCheckbox(nota.Clutch, "Clutch"))
                .Add(MontarCheckbox(nota.Estola, "Estola"))
                .Add(MontarCheckbox(nota.Camisa, "Camisa"))
                .Add(MontarCheckbox(nota.Colete, "Colete"))
                .SetMarginBottom(10);
            document.Add(acessorios);

            // ========== CLÁUSULAS ==========
            
            var clausulas = new Paragraph()
                .SetFont(fontNormal)
                .SetFontSize(6)
                .SetMarginBottom(10)
                .Add("Cláusula 1: Em caso de cancelamento do contrato não devolvemos o valor recebido.\n")
                .Add("Cláusula 2: O valor recebido fica disponível para ser reutilizado em caso de cancelamento tendo um aviso de 15 dias antes do evento.\n")
                .Add("Cláusula 3: Não repassamos pagamentos recebidos de um contrato para outro contrato.\n")
                .Add("Cláusula 4: O cuidado de uso da roupa é responsabilidade do cliente. É necessário o cliente fazer a conferência do vestido e dos itens na hora de buscar os produtos alugados.\n")
                .Add("Cláusula 5: Em caso de estragos pelo por mau uso do cliente será avaliado o dano, se houver perda total do produto o cliente fica responsável por fazer o pagamento total do produto.\n")
                .Add("Cláusula 6: O prazo de entrega deverá ser respeitado em ambas as partes tendo uma tolerância de 8 horas ambas as partes.\n")
                .Add("Cláusula 7: A devolução do produto caso exceda a data do contrato será cobrado uma multa no valor de 30% do valor do contrato.");
            document.Add(clausulas);

            // ========== RODAPÉ ==========
            
            var tabelaRodape = new Table(2).UseAllAvailableWidth();
            tabelaRodape.SetMarginBottom(5);

            tabelaRodape.AddCell(new Cell()
                .Add(new Paragraph($"Contagem, {nota.DataContagem}").SetFont(fontNormal).SetFontSize(9))
                .SetBorder(Border.NO_BORDER));
            tabelaRodape.AddCell(new Cell()
                .SetBorder(Border.NO_BORDER));

            tabelaRodape.AddCell(new Cell()
                .Add(new Paragraph($"Atendente: {nota.Atendente}").SetFont(fontNormal).SetFontSize(9))
                .SetBorder(new SolidBorder(ColorConstants.BLACK, 0.5f, 1))
                .SetBorderLeft(Border.NO_BORDER)
                .SetBorderRight(Border.NO_BORDER)
                .SetBorderBottom(Border.NO_BORDER)
                .SetPaddingTop(20));
            tabelaRodape.AddCell(new Cell()
                .Add(new Paragraph($"Locatário: {nota.Locatario}").SetFont(fontNormal).SetFontSize(9))
                .SetBorder(new SolidBorder(ColorConstants.BLACK, 0.5f, 1))
                .SetBorderLeft(Border.NO_BORDER)
                .SetBorderRight(Border.NO_BORDER)
                .SetBorderBottom(Border.NO_BORDER)
                .SetPaddingTop(20));

            document.Add(tabelaRodape);

            // Endereço final
            var endereco = new Paragraph("Av. Londres - nº 49. Loja 05 - Bairro Eldorado - Contagem/MG")
                .SetFont(fontBold)
                .SetFontSize(9)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop(10);
            document.Add(endereco);

            return caminhoCompleto;
        }

        /// <summary>
        /// Abre o PDF gerado no visualizador padrão do sistema.
        /// </summary>
        /// <param name="caminhoArquivo">Caminho do arquivo PDF</param>
        public void AbrirPdf(string caminhoArquivo)
        {
            if (File.Exists(caminhoArquivo))
            {
                // Abre o arquivo com o programa padrão
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = caminhoArquivo;
                process.StartInfo.UseShellExecute = true;
                process.Start();
            }
        }
    }
}
