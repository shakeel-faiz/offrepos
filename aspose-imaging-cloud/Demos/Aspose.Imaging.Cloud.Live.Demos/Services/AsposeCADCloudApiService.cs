using Aspose.CAD.Cloud.Sdk.Api;
using Aspose.CAD.Cloud.Sdk.Model.Requests;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Aspose.CAD.Cloud.Live.Demos.Services
{

    public interface IAsposeCADCloudApiService
    {
        Stream Convert(Stream file, string fileName, string toFormat);
    }

    public class AsposeCADCloudApiService : IAsposeCADCloudApiService
    {
        public readonly CadApi CadCloudApi;

        public AsposeCADCloudApiService(IConfiguration config)
        {
            string ClientId = config["AsposeCADUserData:ClientId"];
            string ClientSecret = config["AsposeCADUserData:ClientSecret"];

            CadCloudApi = new CadApi(appKey: ClientSecret, appSid: ClientId);
        }

        public Stream Convert(Stream file, string fileName, string toFormat)
        {
            UploadFileRequest uploadFileRequest = new UploadFileRequest()
            {
                Path = fileName,
                File = file
            };

            CadCloudApi.UploadFile(uploadFileRequest);

            GetDrawingSaveAsRequest getDrawingSaveAsRequest = new GetDrawingSaveAsRequest()
            {
                Name = fileName,
                OutputFormat = toFormat
            };

            var convertResult = CadCloudApi.GetDrawingSaveAs(getDrawingSaveAsRequest);

            DeleteFileRequest deleteFileRequest = new DeleteFileRequest()
            {
                Path = fileName
            };

            CadCloudApi.DeleteFile(deleteFileRequest);

            return convertResult;
        }
    }
}
