using System;
using System.Text;

namespace Meshed_Encoding;

/***************************************************************************************
 Meshed Encoding (My name - it may have a different real name)
 Inspired by https://www.youtube.com/watch?v=8Da2fweYTkc
 Each character in the message is split across 8 numbers in the mesh, 1 bit per number
   The first 8 chars in msg are in bit 1 (least significant)
   The second 8 chars are in bit 2 (1 << 1)
   The third 8 chars are in bit 3 (1 << 2)
 msg[0] is in mesh[0..7], bit 1
 msg[1] is in mesh[8..15], bit 1
 ...
 msg[7] is in mesh[56..63], bit 1
 msg[8] is in mesh[0..7], bit 2
 msg[9] is in mesh[8..15], bit 2
 The encoded data will always be 64 bytes
 This can encode messages of up to 64 characters
    It doesn't save any space, but it obfuscates the data pretty well
 If the mesh is a different type (ex: short) then it can store more data, but it still
    doesn't save space
 ****************************************************************************************/

public class Encoder
{
    private byte[,] _mesh = new byte[8, 8];

    public static string Char2Bin(char curChar)
    {
        StringBuilder builder = new();
        for (int x = 7; x >= 0; x--)
        {
            builder.Append((curChar & (1 << x)) > 0 ? "1" : "0");
        }
        return builder.ToString();
    }

    public byte[] Mesh
    {
        get
        {
            byte[] res = new byte[64];
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    res[x * 8 + y] = _mesh[x, y];
                }
            }
            return res;
        }
    }

    public void EncodeMessage(string msg)
    {
        ClearMesh();

        // Each level encodes 8 characters
        int levelsNeeded = (int)Math.Ceiling(msg.Length / 8.0);

        for (int level = 0; level < levelsNeeded; level++)
        {
            int start = level * 8;
            int len = Math.Min(8, msg.Length - start);
            string substr = msg.Substring(start, len);
            EncodeLevel(substr, level);
        }
    }

    protected void EncodeLevel(string msg, int level)
    {
        Console.WriteLine($"Encoding Level {level} => {msg}");
        for (int charIndex = 0; charIndex < msg.Length; charIndex++)
        {
            char curChar = msg[charIndex];
            Console.WriteLine($"Encoding character: {curChar} => {Char2Bin(curChar)}");
            for (byte bitIndex = 7; bitIndex <= 7; bitIndex--) // byte is unsigned and wraps from 0->255, so bitIndex is never < 0
            {
                bool bitEnabled = (curChar & (1 << bitIndex)) > 0;
                if (bitEnabled)
                    _mesh[charIndex, 7 - bitIndex] |= (byte)(1 << level); // If level > 7, (1 << level) becomes 0, so the values at that level are lost
            }
        }
    }

    protected void ClearMesh()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
                _mesh[x, y] = 0;
        }
    }
}