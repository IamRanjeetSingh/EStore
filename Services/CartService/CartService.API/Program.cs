using CartService;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCartService(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddMediatR(mediatrConfig => mediatrConfig.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
