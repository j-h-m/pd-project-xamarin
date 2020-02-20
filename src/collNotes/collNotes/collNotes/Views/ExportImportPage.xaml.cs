﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using collNotes.Settings;
using collNotes.ShareFolderInterface;
using collNotes.ViewModels;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExportImportPage : TabbedPage
    {
        private readonly ExportImportViewModel viewModel;
        private readonly bool IsDeviceIosSimulator = DeviceInfo.Platform == DevicePlatform.iOS &&
                DeviceInfo.DeviceType == DeviceType.Virtual;

        public ExportImportPage()
        {
            InitializeComponent();
            BindingContext = this.viewModel = new ExportImportViewModel();

            Title = GetTitle(string.Empty);

            CurrentPageChanged += ExportImportPage_CurrentPageChanged;
        }

        private void ExportImportPage_CurrentPageChanged(object sender, System.EventArgs e)
        {
            var tabbedPage = (TabbedPage)sender;
            if (tabbedPage.CurrentPage == ImportContentPage)
                Title = GetTitle("Import");
            if (tabbedPage.CurrentPage == ExportContentPage)
                Title = GetTitle("Export");
        }

        private async void ImportTrip_Clicked(object sender, System.EventArgs e)
        {
            if (IsDeviceIosSimulator)
            {
                await MaterialDialog.Instance.AlertAsync("Import Trip not supported on iOS Simulator");
            }
            else
            {
                var stream = await OpenFileDialog();
                string message = string.Empty;

                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Attempting to import Trip"))
                {
                    if (await viewModel.ImportTrip(stream))
                    {
                        message = "Trip imported successfully.";
                    }
                    else
                    {
                        message = "Trip import failed.";
                    }
                }
                await MaterialDialog.Instance.SnackbarAsync(message: message,
                                            actionButtonText: "Ok",
                                            msDuration: MaterialSnackbar.DurationIndefinite);
            }
        }

        private async void ExportTrip_Clicked(object sender, System.EventArgs e)
        {
            if (IsDeviceIosSimulator)
            {
                await MaterialDialog.Instance.AlertAsync("Export Trip not supported on iOS Simulator");
            }
            else
            {
                var trips = await viewModel.tripService.GetAllAsync();
                var result = await MaterialDialog.Instance.SelectChoiceAsync("Select a trip",
                                                                 trips.Select(t => t.TripName).ToList());
                string message = string.Empty;

                if (result != -1)
                {
                    var selectedTrip = trips.ToArray()[result];
                    if (await viewModel.ExportTrip(selectedTrip, GetFilePath(selectedTrip.TripName + ".csv")))
                    {
                        message = "Trip exported successfully";
                    }
                    else
                    {
                        message = "Trip export failed";
                    }
                    await MaterialDialog.Instance.SnackbarAsync(message: message,
                                            actionButtonText: "Ok",
                                            msDuration: MaterialSnackbar.DurationIndefinite);
                }
            }
        }

        private async void ImportBackup_Clicked(object sender, System.EventArgs e)
        {
            if (IsDeviceIosSimulator)
            {
                await MaterialDialog.Instance.AlertAsync("Import Backup not supported on iOS Simulator");
            }
            else
            {
                var stream = await OpenFileDialog();
                string message = string.Empty;

                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Attempting to import Backup"))
                {
                    if (await viewModel.ImportBackup(stream))
                    {
                        message = "Backup imported successfully.";
                    }
                    else
                    {
                        message = "Backup import failed.";
                    }
                }
                await MaterialDialog.Instance.SnackbarAsync(message: message,
                                            actionButtonText: "Ok",
                                            msDuration: MaterialSnackbar.DurationIndefinite);
            }
        }

        private async void ExportBackup_Clicked(object sender, System.EventArgs e)
        {
            if (IsDeviceIosSimulator)
            {
                await MaterialDialog.Instance.AlertAsync("Export Backup not supported on iOS Simulator");
            }
            else
            {
                string message = string.Empty;
                if (await viewModel.ExportBackup(GetFilePath("collNotes_Backup.xml")))
                {
                    message = "Backup exported successfully.";
                }
                else
                {
                    message = "Backup export failed.";
                }
                await MaterialDialog.Instance.SnackbarAsync(message: message,
                                            actionButtonText: "Ok",
                                            msDuration: MaterialSnackbar.DurationIndefinite);
            }
        }

        private string GetTitle(string currentPageName)
        {
            return $"{currentPageName} Collection Data";
        }

        private string GetFilePath(string fileName)
        {
            string filePath = DependencyService.Get<IShareFolder>().GetShareFolder();
            return Path.Combine(filePath, fileName);
        }

        private async Task<Stream> OpenFileDialog()
        {
            FileData fileData = await CrossFilePicker.Current.PickFile();
            Stream stream = null;
            try
            {
                stream = fileData.GetStream();
                string path = fileData?.FilePath ?? string.Empty;
                if (fileData is { })
                    fileData.Dispose();
            }
            catch (Exception ex)
            {
                await viewModel.exceptionRecordService.AddAsync(new Data.Models.ExceptionRecord()
                {
                    Created = DateTime.Now,
                    DeviceInfo = CollNotesSettings.DeviceInfo,
                    ExceptionInfo = ex.Message
                });
            }
            return stream;
        }
    }
}