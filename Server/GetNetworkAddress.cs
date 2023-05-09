using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Net.NetworkInformation;

public class GetNetworkAddress : MonoBehaviour
{
	private string ip;
	private Text[] text;
	private int port;
	// used for passing to Listener
	
	// Start is called before the first frame update
	void Start()
	{
		text = GetComponentsInChildren<Text>();
		port = GetComponent<TCPServer>()._port;

	}

	public void GetAddresses()
	{
		Debug.Log("click!");
		PrintIPAddressInfo();  /// Not getting anything so commented out
		getSocketAddress();
		GetLocalIPAddressWithNetworkInterface(NetworkInterfaceType.Wireless80211);
		PrintPort();
	}

	void PrintIPAddressInfo()
	{
		string strHostName = string.Empty;
		IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
		Debug.Log("Print IPHostEntry from PrintIPAddressInfo: " + ipEntry);
		IPAddress[] addr = ipEntry.AddressList;

		for (int i = 0; i < addr.Length; i++)
		{
			ip = addr[i].ToString();
			text[1].text = text[1].text + "IP-" + i + " : " + ip + "\n";
			Debug.Log("IP Address : " + ip);
		}
		
	}

	void getSocketAddress()
	{

		// connect a UDP socket and then read its local endpoint to get the IP
		string localIP = string.Empty;
		using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
		{
			socket.Connect("8.8.8.8", 65530);
			IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
			localIP = endPoint.Address.ToString();
		}
		Debug.Log("getSocketAddress localIP: " + localIP);
		text[1].text = text[1].text + "Sock: " + localIP + "\n";
	}

	void GetLocalIPAddressWithNetworkInterface(NetworkInterfaceType _type)
	{
		string output = "";
		foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
		{
			if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
			{
				foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
				{
					if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
					{
						output = ip.Address.ToString();
					}
				}
			}
		}
		Debug.Log("GetLocalIPAddressWithNetworkInterface output " + output);
		text[1].text = text[1].text + "NetType: " + output + "\n";
		if (output == null)
		{
			text[1].text = text[1].text + "Connect to Wifi \n";
		}
	}

	void PrintPort()
	{
		Debug.Log("Port " + port);
		text[1].text = text[1].text + "Listening Port: " + port + "\n";
	}


}
