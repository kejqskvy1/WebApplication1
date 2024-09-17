using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using WebApplication1.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// ������ �������� CORS � ��������� ��������
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:5127", "https://localhost:7281") // ������ ������ � launchSettings.json
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // ���������� ������������ �������� ����� (���, ������)
    });
});

builder.Services.AddSignalR();

// ������ ������ � ���������
builder.Services.AddControllersWithViews();
/*builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});*/

var app = builder.Build();

// ����������� ������� ������� �� ����� ��������� ��������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ����������� ������������ CORS
app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

// ����������� �������� ��� ���������� �� SignalR Hub
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ToDoHub>("/todoHub");

app.Run();
