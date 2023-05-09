using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(TcpListenSocketBehaviour))]
public class TCPSocketConnect : MonoBehaviour
{
    public TcpClient _client;
    [Tooltip("The Server IP Address")]
    public string ip = null;

    [Tooltip("The port the service is running on")]
    public int port = 9021;

    public bool connection = true;
    
    private Text[] text;
    //IEnumerator Start()

    private void Start()
    {
        //getSocketAddress();
        text = GetComponentsInChildren<Text>();

    }

    public void ConnectMe()
    {
        if (connection)
        {
            _client = new TcpClient();
            //_socket.Connect(IPAddress.Parse("127.0.0.1"), _port);
            Debug.Log("Socket Connecting on : " + ip + " port " + port);
            text[0].text = "Socket Connecting on : " + ip + " port " + port;
            _client.Connect(IPAddress.Parse(ip), port);
            connection = !connection;
        }
        else
        {
            Debug.Log("Already Connected - Only do it once for now");
            text[0].text = "Already Connected - Only do it once for now";
        }
    }


    public void SendHelloMessage(Byte[] msg)
    {
        _client.GetStream().BeginWrite(msg, 0, msg.Length, Send_Complete, _client);   // Send_Complete is the Callback 
    }

    private void Send_Complete(IAsyncResult ar)
    {
        Debug.Log("Send Complete Called");
        if (ar.IsCompleted)
        {
            var client = ar.AsyncState as TcpClient;
            client.GetStream().EndWrite(ar);
            //print($"{bytesSent} bytes sent");
            print("End Write sent");

        }
    }

    /*private bool getSocketAddress()
    {

        // connect a UDP socket and then read its local endpoint to get the IP
        string returnedIP = string.Empty;
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
        {
            socket.Connect(ip, _port);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            returnedIP = endPoint.Address.ToString();
        }

        if (ip == returnedIP)
        {
            Debug.Log("Address Connect Check: " + returnedIP + " is true");
            return listenerReady = true;
        } else
        {
            Debug.Log("Address Connect Check Failed: " + ip + " dne " + returnedIP);
            return listenerReady = false;
        }
    }*/

    public void OnClick()
    {
        var msg = Encoding.ASCII.GetBytes("0,0:0");
        SendHelloMessage(msg);
        Debug.Log("On Click SendHelloMessage() clicked");
    }
}