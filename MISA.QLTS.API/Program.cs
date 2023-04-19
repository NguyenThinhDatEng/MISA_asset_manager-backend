using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
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
                          policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(origin => true); // allow any origin
                      });
});

builder.Services.AddControllers();

// Bỏ qua validate của attribute trong C# khi debug
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Dependency injection
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));

builder.Services.AddScoped<IFixedAssetDL, FixedAssetDL>();
builder.Services.AddScoped<IFixedAssetBL, FixedAssetBL>();

builder.Services.AddScoped<IDepartmentDL, DepartmentDL>();
builder.Services.AddScoped<IDepartmentBL, DepartmentBL>();

builder.Services.AddScoped<IFixedAssetCategoryDL, FixedAssetCategoryDL>();
builder.Services.AddScoped<IFixedAssetCategoryBL, FixedAssetCategoryBL>();

builder.Services.AddScoped<IBudgetDL, BudgetDL>();
builder.Services.AddScoped<IBudgetBL, BudgetBL>();

builder.Services.AddScoped<IVoucherDL, VoucherDL>();
builder.Services.AddScoped<IVoucherBL, VoucherBL>();

builder.Services.AddScoped<IUserBL, UserBL>();


// Lấy connectionString từ file appsetting
DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("MySql");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.Headers["Location"] = context.RedirectUri;
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    });

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Who are you?
app.UseAuthentication();
// Are you allowed?
app.UseAuthorization();

app.MapControllers();

app.Run();