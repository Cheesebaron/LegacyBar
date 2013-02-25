/*
 * 
 * Copyright (C) 2012 Tomasz Cielecki <tomasz@ostebaronen.dk>
 * and James Montemagno Copyright 2012 http://www.montemagno.com
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
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using MonoDroid.ActionBar.Library;

namespace MonoDroid.ActionBar.Sample
{
    [Activity(Label = "Fragment Demo", MainLauncher = false, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/ic_launcher", Theme = "@style/MyTheme")]
    public class FragmentTabActivity : ActionBarFragmentActivity
    {
        TabHost _tabHost;
        ViewPager _viewPager;
        TabsAdapter _tabsAdapter;

        protected override void OnCreate(Bundle bundle)
        {
            
            base.OnCreate(bundle);


            MenuId = Resource.Menu.mainmenu;

            SetContentView(Resource.Layout.fragment_tabs);
            _tabHost = FindViewById<TabHost>(Android.Resource.Id.TabHost);
            _tabHost.Setup();

            _viewPager = FindViewById<ViewPager>(Resource.Id.pager);

            _tabsAdapter = new TabsAdapter(this, _tabHost, _viewPager);


			ActionBar = FindViewById<Library.ActionBar>(Resource.Id.actionbar);
            ActionBar.Title = "Look Fragments";
            ActionBar.CurrentActivity = this;
            AddHomeAction(typeof(HomeActivity));
         

            var action = new MenuItemActionBarAction(this, this, Resource.Id.menu_search, Resource.Drawable.ic_action_search_dark, Resource.String.menu_string_search);
            ActionBar.AddAction(action);


            var spec = _tabHost.NewTabSpec("tv");
            spec.SetIndicator("Tab 1", Resources.GetDrawable(Resource.Drawable.ic_launcher));
            _tabsAdapter.AddTab(spec, Java.Lang.Class.FromType(typeof(FramgmentTab1)), null);


            spec = _tabHost.NewTabSpec("tab2");
            spec.SetIndicator("Tab 2", Resources.GetDrawable(Resource.Drawable.ic_launcher));
            _tabsAdapter.AddTab(spec, Java.Lang.Class.FromType(typeof(FramgmentTab2)), null);

            if (bundle != null)
            {
                _tabHost.SetCurrentTabByTag(bundle.GetString("tab"));
            }
            else
            {

                _tabHost.CurrentTab = 0;
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutString("tab", _tabHost.CurrentTabTag);

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                
                case Resource.Id.menu_refresh:
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }

    public class FramgmentTab1 : Fragment
    {
        public override void OnCreate(Bundle p0)
        {
            base.OnCreate(p0);
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            var view = inflater.Inflate(Resource.Layout.simple_fragment, container, false);


            view.FindViewById<TextView>(Resource.Id.fragment_text).Text = "Fragment 1";
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

        }
    }

    public class FramgmentTab2 : Fragment
    {
        public override void OnCreate(Bundle p0)
        {
            base.OnCreate(p0);
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = inflater.Inflate(Resource.Layout.simple_fragment, container, false);
            view.FindViewById<TextView>(Resource.Id.fragment_text).Text = "Fragment 2";
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

        }
    }
}