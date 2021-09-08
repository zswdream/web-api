using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SwitDish.Vendor.API.Tests.Utilities
{
    public class FileSystem
    {
        public static string GetFilePath(string filename)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path) + filename;
        }
    }
}
