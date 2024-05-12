using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    //[AHO] Not acceptable (Status code 406) when the requested format (Accept header) is not available
    options.ReturnHttpNotAcceptable = true;
    //[AHO] Explicit Output Formatterd
    options.OutputFormatters.Clear();
    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()); // The first registered Formatter => Default Formatter
    options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions
    {
        TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    }));
});

builder.Services.AddProblemDetails(options =>
 {
     options.CustomizeProblemDetails = ctxt =>
     {
         ctxt.ProblemDetails.Extensions.Add("customInfo", "Take it or leave it!");
     };
 });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

//app.MapControllers();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();
