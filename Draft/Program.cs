using EmbedIO;
using EmbedIO.Actions;

var server = new WebServer(o => o
    .WithUrlPrefix("http://*:8080/")
    .WithMode(HttpListenerMode.EmbedIO))
    .OnGet(c => c.SendStringAsync("ok", "text", System.Text.Encoding.UTF8))
    .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Error" }))); ;

server.RunAsync();
Console.ReadKey();