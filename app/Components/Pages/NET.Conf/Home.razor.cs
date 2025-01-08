using app.domain.ViewModels;

namespace app.Components.Pages.NET_Conf;

public partial class Home
{
    private string EventUrl = "https://forms.office.com/r/ChLwZEYr53";
    private string SpeakerLink = "https://forms.office.com/r/debtL0TDmA";
    private string SponsorLink = "https://forms.office.com/r/YnKLW60s73";

    private string _dotnetSessionsClass = string.Empty;
    private string _microsoftSessionsClass = "hidden";

    private Activity[] Activities = [
        new ("Inspiring Sessions and Conferences", "Dive into cutting-edge topics and explore the latest in .NET technology through insightful sessions and talks. Learn from experts as they cover a wide range of topics, from web and mobile development to cloud solutions and artificial intelligence", "/assets/activities/activity-2.jpeg", "Tech"),
        new ("Meet Microsoft Experts", "Gain exclusive insights from Microsoft employees who work directly on .NET and related technologies. This is your chance to hear directly from those shaping the future of .NET and to ask questions that matter to your projects.", "/assets/featured/net9.png", "Culture"),
        new ("Networking and Connections", "Build meaningful connections with fellow developers, industry professionals, and tech enthusiasts. Exchange ideas, discuss solutions, and expand your network within the vibrant .NET community of Cameroon and beyond.", "/assets/featured/carousel-2.png", "Community"),
    ];

    private Partner[] Partners = [
        new ("/assets/sponsors/dotnetfondation.webp", ".NET Foundation", "https://dotnetfoundation.org"),
        new ("/assets/sponsors/proditech.png", "Proditech Consulting", "https://proditech-digital.com"),
        new ("/assets/sponsors/itia-removebg-preview.png", "ITIA Solutions Consulting", "https://itia-consulting.com"),
        new ("/assets/sponsors/codec.png", "Codec", "https://codec.cm"),
        new ("/assets/sponsors/tcd-cmr.png", "Tech Communities Day Cameroon", "https://www.linkedin.com/company/tech-communities-day-cameroon"),
        new ("/assets/sponsors/examboot.jpg", "Examboot", "https://examboot.net/"),
        new ("/assets/sponsors/Microsoft_PowerPlateform_Cameroun-removebg-preview.png", "Microsoft PowerPlateform Cameroun", "https://www.linkedin.com/company/communaut%C3%A9-microsoft-powerplatform"),
    ];

    private Session[] MicrosoftSessions = [
        new(
            "Keynote",
            null,
            [ new Speaker("Boris Gautier", "Software, SI, DevOps and Lead Engineer", "", null, null, null)],
            new TimeOnly(10, 00),
            new TimeOnly(10, 30),
            SessionType.Keynote, Room.Microsoft),
        new(
            "Copilot: prise en main de copilot sur Microsoft 365",
            null,
            [ new Speaker("Tchassem Eric", "CEO of PRODITECH", "", null, null, null)],
            new TimeOnly(10, 30),
            new TimeOnly(11, 00),
            SessionType.Talk, Room.Microsoft),
        new(
            "ITIA",
            null,
            [ new Speaker("ITIA", "ITIA", "", null, null, null)],
            new TimeOnly(11, 00),
            new TimeOnly(11, 15),
            SessionType.Advertisement, Room.Microsoft),
        new(
            "Git and GitHub for Dummies",
            null,
            [ new Speaker("Steve Yonkeu", "Software Engineer", "", null, null, null)],
            new TimeOnly(11, 15),
            new TimeOnly(11, 45),
            SessionType.Talk, Room.Microsoft),
        new(
            "Microsoft Power BI - Initiation à Power BI : Exploration des données et création de tableaux de bord interactifs",
            null,
            [ new Speaker("BITÉE Méryl", "Ing @ ARII, Architecte des Technologies du Numériques", "", null, null, null)],
            new TimeOnly(11, 45),
            new TimeOnly(12, 15),
            SessionType.Talk, Room.Microsoft),
        new(
            "Break",
            null,
            [ new Speaker("Break", "Break", "", null, null, null)],
            new TimeOnly(12, 15),
            new TimeOnly(12, 45),
            SessionType.Break, Room.Microsoft),
        new(
            "Tech Community",
            null,
            [ new Speaker("Tech Community", "Tech Community", "", null, null, null)],
            new TimeOnly(12, 45),
            new TimeOnly(13, 00),
            SessionType.Advertisement, Room.Microsoft),
        new(
            "Développer une application web moderne avec Python et .NET : Une approche hybride pour les développeurs web",
            null,
            [ new Speaker("Johanna Felicia", "Software Engineer", "", null, null, null)],
            new TimeOnly(13, 00),
            new TimeOnly(13, 30),
            SessionType.Talk, Room.Microsoft),
        new(
            "Overview of Microsoft AI solutions",
            null,
            [ new Speaker("Kana Azeuko Sherelle", "Machine Learning Engineer", "", null, null, null)],
            new TimeOnly(13, 30),
            new TimeOnly(14, 00),
            SessionType.Talk, Room.Microsoft),
        new(
            "PRODITECH",
            null,
            [ new Speaker("PRODITECH", "PRODITECH", "", null, null, null)],
            new TimeOnly(14, 00),
            new TimeOnly(14, 15),
            SessionType.Advertisement, Room.Dotnet),
    ];

