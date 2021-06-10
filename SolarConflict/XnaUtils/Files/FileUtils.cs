using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SolarConflict.XnaUtils.Files
{
    public static class FileUtils
    {       
        // fileTypes="*.png|*.jpg"
        public static string[] GetFiles(string path, string fileTypes, SearchOption searchOption = SearchOption.TopDirectoryOnly) //TODO:
        {
            // ArrayList will hold all file names
            ArrayList alFiles = new ArrayList();

            // Create an array of filter string
            string[] MultipleFilters = fileTypes.Split('|');

            // for each filter find mathing file names
            foreach (string FileFilter in MultipleFilters) 
            {
                // add found file names to array list
                alFiles.AddRange(Directory.GetFiles(path, FileFilter, searchOption));
            }

            // returns string array of relevant file names
            return (string[])alFiles.ToArray(typeof(string));
        }



        //public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo dir, params string[] extensions)
        //{
        //    if (extensions == null)
        //        throw new ArgumentNullException("extensions");
        //    IEnumerable<FileInfo> files = dir.EnumerateFiles();
        //    return files.Where(f => extensions.Contains(f.Extension));
        //}





    }
}



//Product product = new Product();
//product.ExpiryDate = new DateTime(2008, 12, 28);

//JsonSerializer serializer = new JsonSerializer();
//serializer.Converters.Add(new JavaScriptDateTimeConverter());
//serializer.NullValueHandling = NullValueHandling.Ignore;

//using (StreamWriter sw = new StreamWriter(@"c:\json.txt"))
//using (JsonWriter writer = new JsonTextWriter(sw))
//{
//    serializer.Serialize(writer, product);
//    // {"ExpiryDate":new Date(1230375600000),"Price":0}
//}