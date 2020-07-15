using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Getdata
{
    public partial class Form2 : Form
    {
        public double sx, sy, ex, ey;
        

        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            sx = Convert.ToDouble(textBox1.Text);
            sy = Convert.ToDouble(textBox2.Text);
            ex = Convert.ToDouble(textBox3.Text);
            ey = Convert.ToDouble(textBox4.Text);
            this.DialogResult = DialogResult.OK;
        }
    }
}
