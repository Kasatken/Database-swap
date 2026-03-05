using System;
using System.Windows.Forms;
using Npgsql;

namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
        
        private const string AdminConnection = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=Eugene0905"; // Строка подключения к postgres для загрузки списка БД

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                using (var connection = new NpgsqlConnection(AdminConnection))
                {
                    connection.Open();
                    string sql = "SELECT datname FROM pg_database WHERE datistemplate = false ORDER BY datname"; // выбираем все базы postgress, кроме системных

                    NpgsqlCommand command = new NpgsqlCommand(sql, connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            comboBoxDatabases.Items.Add(reader.GetString(0)); // добавляем все базы в combobox
                    }
                }

                if (comboBoxDatabases.Items.Count > 0) // проверка, против пустого списка
                {
                    comboBoxDatabases.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения: " + ex.Message);
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;

            if (comboBoxDatabases.SelectedItem == null)
            {
                MessageBox.Show("Выберите базу данных!");
                return;
            } 
            else if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            string selectedDb = comboBoxDatabases.SelectedItem.ToString(); // выбраную базу записываем в переменную для запроса
            string connStr = $"Host=localhost;Port=5432;Database={selectedDb};Username=postgres;Password=Eugene0905"; // запрос базы данных с подстановкой

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connStr))
                {
                    connection.Open();

                    string sql = "SELECT COUNT(*) FROM \"User\" WHERE \"Login\" = @login AND \"Password\" = @password";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@password", password);

                        long count = (long)command.ExecuteScalar(); // возвращает первую ячейку результата

                        if (count > 0)
                        {
                            MessageBox.Show("Добро пожаловать, " + login + "!");
                            new Form2(connStr).Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка авторизации: " + ex.Message);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}