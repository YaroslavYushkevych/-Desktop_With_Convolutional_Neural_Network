using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diplom
{
    public partial class Form_ADM : Form
    {
        public Form_ADM()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main_m t = new Main_m();
            t.Show();
            Close();
        }

        private void Form_ADM_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ADM_ins t = new ADM_ins();
            t.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ADM_log t = new ADM_log();
            t.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ADM_med t = new ADM_med();
            t.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ADM_pat t = new ADM_pat();
            t.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ADM_res t = new ADM_res();
            t.Show();
        }

        private void закритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Main_no_m t = new Main_no_m();
            t.Show();
            Close();
        }

        private void наСторінкуМедиківToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Main_m t = new Main_m();
            t.Show();
            Close();
        }
    }
}
