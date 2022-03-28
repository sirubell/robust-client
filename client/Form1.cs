using System.Net.Sockets;
using System.Text;

namespace client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() => {
                if (SendDataToServer())
                {
                    ReceiveDataToServer();
                }
            });

            t.Start();
        }

        bool SendDataToServer()
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(textBox1.Text);

                if (buffer.Length > 0)
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(buffer, 0, buffer.Length);

                    return true;
                }
                
            }
            catch (Exception ex)
            {
                Invoke(() => { richTextBox1.Text = ex.Message; });
            }

            return false;
        }

        void ReceiveDataToServer()
        {
            try
            {
                NetworkStream stream = client.GetStream();
                Byte[] buffer = new Byte[3];
                String responseData = String.Empty; 

                do
                {
                    Int32 bytes = stream.Read(buffer, 0, buffer.Length);
                    responseData += Encoding.ASCII.GetString(buffer, 0, bytes);
                } while (stream.DataAvailable);
                
                Invoke(() => { textBox2.Text = responseData; });
            }
            catch (Exception ex)
            {
                Invoke(() => { richTextBox1.Text = ex.Message; });
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient("127.0.0.1", 12345);
            }
            catch (Exception ex)
            {
                richTextBox1.Text = ex.Message;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                client.Close();
            }
            catch (Exception ex)
            {
                richTextBox1.Text = ex.Message;
            }
        }
    }
}