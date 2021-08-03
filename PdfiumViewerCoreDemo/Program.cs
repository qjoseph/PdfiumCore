using System;
using System.Drawing.Printing;
using System.IO;

namespace PdfiumCoreCoreDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            byte[] pdfBytes = File.ReadAllBytes("test.pdf");

            using (Stream s = new MemoryStream(pdfBytes))
            {
                var pdfDoc = PdfiumCore.PdfDocument.Load(s);

                #region PRINT TEST

                try
                {
                    string printDriverName = "";

                    for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                    {
                        string currPrinterDriver = PrinterSettings.InstalledPrinters[i];
                        if (currPrinterDriver.ToLower().Contains("O198_Toshiba".ToLower()))
                        {
                            printDriverName = currPrinterDriver;
                            break;
                        }
                    }

                    // Printer settings
                    var printerSettings = new PrinterSettings
                    {
                        PrinterName = printDriverName,
                        Copies = 1,
                        Duplex = Duplex.Simplex // Single sided printing
                    };

                    if (printerSettings.IsValid == false)
                    {
                        throw new Exception("Printer name is not valid");
                    }

                    PaperSize ps = null;
                    for (int i = 0; i < printerSettings.PaperSizes.Count; i++)
                    {
                        var currPaperSize = printerSettings.PaperSizes[i];
                        if (currPaperSize.PaperName.ToLower().Contains("a4"))
                        {
                            ps = currPaperSize;
                            break;
                        }
                    }

                    // Page settings
                    var pageSettings = new PageSettings(printerSettings)
                    {
                        Margins = new Margins(0, 0, 0, 0),
                        Landscape = false,
                        Color = false // print in b&w
                    };

                    if (ps != null)
                    {
                        pageSettings.PaperSize = ps;
                    }

                    // Print
                    using (pdfDoc)
                    {
                        using (var printDocument = pdfDoc.CreatePrintDocument())
                        {
                            printDocument.PrinterSettings = printerSettings;
                            printDocument.DefaultPageSettings = pageSettings;
                            printDocument.PrintController = new StandardPrintController();
                            printDocument.Print();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                #endregion PRINT TEST

                Console.ReadLine();
            }
        }
    }
}