/*
 * Copyright (C) 2010 Johan Nilsson <http://markupartist.com>
 *
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

using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Content.Res;

namespace MonoDroid.ActionBarSample
{
    public class ActionBar : RelativeLayout, View.IOnClickListener, View.IOnLongClickListener
    {
        private readonly LayoutInflater m_Inflater;
        private readonly RelativeLayout m_BarView;
        private readonly ImageView m_LogoView;
        private readonly View m_BackIndicator;
        private readonly TextView m_TitleView;
        private readonly LinearLayout m_ActionsView;
        private readonly ImageButton m_HomeBtn;
        private readonly RelativeLayout m_HomeLayout;
        private readonly ProgressBar m_Progress;
        private readonly RelativeLayout m_TitleLayout;
        private readonly Context m_Context;
        private readonly OverflowActionBarAction m_OverflowAction;
        private readonly bool m_HasMenuButton;


        //Used to track what we need to hide in the pop up menu.
        public List<int> MenuItemsToHide = new List<int>();


        public Activity CurrentActivity { get; set; }

        public ActionBar(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            m_Context = context;
            m_Inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

            m_BarView = (RelativeLayout)m_Inflater.Inflate(Resource.Layout.ActionBar, null);
            AddView(m_BarView);

            m_LogoView = m_BarView.FindViewById<ImageView>(Resource.Id.actionbar_home_logo);
            m_HomeLayout = m_BarView.FindViewById<RelativeLayout>(Resource.Id.actionbar_home_bg);
            m_HomeBtn = m_BarView.FindViewById<ImageButton>(Resource.Id.actionbar_home_btn);
            m_BackIndicator = m_BarView.FindViewById(Resource.Id.actionbar_home_is_back);

            m_TitleView = m_BarView.FindViewById<TextView>(Resource.Id.actionbar_title);
            m_ActionsView = m_BarView.FindViewById<LinearLayout>(Resource.Id.actionbar_actions);

            m_Progress = m_BarView.FindViewById<ProgressBar>(Resource.Id.actionbar_progress);
            m_TitleLayout = m_BarView.FindViewById<RelativeLayout>(Resource.Id.actionbar_title_layout);
            
            //check if pre-honeycomb. Ideally here you would actually want to check if a menu button exists.
            //however on all pre-honeycomb phones they basically did.
            var currentapiVersion = (int)Build.VERSION.SdkInt;
            m_HasMenuButton = currentapiVersion <= 10;

            m_OverflowAction = new OverflowActionBarAction(context);

            //Custom Attributes
            var a = context.ObtainStyledAttributes(attrs,
                    Resource.Styleable.ActionBar);

            var title = a.GetString(Resource.Styleable.ActionBar_title);

            if (title != null)
            {
                SetTitle(title);
            }
            a.Recycle();
        }

        public void SetHomeAction(ActionBarAction action)
        {
            m_HomeBtn.SetOnClickListener(this);
            m_HomeBtn.Tag = action;
            m_HomeBtn.SetImageResource(action.GetDrawable());
            m_HomeLayout.Visibility = ViewStates.Visible;
            ((RelativeLayout.LayoutParams)m_TitleLayout.LayoutParameters).AddRule(LayoutRules.RightOf, Resource.Id.actionbar_home_bg);
        }

        public void ClearHomeAction()
        {
            m_HomeLayout.Visibility = ViewStates.Gone;
        }

        

        /// <summary>
        /// Shows the provided logo to the left in the action bar.
        /// 
        /// This is ment to be used instead of the setHomeAction and does not draw
        /// a divider to the left of the provided logo.
        /// </summary>
        /// <param name="resId">The drawable resource id</param>
        public void SetHomeLogo(int resId)
        {
            // TODO: Add possibility to add an IntentAction as well.
            m_LogoView.SetImageResource(resId);
            m_LogoView.Visibility = ViewStates.Visible;
            m_HomeLayout.Visibility = ViewStates.Gone;
            ((RelativeLayout.LayoutParams)m_TitleLayout.LayoutParameters).AddRule(LayoutRules.RightOf, Resource.Id.actionbar_home_logo);
        }

        /// <summary>
        /// Emulating Honeycomb, setdisplayHomeAsUpEnabled takes a boolean
        /// and toggles whether the "home" view should have a little triangle
        /// indicating "up"
        /// </summary>
        /// <param name="show"></param>
        public void SetDisplayHomeAsUpEnabled(bool show)
        {
            m_BackIndicator.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
        }

        public void SetTitle(string title)
        {
            m_TitleView.Text = title;
        }

        public void SetTitle(int resid)
        {
            m_TitleView.SetText(resid);
        }

        /// <summary>
        /// The visibility of the circular progress bar in the Action Bar
        /// </summary>
        public ViewStates ProgressBarVisibility
        {
            get { return m_Progress.Visibility; }
            set { m_Progress.Visibility = value; }
        }

        /// <summary>
        /// Function to set a click listener for Title TextView
        /// </summary>
        /// <param name="listener"></param>
        public void SetOnTitleClickListener(IOnClickListener listener)
        {
            m_TitleView.SetOnClickListener(listener);
        }

        public void OnClick(View v)
        {
            var tag = v.Tag;
            var action = tag as ActionBarAction;
            if (action != null)
            {
                action.PerformAction(v);
            }
        }

        /// <summary>
        /// Adds a list of Actions.
        /// </summary>
        /// <param name="actionList">List of Actions</param>
        public void AddActions(ActionBarUtils.ActionList actionList)
        {
            for (var i = 0; i < actionList.Count; i++)
            {
                AddAction(actionList.ElementAt(i));
            }
        }

        /// <summary>
        /// Adds a new Action.
        /// </summary>
        /// <param name="action">Action to add.</param>
        public void AddAction(ActionBarAction action) 
        {
            AddAction(action, m_ActionsView.ChildCount);
        }

        /// <summary>
        /// Adds new action in the overflow
        /// </summary>
        /// <param name="action">Action to add.</param>
        public void AddOverflowAction(ActionBarAction action)
        {
            var index = m_ActionsView.ChildCount;
            m_ActionsView.AddView(InflateOverflowAction(action), index);
            m_OverflowAction.Index = index;
        }

        /// <summary>
        /// Adds a new Action at the specified index.
        /// </summary>
        /// <param name="action">the action to add</param>
        /// <param name="index">the position at which to add the action</param>
        public void AddAction(ActionBarAction action, int index)
        {
            var addActionBar = false;

            var hideAction = false;
            if (!ActionBarUtils.ActionFits(CurrentActivity, index + 1, m_HasMenuButton, action.ActionType))
            {
                if(!m_HasMenuButton)
                {
                    addActionBar = m_OverflowAction.ActionList.Count == 0;
                    m_OverflowAction.AddAction(action);
                    hideAction = true;
                }
            }
            else
            {
                if (m_OverflowAction.ActionList.Count != 0)//exists
                    index = m_OverflowAction.Index;//bring it inside

                hideAction = true;

                m_ActionsView.AddView(InflateAction(action), index);
            }

            //simply put it in the menu items to hide if we are a menu item.
            var taskAction = action as MenuItemActionBarAction;
            if (taskAction != null && hideAction)
                MenuItemsToHide.Add(taskAction.MenuItemId);

            if (addActionBar)
                AddOverflowAction(m_OverflowAction);
        }

        /// <summary>
        /// Removes all action views from this action bar
        /// </summary>
        public void RemoveAllActions()
        {
            m_ActionsView.RemoveAllViews();
            MenuItemsToHide.Clear();
        }

        /// <summary>
        /// Remove a action from the action bar.
        /// </summary>
        /// <param name="index">position of action to remove</param>
        public void RemoveActionAt(int index)
        {
            if (index < 1) return;

            var menuItemAction = m_ActionsView.GetChildAt(index).Tag as MenuItemActionBarAction;
            if (menuItemAction != null)
                MenuItemsToHide.Remove(menuItemAction.MenuItemId);

            m_ActionsView.RemoveViewAt(index);
        }

        /// <summary>
        /// Remove a action from the action bar.
        /// </summary>
        /// <param name="id">position of action to remove</param>
        public void RemoveActionAtMenuId(int id)
        {
            for (var i = 0; i < m_ActionsView.ChildCount; i++)
            {
                var view = m_ActionsView.GetChildAt(i);
                
                if (view == null) continue;

                var tag = view.Tag;
                var actionBarAction = tag as MenuItemActionBarAction;
                
                if (actionBarAction == null || id != actionBarAction.MenuItemId) continue;

                MenuItemsToHide.Remove(actionBarAction.MenuItemId);

                m_ActionsView.RemoveView(view);
            }
        }

        public int ActionCount
        {
            get
            {
                return m_ActionsView.ChildCount;
            }
        }

        /// <summary>
        /// Remove a action from the action bar.
        /// </summary>
        /// <param name="action">The action to remove</param>
        public void RemoveAction(ActionBarAction action)
        {
            for (var i = 0; i < m_ActionsView.ChildCount; i++)
            {
                var view = m_ActionsView.GetChildAt(i);

                if (view == null) continue;

                var tag = view.Tag;
                var actionBarAction = tag as ActionBarAction;

                if (actionBarAction == null || !actionBarAction.Equals(action)) continue;

                var menuItemAction = tag as MenuItemActionBarAction;
                if (menuItemAction != null)
                    MenuItemsToHide.Remove(menuItemAction.MenuItemId);

                m_ActionsView.RemoveView(view);
            }
        }

        
        /// <summary>
        /// Inflates a View with the given Action.
        /// </summary>
        /// <param name="action">the action to inflate</param>
        /// <returns>a view</returns>
        private View InflateAction(ActionBarAction action)
        {
            var view = m_Inflater.Inflate(Resource.Layout.ActionBar_Item, m_ActionsView, false);

            var labelView =
                view.FindViewById<ImageButton>(Resource.Id.actionbar_item);
            labelView.SetImageResource(action.GetDrawable());

            view.Tag = action;
            view.SetOnClickListener(this);
            view.SetOnLongClickListener(this);
            return view;
        }

        private View InflateOverflowAction(ActionBarAction action)
        {
            var view = m_Inflater.Inflate(Resource.Layout.OverflowActionBar_Item, m_ActionsView, false);

            var labelView =
                view.FindViewById<ImageButton>(Resource.Id.actionbar_item);
            labelView.SetImageResource(action.GetDrawable());

            var spinner = view.FindViewById<Spinner>(Resource.Id.overflow_spinner);
            m_OverflowAction.OverflowSpinner = spinner;

            labelView.Tag = action;
            labelView.SetOnClickListener(this);
            //view.SetOnLongClickListener(this);

            m_OverflowAction.Activity = CurrentActivity;
            return view;
        }

        public bool OnLongClick(View v)
        {
            var tag = v.Tag;
            var action = tag as ActionBarAction;
            if (action != null)
            {
                if (action.PopUpMessage == 0)
                    return true;

                if (CurrentActivity == null)
                    return false;

                Toast.MakeText(m_Context, action.PopUpMessage, ToastLength.Short).Show();

                return false;
            }

            return false;
        }
    }
}