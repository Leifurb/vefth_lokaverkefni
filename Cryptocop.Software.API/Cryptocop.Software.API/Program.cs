using System.Text.Json.Serialization;
using Cryptocop.Software.API.Services.Implementations;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Cryptocop.Software.API.Repositories;

using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Repositories.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Cryptocop.Software.API.Middlewares;
using Cryptocop.Software.API.ExceptionHandlerExtensions;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CryptocopDbContext>(options =>
{
    options.UseNpgsql(
    builder.Configuration.GetConnectionString(
        "CryptocopConnectionString"
    ),
    b => b.MigrationsAssembly("Cryptocop.Software.API")
    );
});

builder.Services.AddHttpClient<ICryptoCurrencyService, CryptoCurrencyService>(client => {
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("MessariApiBaseUrl"));
});

builder.Services.AddHttpClient<IExchangeService, ExchangeService>(client => {
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("MessariApiBaseUrl"));
});


builder.Services.AddAuthentication(config =>
{
    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtTokenAuthentication(builder.Configuration);

var jwtConfig = builder.Configuration.GetSection("JwtConfig");
builder.Services.AddTransient<ITokenService>((c) =>
    new TokenService(
        jwtConfig.GetValue<string>("secret"),
        jwtConfig.GetValue<string>("expirationInMinutes"),
        jwtConfig.GetValue<string>("issuer"),
        jwtConfig.GetValue<string>("audience")));

builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddTransient<IQueueService, QueueService>();
builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();


builder.Services.AddTransient<IAddressRepository, AddressRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
builder.Services.AddTransient<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddTransient<ITokenRepository, TokenRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseGlobalExceptionHandler();
app.UseRouting();


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
