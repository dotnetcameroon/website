namespace app.Api.Admin.Dtos;

public record AppSummaryResponse(Guid Id, string ClientName);

public record RegisterAppRequest(string Name);

public record RegisterAppResponse(Guid Id, string ClientName, string ClientSecret);
