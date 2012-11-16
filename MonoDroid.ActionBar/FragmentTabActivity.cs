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

namespace MonoDroid.ActionBarSample
{
      [Activity(Label = "Fragment Demo", MainLauncher = false, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/ic_launcher", Theme = "@style/MyTheme")]
    public class FragmentTabActivity : ActionBarFragmentActivity
    {
        TabHost m_TabHost;
        ViewPager m_ViewPager;
        TabsAdapter m_TabsAdapter;

        protected override void OnCreate(Bundle bundle)
        {
            
            base.OnCreate(bundle);


            MenuId = Resource.Menu.MainMenu;

            SetContentView(Resource.Layout.fragment_tabs);
            m_TabHost = FindViewById<TabHost>(Android.Resource.Id.TabHost);
            m_TabHost.Setup();

            m_ViewPager = FindViewById<ViewPager>(Resource.Id.pager);

            m_TabsAdapter = new TabsAdapter(this, m_TabHost, m_ViewPager);


            ActionBar = FindViewById<ActionBar>(Resource.Id.actionbar);
            ActionBar.Title = "Look Fragments";
            ActionBar.CurrentActivity = this;
            AddHomeAction();
         

            var action = new MenuItemActionBarAction(this, this, Resource.Id.menu_search, Resource.Drawable.ic_action_search_dark, Resource.String.menu_string_search);
            ActionBar.AddAction(action);


            var spec = m_TabHost.NewTabSpec("tv");
            spec.SetIndicator("Tab 1", Resources.GetDrawable(Resource.Drawable.ic_launcher));
            m_TabsAdapter.AddTab(spec, Java.Lang.Class.FromType(typeof(FramgmentTab1)), null);


            spec = m_TabHost.NewTabSpec("tab2");
            spec.SetIndicator("Tab 2", Resources.GetDrawable(Resource.Drawable.ic_launcher));
            m_TabsAdapter.AddTab(spec, Java.Lang.Class.FromType(typeof(FramgmentTab2)), null);

            if (bundle != null)
            {
                m_TabHost.SetCurrentTabByTag(bundle.GetString("tab"));
            }
            else
            {

                m_TabHost.CurrentTab = 0;
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutString("tab", m_TabHost.CurrentTabTag);

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