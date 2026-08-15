namespace CareNest.UiTests;

internal static class RepositoryLocator
{
    public static string Root
    {
        get
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory is not null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "Directory.Build.props")) &&
                    Directory.Exists(Path.Combine(directory.FullName, "src", "CareNest.App")))
                {
                    return directory.FullName;
                }
                directory = directory.Parent;
            }
            throw new DirectoryNotFoundException("CareNest repository root was not found from the test output directory.");
        }
    }

    public static string PathOf(params string[] segments) =>
        Path.Combine(new[] { Root }.Concat(segments).ToArray());

    public static string Read(params string[] segments) =>
        File.ReadAllText(PathOf(segments));
}
