using Grpc_Swagger.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddGrpc(); // 默认
builder.Services.AddGrpc().AddJsonTranscoding(); // Rest API 支持


#region 添加Swagger
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1",
      new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
});
#endregion

#region 添加Grpcui 调试
builder.Services.AddGrpcReflection();
#endregion

var app = builder.Build();

app.UseSwagger(); // 启用Swagger

// 判断当前环境是否为开发环境
if (app.Environment.IsDevelopment())
{
  // Grpcui 调试
  app.MapGrpcReflectionService();
  // swagger ui
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
  });
}
// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
//app.MapGrpcService<HttpApiGreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
