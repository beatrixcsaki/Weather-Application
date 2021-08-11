using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


namespace WeatherApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string path;
        string city;
        private void button1_Click(object sender, EventArgs e)
        {
            city = txtcity.Text;
            string uri = string.Format("http://api.weatherapi.com/v1/current.xml?key=4657ab980733494080b153623210904&q={0}&aqi=no", city);
            XDocument doc = XDocument.Load(uri);
            String iconUri = (string)doc.Descendants("icon").FirstOrDefault();
            WebClient client = new WebClient();
            byte[] image = client.DownloadData("http:" + iconUri);
            MemoryStream stream = new MemoryStream(image);

            Bitmap newBitMap = new Bitmap(stream);
            string temp = (string)doc.Descendants("temp_c").FirstOrDefault();
            string feelslike = (string)doc.Descendants("feelslike_c").FirstOrDefault();
            string rainfall = (string)doc.Descendants("precip_mm").FirstOrDefault();
            string wind = (string)doc.Descendants("wind_kph").FirstOrDefault();
            string humidity = (string)doc.Descendants("humidity").FirstOrDefault();

            string country = (string)doc.Descendants("country").FirstOrDefault();
            string cloud = (string)doc.Descendants("text").FirstOrDefault();

            Bitmap icon = newBitMap;
            txttemp.Text = temp;
            txtfeelslike.Text = feelslike;
            txtrainfall.Text = rainfall;
            txtwindk.Text = wind;
            txthumidity.Text = humidity;

            pictureBox1.Image = icon;

        }

        //private void getweather(string city) {}

        private void button2_Click_1(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Region", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Max Temperature", typeof(string));
            dt.Columns.Add("Min Temperature", typeof(string));
            dt.Columns.Add("Rainfall", typeof(string));
            dt.Columns.Add("Wind speed kph", typeof(string));
            dt.Columns.Add("Humidity", typeof(string));
            dt.Columns.Add("Cloud", typeof(string));
            dt.Columns.Add("Icon", typeof(Bitmap));

            city = txtcity.Text;
            Console.WriteLine(city);

            string uri = string.Format("https://api.weatherapi.com/v1/forecast.xml?key=4657ab980733494080b153623210904&q={0}&days=7", city);
            XDocument doc = XDocument.Load(uri);

            foreach (var npc in doc.Descendants("forecastday"))
            {
                Console.WriteLine(1);
                //day = npc.Descendants("forecastday").FirstOrDefault();
                string iconUri = (string)npc.Descendants("icon").FirstOrDefault();
                WebClient client = new WebClient();
                byte[] image = client.DownloadData("http:" + iconUri);
                MemoryStream stream = new MemoryStream(image);

                Bitmap newBitmap = new Bitmap(stream);


                dt.Rows.Add(new object[]
                {
                   (string)doc.Descendants("region").FirstOrDefault(),
                   (string)npc.Descendants("date").FirstOrDefault(),

                   (string)npc.Descendants("maxtemp_c").FirstOrDefault(),
                   (string)npc.Descendants("mintemp_c").FirstOrDefault(),
                   (string)npc.Descendants("totalprecip_mm").FirstOrDefault(),
                   (string)npc.Descendants("avgvis_km").FirstOrDefault(),
                   (string)npc.Descendants("avghumidity").FirstOrDefault(),

                   (string)npc.Descendants("text").FirstOrDefault(),
                   newBitmap


                });
            }
            dataGridView1.DataSource = dt;

        }


        private void button3_Click_1(object sender, EventArgs e)
        {
            path=textBox1.Text;
            city = txtcity.Text;
            string uri = string.Format("https://api.weatherapi.com/v1/forecast.xml?key=4657ab980733494080b153623210904&q={0}&days=7", city);
            XDocument doc = XDocument.Load(uri);
            doc.Save(path + "Weather.xml");
        }

    }
  
}
