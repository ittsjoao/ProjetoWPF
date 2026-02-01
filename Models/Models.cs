using System;
using System.Collections.Generic;

namespace ProjetoWPF.models
{
    /// <summary>
    /// Modelo para representar um Cliente
    /// </summary>
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Observacoes { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    /// <summary>
    /// Modelo base para Produtos
    /// </summary>
    public abstract class Produto
    {
        public int Id { get; set; }
        public string Modelo { get; set; }
        public string Cor { get; set; }
        public string Tecido { get; set; }
        public string Tamanho { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public string Observacoes { get; set; }
    }

    /// <summary>
    /// Modelo específico para Terno
    /// </summary>
    public class Terno : Produto
    {
        // Propriedades específicas de terno podem ser adicionadas aqui
    }

    /// <summary>
    /// Modelo específico para Vestido
    /// </summary>
    public class Vestido : Produto
    {
        public string Comprimento { get; set; }
        public string Decote { get; set; }
    }

    /// <summary>
    /// Enumeração para tipo de nota
    /// </summary>
    public enum TipoNota
    {
        Terno,
        Vestido
    }

    /// <summary>
    /// Modelo para Nota Fiscal
    /// </summary>
    public class NotaFiscal
    {
        public int Id { get; set; }
        public string NumeroNota { get; set; }
        public DateTime Data { get; set; }
        public TipoNota Tipo { get; set; }
        
        // Dados do Cliente
        public int ClienteId { get; set; }
        public string NomeCliente { get; set; }
        public string CpfCnpjCliente { get; set; }
        public string TelefoneCliente { get; set; }
        public string EmailCliente { get; set; }
        
        // Dados do Produto
        public string Produto { get; set; }
        public string Modelo { get; set; }
        public string Cor { get; set; }
        public string Tecido { get; set; }
        public string Tamanho { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Desconto { get; set; }
        public decimal ValorTotal { get; set; }
        public string Observacoes { get; set; }
        
        // Propriedades específicas para vestido
        public string Comprimento { get; set; }
        public string Decote { get; set; }
    }

    /// <summary>
    /// Modelo para Configurações do Sistema
    /// </summary>
    public class ConfiguracaoSistema
    {
        public int Id { get; set; }
        public string NomeEmpresa { get; set; }
        public string Cnpj { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string CaminhoLogo { get; set; }
        public bool NotificacoesAtivadas { get; set; }
        public bool BackupAutomatico { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
