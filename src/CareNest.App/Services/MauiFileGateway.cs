using CareNest.Application.Contracts;

namespace CareNest.App.Services;

public sealed class MauiFileGateway : IAppFileGateway
{
    public async Task<PickedFile?> PickDocumentAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Choose a health document"
        });
        cancellationToken.ThrowIfCancellationRequested();

        if (result is null)
        {
            return null;
        }

        return new PickedFile(
            result.FileName,
            result.ContentType,
            ct => OpenReadAsync(result, ct));
    }

    public async Task<PickedFile?> CapturePhotoAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!MediaPicker.Default.IsCaptureSupported)
        {
            return null;
        }

        var result = await MediaPicker.Default.CapturePhotoAsync();
        cancellationToken.ThrowIfCancellationRequested();
        if (result is null)
        {
            return null;
        }

        return new PickedFile(
            result.FileName,
            result.ContentType,
            ct => OpenReadAsync(result, ct));
    }

    public async Task<PickedFile?> PickBackupForRestoreAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Choose a CareNest encrypted backup"
        });
        cancellationToken.ThrowIfCancellationRequested();

        if (result is null)
        {
            return null;
        }

        return new PickedFile(
            result.FileName,
            result.ContentType,
            ct => OpenReadAsync(result, ct));
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

    private static async Task<Stream> OpenReadAsync(
        FileResult result,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var stream = await result.OpenReadAsync();
        if (!cancellationToken.IsCancellationRequested)
        {
            return stream;
        }

        await stream.DisposeAsync();
        cancellationToken.ThrowIfCancellationRequested();
        throw new OperationCanceledException(cancellationToken);
    }
}
