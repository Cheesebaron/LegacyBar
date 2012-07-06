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
using Android.OS;

namespace MonoDroid.ActionBarSample
{
    [Activity(Label = "OtherActivity", MainLauncher = false, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    class OtherActivity : ActionBarActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            
            SetContentView(Resource.Layout.Other);

            DarkMenuId = Resource.Menu.MainMenu;//could add a dark menu if you are in dark theme for icons.
            MenuId = Resource.Menu.MainMenu;

            ActionBar = FindViewById<ActionBar>(Resource.Id.actionbar);
            // You can also assign the title programmatically by passing a
            // CharSequence or resource id.
            ActionBar.SetTitle("Other Activity");
            AddHomeAction();
            ActionBar.CurrentActivity = this;

            var itemActionBarAction = new MenuItemActionBarAction(
                this, this, Resource.Id.menu_search,
                Resource.Drawable.ic_action_search_dark,
                Resource.String.menu_string_search)
                                          {
                                              ActionType = ActionType.Always
                                          };
            ActionBar.AddAction(itemActionBarAction);

            itemActionBarAction = new MenuItemActionBarAction(
                this, this, Resource.Id.menu_refresh,
                Resource.Drawable.ic_action_refresh_dark,
                Resource.String.menu_string_refresh)
                                      {ActionType = ActionType.Never};
            ActionBar.AddAction(itemActionBarAction);

            var bottomActionBar = FindViewById<BottomActionBar>(Resource.Id.bottomActionbar);

            var action = new MenuItemActionBarAction(this, this, Resource.Id.menu_up, Resource.Drawable.ic_action_up, Resource.String.menu_string_down);
            bottomActionBar.AddAction(action);
            action = new MenuItemActionBarAction(this, this, Resource.Id.menu_down, Resource.Drawable.ic_action_down, Resource.String.menu_string_down);
            bottomActionBar.AddAction(action);
            action = new MenuItemActionBarAction(this, this, Resource.Id.menu_left, Resource.Drawable.ic_action_left, Resource.String.menu_string_left);
            bottomActionBar.AddAction(action);
            action = new MenuItemActionBarAction(this, this, Resource.Id.menu_right, Resource.Drawable.ic_action_right, Resource.String.menu_string_right);
            bottomActionBar.AddAction(action);

        }
    }
}