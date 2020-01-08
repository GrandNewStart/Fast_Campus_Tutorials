using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class Client : SingletonMonoBehavior<Client>
{
    public string serverIP = "127.0.0.1";
    private Socket clientSocket = null;

    void Start()
    {
        //Create Socket
        this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //Set server access point
        IPAddress serverIPAddress = IPAddress.Parse(this.serverIP);
        IPEndPoint serverEndPoint = new IPEndPoint(serverIPAddress, Server.portNumber);

        //Connect
        try
        {
            Debug.Log("Connecting to server...");
            this.clientSocket.Connect(serverEndPoint);
        }
        catch (SocketException e)
        {
            Debug.Log("Connection failed: " + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        if (this.clientSocket != null)
        {
            this.clientSocket.Close();
            this.clientSocket = null;
        }
    }

    static public void Send(Packet packet)
    {
        if (Client.Instance.clientSocket == null)
        {
            return;
        }
        byte[] sendData = Packet.ToByteArray(packet);
        byte[] prefixSize = new byte[1];
        prefixSize[0] = (byte)sendData.Length;
        Client.Instance.clientSocket.Send(prefixSize);
        Client.Instance.clientSocket.Send(sendData);

        Debug.Log("Send packet from client: " + packet.mouseX.ToString() + " / " + packet.mouseY.ToString());

    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Packet newPacket = new Packet();
            newPacket.mouseX = Input.mousePosition.x;
            newPacket.mouseY = Input.mousePosition.y;
            Send(newPacket);
        }
    }
}
