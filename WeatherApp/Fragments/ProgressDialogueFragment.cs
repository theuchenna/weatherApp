
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace WeatherApp.Fragments
{
    public class ProgressDialogueFragment : Android.Support.V4.App.DialogFragment
    {

        string status;
        public ProgressDialogueFragment(string mStatus)
        {
            status = mStatus;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.progress, container, false);
            TextView statusText = (TextView)view.FindViewById(Resource.Id.progressStatus);

            statusText.Text = status;
            return view;
        }
    }
}
