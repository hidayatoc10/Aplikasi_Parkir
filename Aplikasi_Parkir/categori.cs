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
    public partial class categori : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=persiapan_lks;Uid=root;Pwd=Telkomdso123;");
        public categori()
        {
            InitializeComponent();
            PopulateData();
        }
        private void PopulateData()
        {
            try
            {
                string query = "SELECT * FROM kategori";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
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

        private void nama_kategori_TextChanged(object sender, EventArgs e)
        {

        }

        private void cari_data_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM kategori WHERE nm_kategori LIKE @nm_kategori";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@nm_kategori", "%" + cari_data.Text + "%");
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

       

        private void tambah_kategori_Click(object sender, EventArgs e)
        {
            try
            {
                string namaKategori = nama_kategori.Text.Trim();
                string harga1JamStr = harga_1jamm.Text.Trim();

                if (string.IsNullOrEmpty(namaKategori) || string.IsNullOrEmpty(harga1JamStr))
                {
                    MessageBox.Show("Mohon lengkapi semua field.");
                    return;
                }

                string query = "INSERT INTO kategori (nm_kategori, harga_1jam) VALUES (@nm_kategori, @harga_1jam)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nm_kategori", namaKategori);
                command.Parameters.AddWithValue("@harga_1jam", harga1JamStr);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Kategori berhasil ditambahkan.");
                PopulateData();
                nama_kategori.Text = "";
                harga_1jamm.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void reset_Click(object sender, EventArgs e)
        {
            nama_kategori.Text = "";
            harga_1jamm.Text = "";
        }

        private void update_pengguna_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nama_kategori.Text) || string.IsNullOrWhiteSpace(harga_1jamm.Text))
                {
                    MessageBox.Show("Mohon isi nama kategori dan harga terlebih dahulu.");
                    return;
                }

                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int id_kategori = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id_kategori"].Value);
                    string query = "UPDATE kategori SET nm_kategori = @nm_kategori, harga_1jam = @harga_1jam WHERE id_kategori = @id_kategori";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nm_kategori", nama_kategori.Text);
                    command.Parameters.AddWithValue("@harga_1jam", harga_1jamm.Text);
                    command.Parameters.AddWithValue("@id_kategori", id_kategori);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Kategori berhasil diperbarui.");
                    PopulateData();
                    nama_kategori.Text = "";
                    harga_1jamm.Text = "";
                }
                else
                {
                    MessageBox.Show("Pilih baris terlebih dahulu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void delete_pengguna_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Apakah Anda yakin ingin menghapus kategori ini?", "Konfirmasi Penghapusan", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        int id_kategori = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id_kategori"].Value);
                        string query = "DELETE FROM kategori WHERE id_kategori = @id_kategori";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@id_kategori", id_kategori);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Kategori berhasil dihapus.");
                        PopulateData();
                        nama_kategori.Text = "";
                        harga_1jamm.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Pilih baris terlebih dahulu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                nama_kategori.Text = row.Cells["nm_kategori"].Value.ToString();
                harga_1jamm.Text = row.Cells["harga_1jam"].Value.ToString();
            }
        }

        private void harga_1jamm_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumeric(harga_1jamm.Text))
            {
                harga_1jamm.Text = "";
            }
        }

        private bool IsNumeric(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            data_pengguna Halaman_Home = new data_pengguna();
            Halaman_Home.Show();
            this.Hide();
        }

        private void dashboard_Click(object sender, EventArgs e)
        {
            Halaman_Home Halaman_Home = new Halaman_Home();
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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void parkir_Click(object sender, EventArgs e)
        {
            parkir edit_profil = new parkir();
            edit_profil.Show();
            this.Hide();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
