# README #

### What is this repository for? ###

Подсистема контроля точности расётов ПС МКР

### Компоненты ###

1. PeremClient - клиентское приложение, присутствует на каждом вычислительном узле;
2. PeremServer - серверное приложение, присутствует только на сервере, устанавливать на вычислительные узлы нет необходимости;
3. GeneralRemote - библиотека клиент-серверного взаимоействия, позволяет клиенту выполнять свои методы на стороне сервера;
4. MessageTransfer - устарело, нуждается в удалении.