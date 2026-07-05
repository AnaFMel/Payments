# Projeto Tech Challenge FIAP - PaymentsWorker

Este projeto tem como objetivo receber mensagens referentes a adição de jogos no catálogo de usuários, simular o processo de pagamento e retornar evento com o resultado do pagamento processado.

## Estrutura do Projeto

- **PaymentsWorker (Deployment)**:  Aplicação Console que consome mensagens da fila orders-placed-queue, simula um processo de pagamento e publica o retorno como mensagem na fila payments-queue.
- **Configmap**: configurações relacionadas a fila orders-placed-queue.

## Tecnologias Utilizadas

- **.NET 10**: Framework principal
- **MassTransit.RabbitMQ 8.3.4**: Biblioteca para comunicação com RabbitMQ
