# sistemas-distribuidos-resiliencia-k8s
Implementação e validação de testes de resiliência e Engenharia de Caos em arquitetura de microsserviços com Kubernetes e Chaos Mesh. 
# Trabalho Prático – Sistemas Distribuídos - 2026/1
Caio Pereira Lapa      - 2021200619
Tiago Artém dos Santos - 2021201421

Este repositório contém a implementação de uma arquitetura de microsserviços conteinerizada, desenvolvida para validar conceitos de **Tolerância a Falhas** e **Disponibilidade** utilizando a ferramenta [Chaos Mesh](https://chaos-mesh.org/).

O projeto simula um ecossistema de logística, aplicando injeção de falhas (Chaos Engineering) para avaliar o comportamento do sistema e a eficácia das estratégias de resiliência implementadas.

## Arquitetura da Aplicação

A aplicação é composta por três componentes interdependentes orquestrados via Kubernetes:
1. **API Gateway (Python/FastAPI):** Ponto de entrada do sistema. Implementa os mecanismos de resiliência (Retries e Timeouts) para lidar com falhas de comunicação.
2. **Rota API (C#/.NET):** Microsserviço de negócio responsável pelo cálculo e gerenciamento de rotas.
3. **Banco de Dados (PostgreSQL):** Camada de persistência relacional (`logistica_db`).

## Mecanismos de Tolerância a Falhas Implementados

Para garantir que o sistema não sofra falhas em cascata ou degradação severa, as seguintes estratégias foram aplicadas:
* **Políticas de Retry:** O Gateway realiza até 3 tentativas automáticas (com intervalo de 2 segundos) caso o microsserviço de rotas falhe ou fique indisponível.
* **Timeouts:** As requisições entre o Gateway e a API de Rotas possuem um tempo máximo de espera de 2 segundos para evitar travamento de recursos (Thread Starvation).
* **Réplicas (Kubernetes):** O Gateway e a API de Rotas estão configurados com múltiplas réplicas (`replicas: 2`) para garantir alta disponibilidade durante a queda de instâncias isoladas ou picos de processamento.

---

