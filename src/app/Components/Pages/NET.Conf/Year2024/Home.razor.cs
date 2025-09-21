using app.domain.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace app.Components.Pages.NET_Conf.Year2024;

public partial class Home
{
    [Inject]
    private IStringLocalizerFactory LocalizerFactory { get; set; } = default!;

    private IStringLocalizer? _sharedLocalizer;
    private IStringLocalizer SharedLocalizer => _sharedLocalizer ??= LocalizerFactory.Create("SharedResources", "app");
    private readonly Activity[] Activities = [
        new ("Inspiring Sessions and Conferences", "Dive into cutting-edge topics and explore the latest in .NET technology through insightful sessions and talks. Learn from experts as they cover a wide range of topics, from web and mobile development to cloud solutions and artificial intelligence", "/assets/activities/activity-2.jpeg", "Tech"),
        new ("Meet Microsoft Experts", "Gain exclusive insights from Microsoft employees who work directly on .NET and related technologies. This is your chance to hear directly from those shaping the future of .NET and to ask questions that matter to your projects.", "/assets/featured/net9.png", "Culture"),
        new ("Networking and Connections", "Build meaningful connections with fellow developers, industry professionals, and tech enthusiasts. Exchange ideas, discuss solutions, and expand your network within the vibrant .NET community of Cameroon and beyond.", "/assets/featured/carousel-2.png", "Community"),
    ];

    private readonly Partner[] Partners = [
        new ("/assets/sponsors/dotnetfondation.webp", ".NET Foundation", "https://dotnetfoundation.org"),
        new ("/assets/sponsors/proditech.png", "Proditech Consulting", "https://proditech-digital.com"),
        new ("/assets/sponsors/itia-removebg-preview.png", "ITIA Solutions Consulting", "https://itia-consulting.com"),
        new ("/assets/sponsors/codec.png", "Codec", "https://codec.cm"),
        new ("/assets/sponsors/tcd-cmr.png", "Tech Communities Day Cameroon", "https://www.linkedin.com/company/tech-communities-day-cameroon"),
        new ("/assets/sponsors/examboot.jpg", "Examboot", "https://examboot.net/"),
        new ("/assets/sponsors/Microsoft_PowerPlateform_Cameroun-removebg-preview.png", "Microsoft PowerPlateform Cameroun", "https://www.linkedin.com/company/communaut%C3%A9-microsoft-powerplatform"),
    ];

    private readonly Session[] MicrosoftSessions = [
    ];

