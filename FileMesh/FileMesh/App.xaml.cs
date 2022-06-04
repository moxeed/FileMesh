using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.WebApi;
using FileMatch;
using FileMesh.Controllers;
using FileMesh.Infrastructure;
using Xamarin.Forms;

namespace FileMesh
{
    public partial class App : Application
    {
        WebServer server;
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
            server = new WebServer(o => o
                .WithUrlPrefix($"http://*:{Http.AppPort}/")
                .WithMode(HttpListenerMode.EmbedIO))
                .WithWebApi("/", m => m.WithController<MeshController>())
                .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Error" })));
            
            server.RunAsync();
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
