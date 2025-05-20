using BookLibrary.Repositories;
using BookLibrary.Middleware; // ��������� using ��� ������ Middleware
using BookLibrary.Filters;    // ��������� using ��� ����� ��������

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ����������� ������������ (��� � �����)
builder.Services.AddSingleton<IAuthorRepository, JsonAuthorRepository>();
builder.Services.AddSingleton<IBookRepository, JsonBookRepository>();
builder.Services.AddSingleton<IReaderRepository, JsonReaderRepository>();

// ���������� ������������ � ��������������� � ������������ ���������� ��������
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<LogActionFilterAttribute>();       // ���������� ���������� ������� ��������
    options.Filters.Add<GlobalExceptionFilterAttribute>(); // ���������� ���������� ������� ����������
});
// ���� �� GlobalExceptionFilterAttribute �������� ILogger ����� DI:
// builder.Services.AddScoped<GlobalExceptionFilterAttribute>(); // ���������������� ������ ��� ������
// options.Filters.AddService<GlobalExceptionFilterAttribute>(); // � �������� ��� ��� ������


var app = builder.Build();

// Configure the HTTP request pipeline.

// **����������� ������ ���������� Middleware**
// ������ ����������� � ������ ���������, ����� ������������� ��� �������.
// ���� Middleware ����� ������ ������ �������� � ����� ������ InvokeAsync,
// ��� ����� ���������� ����� app.UseRouting(), �� ��� ������� ������
// (����������� Path � ������ � Route � �����) ������� ���������� ��������.
app.UseRequestLogging();

if (!app.Environment.IsDevelopment())
{
    // ���� ���������� ����� ����� �����������, ���� GlobalExceptionFilter ������������� ���
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    // � ������ ���������� ����� �������� ����������� �������� ������ ��� ��������� �������
    // app.UseDeveloperExceptionPage(); // ��� ������, ���� GlobalExceptionFilter ��������� ��� �����
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); // �����: UseRouting ������ ���� �� UseEndpoints (MapControllerRoute)
                  // � �� Middleware, ���� ��� ����� �������� ������ � RouteData �����

app.UseAuthorization(); // ���� ������������ �����������

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
