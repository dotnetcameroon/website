namespace app.Api.Admin.Dtos;

public record PartnerResponse(Guid Id, string Name, string Logo, string Website);

public record CreateOrUpdatePartnerRequest(string Name, string Logo, string Website);
