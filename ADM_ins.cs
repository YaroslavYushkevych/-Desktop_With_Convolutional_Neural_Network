using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace Diplom
{
    public partial class ADM_ins : Form
    {
        private string connstring = String.Format("Server = {0};" +
            " Port = {1};" +
            " User Id = {2};" +
            " Password = {3};" +
            " Database = {4};",
            "localhost",
            5432,
            "postgres",
            "yorrik",
            "Diplom_medical"
            );

        private NpgsqlConnection conn;
        private string sql;
        private NpgsqlCommand cmd;
        private DataTable dt;

        public ADM_ins()
        {
            InitializeComponent();
        }
        private void WAL(string sql)
        {
            string path = @"..\Debug\WAL.txt";
            File.AppendAllText(path, "Час проведення транзакції: " + DateTime.Now + "\nКод транзакції: " + sql + "\n\n");

        }
        public void Update_table()
        {
            conn = new NpgsqlConnection(connstring);
            try
            {
                conn.Open();
                sql = @"SELECT IT.id_patient AS Номер_пацієнта, IT.id_medic AS Номер_медика, IT.photo AS Фото,
                        IT.result AS Результат, IT.date AS Дата, IT.confirmation AS Підтвердження
                        FROM public.inspection_table AS IT; ";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                dgvData.DataSource = null;
                dgvData.DataSource = dt;
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void закритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ADM_ins_Load(object sender, EventArgs e)
        {
            Update_table();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from u_inspection('" + textBox3.Text + "', '" + textBox1.Text + "', '" + textBox2.Text + "'," +
                    " '" + textBox5.Text + "', '" + textBox4.Text + "', '" + textBox7.Text + "'); ";
                WAL(sql);
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
            Update_table();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from i_inspection('" + textBox1.Text + "', '" + textBox2.Text + "'," +
                    " '" + textBox5.Text + "', '" + textBox4.Text + "', '" + textBox7.Text + "'); ";
                WAL(sql);
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
            Update_table();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from d_inspection('" + textBox6.Text + "'); ";
                WAL(sql);
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
            Update_table();
        }

        private void завантажитиФотоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
                textBox5.Text = openFileDialog1.FileName;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
                textBox5.Text = openFileDialog1.FileName;
            }

        }
    }
}
