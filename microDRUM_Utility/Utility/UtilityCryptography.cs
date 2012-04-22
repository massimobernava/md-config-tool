using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace microDrum
{
    public class UtilityCryptography
    {
        public static void AsymmetricCreateKeys(out string xmlPrivateKey, out string xmlPublicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            xmlPrivateKey = rsa.ToXmlString(true);
            xmlPublicKey = rsa.ToXmlString(false);
        }
        /*
        public static byte[] AsymmetricEncrypt(byte[] Data, string xmlPublicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);

            byte[] encryptedData = rsa.Encrypt(Data, true);

            return encryptedData;
        }
        public static byte[] AsymmetricDecrypt(byte[] Data, string xmlPrivateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);

            byte[] decryptedData = rsa.Decrypt(Data, true);

            return decryptedData;
        }
        public static string AsymmetricEncrypt(string Data, string xmlPublicKey)
        {
            byte[] bData = Encoding.UTF8.GetBytes(Data);
            byte[] encryptedData = AsymmetricEncrypt(bData, xmlPublicKey);
            return Convert.ToBase64String(encryptedData);
        }
        public static string AsymmetricDecrypt(string Data, string xmlPrivateKey)
        {

            byte[] bData = Convert.FromBase64String(Data);
            byte[] decryptedData = AsymmetricDecrypt(bData, xmlPrivateKey);

            return Encoding.UTF8.GetString(decryptedData);

        }
         * */
        public static string AsymmetricSign(string Data, string xmlPrivateKey)
        {
            byte[] bData = Encoding.UTF8.GetBytes(Data);
            byte[] signData = AsymmetricSign(bData, xmlPrivateKey);
            return Convert.ToBase64String(signData);
        }
        public static bool AsymmetricVerify(string Data, string Sign, string xmlPublicKey)
        {
            try
            {
                byte[] bData = Encoding.UTF8.GetBytes(Data);
                byte[] bSign = Convert.FromBase64String(Sign);
                return AsymmetricVerify(bData, bSign, xmlPublicKey);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static byte[] AsymmetricSign(byte[] Data, string xmlPrivateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);

            byte[] signData = rsa.SignData(Data, new SHA1CryptoServiceProvider());

            return signData;
        }

        public static bool AsymmetricVerify(byte[] Data, byte[] Sign, string xmlPublicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);

            return rsa.VerifyData(Data, new SHA1CryptoServiceProvider(), Sign);
        }
        public static string GetMD5Hash(string Value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        }

         private static byte[] Permutation = new byte[] { 0x72, 0x32, 0x25, 0x64, 0x64, 0x4f, 0x1e, 0x26, 0x2a, 0x74, 0x37, 0x09, 0x57, 0x02, 0x28, 0x08, 0x14, 0x23, 0x49, 0x10, 0x62, 0x02, 0x1e, 0x7e, 0x5d, 0x1b, 0x27, 0x76, 0x7a, 0x76, 0x05, 0x2e };

        public static byte GetPearsonHash(byte[] Input)
        {
            byte h = 0;
            for (int i = 0; i < Input.Length; i++)
            {
                byte index = (byte)(h ^ Input[i]);
                h = Permutation[index % Permutation.Length];
            }
            return h;
        }
    }
}
