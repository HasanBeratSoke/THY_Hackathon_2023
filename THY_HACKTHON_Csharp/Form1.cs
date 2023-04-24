using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace THY_HACKTHON
{
    public partial class Form1 : Form
    {
        SerialPort serialPort;
        bool portDurumu = false;

        public Form1()
        {
            InitializeComponent();

            serialPort = new SerialPort("COM5", 9600);
            serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            //serialPort.Open();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string tagID = serialPort.ReadLine();
            Invoke(new MethodInvoker(() => {
                txtTagID.Text = tagID;
                
            }));

            if (portDurumu)
            {
                serialPort.Close();
                portDurumu = false;
                UpdatePortStatus(); // Port Durumu Label'ını güncelle
            }

        }

        private void UpdatePortStatus()
        {
            if (serialPort.IsOpen)
            {
                lblStatus.Invoke((MethodInvoker)delegate {
                    lblStatus.Text = "Açık";
                    lblStatus.ForeColor = Color.Green;
                });
            }
            else
            {
                lblStatus.Invoke((MethodInvoker)delegate {
                    lblStatus.Text = "Kapalı";
                    lblStatus.ForeColor = Color.Red;
                });
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        private async void BilgiGetir_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "https://jsonplaceholder.typicode.com/todos/" + pnrTxtbox.Text;
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();

                    dynamic data = JsonConvert.DeserializeObject(jsonData);

                    yolcuAd.Text = data.userId.ToString();
                    yolcuSoy.Text = data.title.ToString();
                    yolcuAd.Text = data.completed.ToString();
                   
                    MessageBox.Show("HTTP Error: " + response.StatusCode);

                }

            }
            catch (Exception)
            {
                throw;
            }


        }

        private void btnID_Click(object sender, EventArgs e)
        {

            if (!portDurumu)
            {
                try
                {
                    serialPort.Open();
                    if (!serialPort.IsOpen)
                    {
                        MessageBox.Show("Seri port açılamadı.");
                        return;
                    }
                    portDurumu = true;
                    UpdatePortStatus(); // Port Durumu Label'ını güncelle
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
