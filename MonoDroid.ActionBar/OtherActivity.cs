<<<<<<< HEAD
=======
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
 *  Addition by: Copyright (C) 2012 James Montemagno (http://www.montemagno.com)
 */

>>>>>>> james/master
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
    [Activity(Label = "OtherActivity", Theme = "@style/MyTheme")]
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

            var itemActionBarAction = new MenuItemActionBarAction(this, this, Resource.Id.menu_search, Resource.Drawable.ic_action_search_dark, Resource.String.pop_up_sample);
            itemActionBarAction.ActionType = ActionType.Always;
            ActionBar.AddAction(itemActionBarAction);

            itemActionBarAction = new MenuItemActionBarAction(this, this, Resource.Id.menu_refresh, Resource.Drawable.ic_action_refresh_dark, Resource.String.menu_string_refresh);
            itemActionBarAction.ActionType = ActionType.Never;
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

        private Intent createShareIntent() {
            Intent intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraText, "Shared from the ActionBar widget.");
            return Intent.CreateChooser(intent, "Share");
        }
    }
}