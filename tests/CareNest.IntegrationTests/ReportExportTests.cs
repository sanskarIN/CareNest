using System.Text.Json;
using CareNest.Domain.Entities;
using CareNest.Infrastructure.Reports;

namespace CareNest.IntegrationTests;

public sealed class ReportExportTests
{
    [Fact]
    public async Task ProfileDataExport_ContainsStructuredDataAndSafetyDisclaimer()
    {
        await using var store = await TestStore.CreateAsync();
        var profile = new PersonProfile { Name = "Export profile", IsPrimary = true };
        await store.Repository.SaveProfileAsync(profile);
        await store.Repository.SaveEmergencyContactAsync(new EmergencyContact
        {
            ProfileId = profile.Id,
            Name = "Contact"
        });
        await store.Repository.SaveMedicineAsync(new Medicine
        {
            ProfileId = profile.Id,
            Name = "User-entered medicine",
            Form = "Custom",
            StartDate = DateTime.Today
        });

        var path = Path.Combine(store.Root, "profile.json");
        var reports = new ReportService(store.Repository);
        await reports.CreateProfileDataJsonAsync(profile.Id, path);

        await using var stream = File.OpenRead(path);
        using var document = await JsonDocument.ParseAsync(stream);
        var root = document.RootElement;

        Assert.Equal("CareNest profile data export", root.GetProperty("Format").GetString());
        Assert.Contains("does not diagnose", root.GetProperty("Disclaimer").GetString(), StringComparison.OrdinalIgnoreCase);
        Assert.Equal(profile.Id, root.GetProperty("Profile").GetProperty("Id").GetString());
        Assert.Single(root.GetProperty("EmergencyContacts").EnumerateArray());
        Assert.Single(root.GetProperty("Medicines").EnumerateArray());
    }

    [Fact]
    public async Task MedicationLogCsv_LabelsDataAsUnverified()
    {
        await using var store = await TestStore.CreateAsync();
        var path = Path.Combine(store.Root, "log.csv");
        var reports = new ReportService(store.Repository);

        await reports.CreateMedicationLogCsvAsync(null, path);

        var text = await File.ReadAllTextAsync(path);
        Assert.Contains("user-entered/unverified", text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task StockCsv_NeutralizesFormulaLikeUserEnteredText()
    {
        await using var store = await TestStore.CreateAsync();
        var profile = new PersonProfile
        {
            Name = "=2+2",
            IsPrimary = true
        };
        await store.Repository.SaveProfileAsync(profile);
        await store.Repository.SaveMedicineAsync(new Medicine
        {
            ProfileId = profile.Id,
            Name = " @HYPERLINK(\"https://example.invalid\")",
            Form = "Custom",
            StartDate = DateTime.Today
        });

        var path = Path.Combine(store.Root, "stock.csv");
        var reports = new ReportService(store.Repository);
        await reports.CreateStockRefillCsvAsync(profile.Id, path);

        var text = await File.ReadAllTextAsync(path);
        Assert.Contains("'=2+2", text, StringComparison.Ordinal);
        Assert.Contains("' @HYPERLINK", text, StringComparison.Ordinal);
        Assert.DoesNotContain("\n=2+2,", text, StringComparison.Ordinal);
        Assert.DoesNotContain("\n @HYPERLINK", text, StringComparison.Ordinal);
    }
}
