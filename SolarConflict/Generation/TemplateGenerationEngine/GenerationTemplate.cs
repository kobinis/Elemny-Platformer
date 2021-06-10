using Microsoft.Xna.Framework;
using SolarConflict.Framework.Utils;
using SolarConflict.XnaUtils.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.GameContent.ContentGeneration
{
    public abstract class GenerationTemplate
    {
        protected readonly string ID = "Id";

        protected string _directoryName;
        public string DirectoryNane { get { return _directoryName; } }
        protected CsvUtils csvUtils;

        protected IList<string> _parametersName;

        public GenerationTemplate()
        {
            _parametersName = new List<string>();
        }

        protected abstract void ParseAndAddEmitter(string[] parameters);

        public void LoadDirectory(string parentPath)
        {
            string path = Path.Combine(parentPath, _directoryName);

            csvUtils = new CsvUtils(_parametersName.ToArray());
            if (Directory.Exists(path))
            {
                string[] fileNames = FileUtils.GetFiles(path, "*.csv");
                int i = 0;
              
                    for (i = 0; i < fileNames.Length; i++)
                    {
                        ReadFile(fileNames[i]);
                    }               
            }
            else
            {
                Directory.CreateDirectory(path);
                string filename = this.GetType().Name + ".csv"; //REFACTOR: maybe add name member    
                CreateTemplateFile(Path.Combine(path, filename));
            }

        }

        protected void CreateTemplateFile(string fileName)
        {
            string line = string.Empty;

            foreach (var item in _parametersName)
            {
                line = line + item + ",";
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                file.WriteLine(line);
            }
        }


        private void ReadFile(string fileName)
        {

            using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                StreamReader reader = new StreamReader(stream);
                string line = reader.ReadLine();
                csvUtils = new CsvUtils(line.Split(','));
                while ((line = reader.ReadLine()) != null)
                {
                    
                        string[] splitLine = csvUtils.ReadLine(line);
                        ParseAndAddEmitter(splitLine);
                    try
                    {
                    }
                    catch (Exception e)
                    {                        
                        ActivityManager.ErrorLog(e.ToString());
                        ActivityManager.Inst.AddToast($"Error at {fileName} in {line}", 400, Color.Red);
                    }
                    
                }
                Finialzie(fileName);
            }
        }

        public virtual void Finialzie(string fileName)
        {
        }


        protected void AddParametereName(string parameterName)
        {
            _parametersName.Add(parameterName);
        }

        protected void RemoveParametereName(string parameterName)
        {
            _parametersName.Remove(parameterName);
        }
    }
}
