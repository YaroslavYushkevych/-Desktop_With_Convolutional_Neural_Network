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
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;
using System.Threading;

namespace Diplom
{
    public partial class Main_m : Form
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



        public Main_m()
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
                sql = @"select * from inspection_table; ";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                //dgvData.DataSource = null;
                //dgvData.DataSource = dt;
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form_view t = new Form_view();
            t.Show();
        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Autoriz_adm t = new Autoriz_adm();
            t.Show();
            Close();
        }

        private void закритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Main_no_m t = new Main_no_m();
            t.Show();
            Close();
        }

        private void оновитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update_table();
        }



        private void Main_m_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
            Update_table();
            this.label6.Text = "Доброго дня Вам шановний " + Class_DataBank.PIB_medic_active + " !";

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                NpgsqlDataAdapter adap = new NpgsqlDataAdapter("SELECT id, p_fio FROM patient_table", conn);
                adap.Fill(dt);
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "p_fio";
                comboBox1.ValueMember = "id";
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void переглянутиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_view t = new Form_view();
            t.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
            
        }
        public int index_med()
        {
            conn.Open();
            string str = "";
            //str = textBox2.Text;
            sql = @"Select id
                    From medic_table
                    Where m_fio = '" + str + "'; ";
            cmd = new NpgsqlCommand(sql, conn);
            dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            string res = string.Join(Environment.NewLine,
                    dt.Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
            conn.Close();
            //label6.Text = res;
            return int.Parse(res);
        }
        public int index_pat()
        {
            conn.Open();
            string str = "";
            //str = textBox1.Text;
            sql = @"Select id
                    From patient_table
                    Where p_fio = '" + str + "'; ";
            cmd = new NpgsqlCommand(sql, conn);
            dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            string res = string.Join(Environment.NewLine,
                    dt.Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
            conn.Close();
            //label9.Text = res;
            return int.Parse(res);
        }

        public int index_result()
        {

            pictureBox1.Image.Save(@"E:\Neuro\archive\Eval\note.jpeg", ImageFormat.Jpeg);

            progressBar1.Visible = true;
            progressBar1.Maximum = 20;
            progressBar1.Value = 0;

            Thread t = new Thread(new ThreadStart(delegate {
                for (int i = 0; i < 20; ++i)
                {
                    this.Invoke(new ThreadStart(delegate
                    {
                        Thread.Sleep(1000);
                        progressBar1.Value++;
                    }));
                }

            }));
            t.Start();

            pictureBox1.Image.Save(@"E:\Neuro\archive\Eval\note.jpeg", ImageFormat.Jpeg);



            Thread t1 = new Thread(new ThreadStart(delegate {

                Option_Neuro_ExecProcess();

            }));
            t1.Start();


            string line;

            try
            {
                string path = @"E:\Neuro\xyz.txt";
                StreamReader sr = new StreamReader(path);

                line = sr.ReadLine();
                int index_res = 0;

                sr.Close();
                //label4.Text = line;
                int value = int.Parse(line);

                if (value == 0)
                {
                    label2.Text = "Негативний";
                    label2.ForeColor = System.Drawing.Color.Green;
                    index_res = 2;
                    label4.Text = "";
                }
                else if (value != 0)
                {
                    label2.Text = "Позитивний";
                    label2.ForeColor = System.Drawing.Color.Red;
                }
                if (value == 1)
                {
                    label4.Text = "Бактеріальна пневмонія";
                    label4.ForeColor = System.Drawing.Color.Red;
                    index_res = 3;
                }
                if (value == 2)
                {
                    label4.Text = "Вірусна пневмонія (не визвана короновірусом) ";
                    label4.ForeColor = System.Drawing.Color.Red;
                    index_res = 4;
                }
                if (value == 3)
                {
                    label4.Text = "Вірусна пневмонія (визвана короновірусом) ";
                    label4.ForeColor = System.Drawing.Color.Red;
                    index_res = 5;
                }
                return index_res;
            }
            catch (Exception ex)
            {
                label4.Text = "Exception: " + ex.Message;
                return 2;
            }
        }


        public string date_func()
        {
            DateTime d = DateTime.Now;
            //label9.Text = d.ToString("dd-MM-yyyy");
            return d.ToString("dd-MM-yyyy");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            //int ind_pat = index_pat();
            //int ind_med = index_med();

            string str_path = openFileDialog1.FileName;
            string now_date = date_func();
            int combobox_id_pat = Convert.ToInt32(comboBox1.SelectedValue);
            int ind_res = index_result();

            bool cheakd = false;
            //if (checkBox1.Checked){ cheakd = true; }

            string actually_str_path = @"C:\Users\Yarik\Pictures\Pulmologic\" + now_date + "-" + combobox_id_pat + ".jpg";
            pictureBox1.Image.Save(actually_str_path, ImageFormat.Png);
            //pictureBox1.Image.Save(actually_str_path, ImageFormat.Png);

            try
            {
                conn.Open();
                sql = @"select * from i2_inspection('" + combobox_id_pat + "', '" + Class_DataBank.ID_medic_active + "', '" + actually_str_path + "'," +
                    " " + ind_res + ", '" + now_date + "', " + cheakd + "); ";
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
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        static void Option_Neuro_ExecProcess()
        {
            Cursor.Current = Cursors.WaitCursor;
            var psi = new ProcessStartInfo();
            psi.FileName = @"C:\Users\Yarik\AppData\Local\Programs\Python\Python39\python.exe";

            var script = @"E:\Neuro\connector.py";

            psi.Arguments = $"\"{script}\"";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            var errors = "";
            var results = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }
            Cursor.Current = Cursors.Default;

        }
    }
}
