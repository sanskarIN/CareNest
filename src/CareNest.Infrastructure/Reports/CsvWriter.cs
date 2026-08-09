using System.Text;

namespace CareNest.Infrastructure.Reports;

internal static class CsvWriter
{
    public static string Escape(object? value)
    {
        var text = value?.ToString() ?? string.Empty;
        if (text.Contains('"'))
        {
            text = text.Replace("\"", "\"\"", StringComparison.Ordinal);
        }

        return text.IndexOfAny([',', '"', '\r', '\n']) >= 0
            ? $"\"{text}\""
            : text;
    }

    public static async Task WriteAsync(
        string path,
        IEnumerable<IReadOnlyList<object?>> rows,
        CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var stream = File.Create(path);
        await using var writer = new StreamWriter(
            stream,
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

        foreach (var row in rows)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await writer.WriteLineAsync(
                string.Join(',', row.Select(Escape)).AsMemory(),
                cancellationToken);
        }
    }
}
