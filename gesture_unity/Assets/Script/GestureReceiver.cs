using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class GestureReceiver : MonoBehaviour
{
    public int port = 5005;
    public GestureData LatestData { get; private set; }

    UdpClient udpClient;
    Thread receiveThread;

    void Start()
    {
        udpClient = new UdpClient(port);

        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void ReceiveData()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

        while (true)
        {
            try
            {
                byte[] data = udpClient.Receive(ref endPoint);
                string json = Encoding.UTF8.GetString(data);
                LatestData = JsonUtility.FromJson<GestureData>(json);
            }
            catch
            {
                break;
            }
        }
    }

    void OnApplicationQuit()
    {
        receiveThread?.Abort();
        udpClient?.Close();
    }
}