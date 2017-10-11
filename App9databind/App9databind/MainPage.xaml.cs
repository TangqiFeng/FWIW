using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App9databind
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            // when the page is loaded, load the data
            //this.Loaded += MainPage_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // if the data is already loaded, then exit the method
            if (_myList != null)
                return;

            // else, load the data from a local source - Data/myDogs.txt
            // could load remote from server
            // instantiate the list - _myList
            _myList = new List<clsDogs>();
            loadLocalData();
            // list view shows a list of items
            // the source is the list of objects for dogs
            lvDogs.ItemsSource = _myList;

            // get the string from App.xaml.cs
            // get a reference/pointer to the instance
            // of the current app that is running
            App myapp = App.Current as App;
            tblTitle.Text = myapp.strHello;

        }

        #region Copy to App.xaml.cs file for global list.
        private async void loadLocalData()
        {
            /* &
             *  1.  get the file with the data
             *  2.  read the text to a JSON Array
             *  3.  parse the JSON Array and create the list of object
             */
            //1. (like: FILE *fptr;  fptr = fopen("myDogs.txt", "r");
            var dogsFile = await
                Package.Current.InstalledLocation.GetFileAsync("Data\\myDogs.txt");
            var fileText = await FileIO.ReadTextAsync(dogsFile);

            // now have a block of text in fileText
            // send that to a json array to start making sense

            try
            {
                var dogsJArray = JsonArray.Parse(fileText);
                createListOfDogs(dogsJArray);
                tblTitle.Text = _myList.Count().ToString() + " Dog Breeds";
            }
            catch (Exception exJA)
            {
                MessageDialog dialog = new MessageDialog(exJA.Message);
                await dialog.ShowAsync();
            }

        }

        private void createListOfDogs(JsonArray jsonData)
        {
            foreach (var item in jsonData)
            {
                // get the object
                var obj = item.GetObject();

                clsDogs dog = new clsDogs();

                // get each key value pair and sort it to the appropriate elements
                // of the class
                foreach (var key in obj.Keys)
                {
                    IJsonValue value;
                    if (!obj.TryGetValue(key, out value))
                        continue;

                    switch (key)
                    {
                        case "breed": // based on generic object key
                            dog.myBreedName = value.GetString();
                            break;
                        case "origin":
                            dog.origin = value.GetString();
                            break;
                        case "category":
                            dog.category = value.GetString();
                            break;
                        case "activity":
                            dog.activity = value.GetString();
                            break;
                        case "grooming":
                            dog.grooming = value.GetString();
                            break;
                        case "image":
                            dog.imgBreed = value.GetString();
                            break;
                    }
                } // end foreach (var key in obj.Keys)

                _myList.Add(dog);

            } // end foreach (var item in array)
        }

        #endregion
        List<clsDogs> _myList;

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StackPanel curr = (StackPanel)sender;
            // fill in the values on the picture etc
            tblOneBreed.Text = _myList[lvDogs.SelectedIndex].myBreedName;
            tblOneCategory.Text = _myList[lvDogs.SelectedIndex].category;
            tblOneOrigin.Text = _myList[lvDogs.SelectedIndex].origin;

            txtlOneBreed.Text = _myList[lvDogs.SelectedIndex].myBreedName;
            txtlOneCategory.Text = _myList[lvDogs.SelectedIndex].category;
            txtlOneOrigin.Text = _myList[lvDogs.SelectedIndex].origin;

            // get the picture
            // check for the file existing.
            // if( fileexists(_myList[lvdogs.SelectedIndex].imgSource )
            string fileString = "ms-appx:///" +  _myList[lvDogs.SelectedIndex].imgBreed; 
            if ( !File.Exists(_myList[lvDogs.SelectedIndex].imgBreed) )
            {
                fileString = "ms-appx:///Images/images.jpe";
            }
            
            // create a uri from the string in the class
            Uri myUri = new Uri(fileString,
                                UriKind.Absolute);
            // create a bitmap fromt he uri
            BitmapImage myBitmap = new BitmapImage(myUri);
            // use the bitmap as the source for the image.
            imgOneDog.Source = myBitmap;
            imgDog.Source = myBitmap;




        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Page2));
        }
    }
}
