namespace CareNest.UiTests;

public sealed class DataModelContractTests
{
    [Fact]
    public void Domain_ContainsEveryRequiredCareNestEntity()
    {
        var domainRoot = Path.Combine(RepositoryLocator.Root, "src", "CareNest.Domain");
        var source = string.Join(
            "\n",
            Directory.EnumerateFiles(domainRoot, "*.cs", SearchOption.AllDirectories)
                .Select(File.ReadAllText));

        foreach (var typeName in new[]
                 {
                     "PersonProfile",
                     "Medicine",
                     "MedicineSchedule",
                     "ScheduleTime",
                     "ReminderOccurrence",
                     "MedicationLogEntry",
                     "Appointment",
                     "CareDocument",
                     "Tag",
                     "DocumentTag",
                     "StockAdjustment",
                     "EmergencyContact",
                     "AppSetting",
                     "BackupMetadata",
                     "AuditEntry"
                 })
        {
            Assert.Contains($"class {typeName}", source, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Medicine_PreservesStrengthAndInstructionsAsOpaqueText()
    {
        var source = RepositoryLocator.Read("src", "CareNest.Domain", "Entities", "Medicine.cs");

        Assert.Contains("string? StrengthText", source, StringComparison.Ordinal);
        Assert.Contains("string? InstructionText", source, StringComparison.Ordinal);
        Assert.DoesNotContain("CalculatedDose", source, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("RecommendedDose", source, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("DosePerKg", source, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Medicine_StockChangeIsExplicitUserStoredValue()
    {
        var source = RepositoryLocator.Read("src", "CareNest.Domain", "Entities", "Medicine.cs");

        Assert.Contains("decimal? StockChangePerTakenEvent", source, StringComparison.Ordinal);
        Assert.DoesNotContain("StrengthText *", source, StringComparison.Ordinal);
        Assert.DoesNotContain("InstructionText *", source, StringComparison.Ordinal);
    }

    [Fact]
    public void RequiredScheduleAndReminderEnums_ArePresent()
    {
        var enumRoot = Path.Combine(RepositoryLocator.Root, "src", "CareNest.Domain", "Enums");
        var source = string.Join(
            "\n",
            Directory.EnumerateFiles(enumRoot, "*.cs", SearchOption.AllDirectories)
                .Select(File.ReadAllText));

        foreach (var token in new[]
                 {
                     "ScheduleKind",
                     "AsNeeded",
                     "Scheduled",
                     "Snoozed",
                     "Taken",
                     "Skipped",
                     "Delayed",
                     "Missed"
                 })
        {
            Assert.Contains(token, source, StringComparison.Ordinal);
        }
    }
}
