
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";
        public SQLiteConnection mySQLiteConnection = new SQLiteConnection("Data Source=E:/Sample/client/SendingData.sqlite3");
        int i = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ReceivedData();
            }
        }

        #region Sending data logic start here
        private void BindGridSending()
        {
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.CommandText = "select * from SendingData";
                    cmd.Connection = mySQLiteConnection;
                    mySQLiteConnection.Open();
                    gvReceived.DataSource = cmd.ExecuteReader();
                    gvReceived.DataBind();
                    mySQLiteConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            WebSending(txtWeb.Text.Trim());
            txtWeb.Text = "";
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Home.aspx");
        }
        protected void btnRefreshData_Click(object sender, EventArgs e)
        {
            BindGridSending();

        }

        private void WebSending(string Data)
        {
            try
            {
                //---data to send to the server---

                string textToSend = Data;

                //---create a TCPClient object at the IP and port no.---
                TcpClient client = new TcpClient();
                client.Connect(SERVER_IP, PORT_NO);
                NetworkStream nwStream = client.GetStream();
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                //---send the text---
                Console.WriteLine("Sending : " + textToSend);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                client.Close();
                BindGridSending();

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        public void InsertData(string data)
        {
            mySQLiteConnection = new SQLiteConnection("Data Source=E:/Sample/client/SendingData.sqlite3");
            if (!File.Exists("E:/Sample/client/SendingData.sqlite3"))
            {
                SQLiteConnection.CreateFile("E:/Sample/client/SendingData.sqlite3");
            }
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(mySQLiteConnection);
                cmd = mySQLiteConnection.CreateCommand();
                mySQLiteConnection.Open();


                cmd.CommandText = "DROP TABLE IF EXISTS SendingData";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE SendingData(ID INTEGER PRIMARY KEY,
                Data TEXT, Datetime TEXT,Status TEXT)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO SendingData(Data, Datetime,Status) VALUES('" + data + "','" + DateTime.Now + "','sent')";
                cmd.ExecuteNonQuery();

                mySQLiteConnection.Close();


            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                mySQLiteConnection.Close();
            }
        }
        #endregion

        #region Received data logic start here
        private void BindGrid()
        {

            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.CommandText = "select * from SendingData";
                    cmd.Connection = mySQLiteConnection;
                    mySQLiteConnection.Open();
                    gvReceived.DataSource = cmd.ExecuteReader();
                    gvReceived.DataBind();
                    mySQLiteConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
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

                //InsertRecivedData(dataReceived);
                BindGrid();
                client.Close();
                listener.Stop();
                if (i == 0)
                {
                    Response.Redirect("~/Home.aspx");
                    i = 1;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

        }
        public void InsertRecivedData(string dataReceived)
        {
            mySQLiteConnection = new SQLiteConnection("Data Source=E:/Sample/client/SendingData.sqlite3");
            if (!File.Exists("E:/Sample/client/SendingData.sqlite3"))
            {
                SQLiteConnection.CreateFile("E:/Sample/client/SendingData.sqlite3");
            }
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(mySQLiteConnection);
                cmd = mySQLiteConnection.CreateCommand();
                mySQLiteConnection.Open();

                cmd.CommandText = "DROP TABLE IF EXISTS RecivedData";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE RecivedData(ID INTEGER PRIMARY KEY,
                Data TEXT, Datetime TEXT,Status TEXT)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO SendingData(Data, Datetime,Status) VALUES('" + dataReceived + "','" + DateTime.Now + "','Received')";
                cmd.ExecuteNonQuery();

                mySQLiteConnection.Close();

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                mySQLiteConnection.Close();
            }

        }
        #endregion

    }
}