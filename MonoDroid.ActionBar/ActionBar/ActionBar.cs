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
        private LayoutInflater mInflater;
        private RelativeLayout mBarView;
        private ImageView mLogoView;
        private View mBackIndicator;
        private TextView mTitleView;
        private LinearLayout mActionsView;
        private ImageButton mHomeBtn;
        private RelativeLayout mHomeLayout;
        private ProgressBar mProgress;
        private RelativeLayout mTitleLayout;
        private Context m_Context;
        private OverflowActionBarAction m_OverflowAction;
        private bool m_HasMenuButton;


        //Used to track what we need to hide in the pop up menu.
        public List<int> MenuItemsToHide = new List<int>();


        public Activity CurrentActivity { get; set; }

        public ActionBar(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            m_Context = context;
            mInflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

            mBarView = (RelativeLayout)mInflater.Inflate(Resource.Layout.ActionBar, null);
            AddView(mBarView);

            mLogoView = mBarView.FindViewById<ImageView>(Resource.Id.actionbar_home_logo);
            mHomeLayout = mBarView.FindViewById<RelativeLayout>(Resource.Id.actionbar_home_bg);
            mHomeBtn = mBarView.FindViewById<ImageButton>(Resource.Id.actionbar_home_btn);
            mBackIndicator = mBarView.FindViewById(Resource.Id.actionbar_home_is_back);

            mTitleView = mBarView.FindViewById<TextView>(Resource.Id.actionbar_title);
            mActionsView = mBarView.FindViewById<LinearLayout>(Resource.Id.actionbar_actions);

            mProgress = mBarView.FindViewById<ProgressBar>(Resource.Id.actionbar_progress);
            mTitleLayout = mBarView.FindViewById<RelativeLayout>(Resource.Id.actionbar_title_layout);
            TypedArray a = context.ObtainStyledAttributes(attrs,
                    Resource.Styleable.ActionBar);

            m_OverflowAction = new OverflowActionBarAction(context);
            string title = a.GetString(Resource.Styleable.ActionBar_title);

            //check if pre-honeycomb. Ideally here you would actually want to check if a menu button exists.
            //however on all pre-honeycomb phones they basically did.
            var currentapiVersion = (int)Build.VERSION.SdkInt;
            m_HasMenuButton = currentapiVersion <= 10;

            if (title != null)
            {
                SetTitle(title);
            }
            a.Recycle();
        }

        public void SetHomeAction(ActionBarAction action)
        {
            mHomeBtn.SetOnClickListener(this);
            mHomeBtn.Tag = action;
            mHomeBtn.SetImageResource(action.GetDrawable());
            mHomeLayout.Visibility = ViewStates.Visible;
            ((RelativeLayout.LayoutParams)mTitleLayout.LayoutParameters).AddRule(LayoutRules.RightOf, Resource.Id.actionbar_home_bg);
        }

        public void ClearHomeAction()
        {
            mHomeLayout.Visibility = ViewStates.Gone;
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
            mLogoView.SetImageResource(resId);
            mLogoView.Visibility = ViewStates.Visible;
            mHomeLayout.Visibility = ViewStates.Gone;
            ((RelativeLayout.LayoutParams)mTitleLayout.LayoutParameters).AddRule(LayoutRules.RightOf, Resource.Id.actionbar_home_logo);
        }

        /// <summary>
        /// Emulating Honeycomb, setdisplayHomeAsUpEnabled takes a boolean
        /// and toggles whether the "home" view should have a little triangle
        /// indicating "up"
        /// </summary>
        /// <param name="show"></param>
        public void SetDisplayHomeAsUpEnabled(bool show)
        {
            mBackIndicator.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
        }

        public void SetTitle(string title)
        {
            mTitleView.Text = title;
        }

        public void SetTitle(int resid)
        {
            mTitleView.SetText(resid);
        }

        /// <summary>
        /// The visibility of the circular progress bar in the Action Bar
        /// </summary>
        public ViewStates ProgressBarVisibility
        {
            get { return mProgress.Visibility; }
            set { mProgress.Visibility = value; }
        }

        /// <summary>
        /// Function to set a click listener for Title TextView
        /// </summary>
        /// <param name="listener"></param>
        public void SetOnTitleClickListener(IOnClickListener listener)
        {
            mTitleView.SetOnClickListener(listener);
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
            AddAction(action, mActionsView.ChildCount);
        }

        /// <summary>
        /// Adds new action in the overflow
        /// </summary>
        /// <param name="action">Action to add.</param>
        public void AddOverflowAction(ActionBarAction action)
        {
            var index = mActionsView.ChildCount;
            mActionsView.AddView(InflateOverflowAction(action), index);
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

                mActionsView.AddView(InflateAction(action), index);
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
            mActionsView.RemoveAllViews();
            MenuItemsToHide.Clear();
        }

        /// <summary>
        /// Remove a action from the action bar.
        /// </summary>
        /// <param name="index">position of action to remove</param>
        public void RemoveActionAt(int index)
        {
            if (index < 1) return;

            var menuItemAction = mActionsView.GetChildAt(index).Tag as MenuItemActionBarAction;
            if (menuItemAction != null)
                MenuItemsToHide.Remove(menuItemAction.MenuItemId);

            mActionsView.RemoveViewAt(index);
        }

        /// <summary>
        /// Remove a action from the action bar.
        /// </summary>
        /// <param name="id">position of action to remove</param>
        public void RemoveActionAtMenuId(int id)
        {
            for (var i = 0; i < mActionsView.ChildCount; i++)
            {
                var view = mActionsView.GetChildAt(i);
                
                if (view == null) continue;

                var tag = view.Tag;
                var actionBarAction = tag as MenuItemActionBarAction;
                
                if (actionBarAction == null || id != actionBarAction.MenuItemId) continue;

                MenuItemsToHide.Remove(actionBarAction.MenuItemId);

                mActionsView.RemoveView(view);
            }
        }

        public int ActionCount
        {
            get
            {
                return mActionsView.ChildCount;
            }
        }

        /// <summary>
        /// Remove a action from the action bar.
        /// </summary>
        /// <param name="action">The action to remove</param>
        public void RemoveAction(ActionBarAction action)
        {
            for (var i = 0; i < mActionsView.ChildCount; i++)
            {
                var view = mActionsView.GetChildAt(i);

                if (view == null) continue;

                var tag = view.Tag;
                var actionBarAction = tag as ActionBarAction;

                if (actionBarAction == null || !actionBarAction.Equals(action)) continue;

                var menuItemAction = tag as MenuItemActionBarAction;
                if (menuItemAction != null)
                    MenuItemsToHide.Remove(menuItemAction.MenuItemId);

                mActionsView.RemoveView(view);
            }
        }

        
        /// <summary>
        /// Inflates a View with the given Action.
        /// </summary>
        /// <param name="action">the action to inflate</param>
        /// <returns>a view</returns>
        private View InflateAction(ActionBarAction action)
        {
            var view = mInflater.Inflate(Resource.Layout.ActionBar_Item, mActionsView, false);

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
            var view = mInflater.Inflate(Resource.Layout.OverflowActionBar_Item, mActionsView, false);

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