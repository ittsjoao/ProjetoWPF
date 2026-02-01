# Sistema de Gestão de Notas Fiscais - WPF

Sistema completo para gerenciamento de notas fiscais de ternos e vestidos, com cadastro de clientes e configurações.

## 📋 Estrutura do Projeto

### Páginas Principais

1. **HomePage** - Página inicial com seleção de tipo de produto
2. **ClientesPage** - Cadastro e listagem de clientes
3. **NotasPage** - Histórico de notas fiscais com filtros
4. **ConfigPage** - Configurações do sistema

### Controles de Usuário

1. **TernoControl** - Formulário para criar nota de terno
2. **VestidoControl** - Formulário para criar nota de vestido

## 🎨 Características Visuais

### Design Moderno
- ✅ Interface limpa e profissional
- ✅ Cores harmoniosas e organizadas
- ✅ Ícones emoji para melhor UX
- ✅ Feedback visual ao passar o mouse
- ✅ Bordas arredondadas e sombras sutis

### Paleta de Cores
- **Azul (#3498DB)**: Botões de terno e ações principais
- **Rosa (#E91E63)**: Botões de vestido
- **Verde (#27AE60)**: Botões de salvar/sucesso
- **Vermelho (#E74C3C)**: Botões de excluir/cancelar
- **Cinza Claro (#F5F5F5)**: Fundo das páginas

## 🚀 Implementação

### 1. Substituir os Arquivos XAML

Substitua os arquivos XAML existentes pelos novos:

- `TernoControl.xaml`
- `VestidoControl.xaml`
- `NotasPage.xaml`
- `ConfigPage.xaml`
- `ClientesPage.xaml`
- `HomePage.xaml`

### 2. Adicionar o Arquivo de Modelos

Crie uma pasta `Models` no projeto e adicione o arquivo `Models.cs` com as classes:
- `Cliente`
- `Produto` (abstrata)
- `Terno` (herda de Produto)
- `Vestido` (herda de Produto)
- `NotaFiscal`
- `ConfiguracaoSistema`
- `TipoNota` (enum)

### 3. Implementar os Code-Behind

Use o arquivo `ExemploImplementacao.cs` como referência para implementar os eventos e lógica nos arquivos `.xaml.cs`:

#### HomePage.xaml.cs
```csharp
private void Btn_Terno(object sender, RoutedEventArgs e)
{
    NotasFrame.Navigate(new TernoControl());
}

private void Btn_Vestido(object sender, RoutedEventArgs e)
{
    NotasFrame.Navigate(new VestidoControl());
}
```

#### TernoControl.xaml.cs e VestidoControl.xaml.cs
- Implementar evento `BtnSalvar_Click` para salvar nota
- Implementar evento `BtnCalcular_Click` para calcular total
- Implementar evento `BtnLimpar_Click` para limpar formulário
- Adicionar lógica de cálculo automático no `TextChanged`

#### ClientesPage.xaml.cs
- Implementar CRUD de clientes
- Implementar busca/filtro de clientes
- Implementar seleção e edição de clientes no DataGrid

#### ConfigPage.xaml.cs
- Implementar upload de logo
- Implementar salvamento de configurações
- Implementar carregamento de configurações

#### NotasPage.xaml.cs
- Implementar listagem de notas
- Implementar filtros por data e tipo
- Implementar ações de visualizar, imprimir e excluir

## 💾 Persistência de Dados

### Opções de Implementação:

1. **Banco de Dados Local (SQLite)**
   - Recomendado para produção
   - Instalar pacote: `System.Data.SQLite`
   - Criar tabelas para: Clientes, Notas, Configuracoes

2. **Arquivos JSON**
   - Bom para protótipos
   - Usar `System.Text.Json` ou `Newtonsoft.Json`
   - Criar arquivo para cada entidade

3. **Entity Framework**
   - Mais robusto
   - Instalar pacote: `Microsoft.EntityFrameworkCore.Sqlite`

### Exemplo com JSON:

```csharp
// Salvar
string json = JsonSerializer.Serialize(clientes);
File.WriteAllText("clientes.json", json);

// Carregar
string json = File.ReadAllText("clientes.json");
var clientes = JsonSerializer.Deserialize<List<Cliente>>(json);
```

## 📦 Pacotes NuGet Necessários

```
Install-Package System.Data.SQLite
Install-Package Newtonsoft.Json
```

## 🔧 Funcionalidades Implementadas

### HomePage
- ✅ Seleção visual entre Terno e Vestido
- ✅ Navegação para formulários específicos
- ✅ Design responsivo

### TernoControl / VestidoControl
- ✅ Formulário completo de nota fiscal
- ✅ Dados do cliente
- ✅ Detalhes do produto
- ✅ Cálculo automático de totais
- ✅ Desconto em percentual
- ✅ Validação de campos obrigatórios

### NotasPage
- ✅ Listagem em DataGrid
- ✅ Filtro por data (início e fim)
- ✅ Filtro por tipo (Terno/Vestido)
- ✅ Contador de notas
- ✅ Botões de ação (visualizar, imprimir, excluir)

### ClientesPage
- ✅ Formulário de cadastro completo
- ✅ Listagem em DataGrid
- ✅ Busca de clientes
- ✅ Edição de clientes existentes
- ✅ Contador de clientes
- ✅ Validação de campos obrigatórios

### ConfigPage
- ✅ Upload de logo da empresa
- ✅ Preview da logo
- ✅ Dados da empresa (CNPJ, endereço, etc)
- ✅ Preferências do sistema
- ✅ Checkboxes para configurações

## 📝 Próximos Passos

1. **Implementar Persistência**
   - Escolher método (SQLite recomendado)
   - Criar classes de acesso a dados
   - Implementar CRUD completo

2. **Adicionar Validações**
   - Validação de CPF/CNPJ
   - Validação de email
   - Validação de telefone
   - Máscaras de entrada

3. **Implementar Impressão**
   - Criar template de nota fiscal
   - Implementar geração de PDF
   - Adicionar visualização antes de imprimir

4. **Adicionar Relatórios**
   - Relatório de vendas por período
   - Relatório de clientes mais frequentes
   - Gráficos de desempenho

5. **Melhorias Adicionais**
   - Backup automático
   - Export para Excel
   - Envio de nota por email
   - Dashboard com estatísticas

## 🎯 Dicas de Implementação

1. **Use ViewModel Pattern**: Separe a lógica da interface
2. **Implemente INotifyPropertyChanged**: Para binding reativo
3. **Use Commands**: Em vez de eventos diretos
4. **Adicione Try-Catch**: Em todas as operações críticas
5. **Implemente Logging**: Para rastrear erros
6. **Use async/await**: Para operações de I/O

## 📚 Recursos Úteis

- [WPF Tutorial](https://docs.microsoft.com/pt-br/dotnet/desktop/wpf/)
- [SQLite Tutorial](https://www.sqlitetutorial.net/)
- [MVVM Pattern](https://docs.microsoft.com/pt-br/xamarin/xamarin-forms/enterprise-application-patterns/mvvm)

## 🆘 Suporte

Em caso de dúvidas:
1. Consulte a documentação do WPF
2. Verifique os exemplos em `ExemploImplementacao.cs`
3. Teste cada funcionalidade isoladamente

---

**Desenvolvido com ❤️ usando WPF e C#**
