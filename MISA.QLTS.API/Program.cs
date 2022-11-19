using MISA.QLTS.BL;
using MISA.QLTS.DL;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                      });
});

builder.Services.AddControllers();

// Dependency 
builder.Services.AddScoped<IFixedAssetDL, FixedAssetDL>();
builder.Services.AddScoped<IFixedAssetBL, FixedAssetBL>();

builder.Services.AddScoped<IDepartmentDL, DepartmentDL>();
builder.Services.AddScoped<IDepartmentBL, DepartmentBL>();

builder.Services.AddScoped<IFixedAssetCategoryDL, FixedAssetCategoryDL>();
builder.Services.AddScoped<IFixedAssetCategoryBL, FixedAssetCategoryBL>();

// Lấy connectionString từ file appsetting
DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("MySql");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();