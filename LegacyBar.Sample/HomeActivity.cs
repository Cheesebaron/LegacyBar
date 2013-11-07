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
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using LegacyBar.Library.BarActions;
using LegacyBar.Library.BarBase;

namespace LegacyBar.Sample
{
    [Activity(Label = "LegacyBar Sample", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
        Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class HomeActivity : LegacyBarActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.main);

            LegacyBar = FindViewById<Library.Bar.LegacyBar>(Resource.Id.actionbar);
            LegacyBar.SetHomeLogo(Resource.Drawable.icon);

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
            MenuId = Resource.Menu.mainmenu;

            var shareAction = new LegacyBarAction
                {
                    ActionType = ActionType.Always,
                    Drawable =
                        LegacyBar.LightIcons
                            ? Resource.Drawable.ic_action_share
                            : Resource.Drawable.ic_action_share_dark,
                    PopUpMessage = Resource.String.ab_string_share
                };
            shareAction.Clicked += delegate { StartActivity(CreateShareIntent()); };
            LegacyBar.AddAction(shareAction);

            var otherAction = new LegacyBarAction
                {
                    ActionType = ActionType.Always,
                    Drawable =
                        LegacyBar.LightIcons
                            ? Resource.Drawable.ic_action_share
                            : Resource.Drawable.ic_action_share_dark,
                    PopUpMessage = Resource.String.ab_string_other
                };
            otherAction.Clicked += delegate { StartActivity(new Intent(this, typeof (OtherActivity))); };
            LegacyBar.AddAction(otherAction);

            //only put in if there is room
            var searchMenuItemAction = new LegacyBarAction
                {
                    ActionType = ActionType.IfRoom,
                    Drawable =
                        LegacyBar.LightIcons
                            ? Resource.Drawable.ic_action_search
                            : Resource.Drawable.ic_action_search_dark,
                    PopUpMessage = Resource.String.ab_string_search
                };
            LegacyBar.AddAction(searchMenuItemAction);

            //never put this guy in there
            searchMenuItemAction = new LegacyBarAction
            {
                ActionType = ActionType.Never,
                Drawable =
                    LegacyBar.LightIcons
                        ? Resource.Drawable.ic_action_search
                        : Resource.Drawable.ic_action_search_dark,
                PopUpMessage = Resource.String.ab_string_search
            };
            LegacyBar.AddAction(searchMenuItemAction);

            var toggleProgress = FindViewById<Button>(Resource.Id.toggle_progress);
            toggleProgress.Click += (s, e) =>
            {
                LegacyBar.ProgressBarVisibility = LegacyBar.ProgressBarVisibility == ViewStates.Visible ? ViewStates.Gone : ViewStates.Visible;
            };

            var removeActions = FindViewById<Button>(Resource.Id.remove_actions);
            removeActions.Click += (s, e) => LegacyBar.RemoveAllActions();

            var removeShareAction = FindViewById<Button>(Resource.Id.remove_share_action);
            removeShareAction.Click += (s, e) => LegacyBar.RemoveAction(shareAction);

            var addAction = FindViewById<Button>(Resource.Id.add_action);
            addAction.Click += (s, e) =>
                                   {
                                       var action = new LegacyBarAction
                                       {
                                           ActionType = ActionType.Never,
                                           Drawable =
                                               LegacyBar.LightIcons 
                                                   ? Resource.Drawable.ic_action_share 
                                                   : Resource.Drawable.ic_action_share_dark,
                                           PopUpMessage = Resource.String.ab_string_share
                                       };
                                       LegacyBar.AddAction(action);
                                   };

            var removeAction = FindViewById<Button>(Resource.Id.remove_action);
            removeAction.Click += (s, e) =>
                                      {
                                          LegacyBar.RemoveActionAt(LegacyBar.ActionCount - 1);
                                          Toast.MakeText(this, "Removed legacyBarAction.", ToastLength.Short).Show();
                                      };

            var otherActivity = FindViewById<Button>(Resource.Id.other_activity);
            otherActivity.Click += (s, e) =>
                                       {
                                           var intent = new Intent(this, typeof (OtherActivity));
                                           StartActivity(intent);
                                       };

            var black = FindViewById<Button>(Resource.Id.black_activity);
            black.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(OtherActivity));
                intent.PutExtra("Theme", (int)LegacyBarTheme.HoloBlack);
                StartActivity(intent);
            };

            var blue = FindViewById<Button>(Resource.Id.blue_activity);
            blue.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(OtherActivity));
                intent.PutExtra("Theme", (int)LegacyBarTheme.HoloBlue);
                StartActivity(intent);
            };

            var light = FindViewById<Button>(Resource.Id.light_activity);
            light.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(OtherActivity));
                intent.PutExtra("Theme", (int)LegacyBarTheme.HoloLight);
                StartActivity(intent);
            };

            var gray = FindViewById<Button>(Resource.Id.gray_activity);
            gray.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(OtherActivity));
                intent.PutExtra("Theme", (int) LegacyBarTheme.HoloGray);
                StartActivity(intent);
            };


            var fragmentActivity = FindViewById<Button>(Resource.Id.fragment_activity);
            fragmentActivity.Click += (s, e) =>
                                          {
                                              var intent = new Intent(this, typeof (FragmentTabActivity));
                                              StartActivity(intent);
                                          };

            var spinnerActivity = FindViewById<Button>(Resource.Id.dropdown_activity);
            spinnerActivity.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(SpinnerActivity));
                StartActivity(intent);
            };

            var testHomeActionActivity = FindViewById<Button>(Resource.Id.test_home_action_activity);
            testHomeActionActivity.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(TestHomeActionActivity));
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
    }
}