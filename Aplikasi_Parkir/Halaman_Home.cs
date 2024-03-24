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
    public partial class Halaman_Home : Form
    {
        public Halaman_Home()
        {
            InitializeComponent();
        }

        private void dashboard_Click(object sender, EventArgs e)
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

        private void data_pengguna_Click(object sender, EventArgs e)
        {
            data_pengguna edit_profil = new data_pengguna();
            edit_profil.Show();
            this.Hide();
        }

        private void categori_Click(object sender, EventArgs e)
        {
            categori edit_profil = new categori();
            edit_profil.Show();
            this.Hide();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label_username_Click(object sender, EventArgs e)
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
