using System;
using System.IO;
using System.Windows.Forms;
using Npgsql;

namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void buttonChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Текстовые файлы (*.txt)|*.txt";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                comboBoxDatabases.Items.Clear();

                foreach (string line in File.ReadAllLines(dialog.FileName))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split('|'); // разделяем строку на 2 части, слева и справа
                    if (parts.Length == 2)
                    {
                        comboBoxDatabases.Items.Add(new DatabaseItem
                        {
                            DisplayName = parts[0].Trim(),
                            ConnectionString = parts[1].Trim()
                        });
                    }
                }

                if (comboBoxDatabases.Items.Count > 0)
                    comboBoxDatabases.SelectedIndex = 0;
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (comboBoxDatabases.SelectedItem == null)
            {
                MessageBox.Show("Выберите базу данных!");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxLogin.Text) || string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            string connectionString = ((DatabaseItem)comboBoxDatabases.SelectedItem).ConnectionString; // Извлекаем настройки подключения из выбранного элемента списка

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT COUNT(*) FROM \"User\" WHERE \"Login\" = @login AND \"Password\" = @password";
                    string userName = textBoxLogin.Text.Trim();
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@login", textBoxLogin.Text.Trim());
                        command.Parameters.AddWithValue("@password", textBoxPassword.Text.Trim());

                        long count = (long)command.ExecuteScalar();

                        if (count > 0) // Проверяем наличие пользователя: 1 — вход разрешен, 0 — отказ
                        {
                            MessageBox.Show("Добро пожаловать " + userName);
                            new Form2(connectionString).Show();
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
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}