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
    public partial class Autoriz : Form
    {
        private string connstring = String.Format(  "Server = {0};" +
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

        public Autoriz()
        {
            InitializeComponent();
        }

        public int index_log(string log)
        {
            conn.Open();
            sql = @"Select id
                    From login_table
                    Where login = '" + log + "'; ";
            cmd = new NpgsqlCommand(sql, conn);
            dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            string res = string.Join(Environment.NewLine,
                    dt.Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
            conn.Close();
            //label2.Text = res;
            return int.Parse(res);
        }
        public int index_PIB(int id_log)
        {
            conn.Open();
            sql = @"Select id
                    From medic_table
                    Where m_login = " + id_log + "; ";
            cmd = new NpgsqlCommand(sql, conn);
            dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            string res = string.Join(Environment.NewLine,
                    dt.Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
            conn.Close();
            //label1.Text = res;
            return int.Parse(res);
        }
        public string seek_PIB(int id_pib)
        {
            conn.Open();
            sql = @"Select m_fio
                    From medic_table
                    Where id = " + id_pib + "; ";
            cmd = new NpgsqlCommand(sql, conn);
            dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            string res = string.Join(Environment.NewLine,
                    dt.Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
            conn.Close();
            //label1.Text = res;
            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Boolean f = true;

            try
            {
                conn.Open();
                sql = @"select login from login_table;";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                List<string> list_login = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    list_login.Add(row[0].ToString());
                }


                conn.Open();
                sql = @"select password from login_table;";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                List<string> list_password = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    list_password.Add(row[0].ToString());
                }

                int ind_PIB = 0;
                int ind_log = 0;
                string seek_pib;

                for (int i = 0; i < list_password.Count; i++)
                {
                    if (textBox1.Text == list_login[i] && textBox2.Text == list_password[i])
                    {
                        ind_log = index_log(textBox1.Text);
                        ind_PIB = index_PIB(ind_log);
                        seek_pib = seek_PIB(ind_PIB);
                        Class_DataBank.PIB_medic_active = seek_pib;
                        Class_DataBank.ID_medic_active = ind_PIB;
                        f = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error: " + ex.Message);
            }


            if (f == true)
            {
                Main_m t = new Main_m();
                t.Show();
                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Main_no_m t = new Main_no_m();
            t.Show();
            Close();
        }

        private void Autoriz_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
        }
    }
}
