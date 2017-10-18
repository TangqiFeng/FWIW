using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace myRuntimeComponent
{
    public sealed class MyBackgroundTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral = null;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //handle the cancellation
            var cancel = new CancellationTokenSource(); // system.threading
            taskInstance.Canceled += (s, e) => { cancel.Cancel(); cancel.Dispose(); };
            

            try
            {
                SendToast("Hi this is background Task");
            }catch (Exception ex)
            {
                
            }
            finally
            {
                //_deferral.Complete();
            }

        }

        public static void SendToast(string message)
        {
            var template = ToastTemplateType.ToastText01;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            var elements = xml.GetElementsByTagName("text");
            var text = xml.CreateTextNode(message);

            elements[0].AppendChild(text);
            var toast = new ToastNotification(xml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}

