using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RuntimeComponent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string backgroundTaskName = "test";
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void doStuff()
        {
            App myApp = Application.Current as App; // get current instance of the running application

            switch (myApp._inBackgroundMode)
            {
                case false:
                    break;

                case true:
                default:
                    break;
            }
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            // tag new task is/not existed 
            bool taskRegistered = false;
            // go through all tasks, if such one existed, change tag taskRegistered to false
            foreach (var t in BackgroundTaskRegistration.AllTasks)
            {
                if (t.Value.Name == "test")
                {
                    taskRegistered = true;
                    break;
                }
            }
            // if such task is not exist
            if (taskRegistered != true)
            {
                BackgroundAccessStatus access =
                    await BackgroundExecutionManager.RequestAccessAsync();
                switch (access)
                {   case BackgroundAccessStatus.Denied:
                    case BackgroundAccessStatus.DeniedBySystemPolicy:
                    case BackgroundAccessStatus.DeniedByUser:
                        break;
                }
                var builder = new BackgroundTaskBuilder();
                builder.Name = backgroundTaskName;
                // builder.TaskEntryPoint = typeof(myRuntimeComponent.MyBackgroundTask).ToString();
                // *.* *.* this step need create a RuntimeComponent project *.* *.*

                //in-process task
                //builder.SetTrigger(new TimeTrigger(15, false));
                var trigger = new ApplicationTrigger();                 //#.# same wffect as
                

                builder.SetTrigger(trigger);
                BackgroundTaskRegistration t = builder.Register();
                Debug.WriteLine("Task registered !");
                t.Completed += new
                BackgroundTaskCompletedEventHandler(OnCompleted);
                t.Progress += Task_Progress; // update user progress

                await trigger.RequestAsync();                           //#.# new TimeTrigger(15, false)
            }
            else
            {
                Debug.WriteLine("task exist");
            }
        }

        private void Task_Progress(BackgroundTaskRegistration sender, BackgroundTaskProgressEventArgs args)
        {
            var progress = "Progress: " + args.Progress + "%";
            Debug.WriteLine(progress);
        }

        private async void OnCompleted(IBackgroundTaskRegistration task, BackgroundTaskCompletedEventArgs e)
        {
            Debug.WriteLine("complete");
        }
    }
}
