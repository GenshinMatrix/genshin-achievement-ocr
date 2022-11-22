using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace GenshinAchievementOcr.Core;

internal static class BitmapExtension
{
    public static Bitmap ScaleToSize(this Bitmap bitmap, int width, int height)
    {
        if (bitmap.Width == width && bitmap.Height == height)
        {
            return bitmap;
        }

        Bitmap scaledBitmap = new(width, height);
        using Graphics g = Graphics.FromImage(scaledBitmap);
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.DrawImage(bitmap, 0, 0, width, height);
        return scaledBitmap;
    }

    public static byte[] ToByteArray(this Stream stream)
    {
        if (stream.CanSeek)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        else
        {
            const int len = 32768;
            byte[] buffer = new byte[len];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }

            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }
    }

    public async static Task<IRandomAccessStream> ToRandomAccessStream(this Stream stream)
    {
        byte[] bytes = ToByteArray(stream);
        IRandomAccessStream randomStream = new InMemoryRandomAccessStream();
        using DataWriter dataWriter = new(randomStream.GetOutputStreamAt(0));
        dataWriter.WriteBytes(bytes);
        await dataWriter.StoreAsync();

        return randomStream;
    }
}
