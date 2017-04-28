using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ChatImitation
{
    /// <summary>
    /// A class for providing Client functionality
    /// </summary>
	public class Client
    {
        /// <summary>
        /// An endpoint for creating a socket in Client
        /// </summary>
        private IPEndPoint RemoteEP { get; set; }
        /// <summary>
        /// Client's socket
        /// </summary>
        private Socket Sender { get; set; }
        /// <summary>
        /// Client's constructor that initializes client's socket
        /// </summary>
        /// <param name="remoteEp"></param>
        public Client(IPEndPoint remoteEp)
        {
            this.Sender = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            this.RemoteEP = remoteEp;
            this.Sender.Connect(RemoteEP);
        }
        /// <summary>
        /// A method that sends clients data to the server
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public void SendData(System.Windows.Forms.TextBox name, System.Windows.Forms.TextBox message)
        {
            byte[] msg = Encoding.ASCII.GetBytes("Name: " + name.Text + " Message: " + message.Text + "<EOF>");
            int bytesSent = Sender.Send(msg);
        }
        /// <summary>
        /// A method that gets data from the server
        /// </summary>
        /// <returns></returns>
        public string GetData()
        {
            byte[] bytes = new byte[1024];
            int bytesRec = this.Sender.Receive(bytes);
            string Data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            return Data;
        }
        /// <summary>
        /// A method that closes client's socket
        /// </summary>
        public void CloseSocket()
        {
            Sender.Shutdown(SocketShutdown.Both);
            Sender.Close();
        }
	}
}
