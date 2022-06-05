﻿using FileMesh.Infrastructure;
using FileMesh.Service;
using FileSystem;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FileMesh
{
    public partial class MainPage : ContentPage
    {
        public string Name { get; set; } = "sdjskdj";
        public MainPage()
        {
            InitializeComponent();
            label.Text = Http.GetIp();
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            SelectFile().ConfigureAwait(false);
        }

        private async Task SelectFile()
        {
            var file = await FilePicker.PickAsync();

            if (file != null)
            {
                await MeshService.AddFile(file);
            }
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            var context = (PhysicalFile)viewCell.BindingContext;

            if (context.CanOpen)
            {
                Launcher.OpenAsync
                    (new OpenFileRequest()
                    {
                        File = new ReadOnlyFile(context.Path)
                    }
                );
            }
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            var entry = (Entry)sender;
            Search(entry.Text).ConfigureAwait(false);
        }

        private void EntryViewCell_Tapped(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            var context = (FileMatch.Entry)viewCell.BindingContext;
            ShowFileList();
        }

        private async Task Search(string term) {
            var result = await MeshService.Search(term);
            entryListView.ItemsSource = new ObservableCollection<FileMatch.Entry>(result);
            ShowSearchList();
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            if (string.IsNullOrEmpty(entry.Text)) {
                ShowFileList();
            }
        }

        private void ShowSearchList() {
            listView.IsVisible = false;
            entryListView.IsVisible = true;
        }

        private void ShowFileList()
        {
            listView.IsVisible = true;
            entryListView.IsVisible = false;
        }
    }
}
