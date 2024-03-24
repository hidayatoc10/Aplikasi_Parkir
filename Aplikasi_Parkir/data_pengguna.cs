using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplikasi_Kasir
{
    public partial class data_pengguna : Form
    {
        private string connectionString = "Server=localhost;Database=persiapan_lks;Uid=root;Pwd=Telkomdso123;";
        public data_pengguna()
        {
            InitializeComponent();
            DisplayData();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private void DisplayData()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT id_users, nm_user, password, role FROM users", con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void username_TextChanged(object sender, EventArgs e)
        {

        }

        private void password_TextChanged(object sender, EventArgs e)
        {
            password.PasswordChar = '*';
        }

        private void keterangan_TextChanged(object sender, EventArgs e)
        {

        }

        private void cari_data_TextChanged(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT id_users, nm_user, password, role FROM users WHERE id_users LIKE @searchText OR nm_user LIKE @searchText OR password LIKE @searchText OR role LIKE @searchText", con);
                cmd.Parameters.AddWithValue("@searchText", "%" + cari_data.Text + "%");
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void tambah_pengguna_Click(object sender, EventArgs e)
        {
            if ((username.Text == "") || (password.Text == "") || (keterangan.Text == ""))
            {
                MessageBox.Show("Lengkapi Form, Wajib Disini", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO users (nm_user, password, role) VALUES (@nm_user, @password, @role)", con);
                    cmd.Parameters.AddWithValue("@nm_user", username.Text);
                    // Enkripsi password sebelum disimpan
                    cmd.Parameters.AddWithValue("@password", HashPassword(password.Text));
                    cmd.Parameters.AddWithValue("@role", keterangan.Text);
                    cmd.ExecuteNonQuery();
                }
                DisplayData();
                username.Text = "";
                password.Text = "";
                keterangan.Text = "";
            }
        }
        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            string hashedInputPassword = HashPassword(inputPassword);
            return hashedInputPassword == hashedPassword;
        }

        private void reset_Click(object sender, EventArgs e)
        {
            username.Text = "";
            password.Text = "";
            keterangan.Text = "";
        }

        private void update_pengguna_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                int idToUpdate = Convert.ToInt32(dataGridView1.Rows[selectedIndex].Cells["id_users"].Value);

                if ((username.Text == "") || (password.Text == "") || (keterangan.Text == ""))
                {
                    MessageBox.Show("Lengkapi Form, Wajib Disini", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("UPDATE users SET nm_user = @nm_user, password = @password, role = @role WHERE id_users = @id_users", con);
                        cmd.Parameters.AddWithValue("@nm_user", username.Text);
                        cmd.Parameters.AddWithValue("@password", HashPassword(password.Text));
                        cmd.Parameters.AddWithValue("@role", keterangan.Text);
                        cmd.Parameters.AddWithValue("@id_users", idToUpdate);
                        cmd.ExecuteNonQuery();
                    }
                    DisplayData();
                    username.Text = "";
                    password.Text = "";
                    keterangan.Text = "";

                    MessageBox.Show("Data berhasil diubah", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Pilih pengguna yang ingin diperbarui", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void delete_pengguna_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Anda yakin ingin menghapus pengguna ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int selectedIndex = dataGridView1.SelectedRows[0].Index;
                    int idToDelete = Convert.ToInt32(dataGridView1.Rows[selectedIndex].Cells["id_users"].Value);

                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("DELETE FROM users WHERE id_users = @id_users", con);
                        cmd.Parameters.AddWithValue("@id_users", idToDelete);
                        cmd.ExecuteNonQuery();
                    }

                    DisplayData();
                    username.Text = "";
                    password.Text = "";
                    keterangan.Text = "";
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

        private void categori_Click(object sender, EventArgs e)
        {
            categori Halaman_Home = new categori();
            Halaman_Home.Show();
            this.Hide();
        }

        private void parkir_Click(object sender, EventArgs e)
        {
            parkir edit_profil = new parkir();
            edit_profil.Show();
            this.Hide();
        }
    }
}
