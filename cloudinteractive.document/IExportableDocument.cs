using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudinteractive.document
{
    public interface IExportableDocument
    {
        public byte[][]? Images { get; }
    }
}
