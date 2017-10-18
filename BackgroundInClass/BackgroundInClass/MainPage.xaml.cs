using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BackgroundInClass
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string backgroundTaskName = "GPSRuntime";
        private const string backgroundTaskEntryPoint = "GPSRuntimeClass.Run";

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // should contain task registration code.
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // register the background task and ask it to run every 15 minutes.
            var taskRegistered = false;

            // check if already there.
            // do the opposite to unregister = task.Value.Unregister()
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == backgroundTaskName)
                {
                    taskRegistered = true;
                    break;
                }
            }

            // not registered - build the task using the task builder
            // set up background TaskTrigger (time trigger) to run it.
            if (taskRegistered != true)
            {
                try
                {
                    BackgroundAccessStatus bckAccessSts =
                        await BackgroundExecutionManager.RequestAccessAsync();
                    // build the task
                    var builder = new BackgroundTaskBuilder();
                    builder.Name = backgroundTaskName;
                    builder.TaskEntryPoint = backgroundTaskEntryPoint;
                    builder.SetTrigger(new TimeTrigger(15, false));
                    BackgroundTaskRegistration myTask = builder.Register();
                    myTask.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);

                    switch (bckAccessSts)
                    {
                        case BackgroundAccessStatus.Unspecified:
                            tblStatus.Text = "Error Unspecified";
                            break;
                        case BackgroundAccessStatus.AlwaysAllowed:
                            tblStatus.Text = "Always Allowed";
                            RequestLocationAccess();
                            break;
                        case BackgroundAccessStatus.AllowedSubjectToSystemPolicy:
                            tblStatus.Text = "Allowed subject to system policy";
                            RequestLocationAccess();
                            break;
                        case BackgroundAccessStatus.DeniedBySystemPolicy:
                            tblStatus.Text = "Denied by system policy";
                            break;
                        case BackgroundAccessStatus.DeniedByUser:
                            tblStatus.Text = "Denied By User";
                            break;
                        default:
                            tblStatus.Text = "Error Accessing Background Tasks";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    tblStatus.Text = "Error: " + ex.Message;
                }
            } // end if( taskRegistered != true)
            else
            {
                tblStatus.Text = "Background Task already registered";
                RequestLocationAccess();
            }
        }

        private async void RequestLocationAccess()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Unspecified:
                    tblStatus.Text = "Location Unspecified Error";
                    break;
                case GeolocationAccessStatus.Allowed:
                    tblStatus.Text = "Location Access Allowed";
                    break;
                case GeolocationAccessStatus.Denied:
                    tblStatus.Text = "Location Access denied";
                    break;
                default:
                    break;
            }
        }

        private void OnCompleted(IBackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            // using windows.storage
            var settings = ApplicationData.Current.LocalSettings;
            tblLat.Text = settings.Values["Latitude"].ToString();
            tblLong.Text = settings.Values["Longitude"].ToString();

            tblStatus.Text = sender.TaskId.ToString();

        }

    }
}
