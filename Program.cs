
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using pdftron;
using pdftron.Common;
using pdftron.Filters;
using pdftron.SDF;
using pdftron.PDF;
using pdftron.PDF.PDFUA;

using Microsoft.Extensions.Configuration;
using System.IO;

namespace OfficeTemplateTestCS
{
    class Program
    {

        private static pdftron.PDFNetLoader pdfNetLoader = pdftron.PDFNetLoader.Instance();
        static Program() { }
        static string currentDir = Directory.GetCurrentDirectory();
        static String input_path = currentDir + "/TestFiles/";
        static String output_path = input_path + "/Output/";
        static string json = @"{
    ""Date"": ""01.01.25"",
    ""Claim"": ""#1234567"",
    ""Datedamage"": ""01.01.24"",
    ""InsuredCar"": ""123-45-678"",
    ""ProsecutorName"": ""אנה"",
    ""ProsecutorStreet"": ""רחוב חדש"",
    ""ProsecutorCity"": ""תל אביב""
    }";
        static String input_filename = "6076.doc";
        static String output_filename = "6076_filled.pdf";
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


            // Go up 3 levels to reach your project root
            string projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\.."));

            string licenseKey = config["PdfTron:LicenseKey"]!;
            string basePath = currentDir + "/bin/Debug/net9.0";

            PDFNet.AddResourceSearchPath(basePath);

            PDFNet.AddResourceSearchPath(Path.Combine(basePath, "tessdata")); PDFNet.Initialize(licenseKey);
            try
            {
                using (TemplateDocument template_doc = pdftron.PDF.Convert.CreateOfficeTemplate(input_path + input_filename, null))
                {
                    PDFDoc pdfdoc = template_doc.FillTemplateJson(json);

                    pdfdoc.Save(output_path + output_filename, SDFDoc.SaveOptions.e_linearized);
                    PDFUAConformance pdf_ua = new PDFUAConformance();

                    Console.WriteLine("Converting With Options...");
                    {
                        PDFUAOptions pdf_ua_opts = new PDFUAOptions();
                        pdf_ua_opts.SetSaveLinearized(true);
                        pdf_ua.AutoConvert(output_path + output_filename, output_path + output_filename, pdf_ua_opts);
                    }
                    Console.WriteLine("Saved " + output_filename);
                }
            }
            catch (pdftron.Common.PDFNetException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unrecognized Exception: " + e.Message);
            }

            PDFNet.Terminate();
            Console.WriteLine("Done.");
        }
    }
}