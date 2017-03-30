using Ionic.Zip;
using OfficeOpenXml;
using System.IO;
using System.Web;

namespace CB.CSharp.Extentions
{
    public static class HttpResponseExtender
    {
        public static void WriteFileResponse(this HttpResponse HttpResponse, ExcelPackage ExcelPackage, string FileName)
        {
            HttpResponse.ClearResponseSetTypeAndHeader("application/vnd.ms-excel", FileName);
            HttpResponse.BinaryWrite(ExcelPackage.GetAsByteArray());
            HttpResponse.CloseStream();
        }

        public static void WriteFileResponse(this HttpResponse HttpResponse, ZipFile ZipFile, string FileName)
        {
            HttpResponse.ClearResponseSetTypeAndHeader("application/zip", FileName);
            ZipFile.Save(HttpResponse.OutputStream);
            HttpResponse.CloseStream();
        }

        public static void WriteFileResponse(this HttpResponse HttpResponse, string Path, string FileName)
        {
            HttpResponse.ClearResponseSetTypeAndHeader("application/octet-stream", FileName);
            HttpResponse.BinaryWrite(File.ReadAllBytes(Path + FileName));
            HttpResponse.CloseStream();
        }

        public static void ClearResponseSetTypeAndHeader(this HttpResponse HttpResponse, string ContentType, string FileName)
        {
            HttpResponse.Clear();
            HttpResponse.BufferOutput = false; // for large files...
            HttpResponse.ContentType = ContentType;
            HttpResponse.AddHeader("Content-Disposition", @"attachment; filename=""" + FileName + @"""");
        }

        public static void CloseStream(this HttpResponse HttpResponse)
        {
            HttpResponse.Flush();
            HttpResponse.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}