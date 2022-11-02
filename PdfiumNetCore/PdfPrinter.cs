using System.Drawing.Printing;

namespace PdfiumCore
{
    public class PdfPrinter : IPdfPrinter
    {
        public async Task PrintAsync(Func<List<string>, string> printerDriverNameSelector, FileInfo pdf, Action<PrinterSettings>? printerSettingsConfigure = default, Action<PageSettings>? pageSettingsConfigure = default)
        {
            List<string> printerDriversName = new();

            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                string currPrinterDriver = PrinterSettings.InstalledPrinters[i];

                printerDriversName.Add(currPrinterDriver);
            }

            await PrintAsync(printerDriverNameSelector.Invoke(printerDriversName), pdf, printerSettingsConfigure, pageSettingsConfigure);
        }

        public async Task PrintAsync(string printDriverName, FileInfo pdf, Action<PrinterSettings>? printerSettingsConfigure = default, Action<PageSettings>? pageSettingsConfigure = default)
        {
            byte[] pdfBytes = File.ReadAllBytes(pdf.FullName);

            using Stream s = new MemoryStream(pdfBytes);

            var pdfDoc = PdfiumCore.PdfDocument.Load(s);

            try
            {
                // Printer settings
                var printerSettings = new PrinterSettings
                {
                    PrinterName = printDriverName,
                    Copies = 1,
                    Duplex = Duplex.Simplex // Single sided printing
                };

                printerSettingsConfigure?.Invoke(printerSettings);

                if (!printerSettings.IsValid)
                    throw new Exception("Printer name is not valid");

                PaperSize? paperSize = null;

                for (int i = 0; i < printerSettings.PaperSizes.Count; i++)
                {
                    var currPaperSize = printerSettings.PaperSizes[i];

                    if (currPaperSize.PaperName.ToLower().Contains("a4"))
                    {
                        paperSize = currPaperSize;
                        break;
                    }
                }

                if (paperSize is null)
                    throw new InvalidPrinterException(printerSettings);

                // Page settings
                var pageSettings = new PageSettings(printerSettings)
                {
                    Margins = new Margins(0, 0, 0, 0),
                    Landscape = false,
                    Color = false, // print in b&w
                    PaperSize = paperSize
                };

                pageSettingsConfigure?.Invoke(pageSettings);

                // Print
                using var printDocument = pdfDoc.CreatePrintDocument();

                printDocument.PrinterSettings = printerSettings;
                printDocument.DefaultPageSettings = pageSettings;
                printDocument.PrintController = new StandardPrintController();
                printDocument.Print();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                pdfDoc.Dispose();
            }

            await Task.CompletedTask;
        }
    }
}
