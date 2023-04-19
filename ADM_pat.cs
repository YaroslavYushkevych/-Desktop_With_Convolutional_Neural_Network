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

namespace Diplom
{
    public partial class ADM_pat : Form
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

        public ADM_pat()
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
                sql = @"SELECT p_fio AS ПІБ_пацієнта, e_mail, tel AS Телефон, birth AS Дата_народження from patient_table; ";
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from i_patient('" + textBox1.Text + "', '" + textBox2.Text + "'," +
                    " '" + textBox5.Text + "', '" + textBox4.Text + "'); ";
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

        private void ADM_pat_Load(object sender, EventArgs e)
        {
            Update_table();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from u_patient('" + textBox3.Text + "', '" + textBox1.Text + "', '" + textBox2.Text + "'," +
                    " '" + textBox5.Text + "', '" + textBox4.Text + "'); ";
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
                sql = @"select * from d_patient('" + textBox6.Text + "'); ";
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