    private readonly Session[] DotnetSessions = [
        new(
            "Keynote",
            null,
            [ new Speaker("Doriane Mangamtcheuth", "Analyste M365 | Power Platform | Microsoft Dynamics 365", "", null, null, null)],
            new TimeOnly(9, 00),
            new TimeOnly(9, 30),
            SessionType.Keynote, Room.Dotnet),
        new(
            "Keynote",
            null,
            [ new Speaker("Boris Gautier", "Software, SI, DevOps and Lead Engineer", "", null, null, null)],
            new TimeOnly(9, 30),
            new TimeOnly(10, 00),
            SessionType.Keynote, Room.Dotnet),
        new(
            "Copilot: prise en main de copilot sur Microsoft 365",
            null,
            [ new Speaker("Tchassem Eric", "CEO of PRODITECH", "", null, null, null)],
            new TimeOnly(10, 00),
            new TimeOnly(10, 30),
            SessionType.Talk, Room.Dotnet),
        new(
            "ITIA",
            null,
            [ new Speaker("ITIA", "ITIA", "", null, null, null)],
            new TimeOnly(10, 30),
            new TimeOnly(10, 45),
            SessionType.Advertisement, Room.Dotnet),
        new(
            "Deploy .NET Aspire app to Kubernetes using Azure DevOps pipelines",
            null,
            [ new Speaker("Vahid Farahmandian", "CEO of Spoota Co", "", null, null, null)],
            new TimeOnly(10, 45),
            new TimeOnly(11, 15),
            SessionType.Talk, Room.Dotnet),
        new(
            "PowerPlatform Cameroon",
            null,
            [ new Speaker("PowerPlatform Cameroon", "PowerPlatform Cameroon", "", null, null, null)],
            new TimeOnly(11, 15),
            new TimeOnly(11, 30),
            SessionType.Advertisement, Room.Dotnet),
        new(
            "Git and GitHub for Dummies",
            null,
            [ new Speaker("Steve Yonkeu", "Software Engineer", "", null, null, null)],
            new TimeOnly(11, 30),
            new TimeOnly(12, 00),
            SessionType.Talk, Room.Dotnet),
        new(
            "Microsoft Power BI - Introduction to Power BI: Data Exploration and Interactive Dashboard Creation",
            null,
            [ new Speaker("BITÉE Méryl", "Ing @ ARII, Architecte des Technologies du Numériques", "", null, null, null)],
            new TimeOnly(12, 00),
            new TimeOnly(12, 30),
            SessionType.Talk, Room.Dotnet),
        new(
            "Examboot",
            null,
            [ new Speaker("Examboot", "Examboot", "", null, null, null)],
            new TimeOnly(12, 30),
            new TimeOnly(12, 45),
            SessionType.Advertisement, Room.Dotnet),
        new(
            "Break",
            null,
            [ new Speaker("Break", "Break", "", null, null, null)],
            new TimeOnly(12, 45),
            new TimeOnly(13, 00),
            SessionType.Break, Room.Dotnet),
        new(
            "Tech Community",
            null,
            [ new Speaker("Tech Community", "Tech Community", "", null, null, null)],
            new TimeOnly(13, 00),
            new TimeOnly(13, 15),
            SessionType.Advertisement, Room.Dotnet),
        new(
            "Domain-Driven Design from the Client Perspective",
            null,
            [ new Speaker("Aguekeng Arolle", "Software Engineer @ RHOPEN LABS", "", null, null, null)],
            new TimeOnly(13, 15),
            new TimeOnly(13, 45),
            SessionType.Talk, Room.Dotnet),
        new(
            "Cross-Platform Development Simplified: Building a Sync-Enabled To-Do App with .NET MAUI and Parse",
            null,
            [ new Speaker("Yvan Brunel", ".NET Developer", "", null, null, null)],
            new TimeOnly(13, 45),
            new TimeOnly(14, 15),
            SessionType.Talk, Room.Dotnet),
        new(
            "CODEC",
            null,
            [ new Speaker("CODEC", "CODEC", "", null, null, null)],
            new TimeOnly(14, 15),
            new TimeOnly(14, 30),
            SessionType.Advertisement, Room.Dotnet),
        new(
            "Développer une application web moderne avec Python et .NET : Une approche hybride pour les développeurs web",
            null,
            [ new Speaker("Johanna Felicia", "Software Engineer", "", null, null, null)],
            new TimeOnly(14, 30),
            new TimeOnly(15, 00),
            SessionType.Talk, Room.Dotnet),
        new(
            "Overview of Microsoft AI solutions",
            null,
            [ new Speaker("Kana Azeuko Sherelle", "Machine Learning Engineer", "", null, null, null)],
            new TimeOnly(15, 00),
            new TimeOnly(15, 30),
            SessionType.Talk, Room.Dotnet),
        new(
            "PRODITECH",
            null,
            [ new Speaker("PRODITECH", "PRODITECH", "", null, null, null)],
            new TimeOnly(15, 30),
            new TimeOnly(15, 45),
            SessionType.Advertisement, Room.Dotnet),
    ];

    protected override void OnInitialized()
    {
        base.OnInitialized();

        // Localize specific session titles that should use the localization system
        var powerBiKey = "Session_PowerBI_Title";
        var localizedPowerBiTitle = SharedLocalizer[powerBiKey];
        const string originalPowerBiTitle = "Microsoft Power BI - Introduction to Power BI: Data Exploration and Interactive Dashboard Creation";

        for (int i = 0; i < DotnetSessions.Length; i++)
        {
            if (DotnetSessions[i].Title == originalPowerBiTitle)
            {
                var s = DotnetSessions[i];
                DotnetSessions[i] = new Session(localizedPowerBiTitle, s.Description, s.Speakers, s.StartTime, s.EndTime, s.Type, s.Room);
                break;
            }
        }

        // Localize other selected session titles
        ReplaceTitle(
            original: "Deploy .NET Aspire app to Kubernetes using Azure DevOps pipelines",
            key: "Session_DeployAspire_Title");
        ReplaceTitle(
            original: "Git and GitHub for Dummies",
            key: "Session_GitForDummies_Title");
        ReplaceTitle(
            original: "Domain-Driven Design from the Client Perspective",
            key: "Session_DDDClient_Title");
        ReplaceTitle(
            original: "Cross-Platform Development Simplified: Building a Sync-Enabled To-Do App with .NET MAUI and Parse",
            key: "Session_MAUIParse_Title");
        ReplaceTitle(
            original: "Développer une application web moderne avec Python et .NET : Une approche hybride pour les développeurs web",
            key: "Session_PythonDotNetHybrid_Title");
        ReplaceTitle(
            original: "Overview of Microsoft AI solutions",
            key: "Session_MS_AI_Overview_Title");
        ReplaceTitle(
            original: "Keynote",
            key: "Session_Keynote_Title");
    }

    private void ReplaceTitle(string original, string key)
    {
        for (int i = 0; i < DotnetSessions.Length; i++)
        {
            if (DotnetSessions[i].Title == original)
            {
                var s = DotnetSessions[i];
                DotnetSessions[i] = new Session(SharedLocalizer[key], s.Description, s.Speakers, s.StartTime, s.EndTime, s.Type, s.Room);
                return;
            }
        }
    }

    record Activity(
        string Title,
        string Desctiption,
        string ImageUrl,
        string Label
    );

    record Partner(
        string ImageUrl,
        string Name,
        string? Website
    );
}
