using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;

namespace API.Services
{
    public static class UploadFiles
    {
        private const string PathToServiceAccountKeyFile = @"E:\My Work\Thực tập\Task Management Application\API\user_secret.json";
        private const string DirectoryId = "1_MXJ8aZ3UUczunBrRjik-MS2j52SPGM6";

        private static string ReName(string filename)
        {
            string[] names = filename.Split('.');
            string newName = "";
            for (int i = 0; i < names.Length - 1; i++)
            {
                newName += names[i];
            }
            newName += "-" + DateTime.UtcNow.Ticks.ToString();
            newName += "." + names[names.Length - 1];
            return newName;
        }

        public static async Task<string?> Upload(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    var folderName = Path.Combine("Resources");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fileName = ReName(file.FileName);
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.CreateNew))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
                                        .CreateScoped(DriveService.ScopeConstants.Drive);

                    var service = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential
                    });

                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = fileName,
                        Parents = new List<string>() { DirectoryId }
                    };

                    string uploadedFileId;
                    //Create a new file on Google Drive
                    await using (var fsSource = new FileStream(fullPath, FileMode.Open))
                    {
                        // Create a new file, with metadata and stream.
                        var request = service.Files.Create(fileMetadata, fsSource, file.ContentType);
                        request.Fields = "*";
                        var results = await request.UploadAsync(CancellationToken.None);

                        if (results.Status == UploadStatus.Failed)
                        {
                            throw new FileLoadException();
                        }

                        // the file id of the new file we created
                        uploadedFileId = request.ResponseBody?.Id;
                        string Link = request.ResponseBody.WebViewLink;

                        return Link;
                    }
                }

                return null;
            }
            catch
            {
                throw;
            }
        }
    }
}