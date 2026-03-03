using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace WindowsFormsApp7
{
    public partial class Form2 : Form
    {
        private string connectionString;

        public Form2(string receivedConnectionString)
        {
            InitializeComponent();
            connectionString = receivedConnectionString;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = @"SELECT * FROM ""User"" ORDER BY ""UserID""";

                DataTable dataTable = new DataTable();

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, connection))
                    {
                        adapter.Fill(dataTable);
                    }
                }

                dataGridViewUsers.DataSource = dataTable;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}