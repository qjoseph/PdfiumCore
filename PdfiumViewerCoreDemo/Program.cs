using System.Linq;

using PdfiumCore;


PdfPrinter pdfPrinter = new();

await pdfPrinter.PrintAsync(x => x.First(driverName => driverName.Contains("HP ENVY")), new("test.pdf"));
