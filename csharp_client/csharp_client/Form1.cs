﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csharp_client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Connect("10.1.10.200");
        }

        NetworkStream stream;

        //connect to server
        void Connect(String server)
        {
            try
            {
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);
                stream = client.GetStream();
                AppndText("SERVER: You are now connected, welcome!", Color.Gray);
            }
            catch (ArgumentNullException e)
            {
                Debug.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Debug.WriteLine("SocketException: {0}", e);
            }
        }

        //send text with color and scroll textbox down
        void AppndText(string text, Color color)
        {
            msgs.SelectionStart = msgs.TextLength;
            msgs.SelectionLength = 0;

            msgs.SelectionColor = color;
            msgs.AppendText(text + Environment.NewLine);
            msgs.SelectionColor = msgs.ForeColor;

            msgs.SelectionStart = msgs.Text.Length;
            msgs.ScrollToCaret();
        }

        //send text from sendTextbox to TcpListener
        private void button1_Click(object sender, EventArgs e)
        {
            //send
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(sendTextbox.Text);
            stream.Write(data, 0, data.Length);
            AppndText("CLIENT:" + sendTextbox.Text, Color.Green);
            data = new Byte[2048];

            //receive
            String responseData = String.Empty;
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            AppndText("SERVER:" + responseData, Color.Red);
        }
    }
}
