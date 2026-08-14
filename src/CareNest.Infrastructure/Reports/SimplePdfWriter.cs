using System.Globalization;
using System.Text;

namespace CareNest.Infrastructure.Reports;

internal static class SimplePdfWriter
{
    public static async Task WriteTextReportAsync(
        string path,
        string title,
        IEnumerable<string> lines,
        CancellationToken cancellationToken)
    {
        var allLines = new List<string> { title, string.Empty };
        allLines.AddRange(lines);

        var pages = allLines.Chunk(48).ToArray();
        if (pages.Length == 0)
        {
            pages = [Array.Empty<string>()];
        }

        var objects = new List<byte[]>();
        var pageObjectNumbers = new List<int>();

        objects.Add(Ascii("<< /Type /Catalog /Pages 2 0 R >>"));
        objects.Add([]);
        objects.Add(Ascii("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>"));

        foreach (var pageLines in pages)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var pageObjectNumber = objects.Count + 1;
            var contentObjectNumber = objects.Count + 2;
            pageObjectNumbers.Add(pageObjectNumber);

            var content = BuildContent(pageLines);
            var contentBytes = Encoding.ASCII.GetBytes(content);

            objects.Add(Ascii(
                $"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] " +
                $"/Resources << /Font << /F1 3 0 R >> >> /Contents {contentObjectNumber} 0 R >>"));

            objects.Add(Concat(
                Ascii($"<< /Length {contentBytes.Length} >>\nstream\n"),
                contentBytes,
                Ascii("\nendstream")));
        }

        objects[1] = Ascii(
            $"<< /Type /Pages /Kids [{string.Join(' ', pageObjectNumbers.Select(x => $"{x} 0 R"))}] " +
            $"/Count {pageObjectNumbers.Count} >>");

        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var partialPath = path + $".{Guid.NewGuid():N}.partial";
        try
        {
            await using (var stream = File.Create(partialPath))
            {
                await stream.WriteAsync(Ascii("%PDF-1.4\n"), cancellationToken);

                var offsets = new List<long> { 0 };

                for (var i = 0; i < objects.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    offsets.Add(stream.Position);
                    await stream.WriteAsync(Ascii($"{i + 1} 0 obj\n"), cancellationToken);
                    await stream.WriteAsync(objects[i], cancellationToken);
                    await stream.WriteAsync(Ascii("\nendobj\n"), cancellationToken);
                }

                var xref = stream.Position;
                await stream.WriteAsync(
                    Ascii($"xref\n0 {objects.Count + 1}\n0000000000 65535 f \n"),
                    cancellationToken);

                for (var i = 1; i < offsets.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await stream.WriteAsync(
                        Ascii(
                            $"{offsets[i].ToString("D10", CultureInfo.InvariantCulture)} 00000 n \n"),
                        cancellationToken);
                }

                await stream.WriteAsync(
                    Ascii(
                        $"trailer\n<< /Size {objects.Count + 1} /Root 1 0 R >>\n" +
                        $"startxref\n{xref}\n%%EOF"),
                    cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();
            File.Move(partialPath, path, overwrite: true);
        }
        finally
        {
            TryDeleteFile(partialPath);
        }
    }

    private static string BuildContent(IEnumerable<string> lines)
    {
        var builder = new StringBuilder();
        builder.AppendLine("BT");
        builder.AppendLine("/F1 11 Tf");
        builder.AppendLine("50 790 Td");

        var first = true;
        foreach (var line in lines)
        {
            if (!first)
            {
                builder.AppendLine("0 -15 Td");
            }
            first = false;

            builder
                .Append('(')
                .Append(ToPdfAscii(line))
                .AppendLine(") Tj");
        }

        builder.AppendLine("ET");
        return builder.ToString();
    }

    private static string ToPdfAscii(string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var ascii = new StringBuilder();

        foreach (var c in normalized)
        {
            if (c <= 127 && c is not '\r' and not '\n')
            {
                if (c is '(' or ')' or '\\')
                {
                    ascii.Append('\\');
                }
                ascii.Append(c);
            }
            else if (char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                ascii.Append('?');
            }
        }

        return ascii.ToString();
    }

    private static byte[] Ascii(string value) =>
        Encoding.ASCII.GetBytes(value);

    private static byte[] Concat(params byte[][] arrays)
    {
        var result = new byte[arrays.Sum(x => x.Length)];
        var offset = 0;

        foreach (var array in arrays)
        {
            Buffer.BlockCopy(array, 0, result, offset, array.Length);
            offset += array.Length;
        }

        return result;
    }

    private static void TryDeleteFile(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch
        {
            // Best-effort cleanup of an incomplete plaintext export.
        }
    }
}
