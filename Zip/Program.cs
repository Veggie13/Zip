using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Zip
{
    class Program
    {
        static IEnumerable<byte> SubSeq(byte[] arr, int begin, int count)
        {
            for (int i = begin, j = 0; j < count; i++, j++)
                yield return arr[i];
        }

        static void Main(string[] args)
        {
            var stream = File.OpenRead(@"E:\Veggie\Downloads\Saved Installers\NI_FRC2014.zip");
            var header = new byte[30];
            var buffer = new byte[2 << 29];
            stream.Read(header, 0, header.Length);
            int offset = 0;
            while (0 != stream.Read(buffer, offset, buffer.Length - offset))
            {
                int index = Array.IndexOf<byte>(buffer, header[0]);
                while (index >= 0)
                {
                    if (index + header.Length > buffer.Length)
                    {
                        Array.Copy(buffer, index, buffer, 0, buffer.Length - index);
                        offset = buffer.Length - index;
                        break;
                    }
                    if (header.SequenceEqual<byte>(SubSeq(buffer, index, header.Length)))
                    {
                        break;
                    }

                    index = Array.IndexOf<byte>(buffer, header[0], index + 1);
                    offset = 0;
                }
            }
        }
    }
}
