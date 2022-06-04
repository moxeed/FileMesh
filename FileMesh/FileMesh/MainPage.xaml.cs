using FileMesh.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FileMesh
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            label.Text = Http.GetIp();
        }
    }
}
