using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SQLite;
using System.IO;
using Microsoft.Extensions.Logging;

namespace client
{
    public partial class Form1 : Form
    {


        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";
        public SQLiteConnection mySQLiteConnection = new SQLiteConnection("Data Source=E:/Sample/client/SendingData.sqlite3");

        public Form1()
        {
            InitializeComponent();

            ReceivedData();
        }


        private void DataSending(string datasend)
        {
            try
            {
                //---data to send to the server---

                string textToSend = datasend;

                //---create a TCPClient object at the IP and port no.---
                TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
                NetworkStream nwStream = client.GetStream();
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                //---send the text---
                Console.WriteLine("Sending : " + textToSend);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                //---read back the text---
                //byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                //int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

                //Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                // Console.ReadLine();
                client.Close();
                InsertSendingData(datasend);
                BindGridSending();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ReceivedData()
        {
            try
            {

                IPAddress localAdd = IPAddress.Parse(SERVER_IP);
                TcpListener listener = new TcpListener(localAdd, PORT_NO);
                Console.WriteLine("Listening...");

                listener.Start();
                Console.WriteLine("started...");
                //---incoming client connected---
                TcpClient client = listener.AcceptTcpClient();

                //---get the incoming data through a network stream---
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                //---read incoming stream---
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                //---convert the data received into a string---
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received : " + dataReceived);

                //RecievedData_addGrid(dataReceived);

                //---write back the text to the client---
                //Console.WriteLine("Sending back : " + dataReceived);
                //nwStream.Write(buffer, 0, bytesRead);
                client.Close();
                listener.Stop();
                Console.ReadLine();
                InsertReceivedData(dataReceived);
                BindGridReceived();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            DataSending(txtSend.Text);

        }

        private void BindGridSending()
        {
            try
            {
                DataTable dt = new DataTable();
                mySQLiteConnection.Open();
                SQLiteCommand cmd = new SQLiteCommand("select * from SendingData  where Status='sent'", mySQLiteConnection);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(dt);
                mySQLiteConnection.Close();

                gvSending.DataSource = dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void BindGridReceived()
        {
            try
            {
                DataTable dt = new DataTable();
                mySQLiteConnection.Open();
                SQLiteCommand cmd = new SQLiteCommand("select * from SendingData  where Status='Received'", mySQLiteConnection);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(dt);
                mySQLiteConnection.Close();

                gvReceived.DataSource = dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void InsertReceivedData(string dataReceived)
        {
            mySQLiteConnection = new SQLiteConnection("Data Source=E:/Sample/client/SendingData.sqlite3");
            if (!File.Exists("E:/Sample/client/SendingData.sqlite3"))
            {

                SQLiteConnection.CreateFile("E:/Sample/client/SendingData.sqlite3");
                Console.WriteLine("Wrestler Database file created");
            }
            try
            {

                SQLiteCommand cmd = new SQLiteCommand(mySQLiteConnection);
                cmd = mySQLiteConnection.CreateCommand();
                mySQLiteConnection.Open();


                //cmd.CommandText = "DROP TABLE IF EXISTS RecivedData";
                //cmd.ExecuteNonQuery();

                //cmd.CommandText = @"CREATE TABLE RecivedData(id INTEGER PRIMARY KEY,
                //data TEXT, datetime TEXT,status TEXT)";
                //cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO SendingData(Data, Datetime,Status) VALUES('" + dataReceived + "','" + DateTime.Now + "','Received')";
                cmd.ExecuteNonQuery();

                mySQLiteConnection.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                mySQLiteConnection.Close();
            }


        }
        public void InsertSendingData(string data)
        {
            mySQLiteConnection = new SQLiteConnection("Data Source=E:/Sample/client/SendingData.sqlite3");
            if (!File.Exists("E:/Sample/client/SendingData.sqlite3"))
            {

                SQLiteConnection.CreateFile("E:/Sample/client/SendingData.sqlite3");
                Console.WriteLine("Wrestler Database file created");
            }
            try
            {

                SQLiteCommand cmd = new SQLiteCommand(mySQLiteConnection);
                cmd = mySQLiteConnection.CreateCommand();
                mySQLiteConnection.Open();


                //cmd.CommandText = "DROP TABLE IF EXISTS SendingData";
                //cmd.ExecuteNonQuery();

                //cmd.CommandText = @"CREATE TABLE SendingData(id INTEGER PRIMARY KEY,
                //data TEXT, datetime TEXT,status TEXT)";
                //cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO SendingData(Data, Datetime,Status) VALUES('" + data + "','" + DateTime.Now + "','sent')";
                cmd.ExecuteNonQuery();

                mySQLiteConnection.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                mySQLiteConnection.Close();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindGridSending();
            BindGridReceived();
            ReceivedData();
        }
    }
}
