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
using LegacyBar.Library.BarActions;
using LegacyBar.Library.BarBase;

namespace LegacyBar.Sample
{
    [Activity(Label = "OtherActivity", MainLauncher = false, LaunchMode = LaunchMode.SingleTop,
        Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    internal class OtherActivity : LegacyBarActivity
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
            LegacyBar.CurrentActivity = this;

            //always show the search icon no matter what.
            var itemActionBarAction = new MenuItemLegacyBarAction(
                this, this, Resource.Id.menu_search, LegacyBar.LightIcons ? Resource.Drawable.ic_action_search : Resource.Drawable.ic_action_search_dark,
                Resource.String.menu_string_search)
                                          {
                                              ActionType = ActionType.Always
                                          };
            LegacyBar.AddAction(itemActionBarAction);


            //the rest of them I will say NEVER show. now on devices with a menu button you can press it and it will show old menus with the icon you specifies in the menu.xml file
            //on newer devices without a menu button an overflow will appear.
            itemActionBarAction = new MenuItemLegacyBarAction(
                this, this, Resource.Id.menu_refresh, LegacyBar.LightIcons ? Resource.Drawable.ic_action_refresh :
                Resource.Drawable.ic_action_refresh_dark,
                Resource.String.menu_string_refresh)
                                      {ActionType = ActionType.Never};
            LegacyBar.AddAction(itemActionBarAction);

            itemActionBarAction = new MenuItemLegacyBarAction(
               this, this, Resource.Id.menu_test1, LegacyBar.LightIcons ? Resource.Drawable.ic_action_refresh :
               Resource.Drawable.ic_action_refresh_dark,
               Resource.String.menu_string_refresh) { ActionType = ActionType.Never };
            LegacyBar.AddAction(itemActionBarAction);

            itemActionBarAction = new MenuItemLegacyBarAction(
              this, this, Resource.Id.menu_test2, LegacyBar.LightIcons ? Resource.Drawable.ic_action_refresh :
              Resource.Drawable.ic_action_refresh_dark,
              Resource.String.menu_string_refresh) { ActionType = ActionType.Never };
            LegacyBar.AddAction(itemActionBarAction);

            itemActionBarAction = new MenuItemLegacyBarAction(
              this, this, Resource.Id.menu_test3, LegacyBar.LightIcons ? Resource.Drawable.ic_action_refresh :
              Resource.Drawable.ic_action_refresh_dark,
              Resource.String.menu_string_refresh) { ActionType = ActionType.Never };
            LegacyBar.AddAction(itemActionBarAction);

            var bottomActionBar = FindViewById<Library.Bar.LegacyBar>(Resource.Id.bottomActionbar);

            var action = new MenuItemLegacyBarAction(this, this, Resource.Id.menu_up, bottomActionBar.LightIcons ? Resource.Drawable.ic_action_up : Resource.Drawable.ic_action_up_dark,
                                                     Resource.String.menu_string_down)
                             {
                                 ActionType = ActionType.Always
                             };

            bottomActionBar.AddAction(action);
            action = new MenuItemLegacyBarAction(this, this, Resource.Id.menu_down, bottomActionBar.LightIcons ? Resource.Drawable.ic_action_down : Resource.Drawable.ic_action_down_dark,
                                                 Resource.String.menu_string_down)
                         {
                             ActionType = ActionType.Always
                         };
            bottomActionBar.AddAction(action);

            action = new MenuItemLegacyBarAction(this, this, Resource.Id.menu_left, bottomActionBar.LightIcons ? Resource.Drawable.ic_action_left : Resource.Drawable.ic_action_left_dark,
                                                 Resource.String.menu_string_left)
                         {
                             ActionType = ActionType.Always
                         };
            bottomActionBar.AddAction(action);

            action = new MenuItemLegacyBarAction(this, this, Resource.Id.menu_right, bottomActionBar.LightIcons ? Resource.Drawable.ic_action_right : Resource.Drawable.ic_action_right_dark,
                                                 Resource.String.menu_string_right)
                         {
                             ActionType = ActionType.Always
                         };
            bottomActionBar.AddAction(action);
        }
    }
}