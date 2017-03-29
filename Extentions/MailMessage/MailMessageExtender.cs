using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace CB.CSharp.Extentions
{
    public static class MailMessageExtender
    {
        public static PdfDocument GetAsPdf(this MailMessage MailMessage)
        {
            HtmlToPdf converter = new HtmlToPdf();

            UriBuilder UriBuilder = new UriBuilder(
                HttpContext.Current.Request.Url.Scheme,
                HttpContext.Current.Request.Url.Host,
                HttpContext.Current.Request.Url.Port
                );

            return converter.ConvertHtmlString(MailMessage.Body, UriBuilder.Uri.AbsoluteUri);
        }

        public static void AttachFiles(this MailMessage MailMessage, string Path, List<string> Filenames)
        {
            foreach (string file in Filenames)
                MailMessage.Attachments.Add(new Attachment(Path + file));
        }

        public static void AttachPdfDocument(this MailMessage MailMessage, PdfDocument PdfDocument, string FileName)
        {
            using (var MemoryStream = new MemoryStream())
            {
                PdfDocument.Save(MemoryStream);
                MemoryStream.Position = 0;
                ContentType ContentType = new ContentType(MediaTypeNames.Application.Pdf);

                MailMessage.AttachMemoryStream(MemoryStream, ContentType, FileName);
            }
        }

        public static void AttachMemoryStream(this MailMessage MailMessage, MemoryStream MemoryStream, ContentType ContentType, string FileName)
        {
            using (var Attachment = new Attachment(MemoryStream, ContentType))
            {
                Attachment.ContentDisposition.FileName = FileName;
                MailMessage.Attachments.Add(Attachment);
            }
        }

        public static void Send(this MailMessage MailMessage)
        {
            using (SmtpClient SmtpClient = new SmtpClient())
                SmtpClient.Send(MailMessage);
        }
    }
}
