// =============================================================
// Modelo Nota - Representa uma nota/contrato no sistema
// =============================================================
// Este arquivo define a estrutura de dados de uma nota fiscal.
// Cada nota está vinculada a um cliente e contém informações
// sobre o aluguel de roupas (vestido, terno, acessórios, etc).
// =============================================================

namespace ProjetoWPF.Models
{
    /// <summary>
    /// Classe que representa uma nota/contrato de aluguel.
    /// Contém todos os campos presentes na nota física.
    /// </summary>
    public class Nota
    {
        // ID único da nota (gerado automaticamente pelo banco)
        public int Id { get; set; }
        
        // Número da nota (ex: 010376) - visível no canto da nota
        public string NumeroNota { get; set; } = string.Empty;
        
        // ID do cliente vinculado a esta nota
        public int ClienteId { get; set; }
        
        // ---------- DADOS DO CLIENTE (copiados para a nota) ----------
        
        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteEndereco { get; set; } = string.Empty;
        public string ClienteNumero { get; set; } = string.Empty;
        public string ClienteBairro { get; set; } = string.Empty;
        public string ClienteCidade { get; set; } = string.Empty;
        public string ClienteTelefone { get; set; } = string.Empty;
        public string ClienteRG { get; set; } = string.Empty;
        public string ClienteCPF { get; set; } = string.Empty;
        
        // ---------- DATAS IMPORTANTES ----------
        
        // Data do evento (festa, casamento, etc)
        public string DataEvento { get; set; } = string.Empty;
        
        // Data da prova da roupa
        public string DataProva { get; set; } = string.Empty;
        
        // Data para retirar a roupa
        public string DataRetirar { get; set; } = string.Empty;
        
        // Data para devolver a roupa
        public string DataDevolucao { get; set; } = string.Empty;
        
        // ---------- DESCRIÇÃO DOS PRODUTOS ----------
        
        // Descrição detalhada das roupas alugadas
        public string DescricaoProdutos { get; set; } = string.Empty;
        
        // ---------- VALORES ----------
        
        // Valor total do aluguel
        public decimal Valor { get; set; }
        
        // Sinal (entrada/adiantamento)
        public decimal Sinal { get; set; }
        
        // Valor restante (Valor - Sinal)
        public decimal Restante { get; set; }
        
        // ---------- ACESSÓRIOS (checkboxes) ----------
        
        public bool Gravata { get; set; }
        public bool Sapato { get; set; }
        public bool Clutch { get; set; }
        public bool Estola { get; set; }
        public bool Camisa { get; set; }
        public bool Colete { get; set; }
        
        // ---------- RODAPÉ DA NOTA ----------
        
        // Data de contagem/emissão da nota
        public string DataContagem { get; set; } = string.Empty;
        
        // Nome do atendente que fez a nota
        public string Atendente { get; set; } = string.Empty;
        
        // Assinatura/nome do locatário (cliente)
        public string Locatario { get; set; } = string.Empty;
        
        // Data de criação da nota no sistema
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
