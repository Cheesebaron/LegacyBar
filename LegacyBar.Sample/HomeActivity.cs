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
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using LegacyBar.Library.BarActions;
using LegacyBar.Library.BarBase;

namespace LegacyBar.Sample
{
    [Activity(Label = "Action Bar", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
        Icon = "@drawable/ic_launcher", Theme = "@style/MyTheme")]
    public class HomeActivity : LegacyBarActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.main);

            ActionBar = FindViewById<Library.Bar.LegacyBar>(Resource.Id.actionbar);
            ActionBar.CurrentActivity = this;
            ActionBar.SetHomeLogo(Resource.Drawable.ic_launcher);

            /*
             * You can also set the title of the LegacyBar with: 
             * LegacyBar.Title = "MyAwesomeTitle";
             * 
             * or
             * 
             * LegacyBar.Title = Resource.String.<yourStringId>;
             * 
             * Title Color can be set with:
             * LegacyBar.TitleColor = Color.Blue; //Or any other Color you want
             * 
             * The Separator between the Action Bar Items can be set with:
             * LegacyBar.SeparatorColor = Color.Blue;
             * 
             * and with a drawable:
             * 
             * LegacyBar.SeparatorDrawable = myDrawable;
             */

            //always put these 2 in there since they are NOT in my menu.xml
            LegacyBarAction shareAction = new DefaultLegacyBarAction(this, CreateShareIntent(),
                                                                     Resource.Drawable.ic_title_share_default)
                                              {
                                                  ActionType = ActionType.Always
                                              };
            ActionBar.AddAction(shareAction);


            var otherAction = new DefaultLegacyBarAction(this, new Intent(this, typeof (OtherActivity)),
                                                         Resource.Drawable.ic_title_export_default)
                                  {
                                      ActionType = ActionType.Always
                                  };
            ActionBar.AddAction(otherAction);

            //only put in if there is room
            var searchMenuItemAction = new MenuItemLegacyBarAction(
                this, this, Resource.Id.menu_search,
                Resource.Drawable.ic_action_search_dark,
                Resource.String.menu_string_search)
                                           {
                                               ActionType = ActionType.IfRoom
                                           };
            ActionBar.AddAction(searchMenuItemAction);

            //never put this guy in there
            searchMenuItemAction = new MenuItemLegacyBarAction(
                this, this, Resource.Id.menu_refresh,
                Resource.Drawable.ic_action_refresh_dark,
                Resource.String.menu_string_refresh)
                                       {
                                           ActionType = ActionType.Never
                                       };
            ActionBar.AddAction(searchMenuItemAction);

            var startProgress = FindViewById<Button>(Resource.Id.start_progress);
            startProgress.Click += (s, e) => ActionBar.ProgressBarVisibility = ViewStates.Visible;

            var stopProgress = FindViewById<Button>(Resource.Id.stop_progress);
            stopProgress.Click += (s, e) => ActionBar.ProgressBarVisibility = ViewStates.Gone;

            var removeActions = FindViewById<Button>(Resource.Id.remove_actions);
            removeActions.Click += (s, e) => ActionBar.RemoveAllActions();

            var removeShareAction = FindViewById<Button>(Resource.Id.remove_share_action);
            removeShareAction.Click += (s, e) => ActionBar.RemoveAction(shareAction);

            var addAction = FindViewById<Button>(Resource.Id.add_action);
            addAction.Click += (s, e) =>
                                   {
                                       var action = new MyOtherActionBarAction(this, null,
                                                                               Resource.Drawable.ic_title_share_default);
                                       ActionBar.AddAction(action);
                                   };

            var removeAction = FindViewById<Button>(Resource.Id.remove_action);
            removeAction.Click += (s, e) =>
                                      {
                                          ActionBar.RemoveActionAt(ActionBar.ActionCount - 1);
                                          Toast.MakeText(this, "Removed legacyBarAction.", ToastLength.Short).Show();
                                      };

            var otherActivity = FindViewById<Button>(Resource.Id.other_activity);
            otherActivity.Click += (s, e) =>
                                       {
                                           var intent = new Intent(this, typeof (OtherActivity));
                                           StartActivity(intent);
                                       };


            var fragmentActivity = FindViewById<Button>(Resource.Id.fragment_activity);
            fragmentActivity.Click += (s, e) =>
                                          {
                                              var intent = new Intent(this, typeof (FragmentTabActivity));
                                              StartActivity(intent);
                                          };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_search:
                    OnSearchRequested();
                    Toast.MakeText(this, "you pressed SEARCH!!!!", ToastLength.Short).Show();
                    return true;
                case Resource.Id.menu_refresh:
                    Toast.MakeText(this, "you pressed REFRESH!!!", ToastLength.Short).Show();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private static Intent CreateShareIntent()
        {
            var intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraText, "Shared from the LegacyBar widget.");
            return Intent.CreateChooser(intent, "Share");
        }

        #region Nested type: MyOtherActionBarAction

        private class MyOtherActionBarAction : LegacyBarAction
        {
            public MyOtherActionBarAction(Context context, Intent intent, int drawable)
            {
                Drawable = drawable;
                Context = context;
                Intent = intent;
            }

            public override int GetDrawable()
            {
                return Drawable;
            }

            public override void PerformAction(View view)
            {
                Toast.MakeText(Context, "Added legacyBarAction", ToastLength.Short).Show();
            }
        }

        #endregion
    }
}