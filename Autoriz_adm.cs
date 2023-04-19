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
    public partial class Autoriz_adm : Form
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

        public Autoriz_adm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Boolean f = false;

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

                conn.Open();
                sql = @"select admin from login_table;";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                List<string> list_admin = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    list_admin.Add(row[0].ToString());
                }

                for (int i = 0; i < list_password.Count; i++)
                {
                    if (textBox1.Text == list_login[i] && textBox2.Text == list_password[i] && list_admin[i] == "True")
                    {
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
                Form_ADM t = new Form_ADM();
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

        private void Autoriz_adm_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
        }
    }
}
