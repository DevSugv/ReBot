using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

class ReBot
{
    private static readonly string ProfilePath = System.IO.Path.GetTempPath() + "ReBot";
    private static readonly Random Random = new Random();
    private static readonly ConsoleColor PrimaryColor = ConsoleColor.Cyan;
    private static readonly ConsoleColor SuccessColor = ConsoleColor.Green;
    private static readonly ConsoleColor WarningColor = ConsoleColor.Yellow;
    private static readonly ConsoleColor ErrorColor = ConsoleColor.Red;

    static void Main()
    {
        Console.Title = "ReBot";
        Console.CursorVisible = true;

        new DriverManager().SetUpDriver(new ChromeConfig());

        while (true)
        {
            Console.Clear();
            PrintHeader();
            PrintMenu();

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    SetupLogin();
                    break;
                case "2":
                    StartFarming(headless: false);
                    break;
                case "3":
                    StartFarming(headless: true);
                    break;
                case "0":
                    Console.WriteLine("\nExiting...");
                    return;
                default:
                    PrintMessage("Invalid option!", ErrorColor);
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void PrintHeader()
    {
        Console.ForegroundColor = PrimaryColor;
        Console.WriteLine(@"
 _______  _______  ______   _______ _________
(  ____ )(  ____ \(  ___ \ (  ___  )\__   __/
| (    )|| (    \/| (   ) )| (   ) |   ) (   
| (____)|| (__    | (__/ / | |   | |   | |   
|     __)|  __)   |  __ (  | |   | |   | |   
| (\ (   | (      | (  \ \ | |   | |   | |   
| ) \ \__| (____/\| )___) )| (___) |   | |   
|/   \__/(_______/|/ \___/ (_______)   )_(   
                                             ");
        Console.ResetColor();
        Console.WriteLine("\nMicrosoft Rewards Automation Tool");
        Console.WriteLine("Tip: dont run more than once a day, microsoft likes to ban\n");
    }

    static void PrintMenu()
    {
        Console.ForegroundColor = PrimaryColor;
        Console.WriteLine("Menu:");
        Console.ResetColor();
        Console.WriteLine("1) Setup (login to Microsoft account)");
        Console.WriteLine("2) Rewards farming (normal visible browser)");
        Console.WriteLine("3) Rewards farming (runs invisible in background)");
        Console.WriteLine("0) Exit\n");
        Console.Write("Select an option: ");
    }

    static void PrintMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    static void SetupLogin()
    {
        Console.Clear();
        PrintHeader();
        PrintMessage("Setting up Microsoft login...", PrimaryColor);

        Console.WriteLine("\nA browser window will open. Please:");
        Console.WriteLine("1. Log in to your Microsoft account");
        Console.WriteLine("2. Close the browser when done");
        Console.WriteLine("3. Return here to continue\n");
        PrintMessage("Press ENTER to launch the browser...", WarningColor);
        Console.ReadLine();

        var loginOptions = GetChromeOptions(ProfilePath, isMobile: false, headless: false);
        using (IWebDriver driver = new ChromeDriver(loginOptions))
        {
            driver.Navigate().GoToUrl("https://www.bing.com");
            PrintMessage("\nBrowser opened. Please complete the login process.", PrimaryColor);
            PrintMessage("Press ENTER here when you're done...", WarningColor);
            Console.ReadLine();
        }

        PrintMessage("\nLogin setup completed successfully!", SuccessColor);
        PrintMessage("Press any key to return to menu...", WarningColor);
        Console.ReadKey();
    }

    static void StartFarming(bool headless)
    {
        Console.Clear();
        PrintHeader();
        string mode = headless ? "background (headless)" : "normal (visible)";
        PrintMessage($"Starting rewards farming in {mode} mode...\n", PrimaryColor);

        PrintMessage("Running desktop searches...", PrimaryColor);
        var desktopOptions = GetChromeOptions(ProfilePath, isMobile: false, headless: headless);
        RunSearches(30, desktopOptions, "Desktop");

        PrintMessage("\nRunning mobile searches...", PrimaryColor);
        var mobileOptions = GetChromeOptions(ProfilePath, isMobile: true, headless: headless);
        RunSearches(20, mobileOptions, "Mobile");

        PrintMessage("\nAll searches completed successfully!", SuccessColor);
        PrintMessage("Press any key to return to menu...", WarningColor);
        Console.ReadKey();
    }

    static ChromeOptions GetChromeOptions(string profilePath, bool isMobile, bool headless)
    {
        var options = new ChromeOptions();

        if (headless)
            options.AddArgument("--headless");

        options.AddArguments(
            "--disable-gpu",
            "--window-size=1920,1080",
            "--log-level=3",
            "--silent",
            $"user-data-dir={profilePath}",
            "user-agent=" + (isMobile ?
                "Mozilla/5.0 (Linux; Android 14; Pixel 8 Pro) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Mobile Safari/537.36" :
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36")
        );

        return options;
    }

    static void RunSearches(int count, ChromeOptions options, string tag)
    {
        using (IWebDriver driver = new ChromeDriver(options))
        {
            for (int i = 1; i <= count; i++)
            {
                string query = GenerateSearchQuery();
                string url = $"https://www.bing.com/search?q={Uri.EscapeDataString(query)}";

                driver.Navigate().GoToUrl(url);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"[{tag} {i.ToString().PadLeft(2)}/{count}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Search: {query}");
                Console.ResetColor();

                Thread.Sleep(Random.Next(3000, 8000));
            }
        }
    }

    static string GenerateSearchQuery()
    {
        string[] topics = {
                    "dog facts", "latest tech", "weather today", "space news", "ai trends",
"xbox game pass", "cybersecurity", "funny memes", "music charts", "coding tips",
"random facts", "microsoft edge", "windows 11", "steam sale", "deep learning",
"travel tips", "healthy recipes", "movie reviews", "fitness workouts", "financial news",
"car reviews", "gaming laptops", "smart home devices", "cryptocurrency", "mobile apps",
"gardening ideas", "photography tips", "book recommendations", "movie trailers", "celebrity news",
"sports scores", "tech gadgets", "science discoveries", "virtual reality", "blockchain basics",
"climate change", "DIY projects", "home decor", "fashion trends", "startup ideas",
"social media marketing", "language learning", "space exploration", "mental health", "education news",
"coding tutorials", "programming languages", "art exhibitions", "music festivals", "podcast recommendations",
"food delivery", "coffee brewing", "movie soundtracks", "animal behavior", "hiking trails",
"fitness trackers", "smartphones", "public speaking tips", "online courses", "graphic design",
"meditation techniques", "self improvement", "robotics", "electric cars", "mobile photography",
"stock market", "business news", "recipe ideas", "home workouts", "fitness motivation",
"data science", "machine learning", "video editing", "virtual events", "music production",
"board games", "travel photography", "digital marketing", "coffee shops", "film festivals",
"web development", "gardening tips", "nutrition facts", "space missions", "movie recommendations",
"car maintenance", "fitness nutrition", "photography gear", "language apps", "coding challenges",
"technology trends", "sustainable living", "gaming consoles", "mobile games", "fitness apps",
"history facts", "celebrity interviews", "fashion tips", "science experiments", "smartwatch reviews",
"coding bootcamps", "entrepreneurship", "health tips", "movie trivia", "cycling routes",
"travel guides", "music playlists", "digital art", "startup funding", "personal finance",
"mental exercises", "yoga poses", "film reviews", "home automation"
        };

        string topic = topics[Random.Next(topics.Length)];
        string suffix = Guid.NewGuid().ToString().Substring(0, 4);
        return $"{topic} {suffix}";
    }
}