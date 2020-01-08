using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server : SingletonMonoBehavior<Server>
{
    private Socket serverSocket = null;
    private ArrayList Connections = new ArrayList();
    private ArrayList Buffer = new ArrayList();
    private ArrayList ByteBuffers = new ArrayList();

    public const int portNumber = 12345;

    private void Start()
    {
        //Create Socket
        Debug.Log("Server start!");
        this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, Server.portNumber);

        //Bind
        this.serverSocket.Bind(ipLocal);

        //Listen
        Debug.LogWarning("Start Listening...");
        this.serverSocket.Listen(100);
    }

    void SocketCleanUp()
    {
        if(this.serverSocket != null)
        {
            this.serverSocket.Close();
        }
        this.serverSocket = null;

        foreach(Socket client in this.Connections)
        {
            client.Close();
        }
        this.Connections.Clear();
    }

    private void OnApplicationQuit()
    {
        SocketCleanUp();
    }

    private void Update()
    {
        ArrayList listenList = new ArrayList();
        listenList.Add(this.serverSocket);

        Socket.Select(listenList, null, null, 1000);

        for (int i = 0; i < listenList.Count; i++)
        {
            Socket newConnection = ((Socket)listenList[i]).Accept();
            this.Connections.Add(newConnection);
            this.ByteBuffers.Add(new ArrayList());
            Debug.Log("New client is connected.");
        }
    }
}
