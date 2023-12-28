using System.Runtime.Versioning;
using System.Text;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using cloudinteractive.document.util;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Filters;
using System.IO;
using cloudinteractive.document.Util;

namespace cloudinteractive.document
{
    public class PDFDocument : IExportableDocument, IDisposable
    {
        internal PdfDocument Document;
        private bool disposedValue;
        private bool _useImage;

        public PDFDocument(bool useImage = true)
        {
            _useImage = useImage;
        }

        public byte[][]? ExportImages(PdfPage page)
        {
            var resources = page.Elements.GetDictionary("/Resources");
            var xObjects = resources?.Elements.GetDictionary("/XObject");
            if (xObjects is null) return null;

            List<byte[]> images = new List<byte[]>();
            var items = xObjects.Elements.Values;
            foreach (var item in items)
            {
                var reference = item as PdfReference;
                if (reference is null) continue;

                var xObject = reference.Value as PdfDictionary;
                if (xObject != null && xObject.Elements.GetString("/Subtype") == "/Image")
                {
                    var image = ExportImage(xObject);
                    if(image != null) images.Add(image);
                }
            }

            return images.ToArray();
        }

        public byte[]? ExportImage(PdfDictionary image)
        {
            var filter = image.Elements.GetValue("/Filter");
            var array = filter as PdfArray;
            if (array != null)
            {
                // PDF files sometimes contain "zipped" JPEG images.
                if (array.Elements.GetName(0) == "/FlateDecode" &&
                    array.Elements.GetName(1) == "/DCTDecode")
                {
                    return _exportJpegImage(image, true);
                }

            }

            var name = filter as PdfName;
            if (name != null)
            {
                var decoder = name.Value;
                if (decoder is "/DCTDecode") return _exportJpegImage(image, false);
                else return null;
            }

            return null;
        }

        private byte[]? _exportJpegImage(PdfDictionary image, bool flateDecode)
        {
            return flateDecode ? Filtering.Decode(image.Stream.Value, "/FlateDecode") : image.Stream.Value;
        }

        public string[] Export()
        {
            using (var extractor = new PDFTextExtractor(Document))
            {
                foreach (PdfPage page in Document.Pages)
                {
                    var bulider = new StringBuilder();
                    if (_useImage)
                    {
                        var images = ExportImages(page);
                        if (images != null)
                        {
                            int count = 0;
                            foreach (var image in images)
                            {
                                var fs = new FileStream(String.Format("Image{0}.jpeg", count++), FileMode.Create, FileAccess.Write);
                                var bw = new BinaryWriter(fs);
                                bw.Write(image);
                                bw.Close();
                                Console.WriteLine(AzureComputerVision.GetTextFromImage(image).Result);
                            }
                        }
                    }

                    //extractor.ExtractText(page, bulider);
                    //Console.WriteLine(bulider.ToString());
                }
            }
            
            return new string[5];
        }

        public static PDFDocument ImportFromFile(string filename, bool useImage=true)
        {
            var doc = new PDFDocument(useImage = useImage)
            {
                Document = PdfReader.Open(filename)
            };
            return doc;
        }

        public static PDFDocument ImportFromFile(string filename, int[] pages, bool useImage = true)
        {
            var file = PdfReader.Open(filename, PdfDocumentOpenMode.Import);
            var doc = new PDFDocument(useImage = useImage)
            {
                Document = new PdfDocument()
            };

            foreach (int i in pages)
            {
                doc.Document.AddPage(file.Pages[i]);   
            }

            file.Dispose();
            return doc;
        }

        public static PDFDocument ImportFromStream(Stream stream, bool useImage = true)
        {
            var doc = new PDFDocument(useImage = useImage)
            {
                Document = PdfReader.Open(stream)
            };
            return doc;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                Document.Dispose();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}