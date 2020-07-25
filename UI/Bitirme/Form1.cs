using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using HtmlAgilityPack;
using System.Net;
using System.Threading;

namespace Bitirme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string connectionString = "Data Source=DESKTOP-B7GV4JV\\SQLEXPRESS;Initial Catalog=Proje;Integrated Security=True";
        

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable Takimlar = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand takimgetir = new SqlCommand("select * from kulüpler", con);
            Takimlar.Load(takimgetir.ExecuteReader());
            con.Close();
            for (int i = 0; i < Takimlar.Rows.Count; i++)
            {
                comboBox1.Items.Add(Takimlar.Rows[i][0].ToString().Trim());
                comboBox2.Items.Add(Takimlar.Rows[i][0].ToString().Trim());
            }

            try
            {
                label8.Text = PuanDurumDondur() + GolKrallariDondur();
                timer1.Enabled = true;
            }
            catch (Exception)
            {

            }
            

        }

        public string PuanDurumDondur()
        {
            string url = "https://www.trtspor.com.tr/italya-serie-a-puandurumu.html";

            string htmlContent = GetContent(url);

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(htmlContent);
            string puandurumu = "                                                                                                                   Puan durumu: 4";
            string bosluk = "    ";
            for (int i = 2; i < 22; i++)
            {
                var node = document.DocumentNode.SelectSingleNode("/html/body/div[1]/div[10]/div/div/div[1]/div[2]/ul[2]/ul/li[" + i + "]/span[2]");
                puandurumu += i-1 + ". " + node.InnerText + bosluk;
            }
            return puandurumu;
        }

        public string GolKrallariDondur()
        {
            string url = "https://www.soccervista.com/topscorer-Serie_A-2019_2020-863056.html";

            string htmlContent = GetContent(url);

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(htmlContent);
            Thread.Sleep(3000);
            string kral = "                                                                                                                  Gol Krallığı Yarışı: ";
            string bosluk = "     ";
            for (int i = 1; i < 11; i++)
            {
                var node = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div[1]/div[3]/table/tbody/tr["+i+"]/td[2]");
                var node3 = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div[1]/div[3]/table/tbody/tr[" + i + "]/td[3]");
                var node2 = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div[1]/div[3]/table/tbody/tr["+i+"]/td[4]");
                kral += i + ". " + node.InnerText.Substring(6) +" "+node2.InnerText+" Gol" + "  "+node3.InnerText.ToUpper()+bosluk ;
            }
            return kral;
        }




        public string GetContent(string urlAddress)
        {
            Uri url = new Uri(urlAddress);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string html = client.DownloadString(url);
            return html;
        }




        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button3.Visible = false;
            chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            comboBox2.Visible = false;
            pictureBox2.Visible = false;
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            chart1.Visible = false;
            comboBox2.Visible = false;
            //chart1.Series.Clear();
            DataTable Takimlar = new DataTable();
            DataTable Grafikler = new DataTable();
            DataTable LigOrtalama = new DataTable();
            DataTable H = new DataTable();
            DataTable O = new DataTable();
            DataTable D = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand takimgetir = new SqlCommand("select * from kulüpler", con);
            Takimlar.Load(takimgetir.ExecuteReader());
            SqlCommand grafikgetir = new SqlCommand("select * from deneme where kulüp='" + Takimlar.Rows[comboBox1.SelectedIndex][0].ToString() + "'", con);
            Grafikler.Load(grafikgetir.ExecuteReader());
            SqlCommand LigOrtalamaGetir = new SqlCommand("select * from OrtalamaozelliklerKulüp", con);
            LigOrtalama.Load(LigOrtalamaGetir.ExecuteReader());
            LigOrtalamaGetir.CommandText = "select * from genelortalamahücum order by 2 desc";
            H.Load(LigOrtalamaGetir.ExecuteReader());
            LigOrtalamaGetir.CommandText = "select * from genelortalamaortasaha order by 2 desc";
            O.Load(LigOrtalamaGetir.ExecuteReader());
            LigOrtalamaGetir.CommandText = "select * from genelortalamadefans order by 2 desc";
            D.Load(LigOrtalamaGetir.ExecuteReader());
            con.Close();
            try
            {
                pictureBox1.Image = Image.FromFile(Takimlar.Rows[comboBox1.SelectedIndex]["Logo"].ToString());
            }
            catch (Exception)
            {

            }

            gradient1.renk1 = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk1"].ToString());
            gradient1.renk2 = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk2"].ToString());

            label1.ForeColor = gradient1.renk2;
            comboBox1.BackColor = gradient1.renk1;
            comboBox1.ForeColor = gradient1.renk2;
            comboBox3.BackColor = gradient1.renk1;
            comboBox3.ForeColor = gradient1.renk2;
            gradient1.Refresh();
            int hsira=0, osira=0, dsira=0;
            for (int i = 0; i < Takimlar.Rows.Count; i++)
            {
                if(H.Rows[i][0].ToString()== Takimlar.Rows[comboBox1.SelectedIndex]["Kulüp"].ToString().Trim())
                {
                    hsira = i+1;
                }
                if(O.Rows[i][0].ToString() == Takimlar.Rows[comboBox1.SelectedIndex]["Kulüp"].ToString().Trim())
                {
                    osira = i+1;
                }
                if(D.Rows[i][0].ToString() == Takimlar.Rows[comboBox1.SelectedIndex]["Kulüp"].ToString().Trim())
                {
                    dsira = i+1;
                }
            }
            label1.Text = Takimlar.Rows[comboBox1.SelectedIndex][0].ToString();
            label2.Text = "Boy ortalaması: "+LigOrtalama.Rows[comboBox1.SelectedIndex][2].ToString();
            label3.Text = "Yaş ortalaması: " + LigOrtalama.Rows[comboBox1.SelectedIndex][3].ToString();
            label4.Text = "Kilo ortalaması: " + LigOrtalama.Rows[comboBox1.SelectedIndex][1].ToString();
            label5.Text = "Hücumda "+H.Rows[hsira-1][1].ToString()+" ile " +hsira.ToString() +". sırada.";
            label6.Text = "Ortasahada "+O.Rows[osira - 1][1].ToString()+" ile "+ osira.ToString()+". sırada.";
            label7.Text = "Defansa "+D.Rows[dsira-1][1].ToString()+" ile "+  dsira.ToString()+". sırada.";

            

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            gradient1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void gradient1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Visible = true;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;

            comboBox2.ForeColor = gradient1.renk1;
            comboBox2.BackColor = gradient1.renk2;
            if (comboBox3.SelectedIndex==0)
            {
                chart1.Visible = true;
                comboBox2.Visible = true;
                chart1.Series.Clear();
                DataTable Takimlar = new DataTable();
                DataTable Grafikler = new DataTable();
                DataTable LigOrtalama = new DataTable();
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand takimgetir = new SqlCommand("select * from kulüpler", con);
                Takimlar.Load(takimgetir.ExecuteReader());
                SqlCommand grafikgetir = new SqlCommand("select * from deneme where kulüp='" + Takimlar.Rows[comboBox1.SelectedIndex][0].ToString() + "'", con);
                Grafikler.Load(grafikgetir.ExecuteReader());
                SqlCommand LigOrtalamaGetir = new SqlCommand("select * from deneme2", con);
                LigOrtalama.Load(LigOrtalamaGetir.ExecuteReader());
                con.Close();
                try
                {
                    pictureBox1.Image = Image.FromFile(Takimlar.Rows[comboBox1.SelectedIndex]["Logo"].ToString());
                }
                catch (Exception)
                {

                }

                //gradient1.renk1 = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk1"].ToString());
                //gradient1.renk2 = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk2"].ToString());
                //gradient1.Refresh();
                string takim1 = Takimlar.Rows[comboBox1.SelectedIndex]["Kulüp"].ToString();
                chart1.Series.Add(takim1);
                chart1.Series[takim1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                chart1.Series[takim1].Color = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk1"].ToString());
                chart1.Series.Add("Lig Ortalaması");
                chart1.Series["Lig Ortalaması"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                chart1.Series["Lig Ortalaması"].Color = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk2"].ToString());
                for (int i = 1; i < Grafikler.Columns.Count; i++)
                {
                    chart1.Series[takim1].Points.AddXY(Grafikler.Columns[i].ToString(), Convert.ToDouble(Grafikler.Rows[0][i]));
                    chart1.Series["Lig Ortalaması"].Points.AddXY(Grafikler.Columns[i].ToString(), Convert.ToDouble(LigOrtalama.Rows[0][i]));
                }

            }

            else if (comboBox3.SelectedIndex == 1)
            {
                chart1.Visible = true;
                comboBox2.Visible = true;
                chart1.Series.Clear();
                DataTable Takimlar = new DataTable();
                DataTable Grafikler = new DataTable();
                DataTable LigOrtalama = new DataTable();
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand takimgetir = new SqlCommand("select * from kulüpler", con);
                Takimlar.Load(takimgetir.ExecuteReader());
                SqlCommand grafikgetir = new SqlCommand("select * from OrtasahaOrtalamalariKulüp where kulüp='" + Takimlar.Rows[comboBox1.SelectedIndex][0].ToString() + "'", con);
                Grafikler.Load(grafikgetir.ExecuteReader());
                SqlCommand LigOrtalamaGetir = new SqlCommand("select * from OrtasahaOrtalamalariLig", con);
                LigOrtalama.Load(LigOrtalamaGetir.ExecuteReader());
                con.Close();
                try
                {
                    pictureBox1.Image = Image.FromFile(Takimlar.Rows[comboBox1.SelectedIndex]["Logo"].ToString());
                }
                catch (Exception)
                {

                }

                //gradient1.renk1 = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk1"].ToString());
                //gradient1.renk2 = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk2"].ToString());
                //gradient1.Refresh();
                string takim1 = Takimlar.Rows[comboBox1.SelectedIndex]["Kulüp"].ToString();
                chart1.Series.Add(takim1);
                chart1.Series[takim1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                chart1.Series[takim1].Color = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk1"].ToString());
                chart1.Series.Add("Lig Ortalaması");
                chart1.Series["Lig Ortalaması"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                chart1.Series["Lig Ortalaması"].Color = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk2"].ToString());
                for (int i = 1; i < Grafikler.Columns.Count; i++)
                {
                    chart1.Series[takim1].Points.AddXY(Grafikler.Columns[i].ToString(), Convert.ToDouble(Grafikler.Rows[0][i]));
                    chart1.Series["Lig Ortalaması"].Points.AddXY(Grafikler.Columns[i].ToString(), Convert.ToDouble(LigOrtalama.Rows[0][i]));
                }
            }

            else if(comboBox3.SelectedIndex==2)
            {
                chart1.Visible = true;
                comboBox2.Visible = true;
                chart1.Series.Clear();
                DataTable Takimlar = new DataTable();
                DataTable Grafikler = new DataTable();
                DataTable LigOrtalama = new DataTable();
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand takimgetir = new SqlCommand("select * from kulüpler", con);
                Takimlar.Load(takimgetir.ExecuteReader());
                SqlCommand grafikgetir = new SqlCommand("select * from DefansOrtalamalariKulüp where kulüp='" + Takimlar.Rows[comboBox1.SelectedIndex][0].ToString() + "'", con);
                Grafikler.Load(grafikgetir.ExecuteReader());
                SqlCommand LigOrtalamaGetir = new SqlCommand("select * from DefansOrtalamalariLig", con);
                LigOrtalama.Load(LigOrtalamaGetir.ExecuteReader());
                con.Close();
                try
                {
                    pictureBox1.Image = Image.FromFile(Takimlar.Rows[comboBox1.SelectedIndex]["Logo"].ToString());
                }
                catch (Exception)
                {

                }

                //gradient1.renk1 = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk1"].ToString());
                //gradient1.renk2 = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk2"].ToString());
                //gradient1.Refresh();
                string takim1 = Takimlar.Rows[comboBox1.SelectedIndex]["Kulüp"].ToString();
                chart1.Series.Add(takim1);
                chart1.Series[takim1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                chart1.Series[takim1].Color = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk1"].ToString());
                chart1.Series.Add("Lig Ortalaması");
                chart1.Series["Lig Ortalaması"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                chart1.Series["Lig Ortalaması"].Color = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk2"].ToString());
                for (int i = 1; i < Grafikler.Columns.Count; i++)
                {
                    chart1.Series[takim1].Points.AddXY(Grafikler.Columns[i].ToString(), Convert.ToDouble(Grafikler.Rows[0][i]));
                    chart1.Series["Lig Ortalaması"].Points.AddXY(Grafikler.Columns[i].ToString(), Convert.ToDouble(LigOrtalama.Rows[0][i]));
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button3.Visible = true ;
            pictureBox2.Visible = true;
            DataTable Takimlar = new DataTable();
            DataTable Grafikler = new DataTable();
            DataTable LigOrtalama = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand takimgetir = new SqlCommand("select * from kulüpler", con);
            Takimlar.Load(takimgetir.ExecuteReader());
            SqlCommand grafikgetir = new SqlCommand("select * from deneme where kulüp='" + Takimlar.Rows[comboBox2.SelectedIndex][0].ToString() + "'", con);
            Grafikler.Load(grafikgetir.ExecuteReader());
            SqlCommand LigOrtalamaGetir = new SqlCommand("select * from OrtalamaozelliklerKulüp", con);
            LigOrtalama.Load(LigOrtalamaGetir.ExecuteReader());
            con.Close();
            try
            {
                pictureBox2.Image = Image.FromFile(Takimlar.Rows[comboBox2.SelectedIndex]["Logo"].ToString());
            }
            catch (Exception)
            {

            }

            gradient1.renk2 = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox2.SelectedIndex]["Renk1"].ToString());
            comboBox2.ForeColor = label4.ForeColor;
            comboBox2.BackColor = gradient1.renk2;
            comboBox1.BackColor = gradient1.renk1;
            comboBox1.ForeColor = label4.ForeColor;
            comboBox3.BackColor = gradient1.renk1;
            comboBox3.ForeColor = label4.ForeColor;
            gradient1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            if (comboBox1.SelectedIndex!=comboBox2.SelectedIndex)
            {
                chart1.Series.Clear();
                if (comboBox3.SelectedIndex == 0)
                {
                    DataTable Takimlar = new DataTable();
                    DataTable Grafikler = new DataTable();
                    DataTable LigOrtalama = new DataTable();
                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    SqlCommand takimgetir = new SqlCommand("select * from kulüpler", con);
                    Takimlar.Load(takimgetir.ExecuteReader());
                    SqlCommand grafikgetir = new SqlCommand("select * from deneme where kulüp='" + Takimlar.Rows[comboBox1.SelectedIndex][0].ToString() + "'", con);
                    Grafikler.Load(grafikgetir.ExecuteReader());
                    SqlCommand LigOrtalamaGetir = new SqlCommand("select * from deneme where kulüp='" + Takimlar.Rows[comboBox2.SelectedIndex][0].ToString() + "'", con);
                    LigOrtalama.Load(LigOrtalamaGetir.ExecuteReader());
                    con.Close();
                    string takim1 = Takimlar.Rows[comboBox1.SelectedIndex]["Kulüp"].ToString();
                    string takim2 = Takimlar.Rows[comboBox2.SelectedIndex][0].ToString();
                    chart1.Series.Add(takim1);
                    chart1.Series[takim1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                    chart1.Series[takim1].Color = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk1"].ToString());
                    chart1.Series.Add(takim2);
                    chart1.Series[takim2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                    chart1.Series[takim2].Color = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox2.SelectedIndex]["Renk1"].ToString());
                    for (int i = 1; i < Grafikler.Columns.Count; i++)
                    {
                        chart1.Series[takim1].Points.AddXY(Grafikler.Columns[i].ToString(), Convert.ToDouble(Grafikler.Rows[0][i]));
                        chart1.Series[takim2].Points.AddXY(Grafikler.Columns[i].ToString(), Convert.ToDouble(LigOrtalama.Rows[0][i]));
                    }
                }

                else if (comboBox3.SelectedIndex == 1)
                {
                    chart1.Visible = true;
                    comboBox2.Visible = true;
                    chart1.Series.Clear();
                    DataTable Takimlar = new DataTable();
                    DataTable Grafikler = new DataTable();
                    DataTable LigOrtalama = new DataTable();
                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    SqlCommand takimgetir = new SqlCommand("select * from kulüpler", con);
                    Takimlar.Load(takimgetir.ExecuteReader());
                    SqlCommand grafikgetir = new SqlCommand("select * from OrtasahaOrtalamalariKulüp where kulüp='" + Takimlar.Rows[comboBox1.SelectedIndex][0].ToString() + "'", con);
                    Grafikler.Load(grafikgetir.ExecuteReader());
                    SqlCommand LigOrtalamaGetir = new SqlCommand("select * from OrtasahaOrtalamalariKulüp where kulüp='" + Takimlar.Rows[comboBox2.SelectedIndex][0].ToString() + "'", con);
                    LigOrtalama.Load(LigOrtalamaGetir.ExecuteReader());
                    con.Close();
                    string takim1 = Takimlar.Rows[comboBox1.SelectedIndex]["Kulüp"].ToString();
                    string takim2 = Takimlar.Rows[comboBox2.SelectedIndex]["Kulüp"].ToString();
                    chart1.Series.Add(takim1);
                    chart1.Series[takim1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                    chart1.Series[takim1].Color = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk1"].ToString());
                    chart1.Series.Add(takim2);
                    chart1.Series[takim2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                    chart1.Series[takim2].Color = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox2.SelectedIndex]["Renk1"].ToString());
                    for (int i = 1; i < Grafikler.Columns.Count; i++)
                    {
                        chart1.Series[takim1].Points.AddXY(Grafikler.Columns[i].ToString(), Convert.ToDouble(Grafikler.Rows[0][i]));
                        chart1.Series[takim2].Points.AddXY(Grafikler.Columns[i].ToString(), Convert.ToDouble(LigOrtalama.Rows[0][i]));
                    }
                }

                else if (comboBox3.SelectedIndex == 2)
                {
                    chart1.Visible = true;
                    comboBox2.Visible = true;
                    chart1.Series.Clear();
                    DataTable Takimlar = new DataTable();
                    DataTable Grafikler = new DataTable();
                    DataTable LigOrtalama = new DataTable();
                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    SqlCommand takimgetir = new SqlCommand("select * from kulüpler", con);
                    Takimlar.Load(takimgetir.ExecuteReader());
                    SqlCommand grafikgetir = new SqlCommand("select * from DefansOrtalamalariKulüp where kulüp='" + Takimlar.Rows[comboBox1.SelectedIndex][0].ToString() + "'", con);
                    Grafikler.Load(grafikgetir.ExecuteReader());
                    SqlCommand LigOrtalamaGetir = new SqlCommand("select * from DefansOrtalamalariKulüp where kulüp='" + Takimlar.Rows[comboBox2.SelectedIndex][0].ToString() + "'", con);
                    LigOrtalama.Load(LigOrtalamaGetir.ExecuteReader());
                    con.Close();
                    string takim1 = Takimlar.Rows[comboBox1.SelectedIndex]["Kulüp"].ToString();
                    string takim2 = Takimlar.Rows[comboBox2.SelectedIndex]["Kulüp"].ToString();
                    chart1.Series.Add(takim1);
                    chart1.Series[takim1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                    chart1.Series[takim1].Color = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox1.SelectedIndex]["Renk1"].ToString());
                    chart1.Series.Add(takim2);
                    chart1.Series[takim2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                    chart1.Series[takim2].Color = System.Drawing.ColorTranslator.FromHtml(Takimlar.Rows[comboBox2.SelectedIndex]["Renk2"].ToString());
                    for (int i = 1; i < Grafikler.Columns.Count; i++)
                    {
                        chart1.Series[takim1].Points.AddXY(Grafikler.Columns[i].ToString(), Convert.ToDouble(Grafikler.Rows[0][i]));
                        chart1.Series[takim2].Points.AddXY(Grafikler.Columns[i].ToString(), Convert.ToDouble(LigOrtalama.Rows[0][i]));
                    }
                } 
            }
            else
            {
                MessageBox.Show("Aynı 2 Takım Seçilemez!!", "UYARI");
            }
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label8.Text= label8.Text.Substring(1)+label8.Text.Substring(0,1);
        }

        private void label8_MouseHover(object sender, EventArgs e)
        {
            timer1.Interval = 100;
        }

        private void label8_MouseLeave(object sender, EventArgs e)
        {
            timer1.Interval = 40;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Tahmin tahmin = new Tahmin();
            tahmin.Visible = true;
        }
    }
}
