using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace WindowsFormsApp7
{
    public partial class Form2 : Form
    {
        private string connectionString;

        public Form2(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }

        private void Form2_Load(object sender, System.EventArgs e)
        {
            try
            {
                DataTable dataTable = new DataTable();

                string sql = "SELECT * FROM \"User\" ORDER BY \"UserID\"";

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
            catch (System.Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }
    }
}