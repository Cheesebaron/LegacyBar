/*
 * Copyright (C) 2013 LegacyBar - @Cheesebaron & @JamesMontemagno
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
 */

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using LegacyBar.Library.BarActions;
using LegacyBar.Library.BarBase;

namespace LegacyBar.Sample
{
    [Activity(Label = "TestHomeActionActivity", MainLauncher = false, LaunchMode = LaunchMode.SingleTop,
        Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    internal class TestHomeActionActivity : LegacyBarActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.gray_theme);
            
            //First we will specify the menu we are using.
            MenuId = Resource.Menu.othermainmenu;

            LegacyBar = FindViewById<Library.Bar.LegacyBar>(Resource.Id.actionbar);

            // You can also assign the title programmatically by passing a
            // CharSequence or resource id.
            LegacyBar.Title = "Test HomeAction Activity";

            //Set the Up button to go home, also much set current activity on the Legacy Bar
            AddHomeAction(typeof (HomeActivity), Resource.Drawable.icon);

            var homeAction = new LegacyBarAction
            {
                Drawable = Resource.Drawable.icon
            };

            homeAction.Clicked += (s, e) => Toast.MakeText(this, "Home Action was pressed", ToastLength.Short).Show();

            LegacyBar.SetHomeAction(homeAction);
        }
    }
}