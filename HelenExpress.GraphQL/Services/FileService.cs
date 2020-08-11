using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using HelenExpress.GraphQL.Services.Contracts;
using NPOI.XSSF.UserModel;

namespace HelenExpress.GraphQL.Services
{
    using System.IO.Compression;
    using System.IO;
    using Abstracts;

    public class FileService : IFileService
    {
        public string SaveCsv(string fileContent, string fileName)
        {
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            TextWriter sw = new StreamWriter(filePath, false, Encoding.UTF8);
            sw.Write(fileContent);
            sw.Close();

            return filePath;
        }

        public string SaveExcel<T>(IEnumerable<T> data, IDictionary<string, string> headerMappings, string fileName)
        {
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            var header = this.GetHeader<T>(headerMappings);
            var table = this.ToDatatable<T>(data);

            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            var workbook = new XSSFWorkbook();
            var excelSheet = workbook.CreateSheet("Sheet1");

            var row = excelSheet.CreateRow(0);
            var columnIndex = 0;

            var columns = new List<string>();
            foreach (var field in header)
            {
                columns.Add(field.Key);
                row.CreateCell(columnIndex).SetCellValue(field.Value);
                columnIndex++;
            }

            var rowIndex = 1;
            foreach (DataRow dataRow in table.Rows)
            {
                row = excelSheet.CreateRow(rowIndex);
                var cellIndex = 0;
                foreach (var col in columns)
                {
                    row.CreateCell(cellIndex).SetCellValue(dataRow[col].ToString());
                    cellIndex++;
                }

                rowIndex++;
            }

            workbook.Write(fs);

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
                using (var streamWriter = new StreamWriter(theFile.Open(), Encoding.UTF8))
                {
                    streamWriter.Write(File.ReadAllText(filePath));
                }
            }

            return ("application/zip", memoryStream.ToArray(), zipName);
        }

        private IDictionary<string, string> GetHeader<T>(IDictionary<string, string> headerMappings)
        {
            // create header
            var info = typeof(T).GetProperties();
            var header = new Dictionary<string, string>();
            foreach (var prop in typeof(T).GetProperties())
            {
                var headerName = prop.Name;
                if (headerMappings.ContainsKey(prop.Name)) {
                    headerName = headerMappings[prop.Name];
                }

                header.Add(prop.Name, headerName);
            }

            return header;
        }

        private DataTable ToDatatable<T>(IEnumerable<T> data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(data, options);
            var json = new ReadOnlySpan<byte>(jsonUtf8Bytes);

            var result = JsonSerializer.Deserialize<DataTable>(json);

            return result;
        }
    }
}