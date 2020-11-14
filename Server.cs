using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;     
using System.Net.Sockets;

namespace tcp_server
{
    class Server
    {
        public static void Main(string[] args)
        {
            // IPAddress ipAddr = Dns.GetHostEntry("localhost").AddressList[0]; // Gets the first address associated with that host
            IPAddress ipAddr = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener server = new TcpListener(ipAddr, 9999);
            // we set our IP address as server's address, and we also set the port: 9999

            server.Start();  // this will start the server

            while (true)   //we wait for a connection
            {
                string mac_addr = Mac_addr_grab.mac_addr_find();
                // Make the MAC a required header? Use the server as an almost API style? Write a Python script to do it?
                //Console.WriteLine(mac_addr);
                
                TcpClient client = server.AcceptTcpClient();  //if a connection exists, the server will accept it

                NetworkStream ns = client.GetStream(); //networkstream is used to send/receive messages

                byte[] hello = new byte[100];   //any message must be serialized (converted to byte array)
                hello = Encoding.Default.GetBytes("Hello World");  //conversion string => byte array

                if (ns.CanWrite)
                {
                    ns.Write(hello, 0, hello.Length);     //sending the message
                }

                else
                {
                    Console.WriteLine("Cannot write to NetworkStream object");
                }

                while (client.Connected)  //while the client is connected, we look for incoming messages
                {
                    byte[] msg = new byte[1024];     //the messages arrive as byte array
                    try
                    {
                        ns.Read(msg, 0, msg.Length);   //the same networkstream reads the message sent by the client
                    }

                    catch (System.IO.IOException)
                    {
                        Console.WriteLine("Existing connection forcibly closed by the remote host, exiting cleanly:");
                        ns.Close();
                        server.Stop();
                        client.Close();
                        System.Environment.Exit(0);
                    }

                    string msg_decoded = Encoding.Default.GetString(msg).Trim(' '); // Decodes the message
                    Console.WriteLine(msg_decoded);
                }
            }
        }
    }
}
