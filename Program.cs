
using ecomm_project.Areas.customer.Controllers;
using ecomm_project.DataAccess.Data;
using ecomm_project.DataAccess.repository;
using ecomm_project.DataAccess.repository.Irepository;
using ecomm_project.DataAccess.Repository;
using ecomm_project.DataAccess.Repository.Irepository;
using ecomm_utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.DiaSymReader;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Twilio;
using Twilio.AspNet.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("constr") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
//builder.Services.AddScoped<IcategoryRepository, categoryRepository>();
//builder.Services.AddScoped<IcovertypeRepository, covertypeRepository>();
builder.Services.AddScoped<iunitofwork, Unitofwork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = $"/Identity/Account/Login";
    option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    option.LogoutPath = $"/Identity/Account/Logout";
});
string googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
string linkedInClientSecret = Environment.GetEnvironmentVariable("LINKEDIN_CLIENT_SECRET");
string GitHubClientSecret = Environment.GetEnvironmentVariable("GitHub_CLIENT_SECRET");
string FacebookClientSecret = Environment.GetEnvironmentVariable("Facebook_CLIENT_SECRET");
string stripeSecret = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");



builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(30);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
builder.Services.AddTwilioClient(builder.Configuration.GetSection("Twilio"));
builder.Services.AddScoped<ITwilioService, TwilioService>();  
builder.Services.AddTransient<ISmsSender, SmsSender>();  




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("StripeSettings")["SecretKey"];

app.UseAuthorization();

app.MapStaticAssets();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=customer}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
