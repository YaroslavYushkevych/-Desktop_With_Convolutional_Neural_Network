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
    public partial class Form_view : Form
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
        public Form_view()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void закритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        public void Update_table()
        {
            conn = new NpgsqlConnection(connstring);
            try
            {
                conn.Open();
                sql = @"select * from view_inspection; ";
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
        public void Update_table_castom(string view_table)
        {
            conn = new NpgsqlConnection(connstring);
            try
            {
                conn.Open();
                sql = @"select * from " + view_table + "; ";
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

        private void Form_view_Load(object sender, EventArgs e)
        {
            Update_table();
        }

        private void переглянутиToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void оновитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update_table();
        }

        private void оновитиToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void таблицюПацієнтівToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update_table_castom("view_patient");
        }

        private void таблицюЛікарівToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update_table_castom("view_medic");
        }

        private void таблицюОбстеженьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update_table_castom("view_inspection");
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        public int index_find()
        {
            conn.Open();
            sql = @"Select id
                    From patient_table
                    Where p_fio = '" + textBox2.Text + "'; ";
            cmd = new NpgsqlCommand(sql, conn);
            dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            string res = string.Join(Environment.NewLine,
                    dt.Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
            conn.Close();
            //label3.Text = res;
            return int.Parse(res);
        }

        public int index_find_ins()
        {
            int index_p = index_find();
            conn.Open();
            sql = @"Select id
                    From inspection_table 
                    Where id_patient = '" + index_p + "' AND date = '" + textBox1.Text + "'; ";
            cmd = new NpgsqlCommand(sql, conn);
            dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            string res = string.Join(Environment.NewLine,
                    dt.Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
            conn.Close();
            //label3.Text = res;
            return int.Parse(res);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int index = index_find();
            try
            {
                conn.Open();
                sql = @"Select photo 
                From inspection_table 
                Where id_patient = " + index + " AND date = '" + textBox1.Text + "'; ";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                string res = string.Join(Environment.NewLine,
                    dt.Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
                conn.Close();
                pictureBox1.Image = Image.FromFile(res);
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void WAL(string sql)
        {
            string path = @"..\Debug\WAL.txt";
            File.AppendAllText(path, "Час проведення транзакції: " + DateTime.Now + "\nКод транзакції: " + sql + "\n\n");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = index_find_ins();
            bool cheakd = false;


            if (checkBox1.Checked) { cheakd = true; }

            try
            {
                conn.Open();
                sql = @"update inspection_table set confirmation = " + cheakd + " where id = " + index + ";";
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

        private void статистикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
            try
            {
                conn.Open();
                sql = @"Select COUNT(id) From inspection_table; ";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                List<string> list_eval = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    list_eval.Add(row[0].ToString());
                }
                //label4.Text = list_eval[0];

                conn.Open();
                sql = @"Select COUNT(id) From inspection_table WHERE confirmation = TRUE; ";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                List<string> list_eval1 = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    list_eval1.Add(row[0].ToString());
                }
                //label4.Text = list_eval1[0];

                double vids = System.Math.Round((Double.Parse(list_eval1[0]) / Double.Parse(list_eval[0])) * 100);

                MessageBox.Show(
                "Всьго перевірено пацієнтів: " + Double.Parse(list_eval[0]) + "\n" +
                "Підтверджені діагнози: " + Double.Parse(list_eval1[0]) + "\n" +
                "Точність програми: " + vids + "%",
                "Статистика");
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
