

# Eclipseworks - Desafio Técnico

## Overview
Caro avaliador, esse desafio foi idealizado desenvolvendo em DDD, onde foi evitado utilizar entidades anêmicas. Utilizei o conceito de entidades, objeto de valor, uso de interface para marcar agregados e uso de repositorios (baseados nos agregados). Não existia um grande número de regras de negocio para justificar o uso de DDD, mas como isso é um teste...imaginei que seria interessante mostrar alguns recursos de desenvolvimento. Sendo assim, criei dois microsserviços - um com a interface de API para manipulação de Projetos e Tarefas e outro microsserviço para lidar com a auditória. Também vale citar que foi utilizado Event Sourcing, que foi uma solução muito coerente para armazenar a auditoria. O uso de MediatR foi uma estratégia para organizar o código, não necessariamente para desenvolver em CQRS, embora a API esteja organizada logicamente como CQRS, na minha visão, para ser CQRS "puro" deveriamos armazenar os comandos em uma banco de eventos e ter outro banco apenas para queries, realizando as projections necessárias e como pode-se perceber, utilize apenas um banco para API no Postgres. Achei que na API, cabe-se o uso dos padrões DTO e DAO para realizar as consultas/relatórios. Para o microsserviço de auditória, devido ao tempo de desenvolvimento, fiz de uma maneira mais simples - sem criar outra solução com vários projetos - mas, respeitando as separações de camadas entre pastas.
Por fim, toda parte de acesso ao banco de dados foi utilizado Dapper e a comunicação entre os microsserviços utilizamos o RabbitMQ. Existem migrations para criar as tabelas e incluir 2 usuários de testes para aplicação.

## Instruções
Esse projeto utiliza docker-compose na sua estrutura, para rodar a aplicação basta utilizar o comando abaixo no powershell

    docker compose up

Utilizar a ferramenta de acesso ao Postgresql de sua escolha (Host: localhost, Porta: 5432, User: postgres e Password: ABC123teste), utilizo DBeaver, e criar os seguintes bancos de dados:

    EclipseWorks
    EclipseWorks-Audit

Agora será necessário criar as queues e os exchanges no RabbitMQ e fazer o bind com as filas especificas. Para isso, basta acessar o endereço: http://localhost:15672/#/ (User: guest e Password: guest)
Na aba Queues and Streams, criar as seguintes filas com as configurações default:

    project.created.queue
    project.deleted.queue
    task.changed.queue
    task.created.queue
    task.deleted.queue

Na aba Exchanges, criar os seguintes exchanges, todos com a configuração **type: fanout**:

    project.created
    project.deleted
    task.changed
    task.created
    task.deleted

Entrar em cada exchange criado e realizar o bind para a fila respectiva:

    project.created -> binding -> project.created.queue
    project.deleted -> binding -> project.deleted.queue
    task.changed -> binding -> task.changed.queue
    task.created -> binding -> task.created.queue
    task.deleted -> binding -> task.deleted.queue

Agora com as configurações do Postgresql e RabbitMQ feitas, será necessário iniciar o eclipseworks_audit e eclipseworks_webapi no Docker Desktop.

Para acessar a API, utilizar o endereço: https://localhost:8081/swagger/index.html

### Observação

Como não foi solicitado um sistema de autentação/autorização no desafio, criei uma tabela chamada "UserRole" com dois usuários. Position: 1 seria o gerente, foi uma alternativa mais simples possível para satisfazer uma regra de negocio do teste e também para não ficar preocupado com autenticacão, conforme solicitado.

Do mais, espero ter atingindo o desafio técnico, sei que não fiz testes unitários para todos os projetos, mas desenvolvi para a principal parte do código e gostaria de participar de uma conversa técnica para discutir todas as decisões técnicas abordadas, tradeoffs etc - algo que não é possível realizar no README.

Grato pela oportunidade.

## Fase 2: Refinamento
Minhas perguntas para o PO, seriam:

 1. Como seria a integração com o sistema de autorização/autenticação. Pois é muito importante para esse MVC ter uma versão com essa feature;
 2. Nos requisitos anteriores não foi citado uma opção para alterar o projeto na API e por esse motivo não foi implementado. Seria interessante priorizar essa ultima feature do "CRUD"
 3. Um outro recurso bacana seria a possibilidade de inserir várias tarefas de uma só fez para o projeto
 4. Para o microsserviço de auditória, poderia-se criar uma api para recuperar os dados dessa auditoria. 

## Fase 3: Final
Devido ao feriado de pascoa não consegui realizar testes unitários em todos os projetos e isso é algo que melhoraria com certeza numa próxima fase do projeto. Os testes foram desenvolvidos na cama de Domain e Application, que na minha opinião são as mais importantes, porém outros projetos deveriam estar cobertos também.
A parte de logs pode ser melhorada também, utilizar serilog ou uma abordagem melhor ainda, que seria implementar open telemetry e deixar a aplicação agnóstica com relação a Logs.
Uma camada de cache, minha sugestão seria o Redis, para o controller de Relatório para otimizar o tempo de resposta, se necessário.
O projeto esta containerizado e não é um sistema complexo por natureza, podemos utilizar o AWS ECS
Como nuvem, minha sugestão é a AWS que contém todas as ferramentas necessárias para executar esse projeto de forma simples e segura.