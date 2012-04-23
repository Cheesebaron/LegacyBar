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
 */

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MonoDroid.ActionBarSample
{
    [Activity(Label = "Action Bar", MainLauncher = true, Icon = "@drawable/icon")]
    public class HomeActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ActionBar actionBar = FindViewById<ActionBar>(Resource.Id.actionbar);
            actionBar.SetTitle("BingBong");

            ActionBarAction shareAction = new MyActionBarAction(this, createShareIntent(), Resource.Drawable.ic_title_share_default);
            actionBar.AddAction(shareAction);
            ActionBarAction otherAction = new MyActionBarAction(this, new Intent(this, typeof(OtherActivity)), Resource.Drawable.ic_title_export_default);
            actionBar.AddAction(otherAction);

            Button startProgress = FindViewById<Button>(Resource.Id.start_progress);
            startProgress.Click += (s, e) =>
            {
                actionBar.SetProgressBarVisibility(ViewStates.Visible);
            };

            Button stopProgress = FindViewById<Button>(Resource.Id.stop_progress);
            stopProgress.Click += (s, e) =>
            {
                actionBar.SetProgressBarVisibility(ViewStates.Gone);
            };

            Button removeActions = FindViewById<Button>(Resource.Id.remove_actions);
            removeActions.Click += (s, e) =>
            {
                actionBar.RemoveAllActions();
            };

            Button removeShareAction = FindViewById<Button>(Resource.Id.remove_share_action);
            removeShareAction.Click += (s, e) =>
            {
                actionBar.RemoveAction(shareAction);
            };
            
            Button addAction = FindViewById<Button>(Resource.Id.add_action);
            addAction.Click += (s, e) =>
            {
                MyOtherActionBarAction action = new MyOtherActionBarAction(this, null, Resource.Drawable.ic_title_share_default);
                actionBar.AddAction(action);
            };

            Button removeAction = FindViewById<Button>(Resource.Id.remove_action);
            removeAction.Click += (s, e) =>
            {
                actionBar.RemoveActionAt(actionBar.ActionCount - 1);
                Toast.MakeText(this, "Removed action.", ToastLength.Short).Show();
            };
        }

        private class MyOtherActionBarAction : ActionBarAction
        {
            public MyOtherActionBarAction(Context context, Intent intent, int drawable)
            {
                mDrawable = drawable;
                mContext = context;
                mIntent = intent;
            }

            public override int GetDrawable()
            {
                return mDrawable;
            }

            public override void PerformAction(View view)
            {
                Toast.MakeText(mContext, "Added action", ToastLength.Short).Show();
            }
        }


        private Intent createShareIntent() {
            Intent intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraText, "Shared from the ActionBar widget.");
            return Intent.CreateChooser(intent, "Share");
        }
    }
}

