using Data;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// var connectionSettings = new ConnectionSettings(new Uri("http://127.0.0.1:9200"));
    // .DefaultMappingFor<Product>(x => x.IndexName("products"));

//builder.Services.AddSingleton<IElasticClient>(new ElasticClient(connectionSettings));

builder.Services.AddDefaultIdentity<Account>(options =>
{
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<Role>()
    .AddRoleManager<RoleManager<Role>>()
    .AddSignInManager<SignInManager<Account>>()
    .AddRoleValidator<RoleValidator<Role>>()
    .AddEntityFrameworkStores<DataContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
