namespace HelenExpress.GraphQL.Services
{
    using System.IO.Compression;
    using System.IO;
    using Abstracts;

    public class FileService: IFileService
    {
        public string Save(string fileContent, string fileName)
        {
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            TextWriter sw = new StreamWriter(filePath, true);
            sw.Write(fileContent);
            sw.Close();

            return filePath;
        }
        
        public (string fileType, byte[] archiveData, string archiveName) FetchFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return ("", null, "");
            }
            
            var zipName = $"{Path.GetFileNameWithoutExtension(filePath)}.zip";

            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var theFile = archive.CreateEntry(filePath);
                using (var streamWriter = new StreamWriter(theFile.Open()))
                {
                    streamWriter.Write(File.ReadAllText(filePath));
                }
            }

            return ("application/zip", memoryStream.ToArray(), zipName);
        }
    }
}