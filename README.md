Структура проекта
	1. Модель данных (Models):
	2. Клиенты (Clients)
	3. Сделки (Deals)
	4. Взаимодействия (Interactions)
	5. Задачи (Tasks)
	6. Календарь (Calendar)
	7. Рассылка (Mailing)
Логика работы с базой данных (DataAccess):
	1. PostgreSQL с подключением через Npgsql
WPF-интерфейсы:
	1. Главное окно (MainWindow.xaml, MainWindow.xaml.cs)
	2. Вспомогательные окна для добавления/редактирования 
Бизнес-логика (BusinessLogic):
	1. Логика работы с клиентами, сделками, взаимодействиями
	2. Генерация отчетов и аналитика
NuGet пакеты:
	1. Npgsql: Для подключения к PostgreSQL.
	2. Newtonsoft.Json: Для сериализации/десериализации объектов JSON.
	3. LiveCharts.Wpf: Для построения графиков и диаграмм.
	4. System.Net.Mail: Для отправки электронных писем.
	5. Yandex.Cloud.Api: Для интеграции с Яндекс Диалоги.
