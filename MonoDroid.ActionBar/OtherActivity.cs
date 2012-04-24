/*
 * Copyright (C) 2012 Tomasz Cielecki <tomasz@ostebaronen.dk>
 * 
 * Port from https://github.com/johannilsson/android-actionbar
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 *  Addition by: Copyright (C) 2012 James Montemagno (motz2k1@oh.rr.com)
 */

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
    [Activity(Label = "OtherActivity", Theme = "@android:style/Theme.Black.NoTitleBar")]
    class OtherActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Other);

            ActionBar actionBar = FindViewById<ActionBar>(Resource.Id.actionbar);
            // You can also assign the title programmatically by passing a
            // CharSequence or resource id.
            actionBar.SetTitle(Resource.String.some_title);
            var homeIntent = new Intent(this, typeof (HomeActivity));
            homeIntent.AddFlags(ActivityFlags.ClearTop);
            homeIntent.AddFlags(ActivityFlags.NewTask);
            actionBar.SetHomeAction(new MyActionBarAction(this, homeIntent, Resource.Drawable.ic_title_home_default));
            actionBar.SetDisplayHomeAsUpEnabled(true);
            actionBar.AddAction(new MyActionBarAction(this, createShareIntent(), Resource.Drawable.ic_title_share_default));
        }

        private Intent createShareIntent() {
            Intent intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraText, "Shared from the ActionBar widget.");
            return Intent.CreateChooser(intent, "Share");
        }
    }
}