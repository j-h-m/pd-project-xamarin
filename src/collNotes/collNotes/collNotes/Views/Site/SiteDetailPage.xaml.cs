﻿using System;
using System.Linq;
using collNotes.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    public partial class SiteDetailPage : ContentPage
    {
        private readonly SiteDetailViewModel viewModel;
        private Location CurrentLocation { get; set; }
        private Xamarin.Forms.Maps.Map Map { get; set; }
        private const double DEGREES = 0.01;
        private bool LocationChanged = false;

        public SiteDetailPage(SiteDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;

            CurrentLocation = new Location()
            {
                Longitude = double.Parse(viewModel.Site.Longitude),
                Latitude = double.Parse(viewModel.Site.Latitude),
                Accuracy = double.Parse(viewModel.Site.CoordinateUncertaintyInMeters),
                Altitude = double.Parse(viewModel.Site.MinimumElevationInMeters)
            };
            UpdateCurrentLocation();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Update_Clicked(object sender, EventArgs e)
        {
            await viewModel.siteService.UpdateAsync(viewModel.Site);
            await Navigation.PopAsync();
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var childItems = await viewModel.siteService.GetChildrenAsync(viewModel.Site);
            string specimenNames = string.Join(", ", childItems.Select(s => s.SpecimenName).ToArray());

            string message = "Are you sure you want to delete this Site? This will delete all associated Specimen.";

            if (!string.IsNullOrEmpty(specimenNames))
                message = $"Deleting {viewModel.Site.SiteName} will also delete the following:" +
                    Environment.NewLine +
                    $"Specimen: {specimenNames}.";

            bool result = Convert.ToBoolean(await MaterialDialog.Instance.ConfirmAsync(message,
                                    title: "Confirm",
                                    confirmingText: "Yes",
                                    dismissiveText: "No"));
            if (result)
            {
                await viewModel.siteService.DeleteAsync(viewModel.Site);
                await Navigation.PopAsync();
            }
        }

        private async void CheckLocation_Clicked(object sender, EventArgs e)
        {
            Position copyCurrentLocation = new Position(CurrentLocation.Latitude, CurrentLocation.Longitude);
            MapSpan mapSpan = new MapSpan(copyCurrentLocation, DEGREES, DEGREES);

            Pin pin = new Pin()
            {
                Label = "Site Location" + Environment.NewLine +
                        $"Lat: {CurrentLocation.Latitude}" + Environment.NewLine +
                        $"Long: {CurrentLocation.Longitude}",
                Type = PinType.Generic,
                Position = copyCurrentLocation
            };

            Map = new Xamarin.Forms.Maps.Map(mapSpan)
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 300.0,
                WidthRequest = 300.0
            };
            Map.Pins.Add(pin);
            Map.MapClicked += View_MapClicked;

            var result = await MaterialDialog.Instance.ShowCustomContentAsync(Map, "Change Location", null, "Update", "Cancel");
            if (result == true && Map.Pins.Count == 1)
            {
                CurrentLocation = new Location()
                {
                    Latitude = Map.Pins[0].Position.Latitude,
                    Longitude = Map.Pins[0].Position.Longitude
                };
                LocationChanged = true;
                UpdateCurrentLocation();
            }
        }

        private void View_MapClicked(object sender, MapClickedEventArgs e)
        {
            Map.Pins.Clear();

            Map.Pins.Add(new Pin()
            {
                Label = "Site Location" + Environment.NewLine +
                        $"Lat: {e.Position.Latitude}" + Environment.NewLine +
                        $"Long: {e.Position.Longitude}",
                Type = PinType.Generic,
                Position = e.Position
            });
        }

        private void UpdateCurrentLocation()
        {
            if (LocationChanged)
            {
                viewModel.Site.Latitude = CurrentLocation.Latitude.ToString();
                viewModel.Site.Longitude = CurrentLocation.Longitude.ToString();
                viewModel.Site.MinimumElevationInMeters = CurrentLocation.Altitude.ToString();
                viewModel.Site.CoordinateUncertaintyInMeters = CurrentLocation.Accuracy.ToString();
                LocationChanged = false;
            }
            latLbl.Text = $"Latitude: {CurrentLocation.Latitude.ToString()}";
            lngLbl.Text = $"Longitude: {CurrentLocation.Longitude.ToString()}";
            altLbl.Text = $"Altitude: {CurrentLocation.Altitude.ToString()}";
            accLbl.Text = $"Accuracy: {CurrentLocation.Accuracy.ToString()}";
        }
    }
}