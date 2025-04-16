using Microsoft.Extensions.FileProviders;
using WMSCORE.Common;
using WMSCORE.Repositories;
using WMSCORE.Services;
using static WMSCORE.Repositories.LoginRepository;
using static WMSCORE.Repositories.PrinterTypeRepository;
using static WMSCORE.Services.LoginService;
using static WMSCORE.Services.PrinterTypeService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IPrinterTypeService, PrinterTypeService>();
builder.Services.AddScoped<IPrinterTypeRepository, PrinterTypeRepository>();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowReactApp",
		policy => policy.WithOrigins("http://localhost:5173")  // React app's URL
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials());
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");


var env = app.Services.GetRequiredService<IWebHostEnvironment>();
string webRootPath = env.WebRootPath;


var admRoot = string.Empty;
if (Directory.Exists(Path.Combine(env.ContentRootPath, "Areas")))
{
	//admRoot = Path.Combine(env.ContentRootPath, "ADM", "iggrid1", "iggrid1-output");
	admRoot = Path.Combine(env.ContentRootPath, "WMS-app");
}
else
{
	//admRoot = Path.Combine(env.ContentRootPath, "..", "ADM", "iggrid1", "iggrid1-output");
	admRoot = Path.Combine(env.ContentRootPath, "WMS-app");
}

app.UseDefaultFiles(new DefaultFilesOptions
{
	FileProvider = new PhysicalFileProvider(admRoot),
	// RequestPath = "/ADM/iggrid001"
	RequestPath = "/WMS-app"

});
if (Directory.Exists(admRoot))
{
	app.UseStaticFiles(new StaticFileOptions
	{

		FileProvider = new PhysicalFileProvider(admRoot),
		//RequestPath = "/ADM/iggrid001"
		RequestPath = "/WMS-app"
	});
}
app.UseAuthorization();

app.MapControllers();

app.Run();
