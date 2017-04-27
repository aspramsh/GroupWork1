using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatImitation
{
    /// <summary>
    /// A class for providing Client functionality
    /// </summary>
	public class Client
	{
        /// <summary>
        /// A function that starts a Client
        /// </summary>
        /// <param name="iPAddress"></param>
        /// <param name="portNumber"></param>
        /// <param name="name"></param>
        /// <param name="message"></param>
        /// <param name="listbox"></param>
		public void StartClient(System.Windows.Forms.TextBox iPAddress, 
			System.Windows.Forms.TextBox portNumber, System.Windows.Forms.TextBox name,
			System.Windows.Forms.TextBox message, System.Windows.Forms.RichTextBox textbox)
		{
			byte[] bytes = new byte[1024];
			IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(iPAddress.Text), 
				Int32.Parse(portNumber.Text));
            // A socket for sending data to the server
			Socket sender = new Socket(AddressFamily.InterNetwork, 
				SocketType.Stream, ProtocolType.Tcp);
			sender.Connect(remoteEP);
			byte[] msg = Encoding.ASCII.GetBytes("Name: " + name.Text + " Message: " + message.Text + "<EOF>");
			int bytesSent = sender.Send(msg);
            // Receiving dat from the server
            int bytesRec = sender.Receive(bytes);
            sender.Shutdown(SocketShutdown.Both);
			sender.Close();
            textbox.Text = "";
            textbox.Text = (Encoding.ASCII.GetString(bytes, 0, bytesRec));
        }
			
		
	}
}
