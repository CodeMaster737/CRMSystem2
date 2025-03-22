using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
using Newtonsoft.Json;
using LiveCharts;
using LiveCharts.Wpf;
using System.Net.Mail;
using System.Net;

namespace CRMSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Список клиентов
        private List<Client> clients = new List<Client>();

        // Конструктор главного окна
        public MainWindow()
        {
            InitializeComponent();

            // Загрузка данных из базы данных
            LoadClientsFromDatabase();
        }

        //для загрузки клиентов из базы данных
        private void LoadClientsFromDatabase()
        {
            using (var conn = new NpgsqlConnection("Host=localhost;Username=postgres;Password=password;Database=crm"))
            {
                conn.Open();

                var cmd = new NpgsqlCommand("SELECT * FROM clients", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var client = new Client
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3)
                    };

                    clients.Add(client);
                }
            }
        }

        //для отображения графика
        private void DisplaySalesChart()
        {
            var chartValues = new ChartValues<double> { 10, 20, 30, 40, 50 };
            SalesChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Продажи",
                    Values = chartValues
                }
            };
        }

        //для отправки электронного письма
        private void SendEmail(string to, string subject, string body)
        {
            var mail = new MailMessage
            {
                From = new MailAddress("levkosavelij8@gmail.com"),
                Subject = subject,
                Body = body
            };
            mail.To.Add(to);

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("username", "password");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }
        private void AddClient(Client client)
        {
            using (var conn = new NpgsqlConnection("Host=localhost;Username=postgres;Password=user12;Database=postgres"))
            {
                conn.Open();

                var cmd = new NpgsqlCommand("INSERT INTO clients (firstname, lastname, email) VALUES (@firstName, @lastName, @email)", conn);
                cmd.Parameters.AddWithValue("@firstName", client.FirstName);
                cmd.Parameters.AddWithValue("@lastName", client.LastName);
                cmd.Parameters.AddWithValue("@email", client.Email);

                cmd.ExecuteNonQuery();
            }
        }

        //для добавления задачи
        private void AddTask(Task task)
        {
            using (var conn = new NpgsqlConnection("Host=localhost;Username=postgres;Password=user12;Database=postgres"))
            {
                conn.Open();

                var cmd = new NpgsqlCommand("INSERT INTO tasks (title, description, due_date, client_id) VALUES (@title, @description, @dueDate, @clientId)", conn);
                NpgsqlParameter npgsqlParameter = cmd.Parameters.AddWithValue("@title", task.Title);
                cmd.Parameters.AddWithValue("@description", task.Description);
                cmd.Parameters.AddWithValue("@dueDate", task.DueDate);
                cmd.Parameters.AddWithValue("@clientId", task.ClientId);

                cmd.ExecuteNonQuery();
            }
        }

        //для получения данных для аналитики
        private Dictionary<string, int> GetSalesData()
        {
            using (var conn = new NpgsqlConnection("Host=localhost;Username=postgres;Password=password;Database=crm"))
            {
                conn.Open();

                var cmd = new NpgsqlCommand("SELECT SUM(amount) AS total_sales, EXTRACT(MONTH FROM created_at) AS month FROM deals GROUP BY month ORDER BY month ASC", conn);
                var reader = cmd.ExecuteReader();

                var salesData = new Dictionary<string, int>();
                while (reader.Read())
                {
                    var month = reader.GetString(1);
                    var totalSales = reader.GetInt32(0);
                    salesData.Add(month, totalSales);
                }

                return salesData;
            }
        }

        //для отправки автоматической рассылки
        private void SendBirthdayEmails()
        {
            foreach (var client in clients)
            {
                if (IsTodayBirthday(client.Birthday))
                {
                    SendEmail(client.Email, "Happy Birthday!", "Dear " + client.FirstName + ", we wish you a happy birthday!");
                }
            }
        }

        private bool IsTodayBirthday(object birthday)
        {
            throw new NotImplementedException();
        }

        // Проверка, является ли сегодняшний день днем рождения клиента
        private bool IsTodayBirthday(DateTime birthday)
        {
            return birthday.Day == DateTime.Today.Day && birthday.Month == DateTime.Today.Month;
        }
    }

    internal class Client
    {
        internal string FirstName;
        internal int Id;
        internal string LastName;
        internal string Email;
        internal object Birthday;
    }
}
