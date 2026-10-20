# Projeto Tech Challenge FIAP - PaymentsWorker

Este projeto tem como objetivo receber mensagens referentes a adição de jogos no catálogo de usuários, simular o processo de pagamento e retornar evento com o resultado do pagamento processado.

## Estrutura do Projeto

- **PaymentsWorker (Deployment)**:  Aplicação Console que consome mensagens do tópico `orders-topic`, simula um processo de pagamento e publica o retorno como mensagem no tópico `payments-topic`.
- **Payments.Tests**: Testes unitários

## Tecnologias Utilizadas

- **.NET 10**: Framework principal
- **Azure.Extensions.AspNetCore.Configuration.Secrets 1.5.2**: Biblioteca para obtenção de segredos do chave de cofres na Azure Key Vault
- **Azure.Identity 1.21.0**: Biblioteca para autenticar nos serviços da Azure usando conta de Serviço (Microsoft Entrada ID)
- **MassTransit.Azure.ServiceBus.Core 8.3.4**: Biblioteca para abstração de comunicação com Azure Bus Service
