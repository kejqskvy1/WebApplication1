using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using WebApplication1.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Додаємо підтримку CORS з вказаними доменами
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:5127", "https://localhost:7281") // Вказані домени з launchSettings.json
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // Дозволяємо використання облікових даних (кукі, токени)
    });
});

builder.Services.AddSignalR();

// Додаємо сервіси в контейнер
builder.Services.AddControllersWithViews();
/*builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});*/

var app = builder.Build();

// Налаштовуємо обробку помилок та інших виключних ситуацій
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Застосовуємо конфігурацію CORS
app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

// Налаштовуємо маршрути для контролерів та SignalR Hub
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ToDoHub>("/todoHub");

app.Run();
