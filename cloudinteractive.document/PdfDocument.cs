using System.Drawing.Imaging;
using Ghostscript.NET.Rasterizer;

namespace cloudinteractive.document
{
    //NOTE: System.Drawing is only works in Windows.
    public class PdfDocument : IExportableDocument
    {
        private PdfDocument(){}
        public byte[][]? Images { get; private set; }

        public static async Task<PdfDocument> ImportFromFile(string filename, int[]? pages = null, int dpi = 150)
        {
            await using var file = File.OpenRead(filename);
            return await ImportFromStream(file, pages, dpi);
        }

        public static async Task<PdfDocument> ImportFromStream(Stream file, int[]? pages = null, int dpi = 150)
        {
            using var rasterizer = new GhostscriptRasterizer();
            rasterizer.Open(file);

            if (pages == null) pages = Enumerable.Range(1, rasterizer.PageCount).ToArray();
            var images = new List<byte[]>();

            foreach (int i in pages)
            {
                using var image = rasterizer.GetPage(dpi, i);
                using var stream = new MemoryStream();
                image.Save(stream, ImageFormat.Png);
                images.Add(stream.ToArray());
            }

            file.Close();
            return new PdfDocument()
            {
                Images = images.ToArray()
            };
        }
    }
}