using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MonoDroid.ActionBarSample
{
    class OtherActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Other);

            ActionBar actionBar = FindViewById<ActionBar>(Resource.Id.actionbar);
            // You can also assign the title programmatically by passing a
            // CharSequence or resource id.
            //actionBar.setTitle(R.string.some_title);
            actionBar.SetHomeAction(new ActionBarAction(this, new Intent(this.ApplicationContext, typeof(HomeActivity)), Resource.Drawable.ic_title_home_default));
            actionBar.SetDisplayHomeAsUpEnabled(true);
            actionBar.AddAction(new ActionBarAction(this, createShareIntent(), Resource.Drawable.ic_title_share_default));
        }

        private Intent createShareIntent() {
            Intent intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraText, "Shared from the ActionBar widget.");
            return Intent.CreateChooser(intent, "Share");
        }
    }
}