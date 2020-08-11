using System.Collections.Generic;
using HelenExpress.GraphQL.Services.Contracts;

namespace HelenExpress.GraphQL.Services.Abstracts
{
    public interface IFileService
    {
        /// <summary>
        /// Save string to a file csv or txt
        /// </summary>
        /// <param name="fileContent">The file content under string</param>
        /// <param name="fileName">The file name with extension csv or txt</param>
        /// <returns>The file path</returns>
        string SaveCsv(string fileContent, string fileName);

        string SaveExcel<T>(IEnumerable<T> data, IDictionary<string, string> headerMappings, string fileName);

        /// <summary>
        /// Fetch file
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>File data</returns>
        (string fileType, byte[] archiveData, string archiveName) FetchFile(string filePath);
    }
}