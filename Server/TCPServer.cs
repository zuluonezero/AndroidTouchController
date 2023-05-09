using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
	private TcpListener _listener;
	public bool _isReady;
	[Tooltip("The port the service is running on")]
	public int _port = 9021;
	//[SerializeField]
	public System.Net.IPAddress publicIP;
	// for debugging
	public string _publicIP_ro;
	
	void Start()
	{
		//getSocketAddress();
		publicIP = IPAddress.Parse(_publicIP_ro);
		//_listener = new TcpListener(IPAddress.Any, _port);
		_listener = new TcpListener(publicIP, _port);
		// Start listening for client requests.
		_listener.Start();
		// Begins an asynchronous operation to accept an incoming connection attempt.
		// public IAsyncResult BeginAcceptSocket (AsyncCallback? callback, object? state);
		// An AsyncCallback delegate that references the method to invoke when the operation is complete.
		// A user-defined object containing information about the accept operation. This object is passed to the callback delegate when the operation is complete
		_listener.BeginAcceptSocket(Socket_Connected, _listener);
		_isReady = true;
		Debug.Log("Listener has started and is ready on: " + _listener.LocalEndpoint.ToString());
	}

	private void Socket_Connected(IAsyncResult ar)
	{
		Debug.Log("Socket Connected");
		if (ar.IsCompleted)
		{
			var client = (ar.AsyncState as TcpListener).EndAcceptTcpClient(ar);
			var state = new ClientStateObject(client);
			client.GetStream().BeginRead(state.Buffer, 0, state.Buffer.Length, Client_Received, state);
			Debug.Log("Socket is Completed");
		}
	}

	private void Client_Received(IAsyncResult ar)
	{
		Debug.Log("Client Recieved");
		if (ar.IsCompleted)
		{
			var state = ar.AsyncState as ClientStateObject;
			var bytesIn = state.Stream.EndRead(ar);
			if (bytesIn > 0)
			{
				var msg = Encoding.ASCII.GetString(state.Buffer, 0, bytesIn);
				print($"From client: {msg}");
			}
			var newState = new ClientStateObject(state.Client);
			state.Stream.BeginRead(state.Buffer, 0, state.Buffer.Length, Client_Received, newState);
			Debug.Log("Socket Recvieved Completed");
		}
	}

	void getSocketAddress()
	{
		// connect a UDP socket and then read its local endpoint to get the IP
		// InterNetwork indicates that an IP version 4 address is expected when a Socket connects to an endpoint.
		// https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.addressfamily?view=net-6.0
		using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
		{
			socket.Connect("8.8.8.8", 65530);  // 8.8.8.8 is dns.google port 65530 (don't use port 53 it hangs the system)  // means internet access is required.
			IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
			publicIP = endPoint.Address;
			_publicIP_ro = endPoint.Address.ToString();
			socket.Close();
		}
		//publicIP = System.Net.IPAddress.Parse(publicIP);
		Debug.Log("Public IP Set: " + publicIP);
	}


	private void OnDestroy()
	{
		_listener?.Stop();
		_listener = null;
		Debug.Log("Listener Destroyed");
	}
}