using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System;
[Serializable]
public class RemoteControlConfig
{
    public int ListenPort;
}
public class NetworkInput : MonoBehaviour
{
    public RemoteControlConfig remoteControlConfig;
    private int DefaultListenPort = 25001;
    private TcpListener listener;
    private Thread listenerThread;
    private Queue<string> messageQueue = new Queue<string>();
    private List<TcpClient> clients = new List<TcpClient>();
    public static event Action<string> OnMessageReceived;
    private void Start()
    {
        Debug.Log("Starting network input");
        LoadConfig();
        int listenPort = 0;
        if (remoteControlConfig == null)
        {
            listenPort = DefaultListenPort;
        }
        else
        {
            listenPort = remoteControlConfig.ListenPort;
        }
        Debug.Log("Listening on port " + listenPort);
        listener = new TcpListener(IPAddress.Any, listenPort);
        listenerThread = new Thread(new ThreadStart(ListenForClients));
        listenerThread.IsBackground = true;
        listenerThread.Start();
    }
    private void OnDestroy()
    {
        ReleaseNetwork();
    }
    private void OnApplicationQuit()
    {
        ReleaseNetwork();
    }
    private void LoadConfig()
    {
        string configPath = Application.streamingAssetsPath + "/RemoteControlConfig.json";
        string configText = System.IO.File.ReadAllText(configPath);
        remoteControlConfig = JsonUtility.FromJson<RemoteControlConfig>(configText);
    }
    private void ListenForClients()
    {
        listener.Start();
        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
            clientThread.IsBackground = true;
            clientThread.Start(client);
        }
    }
    private void Update()
    {
        ProcessMessages();
    }
    private void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        clients.Add(client);
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;
        while (true)
        {
            bytesRead = 0;
            try
            {
                bytesRead = stream.Read(buffer, 0, 1024);
            }
            catch
            {
                break;
            }
            if (bytesRead == 0)
            {
                break;
            }
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            messageQueue.Enqueue(message);
        }
        client.Close();
    }

    private void ProcessMessages()
    {
        lock (messageQueue)
        {
            while (messageQueue.Count > 0)
            {
                string message = messageQueue.Dequeue();
                // Handle the message
                Debug.Log("Received message: " + message);
                OnMessageReceived?.Invoke(message);
            }
        }
    }
    private void ReleaseNetwork()
    {
        listener.Stop();
        listenerThread.Abort();
        foreach (TcpClient client in clients)
        {
            client.Close();
        }
    }
}
