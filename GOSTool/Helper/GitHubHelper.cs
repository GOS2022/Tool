using System.IO;
using System.Net;

namespace GOSTool
{
    class GitHubHelper
    {
        public static void UpdateOsList()
        {
            var repo = "GOS2022/OS";
            var contentsUrl = $"https://api.github.com/repos/{repo}/contents/?ref=release";

            var content = $"https://github.com/GOS2022/OS/archive/refs/heads/release.zip";
            using (var client = new WebClient())
            {
                Directory.Delete(ProgramData.OSPath, true);
                Directory.CreateDirectory(ProgramData.OSPath);
                client.DownloadFile(content, ProgramData.OSPath + "/os.zip");
                System.IO.Compression.ZipFile.ExtractToDirectory(ProgramData.OSPath + "/os.zip", ProgramData.OSPath);
                File.Delete(ProgramData.OSPath + "/os.zip");
                var allDirectories = Directory.GetDirectories(ProgramData.OSPath + "/OS-release", "*", SearchOption.AllDirectories);
                foreach (string dir in allDirectories)
                {
                    string dirToCreate = dir.Replace(ProgramData.OSPath + "/OS-release", ProgramData.OSPath);
                    Directory.CreateDirectory(dirToCreate);
                }
                var allFiles = Directory.GetFiles(ProgramData.OSPath + "/OS-release", "*.*", SearchOption.AllDirectories);
                foreach (string newPath in allFiles)
                {
                    File.Copy(newPath, newPath.Replace(ProgramData.OSPath + "/OS-release", ProgramData.OSPath), true);
                }
                Directory.Delete(ProgramData.OSPath + "/OS-release", true);
            }
        }

#if false
        private async static void UpdateRoot (string contentsUrl)
        {
            string version = "";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
            httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("GOSTool", ProgramData.Version));
            var contentsJson = await httpClient.GetStringAsync(contentsUrl);
            var contents = (JArray)JsonConvert.DeserializeObject(contentsJson);

            foreach (var file in contents)
            {
                var fileType = (string)file["type"];
                if (fileType == "dir")
                {
                    var directoryContentsUrl = (string)file["url"];
                    version = (string)file["name"];
                    UpdateFilesInDirectory(directoryContentsUrl, version);
                }
            }
        }
        
        private async static void UpdateFilesInDirectory(string contentsUrl, string version)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("GOSTool", ProgramData.Version));
            var contentsJson = await httpClient.GetStringAsync(contentsUrl);
            var contents = (JArray)JsonConvert.DeserializeObject(contentsJson);

            foreach (var file in contents)
            {
                var fileType = (string)file["type"];
                if (fileType == "dir")
                {
                    var directoryContentsUrl = (string)file["url"];
                    UpdateFilesInDirectory(directoryContentsUrl, version);
                }
                else if (fileType == "file")
                {
                    var downloadUrl = (string)file["download_url"];
                    var fileName = (string)file["name"];
                    var path = (string)file["path"];

                    using (var client = new WebClient())
                    {
                        if (!Directory.Exists(ProgramData.OSPath + /*"/current/"*/ "/" + version + "/" + path.Replace(fileName, "")))
                            Directory.CreateDirectory(ProgramData.OSPath + /*"/current/"*/"/" + version + "/" + path.Replace(fileName, ""));
                        client.DownloadFile(downloadUrl, ProgramData.OSPath + /*"/current/"*/"/" + version + "/" + path);
                    }
                }
            }
        }
#endif
    }
}
