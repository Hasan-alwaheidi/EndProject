using EndProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient("FootballApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
});
builder.Services.AddHttpClient<ITeamService, TeamService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7118/"); 
});
builder.Services.AddHttpClient<IPlayerService, PlayerService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7118/");
});
builder.Services.AddHttpClient<ILeagueService, LeagueService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7118/");
});
builder.Services.AddHttpClient<IMatchService, MatchService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7118/"); 
});
builder.Services.AddHttpClient<IStadiumService, StadiumService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7118/");
});
builder.Services.AddHttpClient<INewsService,NewsService >(client =>
{
    client.BaseAddress = new Uri("https://localhost:7118/");
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<ILeagueService, LeagueService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IStadiumService, StadiumService>();
builder.Services.AddScoped<INewsService, NewsService>();

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<LiveScoreService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


