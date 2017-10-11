using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Popups;

namespace App9databind.Data
{
    class clsDogs
    {
        public string myBreedName { get; set; }
        public string origin { get; set; }
        public string category { get; set; }
        public string activity { get; set; }
        public string grooming { get; set; }
        public string imgBreed { get; set; }
    }
    
    class service
    {
        private static List<clsDogs> _myList;
        public static List<clsDogs> GetData()
        {
            Debug.WriteLine("GET for people.");
            
            // instantiate the list - _myList
            _myList = new List<clsDogs>();
            loadLocalData();

            return _myList;
            // list view shows a list of items
            // the source is the list of objects for dogs
            //lvDogs.ItemsSource = _myList;
        }

        public static void Write(clsDogs dog)
        {
            Debug.WriteLine("INSERT person with name " + dog.myBreedName);
        }

        public static void Delete(clsDogs dog)
        {
            Debug.WriteLine("DELETE person with name " + dog.myBreedName);
        }

        #region get data from json
        private static async void loadLocalData()
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
                //tblTitle.Text = _myList.Count().ToString() + " Dog Breeds";
            }
            catch (Exception exJA)
            {
                MessageDialog dialog = new MessageDialog(exJA.Message);
                await dialog.ShowAsync();
            }

        }

        private static void createListOfDogs(JsonArray jsonData)
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
    }
}
