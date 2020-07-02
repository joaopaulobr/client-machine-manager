# client-machine-manager
Controla máquinas clientes por comandos via websockets

A solução contém 3 projetos principais:

1) ClientService

 Projeto Windows Service para ser executado na máquina cliente que será controlada. É responsável por iniciar a comunicação via websockets com o servidor e receber os comandos passados pelo cliente web.

2) ServerAPI

Web API responsável por intermediar a comunicação entre o cliente web e a máquina cliente.
Este projeto possui a pasta *ClientApp* que é um projeto Angular independente e vinculado à aplicação no *Startup.cs* do projeto.

3) Domain

Projeto que contém classes comuns aos projetos anteriores.

----------------------------------------------------------------------------------------------------------------------------------
