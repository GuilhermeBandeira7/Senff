<h1 align="center"> Senff Library </h1>

## 🎯 Sobre o projeto
 Solução de mensageria, com microsserviços em .NET e RabbitMQ

## 🔨 Funcionalidades do projeto

A Biblioteca SenffLib (disponível como nuget package https://www.nuget.org/packages/RabbitMqSenffLib/) é responsável por abstrair a integração com o RabbitMQ.
A solução possui uma minimal API feita em .NET8 que usa a SenffLib para enviar mensagens ao Broker sempre que um produto ou fornecedor for adicionado no 
bando de dados. O Projeto RabbitMqConsumer usa a SenffLib para ouvir as mensagens de determinada fila.

## 🛠️ Rodar Com Docker

 Para rodar o projeto com o docker é necessário entrar na pasta Senff.Api, e buildar o docker file para gerar a imagem com o seguinte comando: docker build -t senffapi:1.0
 Em seguida, voltar para o pasta raiz do projeto e subir o docker compose com o seguinte comando: docker-compose up
 Feito isso, a API estará disponível na porta 5000 do localhost e o rabbit mq na porta 5672.
 É necessário criar as queues e exchange acessando o seguinte end-point: localhost:5000/rabbit
<br>

## 📚 Documentação e arquitetura

 Ao subir a API e acessar a URL, o usuário será direcionado ao browser com a mensagem "OK" de health-check.
A Api está documentada no Swagger, que pode ser acessada colocando /swagger no final da URL do localhost. <br>

 A arquitetura do projeto usa microsserviços para se comunicar com o Broker do RabbitMQ, que por sua vez disponibiliza a fila de mensagens para os consumidores.
A minimal API acessa o banco de dados usando entity framework e sempre publica uma mensagem ao criar fornecedor ou produto.
