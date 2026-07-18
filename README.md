# sistemas-distribuidos-resiliencia-k8s
Implementação e validação de testes de resiliência e Engenharia de Caos em arquitetura de microsserviços com Kubernetes e Chaos Mesh. # Trabalho Prático – Sistemas Distribuídos - 2026/1
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

## Passo a Passo para Funcionamento

### 1. Inicializando o Cluster e a Aplicação
Inicie o seu cluster Kubernetes (exemplo com Minikube):
```bash
minikube start

Aplique os manifestos de implantação contidos no arquivo deployments.yaml (que provisionam os Services e Deployments):
# kubectl apply -f deployments.yaml

Verifique se todos os pods estão com o status Running:
#kubectl get pods

Caso ainda não tenha o Chaos Mesh rodando no seu cluster, execute o script oficial:
#curl -sSL [https://mirrors.chaos-mesh.org/v2.5.1/install.sh](https://mirrors.chaos-mesh.org/v2.5.1/install.sh) | bash

3. Executando os Experimentos de Caos
Os experimentos estão declarados na pasta /chaos-experiments. Para testar a resiliência, aplique os ataques de forma isolada e observe o comportamento da aplicação:

Falha de Rede (Latência): #kubectl apply -f chaos-experiments/01-network-chaos.yaml

Falha de Instância (API de Rotas): #kubectl apply -f chaos-experiments/02-pod-chaos.yaml

Falha de Recurso (Stress de CPU): #kubectl apply -f chaos-experiments/03-stress-chaos.yaml

Falha de Instância (Banco de Dados): #kubectl apply -f chaos-experiments/04-db-chaos.yaml





