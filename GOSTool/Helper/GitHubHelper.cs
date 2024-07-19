using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GOSTool
{
    class GitHubHelper
    {
        public static void UpdateOsList()
        {
            var repo = "GOS2022/OS";
            var contentsUrl = $"https://api.github.com/repos/{repo}/contents";
            UpdateFilesInDirectory(contentsUrl);
        }

        private async static void UpdateFilesInDirectory(string contentsUrl)
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
                    UpdateFilesInDirectory(directoryContentsUrl);
                }
                else if (fileType == "file")
                {
                    var downloadUrl = (string)file["download_url"];
                    var fileName = (string)file["name"];
                    var path = (string)file["path"];

                    using (var client = new WebClient())
                    {
                        if (!Directory.Exists(ProgramData.OSPath + "/current/" + path.Replace(fileName, "")))
                            Directory.CreateDirectory(ProgramData.OSPath + "/current/" + path.Replace(fileName, ""));
                        client.DownloadFile(downloadUrl, ProgramData.OSPath + "/current/" + path);
                    }
                }
            }

        }
    }
}
