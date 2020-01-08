using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]

public class Packet : MonoBehaviour
{
    public float mouseX = 0.0f;
    public float mouseY = 0.0f;

    public static byte[] ToByteArray(Packet packet)
    {
        MemoryStream stream = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, packet.mouseX);
        formatter.Serialize(stream, packet.mouseY);

        return stream.ToArray();
    }

    public static Packet FromByteArray(byte[] input)
    {
        MemoryStream stream = new MemoryStream(input);
        BinaryFormatter formatter = new BinaryFormatter();
        Packet packet = new Packet();
        packet.mouseX = (float)formatter.Deserialize(stream);
        packet.mouseY = (float)formatter.Deserialize(stream);

        return packet;
    }
}
