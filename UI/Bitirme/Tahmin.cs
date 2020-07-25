using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bitirme
{
    public partial class Tahmin : Form
    {
        public Tahmin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public double SayiDondur (String text)
        {
            String newText = text.Replace(",", ".");
            double sonuc = Convert.ToDouble(newText);
            return sonuc;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tree agac = new Tree();
            string sonuc = agac.AltUst(SayiDondur(textBox1.Text), SayiDondur(textBox2.Text), SayiDondur(textBox3.Text), SayiDondur(textBox5.Text), SayiDondur(textBox4.Text));
            label6.Text = sonuc;
            label6.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Tree agac = new Tree();
            string sonuc = agac.MacSonucu(SayiDondur(textBox1.Text), SayiDondur(textBox2.Text), SayiDondur(textBox3.Text));
            label7.Text = sonuc;
            label7.Visible = true;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
