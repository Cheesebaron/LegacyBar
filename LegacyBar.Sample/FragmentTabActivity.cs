/*
 * 
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
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Java.Lang;
using LegacyBar.Library.Adapters;
using LegacyBar.Library.BarActions;
using LegacyBar.Library.BarBase;

namespace LegacyBar.Sample
{
    [Activity(Label = "Fragment Demo", MainLauncher = false, LaunchMode = LaunchMode.SingleTop,
        Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class FragmentTabActivity : LegacyBarFragmentActivity
    {
        private TabHost _tabHost;
        private TabsAdapter _tabsAdapter;
        private ViewPager _viewPager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            MenuId = Resource.Menu.mainmenu;

            SetContentView(Resource.Layout.fragment_tabs);
            _tabHost = FindViewById<TabHost>(Android.Resource.Id.TabHost);
            _tabHost.Setup();

            _viewPager = FindViewById<ViewPager>(Resource.Id.pager);

            _tabsAdapter = new TabsAdapter(this, _tabHost, _viewPager);


            LegacyBar = FindViewById<Library.Bar.LegacyBar>(Resource.Id.actionbar);
            LegacyBar.Title = "Look Fragments";
            AddHomeAction(typeof (HomeActivity), Resource.Drawable.icon);


            var action = new MenuItemLegacyBarAction(this, this, Resource.Id.menu_search,
                                                     Resource.Drawable.ic_action_search_dark,
                                                     Resource.String.menu_string_search);
            LegacyBar.AddAction(action);


            TabHost.TabSpec spec = _tabHost.NewTabSpec("tv");
            spec.SetIndicator("Tab 1", Resources.GetDrawable(Resource.Drawable.icon));
            _tabsAdapter.AddTab(spec, Class.FromType(typeof (FramgmentTab1)), null);


            spec = _tabHost.NewTabSpec("tab2");
            spec.SetIndicator("Tab 2", Resources.GetDrawable(Resource.Drawable.icon));
            _tabsAdapter.AddTab(spec, Class.FromType(typeof (FramgmentTab2)), null);

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