using System.Xml.Linq;

namespace CareNest.UiTests;

public sealed class CompiledBindingContractTests
{
    private static readonly XNamespace Xaml =
        "http://schemas.microsoft.com/winfx/2009/xaml";

    [Fact]
    public void Every_binding_view_declares_a_real_root_data_type()
    {
        foreach (var path in BindingViewPaths())
        {
            var document = XDocument.Load(path, LoadOptions.PreserveWhitespace);
            var root = Assert.IsType<XElement>(document.Root);
            var dataType = root.Attribute(Xaml + "DataType")?.Value;

            Assert.False(
                string.IsNullOrWhiteSpace(dataType),
                $"{Path.GetFileName(path)} contains bindings but its root has no x:DataType.");
            AssertRealDataType(dataType!, path);
        }
    }

    [Fact]
    public void Every_binding_data_template_declares_its_item_data_type()
    {
        foreach (var path in BindingViewPaths())
        {
            var document = XDocument.Load(path, LoadOptions.PreserveWhitespace);
            var root = Assert.IsType<XElement>(document.Root);

            foreach (var template in root
                .Descendants()
                .Where(element => element.Name.LocalName == "DataTemplate")
                .Where(ContainsBinding))
            {
                var dataType = template.Attribute(Xaml + "DataType")?.Value;
                Assert.False(
                    string.IsNullOrWhiteSpace(dataType),
                    $"{Path.GetFileName(path)} has a binding DataTemplate without x:DataType.");
                AssertRealDataType(dataType!, path);
            }
        }
    }

    [Fact]
    public void Explicit_source_bindings_declare_their_source_data_type()
    {
        foreach (var path in BindingViewPaths())
        {
            var document = XDocument.Load(path, LoadOptions.PreserveWhitespace);
            var root = Assert.IsType<XElement>(document.Root);

            var sourceBindings = root
                .DescendantsAndSelf()
                .Attributes()
                .Where(IsBindingAttribute)
                .Where(attribute =>
                    attribute.Value.Contains("Source=", StringComparison.Ordinal));

            foreach (var binding in sourceBindings)
            {
                Assert.True(
                    binding.Value.Contains("x:DataType=", StringComparison.Ordinal),
                    $"{Path.GetFileName(path)} has an explicit Source binding without a binding-specific x:DataType: {binding.Value}");
            }
        }
    }

    [Fact]
    public void Picker_display_bindings_declare_their_item_data_type()
    {
        foreach (var path in BindingViewPaths())
        {
            var document = XDocument.Load(path, LoadOptions.PreserveWhitespace);
            var root = Assert.IsType<XElement>(document.Root);

            var itemDisplayBindings = root
                .DescendantsAndSelf()
                .Attributes()
                .Where(attribute => attribute.Name.LocalName == "ItemDisplayBinding")
                .Where(IsBindingAttribute);

            foreach (var binding in itemDisplayBindings)
            {
                Assert.True(
                    binding.Value.Contains("x:DataType=", StringComparison.Ordinal),
                    $"{Path.GetFileName(path)} has an ItemDisplayBinding without an item x:DataType: {binding.Value}");
            }
        }
    }

    [Fact]
    public void App_project_enables_and_enforces_compiled_binding_warnings()
    {
        var projectPath = RepositoryLocator.PathOf(
            "src",
            "CareNest.App",
            "CareNest.App.csproj");
        var project = XDocument.Load(projectPath);

        Assert.Equal(
            "true",
            ProjectProperty(project, "MauiEnableXamlCBindingWithSourceCompilation"));
        Assert.Equal(
            "true",
            ProjectProperty(project, "MauiStrictXamlCompilation"));

        var warningsAsErrors = ProjectProperty(project, "WarningsAsErrors") ?? string.Empty;
        foreach (var warning in new[] { "XC0022", "XC0023", "XC0024", "XC0025" })
        {
            Assert.Contains(warning, warningsAsErrors);
        }

        var noWarn = string.Join(
            ';',
            project.Descendants("NoWarn").Select(element => element.Value));
        Assert.DoesNotContain("XC0022", noWarn);
        Assert.DoesNotContain("XC0023", noWarn);
        Assert.DoesNotContain("XC0024", noWarn);
        Assert.DoesNotContain("XC0025", noWarn);
    }

    [Fact]
    public void Compiled_binding_views_do_not_disable_type_safety()
    {
        foreach (var path in BindingViewPaths())
        {
            var document = XDocument.Load(path, LoadOptions.PreserveWhitespace);
            var root = Assert.IsType<XElement>(document.Root);

            foreach (var attribute in root
                .DescendantsAndSelf()
                .Attributes(Xaml + "DataType"))
            {
                AssertRealDataType(attribute.Value, path);
            }
        }
    }

    private static IEnumerable<string> BindingViewPaths()
    {
        var viewsDirectory = RepositoryLocator.PathOf(
            "src",
            "CareNest.App",
            "Views");

        return Directory
            .EnumerateFiles(viewsDirectory, "*.xaml", SearchOption.TopDirectoryOnly)
            .Where(path =>
                File.ReadAllText(path).Contains("{Binding", StringComparison.Ordinal))
            .OrderBy(path => path, StringComparer.Ordinal);
    }

    private static bool ContainsBinding(XElement element) =>
        element
            .DescendantsAndSelf()
            .Attributes()
            .Any(IsBindingAttribute);

    private static bool IsBindingAttribute(XAttribute attribute) =>
        attribute.Value.Contains("{Binding", StringComparison.Ordinal);

    private static void AssertRealDataType(string dataType, string path)
    {
        Assert.False(
            dataType.Contains("x:Object", StringComparison.Ordinal),
            $"{Path.GetFileName(path)} disables compiled binding type safety with x:Object.");
        Assert.False(
            dataType.Contains("x:Null", StringComparison.Ordinal),
            $"{Path.GetFileName(path)} disables compiled binding type safety with x:Null.");
    }

    private static string? ProjectProperty(XDocument project, string name) =>
        project
            .Descendants(name)
            .Select(element => element.Value.Trim())
            .LastOrDefault();
}
