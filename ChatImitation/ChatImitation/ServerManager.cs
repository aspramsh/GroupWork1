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
    /// A class for providing server functionality
    /// </summary>
	public class ServerManager
	{
        /// <summary>
        /// A property that keeps data transferred to the server
        /// </summary>
		public string Data { get; set; }
        /// <summary>
        /// A structure for standardizing sent data
        /// </summary>
		public struct ClientRequestType
		{
			public string Name;
			public string Message;
		}
        /// <summary>
        /// A method that starts server and runs it's methods
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="portNumber"></param>
		public void Run(IPAddress ipAddress, int portNumber)
		{
			IPEndPoint localEndPoint = new IPEndPoint(ipAddress, portNumber);
            // A server socket
			Socket listener = new Socket(AddressFamily.InterNetwork,
				SocketType.Stream, ProtocolType.Tcp);
			Data = null;
			listener.Bind(localEndPoint);
			listener.Listen(10);
			while (true)
			{
                // A socket that acceps data transferred from a client
				Socket handler = listener.Accept();
				while (true)
				{
					byte[] bytes = new byte[1024];
                    // Receiving data in binary representation
					int bytesRec = handler.Receive(bytes);
                    // Transforming binary data to string
					Data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
					if (Data.IndexOf("<EOF>") > -1)
					{
						break;
					}
                }
                Thread.Sleep(1000);
                // Adding received data to the database
                ADOHandler.AddEntry(Form1.Manager.Parse(Form1.Manager.Data).Name, 
                    Form1.Manager.Parse(Form1.Manager.Data).Message);
                // Getting all data from database
                ADOHandler.GetAllEntries();
                string entry = ADOHandler.data;
                // transforming data received from database into bytes
                byte[] msg = Encoding.ASCII.GetBytes(entry);
                // sending binary data back to the client / clients
                handler.Send(msg);
                // Deactivating the socket
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
		} 
        /// <summary>
        /// A method that standardizes the data received from a client to write it
        /// into the database
        /// </summary>
        /// <param name="dataStream"></param>
        /// <returns></returns>
		public ClientRequestType Parse(string dataStream)
		{
			ClientRequestType newRequest;
			newRequest.Name = "";
            newRequest.Message = "";
            int i = dataStream.IndexOf("Name: ");
            int j = i + "Name: ".Length;
            int f = dataStream.IndexOf("Message: ");
            int s = f + "Message: ".Length;
            int l = dataStream.IndexOf("<EOF>");
            for (int e = j; e < f; ++e)
            {
                newRequest.Name += dataStream[e];
            }
            for (int e = s; e < l; ++e)
            {
                newRequest.Message += dataStream[e];
            }
            return newRequest;
		}
	}
}
