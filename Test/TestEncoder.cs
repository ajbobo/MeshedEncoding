using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Meshed_Encoding;

// This creates public versions of the Encoder's protected functions
public class EncoderWrapper : Encoder
{
    new public void EncodeLevel(string msg, int level) => base.EncodeLevel(msg, level);
    new public void ClearMesh() => base.ClearMesh();
}

[TestClass]
public class TestEncoder
{
    [DataTestMethod]
    [DataRow('a', "01100001")]
    [DataRow('A', "01000001")]
    [DataRow('0', "00110000")]
    public void Char2Bin_Test(char inChar, string expected)
    {
        string bin = Encoder.Char2Bin(inChar);
        Assert.AreEqual(expected, bin);
    }

    [TestMethod]
    public void TestClearMesh()
    {
        EncoderWrapper en = new ();
        en.ClearMesh();

        foreach (byte c in en.Mesh)
            Assert.AreEqual(0, c);
    }

    [DataTestMethod]
    [DataRow("abc123", "0,1,1,0,0,0,0,1,0,1,1,0,0,0,1,0,0,1,1,0,0,0,1,1,0,0,1,1,0,0,0,1,0,0,1,1,0,0,1,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0")]
    [DataRow("A1B2C3D4", "0,1,0,0,0,0,0,1,0,0,1,1,0,0,0,1,0,1,0,0,0,0,1,0,0,0,1,1,0,0,1,0,0,1,0,0,0,0,1,1,0,0,1,1,0,0,1,1,0,1,0,0,0,1,0,0,0,0,1,1,0,1,0,0")]
    public void TestEncodeLevel0(string input, string expected)
    {
        EncoderWrapper en = new ();
        en.EncodeLevel(input, 0);

        byte[] mesh = en.Mesh;
        string str = string.Join(',', mesh);
        Assert.AreEqual(expected, str);
    }

    [DataTestMethod]
    [DataRow("abc123", "0,2,2,0,0,0,0,2,0,2,2,0,0,0,2,0,0,2,2,0,0,0,2,2,0,0,2,2,0,0,0,2,0,0,2,2,0,0,2,0,0,0,2,2,0,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0")]
    [DataRow("A1B2C3D4", "0,2,0,0,0,0,0,2,0,0,2,2,0,0,0,2,0,2,0,0,0,0,2,0,0,0,2,2,0,0,2,0,0,2,0,0,0,0,2,2,0,0,2,2,0,0,2,2,0,2,0,0,0,2,0,0,0,0,2,2,0,2,0,0")]
    public void TestEncodeLevel1(string input, string expected)
    {
        EncoderWrapper en = new ();
        en.EncodeLevel(input, 1);

        byte[] mesh = en.Mesh;
        string str = string.Join(',', mesh);
        Assert.AreEqual(expected, str);
    }

    [DataTestMethod]
    [DataRow("abc123", "0,4,4,0,0,0,0,4,0,4,4,0,0,0,4,0,0,4,4,0,0,0,4,4,0,0,4,4,0,0,0,4,0,0,4,4,0,0,4,0,0,0,4,4,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0")]
    [DataRow("A1B2C3D4", "0,4,0,0,0,0,0,4,0,0,4,4,0,0,0,4,0,4,0,0,0,0,4,0,0,0,4,4,0,0,4,0,0,4,0,0,0,0,4,4,0,0,4,4,0,0,4,4,0,4,0,0,0,4,0,0,0,0,4,4,0,4,0,0")]
    public void TestEncodeLevel2(string input, string expected)
    {
        EncoderWrapper en = new ();
        en.EncodeLevel(input, 2);

        byte[] mesh = en.Mesh;
        string str = string.Join(',', mesh);
        Assert.AreEqual(expected, str);
    }

    [DataTestMethod]
    [DataRow("MR=EvilGeniuses!", "0,3,2,0,1,3,0,3,0,3,2,1,2,2,3,0,0,2,3,1,3,1,0,3,0,3,2,2,0,3,0,3,0,3,3,3,0,1,3,2,0,3,3,0,1,2,0,3,0,3,3,2,1,1,2,2,0,1,2,0,0,1,1,3")]
    [DataRow("12345678abcdefgh12345678abcdefgh12345678abcdefgh12345678abcdefgh", "0,170,255,85,0,0,0,255,0,170,255,85,0,0,255,0,0,170,255,85,0,0,255,255,0,170,255,85,0,255,0,0,0,170,255,85,0,255,0,255,0,170,255,85,0,255,255,0,0,170,255,85,0,255,255,255,0,170,255,85,255,0,0,0")]
    [DataRow("This can encode UpTo 64 characters, but that's all. No more.See?", "0,253,250,53,192,229,152,140,0,255,255,148,105,66,144,18,0,175,251,4,83,214,66,169,0,47,255,41,132,164,143,7,0,218,63,128,66,98,242,170,0,251,255,52,64,214,109,249,0,155,255,28,0,158,0,131,0,105,255,128,193,201,129,232")]
    [DataRow("This can encode UpTo 64 characters, but that's all. No more.See?XXXXXX", "0,253,250,53,192,229,152,140,0,255,255,148,105,66,144,18,0,175,251,4,83,214,66,169,0,47,255,41,132,164,143,7,0,218,63,128,66,98,242,170,0,251,255,52,64,214,109,249,0,155,255,28,0,158,0,131,0,105,255,128,193,201,129,232")]
    public void EncodeMultipleLevels(string msg, string expected)
    {
        EncoderWrapper en = new();
        en.EncodeMessage(msg);

        byte[] mesh = en.Mesh;
        string str = string.Join(',', mesh);
        Assert.AreEqual(expected, str);
    }
}