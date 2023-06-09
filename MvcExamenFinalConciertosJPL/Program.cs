using Amazon.S3;
using MvcExamenFinalConciertosJPL.Helper;
using MvcExamenFinalConciertosJPL.Models;
using MvcExamenFinalConciertosJPL.Services;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ServicesEventos>();
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddTransient<ServiceStoragesS3>();

string miSecreto = await HelperSecretManager.GetSecretAsync();
KeysModel model = JsonConvert.DeserializeObject<KeysModel>(miSecreto);
builder.Services.AddSingleton<KeysModel>(model);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Eventos}/{action=Index}/{id?}");

app.Run();
