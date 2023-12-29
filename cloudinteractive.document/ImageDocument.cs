using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace cloudinteractive.document
{
    public class ImageDocument : IExportableDocument
    {
        private ImageDocument(){}

        public byte[][]? Images { get; private set; }

        public static async Task<ImageDocument> ImportFromFile(string filename)
        {
            var file = File.ReadAllBytes(filename);

            return new ImageDocument()
            {
                Images = new byte[][] { file }
            };
        }

        public static async Task<ImageDocument> ImportFromStream(MemoryStream stream)
        {
            return new ImageDocument()
            {
                Images = new byte[][] { stream.ToArray() }
            };
        }
    }
}
