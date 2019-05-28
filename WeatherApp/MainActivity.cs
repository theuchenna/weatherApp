using System;
using System.Net.Http;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.IO;
using Android.Graphics;
using WeatherApp.Fragments;

namespace WeatherApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button getWeatherButton;
        TextView placeText;
        TextView temperatureText;
        TextView weatherDescriptionText;
        EditText cityNameText;
        ImageView weatherImage;

        ProgressDialogueFragment progressDialogue;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            cityNameText = (EditText)FindViewById(Resource.Id.cityNameText);
            temperatureText = (TextView)FindViewById(Resource.Id.temperatureText);
            placeText = (TextView)FindViewById(Resource.Id.placeText);
            weatherDescriptionText = (TextView)FindViewById(Resource.Id.weatherDescriptionText);
            weatherImage = (ImageView)FindViewById(Resource.Id.weatherImage);
            getWeatherButton = (Button)FindViewById(Resource.Id.getWeatherButton);
            getWeatherButton.Click += GetWeatherButton_Click;

            GetWeather("London");
        }


        void GetWeatherButton_Click(object sender, EventArgs e)
        {
            string place = cityNameText.Text;
            GetWeather(place);
            cityNameText.Text = "";
        }

        async void GetWeather(string CityName)
        {
            string appID = "869a1bfdfb679bdbbbf37f4fac4dd3f1";
            string units = "metric";
            string apibase = "https://api.openweathermap.org/data/2.5/weather?q=";


            if (string.IsNullOrEmpty(CityName))
            {
                Toast.MakeText(this, "Please enter a valid city name", ToastLength.Short).Show();
                return;
            }

            ShowProgressDialogue("Fetching Weather Data...", false);

            string url = apibase + CityName + "&appid=" + appID + "&units=" + units;

            var handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            string result = await client.GetStringAsync(url);

            Console.WriteLine(result);

            var resultObject = JObject.Parse(result);
            string WeatherDescription = resultObject["weather"][0]["description"].ToString();
            string icon = resultObject["weather"][0]["icon"].ToString();
            string temperature = resultObject["main"]["temp"].ToString();
            string placeAddress = resultObject["name"].ToString() + ", " + resultObject["sys"]["country"].ToString();
            WeatherDescription = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(WeatherDescription);

            temperatureText.Text = temperature;
            placeText.Text = placeAddress;
            weatherDescriptionText.Text = WeatherDescription;


            // Download Image Using WebRequest

            string imageUrl = "http://openweathermap.org/img/w/" + icon + ".png";
            System.Net.WebRequest request = default(System.Net.WebRequest);
            request = System.Net.WebRequest.Create(imageUrl);
            request.Timeout = int.MaxValue;
            request.Method = "GET";

            System.Net.WebResponse response = default(System.Net.WebResponse);
            response = await request.GetResponseAsync();
            MemoryStream ms = new MemoryStream();
            response.GetResponseStream().CopyTo(ms);
            byte[] imageData = ms.ToArray();

            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            weatherImage.SetImageBitmap(bitmap);

            CloseProgressDialogue();

        }


        void ShowProgressDialogue(string status, bool cancelable)
        {
            progressDialogue = new ProgressDialogueFragment(status);
            var trans = SupportFragmentManager.BeginTransaction();
            progressDialogue.Cancelable = cancelable;
            progressDialogue.Show(trans, "dialogue");
        }

        void CloseProgressDialogue()
        {
            if (progressDialogue != null)
            {
                progressDialogue.Dismiss();
                progressDialogue = null;
            }
        }
    }
}

