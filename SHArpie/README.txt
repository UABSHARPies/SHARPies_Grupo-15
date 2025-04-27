# Projeto SHARPie Bot - Discord

## 📋 Descrição
O SHARPie é um bot de Discord desenvolvido segundo a arquitetura MVC (Model-View-Controller/Curry&Grace).  
Tem como principal objetivo contabilizar mensagens enviadas pelos utilizadores, responder a comandos e apresentar estatísticas como o Top 3 de utilizadores mais ativos.

O bot também é capaz de recuperar mensagens enviadas enquanto esteve offline, garantindo uma contagem correta e persistente.

---

## 🛠️ Tecnologias Utilizadas
- Linguagem: C# (.NET 6.0 ou superior)
- Biblioteca: DSharpPlus (versão 4.5.1)
- Persistência de Dados: Ficheiro JSON (System.Text.Json)

---

## 🧩 Estrutura de Ficheiros
- **Program.cs** - Ponto de entrada da aplicação.
- **Controller.cs** - Responsável pela lógica de orquestração entre a View e o Model.
- **Model.cs** - Gerência dos dados e regras de negócio.
- **DiscordView.cs** - Comunicação com o Discord (envio e receção de mensagens).
- **ComandoInvalidoException.cs** - Exceção personalizada para comandos inválidos.
- **interacoes.json** - Ficheiro de armazenamento dos dados de interações (criado automaticamente).

---

## 🚀 Como Executar
1. Instalar as dependências:
   - Biblioteca DSharpPlus
2. Inserir o **Token do Bot** no ficheiro `DiscordView.cs` (linha da configuração do `DiscordClient`).
3. Garantir que o canal de texto no servidor Discord se chama **comandosbot**.
4. Compilar o projeto em ambiente .NET 6.0 (ou superior).
5. Executar a aplicação.

**Nota:**  
Se existir o ficheiro `interacoes.json` no diretório, o bot irá carregar automaticamente os dados anteriores.

---

## 💬 Comandos Disponíveis
- `!help` – Lista todos os comandos disponíveis.
- `!mensagens` – Mostra quantas mensagens já enviaste.
- `!top` – Apresenta o Top 3 de utilizadores mais ativos.
- `!reset` – Limpa todas as interações registadas.

---

## ⚙️ Funcionalidades Implementadas
- Contabilização automática de mensagens enviadas.
- Recuperação de mensagens do histórico ao ligar o bot.
- Validação e resposta a comandos específicos.
- Persistência segura dos dados entre sessões.
- Tratamento de erros robusto com feedback amigável ao utilizador.

---

## 🛡️ Boas Práticas Seguidas
- Padrão arquitetural **MVC** modelo Curry & Grace
- Separação de responsabilidades (Model, View, Controller).
- Comentários e documentação interna no código.
- Tratamento de exceções personalizado.
- Persistência e atualização eficiente de dados.

---

## 📢 Observações
- O token do bot deve ser mantido privado. Nunca partilhar publicamente.
- Recomenda-se adicionar um ficheiro `.gitignore` para excluir `interacoes.json` de repositórios públicos.

---

## 👨‍💻 Autores
- Marco Correia
- Yrma Martins
- Inês Duarte
- Carina Machado
- Ricardo Balseiro


Projeto desenvolvido no contexto da unidade curricular de **Laboratório de Desenvolvimento de Software** - Empresa SimProgramming.

