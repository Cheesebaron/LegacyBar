/*
 * Copyright (C) 2012 James Montemagno (http://www.montemagno.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *    
 *          http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/
using Android.App;
using Android.Content;
using Android.Views;

namespace MonoDroid.ActionBarSample
{
    [Activity(Label = "")]
    public class ActionBarActivity : Activity
    {
        public ActionBar ActionBar { get; set; }
        public int DarkMenuId { get; set; }
        public int MenuId { get; set; }
        public bool IsDarkTheme { get; set; }


        protected override void OnStart()
        {
            base.OnStart();
            //FlurryAgent.OnStartSession(this, App.FlurryApiKeyValue);
        }

        protected override void OnStop()
        {
            base.OnStop();
            //FlurryAgent.OnEndSession(this);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            if(ActionBar == null)
                return base.OnPrepareOptionsMenu(menu);

            menu.Clear();
            MenuInflater.Inflate(IsDarkTheme ? DarkMenuId : MenuId, menu);

            for (int i = 0; i < menu.Size(); i++)
            {
                var menuItem = menu.GetItem(i);
                menuItem.SetVisible(!ActionBar.MenuItemsToHide.Contains(menuItem.ItemId));
            }
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(IsDarkTheme ? DarkMenuId : MenuId, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public void AddHomeAction()
        {
            var homeIntent = new Intent(this, typeof(HomeActivity));
            homeIntent.AddFlags(ActivityFlags.ClearTop);
            homeIntent.AddFlags(ActivityFlags.NewTask);
            ActionBar.SetHomeAction(new MyActionBarAction(this, homeIntent, Resource.Drawable.ic_title_home_default));
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }
    }
}