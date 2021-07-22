using Aspose.Diagram.Cloud.SDK.Api;
using Aspose.Diagram.Cloud.SDK.Model;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Aspose.Diagram.Cloud.Live.Demos.Services
{
    public interface IAsposeDiagramCloudApiService
    {
        Stream Convert(Stream file, string fileName, string outputFileName);
    }

    public class AsposeDiagramCloudApiService : IAsposeDiagramCloudApiService
    {
        public readonly DiagramApi DiagramCloudApi;
        public readonly StorageApi StorageCloudApi;

        public AsposeDiagramCloudApiService(IConfiguration config)
        {
            string ClientId = config["AsposeDiagramUserData:ClientId"];
            string ClientSecret = config["AsposeDiagramUserData:ClientSecret"];

            DiagramCloudApi = new DiagramApi(grantType: "client_credentials", appKey: ClientSecret, appSID: ClientId);
            StorageCloudApi = new StorageApi(grantType: "client_credentials", appKey: ClientSecret, appSID: ClientId);
        }

        public Stream Convert(Stream file, string fileName, string outputFileName)
        {
            // Upload input file to cloud storage
            StorageCloudApi.UploadFile(path: fileName, file: file);

            SaveOptionsRequest request = new SaveOptionsRequest()
            {
                FileName = outputFileName,
                Folder = ""
            };

            // Convert input file (format) to output file (format).
            // Output file will be saved to cloud storage.
            DiagramCloudApi.SaveAs(name: fileName, saveOptionsRequest: request);

            // After conversion, download output file from cloud storage.
            Stream convertResult = StorageCloudApi.DownloadFile(path: outputFileName);

            // Delete both input and output files from cloud storage.
            StorageCloudApi.DeleteFile(path: fileName);
            StorageCloudApi.DeleteFile(path: outputFileName);

            // Return the converted stream
            return convertResult;
        }
    }
}