    private Session[] DotnetSessions = [
        new(
            "Keynote",
            null,
            [ new Speaker("Doriane Mangamtcheuth", "Analyste M365 | Power Platform | Microsoft Dynamics 365", "", null, null, null)],
            new TimeOnly(10, 00),
            new TimeOnly(10, 30),
            SessionType.Keynote, Room.Dotnet),
        new(
            "Deploy .NET Aspire app to Kubernetes using Azure DevOps pipelines",
            null,
            [ new Speaker("Vahid Farahmandian", "CEO of Spoota Co", "", null, null, null)],
            new TimeOnly(10, 30),
            new TimeOnly(11, 00),
            SessionType.Talk, Room.Dotnet),
        new(
            "CODEC",
            null,
            [ new Speaker("CODEC", "CODEC", "", null, null, null)],
            new TimeOnly(11, 00),
            new TimeOnly(11, 15),
            SessionType.Advertisement, Room.Dotnet),
        new(
            "Moderniser son architecture avec .NET et Azure",
            null,
            [ new Speaker("NTONGA Franck Loïc", "DevOps Engineer", "", null, null, null)],
            new TimeOnly(11, 15),
            new TimeOnly(11, 45),
            SessionType.Talk, Room.Dotnet),
        new(
            "Pourquoi choisir .NET 9 en 2025: Fonctionnalités, Avantages et Innovations",
            null,
            [ new Speaker("Djoufson Che", "Software Engineer @ L'Agence Digitale", "", null, null, null)],
            new TimeOnly(11, 45),
            new TimeOnly(12, 15),
            SessionType.Talk, Room.Dotnet),
        new(
            "Break",
            null,
            [ new Speaker("Break", "Break", "", null, null, null)],
            new TimeOnly(12, 15),
            new TimeOnly(12, 45),
            SessionType.Break, Room.Dotnet),
        new(
            "Examboot",
            null,
            [ new Speaker("Examboot", "Examboot", "", null, null, null)],
            new TimeOnly(12, 45),
            new TimeOnly(13, 00),
            SessionType.Advertisement, Room.Dotnet),
        new(
            "Le Domain-Driven Design du point de vue du client",
            null,
            [ new Speaker("Aguekeng Arolle", "Software Engineer @ RHOPEN LABS", "", null, null, null)],
            new TimeOnly(13, 00),
            new TimeOnly(13, 30),
            SessionType.Talk, Room.Dotnet),
        new(
            "Cross-Platform Development Simplified: Building a Sync-Enabled To-Do App with .NET MAUI and Parse",
            null,
            [ new Speaker("Yvan Brunel", ".NET Developer", "", null, null, null)],
            new TimeOnly(13, 30),
            new TimeOnly(14, 00),
            SessionType.Talk, Room.Dotnet),
        new(
            "PowerPlatform Cameroon",
            null,
            [ new Speaker("PowerPlatform Cameroon", "PowerPlatform Cameroon", "", null, null, null)],
            new TimeOnly(14, 00),
            new TimeOnly(14, 15),
            SessionType.Advertisement, Room.Dotnet),
    ];

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
