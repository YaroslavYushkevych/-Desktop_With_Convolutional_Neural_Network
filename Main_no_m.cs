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

using IronPython.Hosting;
using IronPython.Runtime;
using IronPython;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Threading;

namespace Diplom
{
    public partial class Main_no_m : Form
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


        public Main_no_m()
        {
            InitializeComponent();
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

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
            //Update_table();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Autoriz t = new Autoriz();
            t.Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Autoriz_adm t = new Autoriz_adm();
            t.Show();
            Hide();
        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void якМедикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Autoriz t = new Autoriz();
            t.Show();
            Hide();
        }

        private void якАдмінToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Autoriz_adm t = new Autoriz_adm();
            t.Show();
            Hide();
        }

        private void оновитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update_table();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

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

                sr.Close();
                //label4.Text = line;
                int value = int.Parse(line);

                if (value == 0)
                {
                    label2.Text = "Негативний";
                    label2.ForeColor = System.Drawing.Color.Green;
                    //label4.Text = "";
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
                }
                if (value == 2)
                {
                    label4.Text = "Вірусна пневмонія (не визвана короновірусом) ";
                    label4.ForeColor = System.Drawing.Color.Red;
                }
                if (value == 3)
                {
                    label4.Text = "Вірусна пневмонія (визвана короновірусом) ";
                    label4.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                label4.Text = "Exception: " + ex.Message;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            progressBar1.Maximum = 40;
            progressBar1.Value = 0;

            Thread t = new Thread(new ThreadStart(delegate {
                for (int i = 0; i < 40; ++i)
                {
                    this.Invoke(new ThreadStart(delegate
                    {
                        Thread.Sleep(1000);
                        progressBar1.Value++;
                    }));
                }

            }));
            t.Start();
            //progressBar1.Visible = false;
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

        private void cnfnbcnbrfToolStripMenuItem_Click(object sender, EventArgs e)
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
