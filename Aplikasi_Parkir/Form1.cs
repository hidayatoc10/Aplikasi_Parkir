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
    public partial class halaman_login : Form
    {
        MySqlConnection conn;
        public halaman_login()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Username dan password wajib diisi.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string koneksi = "server=localhost;uid=root;password=Telkomdso123;database=persiapan_lks";
                using (MySqlConnection conn = new MySqlConnection(koneksi))
                {
                    conn.Open();
                    string query = "SELECT * FROM users WHERE nm_user = @nm_user";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nm_user", textBox1.Text);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hashedPassword = reader["password"].ToString();
                            if (VerifyPassword(textBox2.Text, hashedPassword))
                            {
                                string role = reader["role"].ToString();
                                if (role == "admin")
                                {
                                    MessageBox.Show("Login berhasil! Selamat Datang Admin.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    Halaman_Home halaman_home = new Halaman_Home();
                                    halaman_home.Show();
                                }
                                else if (role == "kasir")
                                {
                                    MessageBox.Show("Login berhasil! Selamat Datang Kasir.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    Halaman_Kasir halaman_kasir = new Halaman_Kasir();
                                    halaman_kasir.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Peran pengguna tidak valid.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Login Gagal, Coba Lagi.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Username tidak ditemukan.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi ke database gagal. Silakan coba lagi nanti.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString() == hashedPassword;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

    }
}
