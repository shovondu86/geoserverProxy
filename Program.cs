//.Net core Geoserver proxy 
//Author: Mohiuddin Shovon
//Dated: 18.06.23


using Microsoft.AspNetCore.Http.Extensions;

//const string CorsPolicyName = "_myCorsPolicy";
var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddCors(
//   options => options.AddPolicy(
//    name: CorsPolicyName, builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
//     )
//   );
var app = builder.Build();
app.UseStaticFiles();
//app.UseCors(CorsPolicyName);

var proxyClient = new HttpClient();
var geosBaseUrl="http://stsi.lged.gov.bd:8080/geoserver/";

app.MapGet("/", () => "Hello World!");
app.MapGet("/wms/", async (HttpRequest request) => 
{
 var restUrl=request.GetDisplayUrl();
 var restUrlStr = restUrl.ToString();
 string restWmsUrl = restUrlStr.Substring(restUrlStr.LastIndexOf("wms"));;
 //Console.WriteLine(restWmsUrl);

 var url=geosBaseUrl+restWmsUrl;
 //var stream = await proxyClient.GetStreamAsync(url);
 var response = await proxyClient.GetAsync(url);
 var respStream=response.Content.ReadAsStream();
 var header=(response.Content.Headers.ContentType).ToString();
 
    return Results.Stream(respStream, header);
});

app.MapGet("/wfs/", async (HttpRequest request) => 
{
 var restUrl=request.GetDisplayUrl();
 var restUrlStr = restUrl.ToString();
 string restWmsUrl = restUrlStr.Substring(restUrlStr.LastIndexOf("wfs"));;
 //Console.WriteLine(restWmsUrl);
 
 var url=geosBaseUrl+restWmsUrl;

 var response = await proxyClient.GetAsync(url);
 var respStream=response.Content.ReadAsStream();
 var header=(response.Content.Headers.ContentType).ToString();
 
    return Results.Stream(respStream, header);
});

app.Run();
