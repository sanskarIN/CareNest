using CareNest.Application.Contracts;

namespace CareNest.App.Services;

public sealed class MauiFileGateway : IAppFileGateway
{
    public async Task<PickedFile?> PickDocumentAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Choose a health document"
        });

        if (result is null)
        {
            return null;
        }

        return new PickedFile(
            result.FileName,
            result.ContentType,
            async ct =>
            {
                ct.ThrowIfCancellationRequested();
                return await result.OpenReadAsync();
            });
    }

    public async Task<PickedFile?> CapturePhotoAsync(
        CancellationToken cancellationToken = default)
    {
        if (!MediaPicker.Default.IsCaptureSupported)
        {
            return null;
        }

        var result = await MediaPicker.Default.CapturePhotoAsync();
        if (result is null)
        {
            return null;
        }

        return new PickedFile(
            result.FileName,
            result.ContentType,
            async ct =>
            {
                ct.ThrowIfCancellationRequested();
                return await result.OpenReadAsync();
            });
    }

    public async Task<PickedFile?> PickBackupForRestoreAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Choose a CareNest encrypted backup"
        });

        if (result is null)
        {
            return null;
        }

        return new PickedFile(
            result.FileName,
            result.ContentType,
            async ct =>
            {
                ct.ThrowIfCancellationRequested();
                return await result.OpenReadAsync();
            });
    }

    public Task ShareFileAsync(
        string filePath,
        string title,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = title,
            File = new ShareFile(filePath)
        });
    }

    public Task ShareTextAsync(
        string text,
        string title,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Share.Default.RequestAsync(new ShareTextRequest
        {
            Title = title,
            Text = text
        });
    }
}
