﻿using collNotes.Data.Models;
using collNotes.Services;
using collNotes.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace collNotes.ViewModels
{
    public class SitesViewModel : BaseViewModel
    {
        public ObservableCollection<Site> Sites { get; set; }
        public Command LoadSitesCommand { get; set; }
        public SiteService SiteService { get; set; }
        public TripService TripService { get; set; }
        private ExceptionRecordService ExceptionRecordService { get; set; }

        public SitesViewModel()
        {
            SiteService = new SiteService(Context);
            TripService = new TripService(Context);
            ExceptionRecordService = new ExceptionRecordService(Context);

            Title = "Sites";
            Sites = new ObservableCollection<Site>();

            LoadSitesCommand = new Command(async () => await ExecuteLoadSitesCommand());
        }

        private async Task ExecuteLoadSitesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Sites.Clear();
                var sites = await SiteService.GetAllAsync(true);
                foreach (var site in sites)
                {
                    Sites.Add(site);
                }
            }
            catch (Exception ex)
            {
                await ExceptionRecordService.CreateExceptionRecord(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}