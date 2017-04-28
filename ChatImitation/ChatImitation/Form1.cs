using System;
using System.Windows.Forms;
using System.Net;
using System.Threading;

namespace ChatImitation
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// An instance of ServerManager class
        /// </summary>
		public static ServerManager Manager;
        /// <summary>
        /// An instance of Client class
        /// </summary>
        Client client;
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Initial functionality of the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Form1_Load(object sender, EventArgs e)
        {
            int PortNumber = 1200;
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            serverIPAddress.Text = ipAddress.ToString();
            serverPortNumber.Text = PortNumber.ToString();
            ADOHandler.CreateDatabase();
        }
        /// <summary>
        /// A button that starts the server in a new thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void startServerButton_Click(object sender, EventArgs e)
        {
            Manager = new ServerManager();
            iPAddress.Text = serverIPAddress.Text;
            portNumber.Text = serverPortNumber.Text;
            (new Thread(() =>
            {
                Manager.Run(IPAddress.Parse(serverIPAddress.Text),
                    Int32.Parse(serverPortNumber.Text));
            })).Start();
        }
        /// <summary>
        /// A button that starts the client and updates clients richTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void addTextButton_Click(object sender, EventArgs e)
        {
            // Creating end point for a client
            IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(iPAddress.Text),
                Int32.Parse(portNumber.Text));
            // Creating client object
            client = new Client(remoteEP);
            Manager.Data = null;
            client.SendData(nameBox, messageBox);
            nameBox.Text = "";
            messageBox.Text = "";
            String message = client.GetData();
            client.CloseSocket();
            // updating richTextBox with the data received from the client
            updateBox.Text = message;
        }
    }
}