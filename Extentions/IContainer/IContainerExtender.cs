using CB.CSharp.Interfaces;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace CB.CSharp.Extentions
{
    public static class IContainerExtender
    {
        public static string GetPath(this IContainer container)
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(container.Path)))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(container.Path));
            return HttpContext.Current.Server.MapPath(container.Path);
        }

        public static void RemoveContainer(this IContainer container) =>
            Directory.Delete(container.GetPath(), true);

        public static IEnumerable<string> GetFiles(this IContainer container, string filter = "*.*", SearchOption searchOption = SearchOption.AllDirectories) =>
            Directory.GetFiles(container.GetPath(), filter, searchOption);

        public static void Delete(this IContainer container, string file)
        {
            if (container.Exists(file))
            {
                Guid uid = Guid.NewGuid();
                container.Move(file, file + uid);
                File.Delete(container.GetPath() + file + uid);
            }
        }

        public static bool Exists(this IContainer container, string file) =>
            File.Exists(container.GetPath() + file);

        public static void Copy(this IContainer container, string sourceFileName, string destFileName, bool overwrite = true)
        {
            if (container.Exists(sourceFileName))
                File.Copy(container.GetPath() + sourceFileName, container.GetPath() + destFileName, overwrite);
        }

        public static void SaveFile(this IContainer container, System.Web.UI.WebControls.FileUpload sender, string filename) =>
            sender.SaveAs(container.GetPath() + filename);

        public static void SaveFile(this IContainer container, Image image, string filename, ImageFormat imageFormat) =>
            image.Save(container.GetPath() + filename, imageFormat);

        public static void SaveFile(this IContainer container, Image image, string filename) =>
            container.SaveFile(image, filename, ImageFormat.Png);

        public static void SwitchFiles(this IContainer container, string filename1, string filename2)
        {
            Guid uid = Guid.NewGuid();
            container.Move(filename1, filename1 + uid);
            container.Move(filename2, filename1);
            container.Move(filename1 + uid, filename2);
        }

        public static void Move(this IContainer container, string from, string to)
        {
            if (container.Exists(from))
                File.Move(container.GetPath() + from, container.GetPath() + to);
        }

        public static long FileSize(this IContainer container, string filename) =>
            new FileInfo(container.GetPath() + filename).Length;

        public static ZipFile getZip(this IContainer container)
        {
            ZipFile zip = new ZipFile();
            zip.AddDirectory(container.GetPath());
            return zip;
        }

        public static ZipFile getZip(this IContainer container, IEnumerable<string> files)
        {
            ZipFile zip = new ZipFile();

            foreach (string file in files)
                zip.AddFile(container.GetPath() + file, string.Empty);

            return zip;
        }
    }
}