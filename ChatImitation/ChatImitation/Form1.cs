using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
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
            client = new Client();

            iPAddress.Text = serverIPAddress.Text;
            portNumber.Text = serverPortNumber.Text;
            (new Thread(() =>
            {
                Manager.Run(IPAddress.Parse(serverIPAddress.Text),
                    Int32.Parse(serverPortNumber.Text));
            })).Start();
        }
        /// <summary>
        /// A button that starts the client and updates clients listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void addTextButton_Click(object sender, EventArgs e)
        {
            Manager.Data = null;
            client.StartClient(iPAddress, portNumber, nameBox, messageBox, updateBox);
            nameBox.Text = "";
            messageBox.Text = "";
        }
    }
}