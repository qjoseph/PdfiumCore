# PdfiumCore
Google's Pdfium .NET wrapper ported to .net 5 and .net core 3.1. Removed the Windows.Forms dependency.


The goals of this project are:

- Cleanup\reorganize project code
- Remove System.Windows.Forms dependency
- Compile in .net 5 \ .net core 3.1
- Fix bugs in the original implementation (like AccessViolationException, etc)

The scope of this project is to consume this lib to print pdf through SpoolName (not to EDIT pdfs!)


NEXT STEPS:
- Build the latest pdfium c++ releases x86 and x64
- Create a minimal wrapper specific for the spoolname print calling only the necessary pdfium c++ methods
- Rename the project in "PdfiumCorePrint"
