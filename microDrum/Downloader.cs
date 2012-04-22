using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace microDrum
{
    public class Downloader
    {

        public static void Download(string url, string filePath)
        {
            HttpWebResponse Response;

            //Retrieve the File
            HttpWebRequest Request = (HttpWebRequest)HttpWebRequest.Create(url);
            Request.Headers.Add("Translate: f");
            Request.Credentials = CredentialCache.DefaultCredentials;

            //Set up the last modfied time header
            if (File.Exists(filePath))
                Request.IfModifiedSince = LastModFromDisk(filePath);

            try
            {
                Response = (HttpWebResponse)Request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    MessageBox.Show("Error accessing Url " + url);
                    throw;
                }

                HttpWebResponse errorResponse = (HttpWebResponse)e.Response;

                //if the file has not been modified
                if (errorResponse.StatusCode == HttpStatusCode.NotModified)
                {
                    e.Response.Close();
                    return;
                }
                else
                {
                    e.Response.Close();
                    MessageBox.Show("Error accessing Url " + url);
                    throw;
                }
            }

            Stream respStream = null;

            try
            {
                respStream = Response.GetResponseStream();
                CopyStreamToDisk(respStream, filePath);

                DateTime d = System.Convert.ToDateTime(Response.GetResponseHeader("Last-Modified"));
                File.SetLastWriteTime(filePath, d);
            }
            catch (Exception)
            {
                MessageBox.Show("Error writing to:  " + filePath);
                throw;
            }
            finally
            {
                if (respStream != null)
                    respStream.Close();
                if (Response != null)
                    Response.Close();
            }
        }

        public static DateTime LastModFromDisk(string filePath)
        {
            FileInfo f = new FileInfo(filePath);
            return (f.LastWriteTime);
        }

        private static void CopyStreamToDisk(Stream responseStream, String filePath)
        {
            byte[] buffer = new byte[4096];
            int length;

            //Copy to a temp file first so that if anything goes wrong with the network
            //while downloading the file, we don't actually update the real on file disk
            //This essentially gives us transaction like semantics.
            Random Rand = new Random();
            string tempPath = Environment.GetEnvironmentVariable("temp") + "\\";
            tempPath += filePath.Remove(0, filePath.LastIndexOf("\\") + 1);
            tempPath += Rand.Next(10000).ToString() + ".tmp";

            FileStream AFile = File.Open(tempPath, FileMode.Create, FileAccess.ReadWrite);

            length = responseStream.Read(buffer, 0, 4096);
            while (length > 0)
            {
                AFile.Write(buffer, 0, length);
                length = responseStream.Read(buffer, 0, 4096);
            }
            AFile.Close();

            if (File.Exists(filePath))
                File.Delete(filePath);
            File.Move(tempPath, filePath);
        }
    }
}
