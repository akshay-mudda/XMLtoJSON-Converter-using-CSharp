using System;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using System.Xml;

namespace XMLtoJSON_Converter_using_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read source and destination paths from app.config
            string sourcePath = ConfigurationManager.AppSettings["SourcePath"];
            string destinationPath = ConfigurationManager.AppSettings["DestinationPath"];

            // Check if destination folder exists, create if not
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // Get all XML files in the source folder
            string[] xmlFiles = Directory.GetFiles(sourcePath, "*.xml");

            if (xmlFiles.Length == 0)
            {
                Console.WriteLine("No XML files found in the source folder.");
                return;
            }

            foreach (var xmlFilePath in xmlFiles)
            {
                try
                {
                    // Get file name without extension
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(xmlFilePath);
                    string jsonFilePath = Path.Combine(destinationPath, fileNameWithoutExtension + ".json");

                    // Check if the JSON already exists
                    if (File.Exists(jsonFilePath))
                    {
                        Console.WriteLine($"JSON already exists for {fileNameWithoutExtension}. Skipping file.");
                        continue;
                    }

                    // Convert XML file to JSON
                    ConvertXmlToJson(xmlFilePath, jsonFilePath);

                    // After conversion, delete the source XML file
                    File.Delete(xmlFilePath);

                    Console.WriteLine($"Successfully converted {fileNameWithoutExtension}.xml to JSON.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting {xmlFilePath}: {ex.Message}");
                }
            }
        }

        // Convert XML file to JSON using Newtonsoft.Json
        static void ConvertXmlToJson(string xmlFilePath, string jsonFilePath)
        {
            // Load the XML document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);

            // Convert XML to JSON with formatting (specifying the namespace)
            string jsonText = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);

            // Write JSON to the destination file
            File.WriteAllText(jsonFilePath, jsonText);
        }
    }
}