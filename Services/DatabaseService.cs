// =============================================================
// DatabaseService - Serviço de Banco de Dados SQLite
// =============================================================
// Este arquivo gerencia todas as operações do banco de dados.
// O banco é criado automaticamente na pasta do executável.
// Funções disponíveis:
//   - Inicializar banco (criar tabelas se não existirem)
//   - Salvar/Buscar/Atualizar/Deletar Clientes
//   - Salvar/Buscar/Atualizar/Deletar Notas
// =============================================================

using System.IO;
using Microsoft.Data.Sqlite;
using ProjetoWPF.Models;

namespace ProjetoWPF.Services
{
    /// <summary>
    /// Serviço que gerencia o banco de dados SQLite.
    /// Usa o padrão Singleton (uma única instância para toda a aplicação).
    /// </summary>
    public class DatabaseService
    {
        // ---------- SINGLETON ----------
        // Garante que só existe uma instância do serviço
        
        private static DatabaseService? _instance;
        public static DatabaseService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseService();
                }
                return _instance;
            }
        }

        // ---------- CONFIGURAÇÃO DO BANCO ----------
        
        // Caminho do arquivo do banco de dados
        private readonly string _connectionString;

        // Construtor privado (só pode ser criado internamente)
        private DatabaseService()
        {
            // O banco fica na mesma pasta do executável
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blackteam.db");
            _connectionString = $"Data Source={dbPath}";
            
            // Cria as tabelas na primeira execução
            InicializarBanco();
        }

        /// <summary>
        /// Cria as tabelas no banco se elas não existirem.
        /// Chamado automaticamente ao iniciar a aplicação.
        /// </summary>
        private void InicializarBanco()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            // ---------- TABELA DE CLIENTES ----------
            var cmdClientes = connection.CreateCommand();
            cmdClientes.CommandText = @"
                CREATE TABLE IF NOT EXISTS Clientes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL,
                    Endereco TEXT,
                    Numero TEXT,
                    Bairro TEXT,
                    Cidade TEXT,
                    Telefone TEXT,
                    RG TEXT,
                    CPF TEXT
                )";
            cmdClientes.ExecuteNonQuery();

            // ---------- TABELA DE NOTAS ----------
            var cmdNotas = connection.CreateCommand();
            cmdNotas.CommandText = @"
                CREATE TABLE IF NOT EXISTS Notas (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    NumeroNota TEXT NOT NULL,
                    ClienteId INTEGER,
                    ClienteNome TEXT,
                    ClienteEndereco TEXT,
                    ClienteNumero TEXT,
                    ClienteBairro TEXT,
                    ClienteCidade TEXT,
                    ClienteTelefone TEXT,
                    ClienteRG TEXT,
                    ClienteCPF TEXT,
                    DataEvento TEXT,
                    DataProva TEXT,
                    DataRetirar TEXT,
                    DataDevolucao TEXT,
                    DescricaoProdutos TEXT,
                    Valor REAL,
                    Sinal REAL,
                    Restante REAL,
                    Gravata INTEGER,
                    Sapato INTEGER,
                    Clutch INTEGER,
                    Estola INTEGER,
                    Camisa INTEGER,
                    Colete INTEGER,
                    DataContagem TEXT,
                    Atendente TEXT,
                    Locatario TEXT,
                    DataCriacao TEXT
                )";
            cmdNotas.ExecuteNonQuery();
        }

        // ==========================================================
        // OPERAÇÕES DE CLIENTES
        // ==========================================================

        /// <summary>
        /// Salva um novo cliente no banco de dados.
        /// </summary>
        /// <param name="cliente">Objeto Cliente com os dados</param>
        /// <returns>ID do cliente criado</returns>
        public int SalvarCliente(Cliente cliente)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Clientes (Nome, Endereco, Numero, Bairro, Cidade, Telefone, RG, CPF)
                VALUES (@Nome, @Endereco, @Numero, @Bairro, @Cidade, @Telefone, @RG, @CPF);
                SELECT last_insert_rowid();";

            // Adiciona os parâmetros (evita SQL Injection)
            cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@Endereco", cliente.Endereco);
            cmd.Parameters.AddWithValue("@Numero", cliente.Numero);
            cmd.Parameters.AddWithValue("@Bairro", cliente.Bairro);
            cmd.Parameters.AddWithValue("@Cidade", cliente.Cidade);
            cmd.Parameters.AddWithValue("@Telefone", cliente.Telefone);
            cmd.Parameters.AddWithValue("@RG", cliente.RG);
            cmd.Parameters.AddWithValue("@CPF", cliente.CPF);

            // Retorna o ID gerado
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// Busca todos os clientes cadastrados.
        /// </summary>
        /// <returns>Lista de todos os clientes</returns>
        public List<Cliente> BuscarTodosClientes()
        {
            var clientes = new List<Cliente>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Clientes ORDER BY Nome";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                clientes.Add(new Cliente
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Endereco = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Numero = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Bairro = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Cidade = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Telefone = reader.IsDBNull(6) ? "" : reader.GetString(6),
                    RG = reader.IsDBNull(7) ? "" : reader.GetString(7),
                    CPF = reader.IsDBNull(8) ? "" : reader.GetString(8)
                });
            }

            return clientes;
        }

        /// <summary>
        /// Busca um cliente pelo ID.
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <returns>Cliente encontrado ou null</returns>
        public Cliente? BuscarClientePorId(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Clientes WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Cliente
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Endereco = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Numero = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Bairro = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Cidade = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Telefone = reader.IsDBNull(6) ? "" : reader.GetString(6),
                    RG = reader.IsDBNull(7) ? "" : reader.GetString(7),
                    CPF = reader.IsDBNull(8) ? "" : reader.GetString(8)
                };
            }

            return null;
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        /// <param name="cliente">Cliente com os dados atualizados</param>
        public void AtualizarCliente(Cliente cliente)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE Clientes SET
                    Nome = @Nome,
                    Endereco = @Endereco,
                    Numero = @Numero,
                    Bairro = @Bairro,
                    Cidade = @Cidade,
                    Telefone = @Telefone,
                    RG = @RG,
                    CPF = @CPF
                WHERE Id = @Id";

            cmd.Parameters.AddWithValue("@Id", cliente.Id);
            cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@Endereco", cliente.Endereco);
            cmd.Parameters.AddWithValue("@Numero", cliente.Numero);
            cmd.Parameters.AddWithValue("@Bairro", cliente.Bairro);
            cmd.Parameters.AddWithValue("@Cidade", cliente.Cidade);
            cmd.Parameters.AddWithValue("@Telefone", cliente.Telefone);
            cmd.Parameters.AddWithValue("@RG", cliente.RG);
            cmd.Parameters.AddWithValue("@CPF", cliente.CPF);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Deleta um cliente do banco.
        /// </summary>
        /// <param name="id">ID do cliente a deletar</param>
        public void DeletarCliente(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Clientes WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }

        // ==========================================================
        // OPERAÇÕES DE NOTAS
        // ==========================================================

        /// <summary>
        /// Salva uma nova nota no banco de dados.
        /// </summary>
        /// <param name="nota">Objeto Nota com os dados</param>
        /// <returns>ID da nota criada</returns>
        public int SalvarNota(Nota nota)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Notas (
                    NumeroNota, ClienteId, ClienteNome, ClienteEndereco, ClienteNumero,
                    ClienteBairro, ClienteCidade, ClienteTelefone, ClienteRG, ClienteCPF,
                    DataEvento, DataProva, DataRetirar, DataDevolucao, DescricaoProdutos,
                    Valor, Sinal, Restante, Gravata, Sapato, Clutch, Estola, Camisa, Colete,
                    DataContagem, Atendente, Locatario, DataCriacao
                ) VALUES (
                    @NumeroNota, @ClienteId, @ClienteNome, @ClienteEndereco, @ClienteNumero,
                    @ClienteBairro, @ClienteCidade, @ClienteTelefone, @ClienteRG, @ClienteCPF,
                    @DataEvento, @DataProva, @DataRetirar, @DataDevolucao, @DescricaoProdutos,
                    @Valor, @Sinal, @Restante, @Gravata, @Sapato, @Clutch, @Estola, @Camisa, @Colete,
                    @DataContagem, @Atendente, @Locatario, @DataCriacao
                );
                SELECT last_insert_rowid();";

            // Adiciona todos os parâmetros
            cmd.Parameters.AddWithValue("@NumeroNota", nota.NumeroNota);
            cmd.Parameters.AddWithValue("@ClienteId", nota.ClienteId);
            cmd.Parameters.AddWithValue("@ClienteNome", nota.ClienteNome);
            cmd.Parameters.AddWithValue("@ClienteEndereco", nota.ClienteEndereco);
            cmd.Parameters.AddWithValue("@ClienteNumero", nota.ClienteNumero);
            cmd.Parameters.AddWithValue("@ClienteBairro", nota.ClienteBairro);
            cmd.Parameters.AddWithValue("@ClienteCidade", nota.ClienteCidade);
            cmd.Parameters.AddWithValue("@ClienteTelefone", nota.ClienteTelefone);
            cmd.Parameters.AddWithValue("@ClienteRG", nota.ClienteRG);
            cmd.Parameters.AddWithValue("@ClienteCPF", nota.ClienteCPF);
            cmd.Parameters.AddWithValue("@DataEvento", nota.DataEvento);
            cmd.Parameters.AddWithValue("@DataProva", nota.DataProva);
            cmd.Parameters.AddWithValue("@DataRetirar", nota.DataRetirar);
            cmd.Parameters.AddWithValue("@DataDevolucao", nota.DataDevolucao);
            cmd.Parameters.AddWithValue("@DescricaoProdutos", nota.DescricaoProdutos);
            cmd.Parameters.AddWithValue("@Valor", nota.Valor);
            cmd.Parameters.AddWithValue("@Sinal", nota.Sinal);
            cmd.Parameters.AddWithValue("@Restante", nota.Restante);
            cmd.Parameters.AddWithValue("@Gravata", nota.Gravata ? 1 : 0);
            cmd.Parameters.AddWithValue("@Sapato", nota.Sapato ? 1 : 0);
            cmd.Parameters.AddWithValue("@Clutch", nota.Clutch ? 1 : 0);
            cmd.Parameters.AddWithValue("@Estola", nota.Estola ? 1 : 0);
            cmd.Parameters.AddWithValue("@Camisa", nota.Camisa ? 1 : 0);
            cmd.Parameters.AddWithValue("@Colete", nota.Colete ? 1 : 0);
            cmd.Parameters.AddWithValue("@DataContagem", nota.DataContagem);
            cmd.Parameters.AddWithValue("@Atendente", nota.Atendente);
            cmd.Parameters.AddWithValue("@Locatario", nota.Locatario);
            cmd.Parameters.AddWithValue("@DataCriacao", nota.DataCriacao.ToString("yyyy-MM-dd HH:mm:ss"));

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// Busca todas as notas cadastradas.
        /// </summary>
        /// <returns>Lista de todas as notas</returns>
        public List<Nota> BuscarTodasNotas()
        {
            var notas = new List<Nota>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Notas ORDER BY DataCriacao DESC";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                notas.Add(LerNota(reader));
            }

            return notas;
        }

        /// <summary>
        /// Busca uma nota pelo ID.
        /// </summary>
        /// <param name="id">ID da nota</param>
        /// <returns>Nota encontrada ou null</returns>
        public Nota? BuscarNotaPorId(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Notas WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return LerNota(reader);
            }

            return null;
        }

        /// <summary>
        /// Gera o próximo número de nota.
        /// </summary>
        /// <returns>Número formatado com 6 dígitos</returns>
        public string GerarProximoNumeroNota()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Notas";
            int count = Convert.ToInt32(cmd.ExecuteScalar());

            // Começa em 010001 e incrementa
            return (10001 + count).ToString("D6");
        }

        /// <summary>
        /// Método auxiliar para ler uma nota do banco.
        /// </summary>
        private Nota LerNota(SqliteDataReader reader)
        {
            return new Nota
            {
                Id = reader.GetInt32(0),
                NumeroNota = reader.IsDBNull(1) ? "" : reader.GetString(1),
                ClienteId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                ClienteNome = reader.IsDBNull(3) ? "" : reader.GetString(3),
                ClienteEndereco = reader.IsDBNull(4) ? "" : reader.GetString(4),
                ClienteNumero = reader.IsDBNull(5) ? "" : reader.GetString(5),
                ClienteBairro = reader.IsDBNull(6) ? "" : reader.GetString(6),
                ClienteCidade = reader.IsDBNull(7) ? "" : reader.GetString(7),
                ClienteTelefone = reader.IsDBNull(8) ? "" : reader.GetString(8),
                ClienteRG = reader.IsDBNull(9) ? "" : reader.GetString(9),
                ClienteCPF = reader.IsDBNull(10) ? "" : reader.GetString(10),
                DataEvento = reader.IsDBNull(11) ? "" : reader.GetString(11),
                DataProva = reader.IsDBNull(12) ? "" : reader.GetString(12),
                DataRetirar = reader.IsDBNull(13) ? "" : reader.GetString(13),
                DataDevolucao = reader.IsDBNull(14) ? "" : reader.GetString(14),
                DescricaoProdutos = reader.IsDBNull(15) ? "" : reader.GetString(15),
                Valor = reader.IsDBNull(16) ? 0 : reader.GetDecimal(16),
                Sinal = reader.IsDBNull(17) ? 0 : reader.GetDecimal(17),
                Restante = reader.IsDBNull(18) ? 0 : reader.GetDecimal(18),
                Gravata = reader.IsDBNull(19) ? false : reader.GetInt32(19) == 1,
                Sapato = reader.IsDBNull(20) ? false : reader.GetInt32(20) == 1,
                Clutch = reader.IsDBNull(21) ? false : reader.GetInt32(21) == 1,
                Estola = reader.IsDBNull(22) ? false : reader.GetInt32(22) == 1,
                Camisa = reader.IsDBNull(23) ? false : reader.GetInt32(23) == 1,
                Colete = reader.IsDBNull(24) ? false : reader.GetInt32(24) == 1,
                DataContagem = reader.IsDBNull(25) ? "" : reader.GetString(25),
                Atendente = reader.IsDBNull(26) ? "" : reader.GetString(26),
                Locatario = reader.IsDBNull(27) ? "" : reader.GetString(27),
                DataCriacao = reader.IsDBNull(28) ? DateTime.Now : DateTime.Parse(reader.GetString(28))
            };
        }
    }
}
