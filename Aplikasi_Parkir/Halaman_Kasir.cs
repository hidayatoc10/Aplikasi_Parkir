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
    public partial class Halaman_Kasir : Form
    {
        MySqlConnection conn = new MySqlConnection("Server=localhost;Database=persiapan_lks;Uid=root;Pwd=Telkomdso123;");
        public Halaman_Kasir()
        {
            InitializeComponent();
            TampilkanKategori();
            TampilkanDataKendaraan();
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;

        }
        private void TampilkanKategori()
        {
            try
            {
                string query = "SELECT * FROM kategori";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["nm_kategori"].ToString());
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void TampilkanDataKendaraan()
        {
            try
            {
                string query = "SELECT dk.id_kendaraan, dk.no_pol, k.nm_kategori, dk.jam_masuk, dk.jam_keluar, dk.total_harga FROM data_kendaraan dk JOIN kategori k ON dk.id_kategori = k.id_kategori WHERE dk.no_pol LIKE @search";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", "%" + cari_data.Text + "%");
                conn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Halaman_Kasir_Load(object sender, EventArgs e)
        {
            
        }
        private void logout_Click(object sender, EventArgs e)
        {

        }

        private void button_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void logout_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Anda yakin ingin logout?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                halaman_login loginForm = new halaman_login();
                loginForm.Show();
                this.Hide();
            }
        }

        private void reset_Click(object sender, EventArgs e)
        {
            nomor_polisi.Text = "";
            comboBox1.SelectedIndex = -1;
        }

        private void nomor_polisi_TextChanged(object sender, EventArgs e)
        {

        }

        private void tambah_parkir_Click(object sender, EventArgs e)
        {
            try
            {
                string nomorPolisi = nomor_polisi.Text;
                string namaKategori = comboBox1.SelectedItem.ToString();
                string query = "SELECT id_kategori FROM kategori WHERE nm_kategori = @nm_kategori";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nm_kategori", namaKategori);

                conn.Open();
                int idKategori = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();

                DateTime jamMasuk = DateTime.Now;

                string insertQuery = "INSERT INTO data_kendaraan (no_pol, id_kategori, jam_masuk) VALUES (@no_pol, @id_kategori, @jam_masuk)";
                cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@no_pol", nomorPolisi);
                cmd.Parameters.AddWithValue("@id_kategori", idKategori);
                cmd.Parameters.AddWithValue("@jam_masuk", jamMasuk);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Data kendaraan berhasil ditambahkan.");
                    nomor_polisi.Text = "";
                    comboBox1.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Gagal menambahkan data kendaraan.");
                }
                TampilkanDataKendaraan();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox_kategori_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void cari_data_TextChanged_1(object sender, EventArgs e)
        {
            TampilkanDataKendaraan();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Apakah Anda yakin ingin menghapus data kendaraan ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        int idKendaraan = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id_kendaraan"].Value);

                        string deleteQuery = "DELETE FROM data_kendaraan WHERE id_kendaraan = @id_kendaraan";

                        MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                        cmd.Parameters.AddWithValue("@id_kendaraan", idKendaraan);
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        conn.Close();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data kendaraan berhasil dihapus.");
                            TampilkanDataKendaraan();
                        }
                        else
                        {
                            MessageBox.Show("Gagal menghapus data kendaraan.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Silakan pilih data kendaraan yang akan dihapus.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "total_harga" && e.Value != null)
            {
                if (e.Value is decimal totalHarga)
                {
                    // Format nilai total_harga menjadi mata uang
                    e.Value = string.Format("Rp. {0:N0}", totalHarga);
                    e.FormattingApplied = true;
                }
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void parkir_Click(object sender, EventArgs e)
        {

        }

        private void selesai_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Apakah Anda yakin ingin menyelesaikan parkir untuk kendaraan ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        int idKendaraan = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id_kendaraan"].Value);
                        DateTime jamKeluar = DateTime.Now;
                        DateTime jamMasuk = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["jam_masuk"].Value);
                        TimeSpan durasiParkir = jamKeluar - jamMasuk;
                        string namaKategori = dataGridView1.SelectedRows[0].Cells["nm_kategori"].Value.ToString();
                        decimal hargaPerJam = 0;

                        string queryHarga = "SELECT harga_1jam FROM kategori WHERE nm_kategori = @nm_kategori";
                        MySqlCommand cmdHarga = new MySqlCommand(queryHarga, conn);
                        cmdHarga.Parameters.AddWithValue("@nm_kategori", namaKategori);

                        conn.Open();
                        hargaPerJam = Convert.ToDecimal(cmdHarga.ExecuteScalar());
                        conn.Close();

                        decimal totalHarga = Convert.ToDecimal(durasiParkir.TotalHours) * hargaPerJam;

                        string updateQuery = "UPDATE data_kendaraan SET jam_keluar = @jam_keluar, total_harga = @total_harga WHERE id_kendaraan = @id_kendaraan";
                        MySqlCommand cmdUpdate = new MySqlCommand(updateQuery, conn);
                        cmdUpdate.Parameters.AddWithValue("@jam_keluar", jamKeluar);
                        cmdUpdate.Parameters.AddWithValue("@total_harga", totalHarga);
                        cmdUpdate.Parameters.AddWithValue("@id_kendaraan", idKendaraan);

                        conn.Open();
                        int rowsAffected = cmdUpdate.ExecuteNonQuery();
                        conn.Close();

                        if (rowsAffected > 0)
                        {
                            TampilkanDataKendaraan();
                        }
                        else
                        {
                            MessageBox.Show("Gagal menyelesaikan parkir.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Silakan pilih data kendaraan yang akan diselesaikan parkirnya.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


    }
}
