/*
 * Original (https://github.com/johannilsson/android-actionbar) Ported to Mono for Android
 * Copyright (C) 2012 Tomasz Cielecki <tomasz@ostebaronen.dk>
 * 
 * Modified by James Montemagno Copyright 2012 http://www.montemagno.com
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
using LegacyBar.Library.BarActions;
using LegacyBar.Library.BarBase;

namespace LegacyBar.Sample
{
    [Activity(Label = "OtherActivity", MainLauncher = false, LaunchMode = LaunchMode.SingleTop,
        Icon = "@drawable/ic_launcher", Theme = "@style/MyTheme")]
    internal class OtherActivity : LegacyBarActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.other);

            MenuId = Resource.Menu.mainmenu;

            ActionBar = FindViewById<Library.Bar.LegacyBar>(Resource.Id.actionbar);
            // You can also assign the title programmatically by passing a
            // CharSequence or resource id.
            ActionBar.Title = "Other Activity";
            AddHomeAction(typeof (HomeActivity));
            ActionBar.CurrentActivity = this;

            var itemActionBarAction = new MenuItemLegacyBarAction(
                this, this, Resource.Id.menu_search,
                Resource.Drawable.ic_action_search_dark,
                Resource.String.menu_string_search)
                                          {
                                              ActionType = ActionType.Always
                                          };
            ActionBar.AddAction(itemActionBarAction);

            itemActionBarAction = new MenuItemLegacyBarAction(
                this, this, Resource.Id.menu_refresh,
                Resource.Drawable.ic_action_refresh_dark,
                Resource.String.menu_string_refresh)
                                      {ActionType = ActionType.Never};
            ActionBar.AddAction(itemActionBarAction);

            var bottomActionBar = FindViewById<Library.Bar.LegacyBar>(Resource.Id.bottomActionbar);

            var action = new MenuItemLegacyBarAction(this, this, Resource.Id.menu_up, Resource.Drawable.ic_action_up,
                                                     Resource.String.menu_string_down)
                             {
                                 ActionType = ActionType.Always
                             };

            bottomActionBar.AddAction(action);
            action = new MenuItemLegacyBarAction(this, this, Resource.Id.menu_down, Resource.Drawable.ic_action_down,
                                                 Resource.String.menu_string_down)
                         {
                             ActionType = ActionType.Always
                         };
            bottomActionBar.AddAction(action);

            action = new MenuItemLegacyBarAction(this, this, Resource.Id.menu_left, Resource.Drawable.ic_action_left,
                                                 Resource.String.menu_string_left)
                         {
                             ActionType = ActionType.Always
                         };
            bottomActionBar.AddAction(action);

            action = new MenuItemLegacyBarAction(this, this, Resource.Id.menu_right, Resource.Drawable.ic_action_right,
                                                 Resource.String.menu_string_right)
                         {
                             ActionType = ActionType.Always
                         };
            bottomActionBar.AddAction(action);
        }
    }
}