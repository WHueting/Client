using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    // Class TCPClient represents controller for communication with host
    class TCPClient
    {
        private int PORT_NO;
        private string SERVER_IP;

        public TCPClient(int port_no, string server_ip) 
        {
            PORT_NO = port_no;
            SERVER_IP = server_ip;
        }

        // Method to send a ready message to the host
        // return string; the next action to be executed
        public string SendReady()
        {
            string textToSend = "Client_Ready";

            TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToSend = Encoding.UTF8.GetBytes(textToSend);

            Console.WriteLine("Sending : " + textToSend);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            string recieved = Encoding.UTF8.GetString(bytesToRead, 0, bytesRead);
            Console.WriteLine(recieved);
            client.Close();

            return recieved;

        }

        // Method for getting a stream of the file to be placed in a directory
        // param newpath; string containing the path of the file in the repository
        // return Stream; stream of file
        public Stream getFile(string newpath)
        {
            int bufferSize = 1024;
            int bytesRead = 0;
            int allBytesRead = 0;

            string textToSend = "Client_GetFile_" + newpath;

            TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToSend = Encoding.UTF8.GetBytes(textToSend);

            Console.WriteLine("Sending : " + textToSend);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            byte[] length = new byte[4];
            bytesRead = nwStream.Read(length, 0, 4);
            int dataLength = BitConverter.ToInt32(length, 0);

            int bytesLeft = dataLength;
            byte[] data = new byte[dataLength];

            while (bytesLeft > 0)
            {
                int nextPacketSize = (bytesLeft > bufferSize) ? bufferSize : bytesLeft;

                bytesRead = nwStream.Read(data, allBytesRead, nextPacketSize);
                allBytesRead += bytesRead;
                bytesLeft -= bytesRead;


            }

            Stream stream = new MemoryStream(data);

            return stream;
        }

        // Method for getting a stream of the contents of the commit
        // return Stream; stream of commit contents
        public Stream getCommit()
        {
            List<string> commit = new List<string>();
            int bufferSize = 1024;
            int bytesRead = 0;
            int allBytesRead = 0;

            string textToSend = "Client_Get_Commit";

            TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToSend = Encoding.UTF8.GetBytes(textToSend);

            Console.WriteLine("Sending : " + textToSend);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            byte[] length = new byte[4];
            bytesRead = nwStream.Read(length, 0, 4);
            int dataLength = BitConverter.ToInt32(length, 0);

            int bytesLeft = dataLength;
            byte[] data = new byte[dataLength];

            while (bytesLeft > 0)
            {
                int nextPacketSize = (bytesLeft > bufferSize) ? bufferSize : bytesLeft;

                bytesRead = nwStream.Read(data, allBytesRead, nextPacketSize);
                allBytesRead += bytesRead;
                bytesLeft -= bytesRead;


            }

            Stream stream = new MemoryStream(data);

            return stream;
        }
    }
}
