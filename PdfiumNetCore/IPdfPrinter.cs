using System.Drawing.Printing;

namespace PdfiumCore
{
    public interface IPdfPrinter
    {
        Task PrintAsync(string printDriverName, FileInfo pdf, Action<PrinterSettings>? printerSettingsConfigure = default, Action<PageSettings>? pageSettingsConfigure = default);
        Task PrintAsync(Func<List<string>, string> printerDriverNameSelector, FileInfo pdf, Action<PrinterSettings>? printerSettingsConfigure = default, Action<PageSettings>? pageSettingsConfigure = default);
    }
}