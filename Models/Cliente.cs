// =============================================================
// Modelo Cliente - Representa um cliente no sistema
// =============================================================
// Este arquivo define a estrutura de dados de um cliente.
// Cada cliente tem um ID único e informações pessoais/contato.
// Esses dados são salvos no banco de dados SQLite.
// =============================================================

namespace ProjetoWPF.Models
{
    /// <summary>
    /// Classe que representa um cliente no sistema.
    /// Contém todas as informações necessárias para o cadastro.
    /// </summary>
    public class Cliente
    {
        // ID único do cliente (gerado automaticamente pelo banco)
        public int Id { get; set; }
        
        // Nome completo do cliente
        public string Nome { get; set; } = string.Empty;
        
        // Endereço (rua/avenida)
        public string Endereco { get; set; } = string.Empty;
        
        // Número da residência
        public string Numero { get; set; } = string.Empty;
        
        // Bairro
        public string Bairro { get; set; } = string.Empty;
        
        // Cidade
        public string Cidade { get; set; } = string.Empty;
        
        // Telefone para contato
        public string Telefone { get; set; } = string.Empty;
        
        // RG (documento de identidade)
        public string RG { get; set; } = string.Empty;
        
        // CPF (documento fiscal)
        public string CPF { get; set; } = string.Empty;
    }
}
