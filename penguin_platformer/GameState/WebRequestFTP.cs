using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;


namespace Platformer_Slutprojekt
{
    class WebRequestFTP
    {
        // Names of files in directory
        public List<string> directories;

        // Credentials used to login to ftp server
        NetworkCredential credentials;

        // Address to ftp host
        string ftpHost;

        // Filepath to txt files in ftp directory
        string ftpFilePath;

        /// <summary>
        /// Connects to ftp server and returns txt file names in directory
        /// </summary>
        /// <param name="url">URL used to connect to ftp server</param>
        /// <param name="username">Username used to connect to ftp server</param>
        /// <param name="password">Password used to connect to ftp server</param>
        /// <returns>List of all the txt file names in ftp directory</returns>
        public List<string> EstablishConnection(string url, string username, string password)
        {
            // Save credentials for later use when downloading files
            credentials = new NetworkCredential(username, password);

            // Save URI string for later use
            ftpHost = @"ftp://" + url;

            // Create new webrequest
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpHost + "/PenguinPlatformer/CustomLevels/");

            // List all files in directory
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            request.KeepAlive = false;

            // Assign login credentials
            request.Credentials = credentials;

            directories = new List<string>();

            // Check if ftp connection exists
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    // Filters ftp folder from other files than txt files
                    if(line.Contains(".txt"))
                        directories.Add(line);
                    
                    line = streamReader.ReadLine();
                }
                streamReader.Close();
            }

            // Catch error if ftp connection doesn't exist
            catch (WebException ex)
            {
                directories.Add("No Connection to FTP");
            }

            return directories;
        }
  
        /// <summary>
        /// Download specific levels based on levelname in button
        /// </summary>
        public void DownloadLevel(string levelName)
        {
            // Roaming will be used to place downloaded txt files
            // No need for extra permission to create files here
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\PenguinPlatformer\CustomLevels\";

            // Create new webrequest
            WebClient request = new WebClient();

            // Assign login credentials
            request.Credentials = credentials;

            // Download data into a byte array
                byte[] fileData = request.DownloadData(ftpHost + "/PenguinPlatformer/CustomLevels/" +  levelName);

            // Create a filestream that will write to byte array
            FileStream file = File.Create(dir + levelName);

            // Write full byte array into file
            file.Write(fileData, 0, fileData.Length);

            // Close file so other processes can access it
            file.Close();   
        }
    }
}
