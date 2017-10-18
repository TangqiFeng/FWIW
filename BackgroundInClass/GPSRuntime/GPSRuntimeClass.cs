using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Storage;

namespace GPSRuntime
{
    public sealed class GPSRuntimeClass : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral = null;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var cost = BackgroundWorkCost.CurrentBackgroundWorkCost;
            var settings = ApplicationData.Current.LocalSettings;

            if (cost == BackgroundWorkCostValue.High)
            {
                // create a setting key for the foreground app to read.
                settings.Values["BackgroundCost"] = cost.ToString();
                return;
            }

            //handle the cancellation
            var cancel = new CancellationTokenSource(); // system.threading
            taskInstance.Canceled += (s, e) => { cancel.Cancel(); cancel.Dispose(); };

            settings.Values["Error"] = "";

            // now do what I need to do
            try
            {
                // get the GPS location and write to settings
                Geolocator geoLocator = new Geolocator();
                Geoposition geoPosition = await geoLocator.GetGeopositionAsync().AsTask();

                settings.Values["Latitude"] = geoPosition.Coordinate.Point.Position.Latitude; 
                settings.Values["Longitute"] = geoPosition.Coordinate.Point.Position.Longitude;
            }
            catch (Exception ex)
            {
                settings.Values["Error"] = ex.Message;
            }
            finally
            {
                _deferral.Complete();
            }



        }


    }
}
