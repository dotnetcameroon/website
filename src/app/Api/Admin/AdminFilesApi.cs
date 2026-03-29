using app.business.Services;

namespace app.Api.Admin;

public static class AdminFilesApi
{
    public static RouteGroupBuilder MapAdminFilesApi(this RouteGroupBuilder group)
    {
        group.MapPost("/files/upload", async (
            IFormFile file,
            string? folder,
            IFileUploader fileUploader,
            CancellationToken ct) =>
        {
            if (file.Length == 0)
                return Results.BadRequest("No file provided.");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var location = folder ?? "images";

            using var stream = file.OpenReadStream();
            var url = await fileUploader.UploadAsync(stream, fileName, location, ct);

            return Results.Ok(new FileUploadResponse(url, fileName));
        })
        .DisableAntiforgery();

        return group;
    }

    private record FileUploadResponse(string Url, string FileName);
}
