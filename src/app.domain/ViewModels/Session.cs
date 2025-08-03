namespace app.domain.ViewModels;

public record Session(
    string Title,
    string? Description,
    Speaker[] Speakers,
    TimeOnly StartTime,
    TimeOnly EndTime,
    SessionType Type,
    Room Room
);
