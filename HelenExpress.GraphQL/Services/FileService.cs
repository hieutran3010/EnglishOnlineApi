using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using HelenExpress.GraphQL.Services.Contracts;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
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

        public string SaveExcel<T>(IEnumerable<T> data, IDictionary<string, ExcelFieldDefinition> headerMappings,
            string fileName)
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
                var propName = field.Key;
                var propDisplayName = field.Value;

                columns.Add(propName);
                row.CreateCell(columnIndex).SetCellValue(propDisplayName);
                columnIndex++;
            }

            var rowIndex = 1;
            foreach (DataRow dataRow in table.Rows)
            {
                row = excelSheet.CreateRow(rowIndex);
                var cellIndex = 0;
                foreach (var col in columns)
                {
                    var propName = col;
                    var cellType = CellType.String;
                    if (headerMappings.ContainsKey(propName))
                    {
                        var fieldDefinition = headerMappings[propName];
                        if (fieldDefinition.FieldType != null && (fieldDefinition.FieldType == typeof(int) ||
                                                                  fieldDefinition.FieldType == typeof(double)))
                        {
                            cellType = CellType.Numeric;
                        }
                    }

                    var cellValue = dataRow[col]?.ToString();
                    if (cellType == CellType.String)
                    {
                        row.CreateCell(cellIndex).SetCellValue(cellValue);
                    }
                    else
                    {
                        var value = double.Parse(string.IsNullOrWhiteSpace(cellValue) ? "0" : cellValue,
                            NumberStyles.Any);
                        row.CreateCell(cellIndex, cellType).SetCellValue(value);
                    }

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
                archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
            }

            return ("application/zip", memoryStream.ToArray(), zipName);
        }

        private IDictionary<string, string> GetHeader<T>(IDictionary<string, ExcelFieldDefinition> headerMappings)
        {
            // create header
            var header = new Dictionary<string, string>();
            foreach (var prop in typeof(T).GetProperties())
            {
                var headerName = prop.Name;
                if (headerMappings.ContainsKey(prop.Name))
                {
                    headerName = headerMappings[prop.Name].DisplayText;
                }

                header.Add(prop.Name, headerName);
            }

            return header;
        }

        private DataTable ToDatatable<T>(IEnumerable<T> data)
        {
            return (DataTable) JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data), (typeof(DataTable)));
        }
    }
}