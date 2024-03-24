using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplikasi_Kasir
{
    public partial class parkir : Form
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public parkir()
        {
            InitializeComponent();
            InitializeDatabase();
            DisplayData();
            dataGridView1.CellFormatting += dataGridView1_CellContentClick;
        }

        private void InitializeDatabase()
        {
            server = "localhost";
            database = "persiapan_lks";
            uid = "root";
            password = "Telkomdso123";
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

            connection = new MySqlConnection(connectionString);
        }

        private void DisplayData()
        {
            string query = "SELECT dk.id_kendaraan, dk.no_pol, kt.nm_kategori, dk.jam_masuk, dk.jam_keluar, dk.total_harga " +
                           "FROM data_kendaraan dk " +
                           "JOIN kategori kt ON dk.id_kategori = kt.id_kategori";
            MySqlCommand command = new MySqlCommand(query, connection);

            try
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                DataGridViewTextBoxColumn totalHargaColumn = new DataGridViewTextBoxColumn();
                totalHargaColumn.Name = "total_harga";
                totalHargaColumn.HeaderText = "Total Harga";
                dataGridView1.Columns.Add(totalHargaColumn);

                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            categori Halaman_Home = new categori();
            Halaman_Home.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            data_pengguna Halaman_Home = new data_pengguna();
            Halaman_Home.Show();
            this.Hide();
        }

        private void logout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Anda yakin ingin logout?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                halaman_login loginForm = new halaman_login();
                loginForm.Show();
                this.Hide();
            }
        }

        private void dashboard_Click(object sender, EventArgs e)
        {
            Halaman_Home Halaman_Home = new Halaman_Home();
            Halaman_Home.Show();
            this.Hide();
        }


        private void cari_Click(object sender, EventArgs e)
        {
            string keyword = cari_data.Text.Trim();
            SearchData(keyword);
        }
    

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "total_harga" && e.Value != null)
            {
                decimal totalHarga = Convert.ToDecimal(e.Value);
                e.Value = string.Format("Rp. {0:N0}", totalHarga);
            }
        }
        private void SearchData(string keyword)
        {
            string query = "SELECT dk.id_kendaraan, dk.no_pol, kt.nm_kategori, dk.jam_masuk, dk.jam_keluar " +
                   "FROM data_kendaraan dk " +
                   "JOIN kategori kt ON dk.id_kategori = kt.id_kategori " +
                   "WHERE kt.nm_kategori LIKE @keyword";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
            try
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void cari_data_TextChanged(object sender, EventArgs e)
        {
            string keyword = cari_data.Text.Trim();
            SearchData(keyword);
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Apakah Anda yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int selectedIndex = dataGridView1.SelectedRows[0].Index;
                    int idToDelete = Convert.ToInt32(dataGridView1.Rows[selectedIndex].Cells["id_kendaraan"].Value);

                    DeleteData(idToDelete);
                    DisplayData();
                }
            }
            else
            {
                MessageBox.Show("Pilih baris yang ingin dihapus.");
            }
        }

        private void DeleteData(int id)
        {
            string query = "DELETE FROM data_kendaraan WHERE id_kendaraan = @id";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Data berhasil dihapus.");
                }
                else
                {
                    MessageBox.Show("Tidak ada data yang dihapus.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
