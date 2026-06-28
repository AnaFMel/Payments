# Projeto Tech Challenge FIAP - PaymentsWorker

Este projeto tem como objetivo receber mensagens referentes a adição de jogos no catálogo de usuários, simular o processo de pagamento e retornar evento com o resultado do pagamento processado.

## Estrutura do Projeto

- **PaymentsWorker**:  Aplicação Console que consome mensagens da fila orders-placed-queue, simular um processo de pagamento e publica o retorno como mensagem na fila payments-queue.
- **docker-compose.yml**: Configuração do RabbitMQ.

## Pré-requisitos

- .NET 10 SDK
- Docker e Docker Compose

## Como executar

### 1. Iniciar todos os serviços (RabbitMQ)
```bash
# Docker-compose
docker-compose up -d
```

### 2. Executar o PaymentsWorker
```bash
cd PaymentsWorker
dotnet run
```

### Acesso ao RabbitMQ Management
- URL: http://localhost:15672
- Usuário: guest
- Senha: guest

## Tecnologias Utilizadas

- **.NET 10**: Framework principal
- **MassTransit.RabbitMQ 8.3.4**: Biblioteca para comunicação com RabbitMQ