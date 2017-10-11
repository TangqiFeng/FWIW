using App9databind.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace App9databind.ViewModels
{
    class DogViewModel : NotificationBase<clsDogs>
    {
        public DogViewModel(clsDogs dog = null) : base(dog) { }
        public String myBreedName
        {
            get { return This.myBreedName; }
            set { SetProperty(This.myBreedName, value, () => This.myBreedName = value); }
        }

        public String category
        {
            get { return This.category; }
            set { SetProperty(This.category, value, () => This.category = value); }
        }
        
        public String origin  
        {
            get { return This.origin; }
            set { SetProperty(This.origin, value, () => This.origin = value); }
        }

        public String imgBreed
        {
            get { return This.imgBreed; }
            set { SetProperty(This.origin, value, () => This.imgBreed = value); }
        }

        public BitmapImage source
        {
            get { return getSource(imgBreed); }
        }

        public BitmapImage getSource(String imgBreed)
        {
            // get the picture
            // check for the file existing.
            // if( fileexists(_myList[lvdogs.SelectedIndex].imgSource )
            string fileString = "ms-appx:///" + imgBreed;
            if (!File.Exists(imgBreed))
            {
                fileString = "ms-appx:///Images/images.jpe";
            }

            // create a uri from the string in the class
            Uri myUri = new Uri(fileString,
                                UriKind.Absolute);
            // create a bitmap fromt he uri
            BitmapImage myBitmap = new BitmapImage(myUri);
            // use the bitmap as the source for the image.
            return myBitmap;
        }
        
    }
}
