
# Solar Panel Calculator API

Este projeto é uma API para calcular painéis solares, desenvolvida em .NET Core, projetada para ajudar os usuários a calcular o número de painéis solares necessários com base no consumo de energia e nas horas de luz solar. A API utiliza o modelo GPT-3.5-turbo da OpenAI para realizar cálculos e fornecer resultados em linguagem natural.

## **Índice**

- [Recursos](#recursos)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Requisitos Atendidos](#requisitos-atendidos)
- [Configuração e Instalação](#configuração-e-instalação)
- [Documentação da API](#documentação-da-api)
- [Testes](#testes)
- [Contribuidores](#contribuidores)
- [Notas Importantes](#notas-importantes)

## **Recursos**

- Autenticação e autorização de usuários usando tokens JWT.
- Operações CRUD para dispositivos/aparelhos.
- Cálculo de painéis solares necessários com base no consumo de energia dos aparelhos e nas horas de luz solar.
- Integração com GPT-3.5-turbo da OpenAI para cálculos inteligentes.
- Práticas de código limpo e padrões de design.
- Documentação completa da API com Swagger.
- Testes unitários e de integração usando xUnit e Moq.
- Integração com banco de dados Oracle.

## **Tecnologias Utilizadas**

- **.NET Core 8**
- **C#**
- **Entity Framework Core**
- **Banco de Dados Oracle**
- **API GPT-3.5-turbo da OpenAI**
- **Swagger para Documentação de API**
- **xUnit e Moq para Testes**
- **Padrões de Design**: Repository, Unit of Work, Injeção de Dependência
- **Arquitetura Limpa**: Separação de camadas (Domain, Application, Presentation, Migrations)

## **Estrutura do Projeto**

O projeto está organizado nas seguintes camadas:

- **Application**: Contém serviços, objetos de transferência de dados (DTOs) e helpers.
  - `DTOs`
  - `Helpers`
  - `Services`
- **Domain**: Contém interfaces e repositórios.
  - `Interfaces`
  - `Repositories`
- **Migrations**: Contém as migrações do banco de dados.
- **Presentation**: Contém os controladores e middlewares.
  - `Controllers`
  - `Middlewares`

## **Requisitos Atendidos**

- **Padrões de Design em .NET**: Implementados os padrões Repository, Unit of Work e Injeção de Dependência.
- **Documentação e Testes Automatizados de API**: A API está totalmente documentada com Swagger. Testes automatizados foram escritos usando xUnit e Moq.
- **Qualidade de Código e Análise Estática**: Segue boas práticas de código limpo, princípios SOLID e inclui análise estática.
- **Banco de Dados**: Usa banco de dados Oracle para persistência de dados.
- **Implementação de IA Generativa com .NET**: Integração com GPT-3.5-turbo da OpenAI para cálculos inteligentes.

## **Configuração e Instalação**

### **Pré-requisitos**

- **.NET 8 SDK** instalado
- Acesso ao Banco de Dados Oracle
- **Visual Studio 2022** ou IDE compatível
- **Git**

### **Passos**

1. **Clone o Repositório**

   ```bash
   git clone https://github.com/<seu-repositorio>/SolarPanelCalculatorApi.git
   ```

2. **Configure o Banco de Dados**

   Atualize a string de conexão no `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "User Id=<usuario>;Password=<senha>;Data Source=<data_source>"
   }
   ```

3. **Configure a Chave da API OpenAI**

   Adicione sua chave da API OpenAI no `appsettings.json`:

   ```json
   "OpenAI": {
     "ApiKey": "sua-chave-openai"
   }
   ```

4. **Execute as Migrações do Banco de Dados**

   ```bash
   dotnet ef database update
   ```

5. **Execute a Aplicação**

   ```bash
   dotnet run
   ```

6. **Acesse o Swagger UI**

   Navegue para `https://localhost:<porta>/swagger` para visualizar a documentação da API.

### **Front-End**

Para uma experiência completa, inicie o projeto front-end disponível no seguinte repositório:

[Repositório Front-End no GitHub](https://github.com/rozyar/Mobile-Global-Solution)

Siga as instruções do repositório para configurar e executar o front-end.

## **Documentação da API**

A API está documentada usando Swagger. Você pode acessar a documentação executando a aplicação e navegando para `/swagger` no navegador.

### **Endpoints Disponíveis**

#### **Autenticação**

- **POST /api/users/authenticate**: Autentica um usuário e retorna um token JWT.
- **POST /api/users/register**: Registra um novo usuário.

#### **Aparelhos**

- **GET /api/appliances**: Obtém todos os aparelhos do usuário autenticado.
- **POST /api/appliances**: Adiciona um novo aparelho.
- **PUT /api/appliances/{id}**: Atualiza um aparelho existente.
- **DELETE /api/appliances/{id}**: Exclui um aparelho.

#### **Análises**

- **GET /api/analyses**: Obtém todas as análises do usuário autenticado.
- **POST /api/analyses**: Cria uma nova análise.
- **DELETE /api/analyses/{id}**: Exclui uma análise.

## **Testes**

Os testes unitários e de integração foram escritos usando xUnit e Moq. Para executar os testes:

```bash
dotnet test
```

## **Contribuidores**

- **Razyel Ferrari (RM551875)** - GitHub: [irazyel](https://github.com/irazyel)
- **Rayzor Anael (RM551832)** - GitHub: [rozyar](https://github.com/rozyar)
- **Derick Araújo (RM551007)** - GitHub: [dericki](https://github.com/dericki)
- **Kalel Schlichting (RM550620)** - GitHub: [K413l](https://github.com/K413l)

## **Notas Importantes**

### **Uso de OpenAI LLM**

Este projeto utiliza o modelo GPT-3.5-turbo da OpenAI para realizar cálculos inteligentes sobre o número de painéis solares necessários. Esta decisão foi tomada para fornecer uma resposta mais dinâmica e em linguagem natural aos usuários. Essa abordagem substitui o uso do UML.NET conforme especificado nos requisitos.
