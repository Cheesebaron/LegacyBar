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

using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using LegacyBar.Library.BarActions;
using LegacyBar.Library.BarBase;

namespace LegacyBar.Sample
{
    [Activity(Label = "SpinnerActivity", MainLauncher = false, LaunchMode = LaunchMode.SingleTop,
        Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    internal class SpinnerActivity : LegacyBarActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            var theme = (LegacyBarTheme)Intent.GetIntExtra("Theme", 0);
            switch (theme)
            {
                case LegacyBarTheme.HoloBlack:
                    SetContentView(Resource.Layout.black_theme);
                    break;
                case LegacyBarTheme.HoloBlue:
                    SetContentView(Resource.Layout.blue_theme);
                    break;
                case LegacyBarTheme.HoloGray:
                    SetContentView(Resource.Layout.gray_theme);
                    break;
                case LegacyBarTheme.HoloLight:
                    SetContentView(Resource.Layout.light_theme);
                    break;
                default:
                    SetContentView(Resource.Layout.other);
                    break;
            }
            
            //First we will specify the menu we are using.
            MenuId = Resource.Menu.othermainmenu;

            LegacyBar = FindViewById<Library.Bar.LegacyBar>(Resource.Id.actionbar);

            // You can also assign the title programmatically by passing a
            // CharSequence or resource id.
            LegacyBar.Title = "Other Activity";

            //Set the Up button to go home, also much set current activity on the Legacy Bar
            AddHomeAction(typeof (HomeActivity), Resource.Drawable.icon);

            //always show the search icon no matter what.
            var itemActionBarAction = new LegacyBarAction
            {
                ActionType = ActionType.Always,
                Drawable = LegacyBar.LightIcons
                               ? Resource.Drawable.ic_action_search
                               : Resource.Drawable.ic_action_search_dark,
                PopUpMessage = Resource.String.ab_string_search
            };
            LegacyBar.AddAction(itemActionBarAction);

            var bottomActionBar = FindViewById<Library.Bar.LegacyBar>(Resource.Id.bottomActionbar);

            var action = new LegacyBarAction
            {
                ActionType = ActionType.Always,
                Drawable =
                    bottomActionBar.LightIcons
                        ? Resource.Drawable.ic_action_up
                        : Resource.Drawable.ic_action_up_dark,
                PopUpMessage = Resource.String.menu_string_up
            };
            bottomActionBar.AddAction(action);

            action = new LegacyBarAction
            {
                ActionType = ActionType.Always,
                Drawable =
                    bottomActionBar.LightIcons
                        ? Resource.Drawable.ic_action_down
                        : Resource.Drawable.ic_action_down_dark,
                PopUpMessage = Resource.String.menu_string_down
            };
            bottomActionBar.AddAction(action);

            action = new LegacyBarAction
            {
                ActionType = ActionType.Always,
                Drawable =
                    bottomActionBar.LightIcons
                        ? Resource.Drawable.ic_action_left
                        : Resource.Drawable.ic_action_left_dark,
                PopUpMessage = Resource.String.menu_string_left
            };
            bottomActionBar.AddAction(action);

            action = new LegacyBarAction
            {
                ActionType = ActionType.Always,
                Drawable =
                    bottomActionBar.LightIcons
                        ? Resource.Drawable.ic_action_right
                        : Resource.Drawable.ic_action_right_dark,
                PopUpMessage = Resource.String.menu_string_right
            };
            bottomActionBar.AddAction(action);

            LegacyBar.SetDropDown(this, new[] { "My First Account", "My Second Account", "My Third Account" }, DropDownSelected);
        }

        private void DropDownSelected(object sender, AdapterView.ItemSelectedEventArgs args)
        {
            RunOnUiThread(() => Toast.MakeText(this, "You selected account: " + args.Position, ToastLength.Short).Show());
        }
    }
}