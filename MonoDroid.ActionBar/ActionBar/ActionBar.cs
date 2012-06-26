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
 * 
 * Changes by: Copyright (C) 2012 James Montemagno (http://www.montemagno.com)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Content.Res;

namespace MonoDroid.ActionBarSample
{
    public class ActionBar : RelativeLayout, Android.Views.View.IOnClickListener, View.IOnLongClickListener
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
            int currentapiVersion = (int)Android.OS.Build.VERSION.SdkInt;
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

        /**
         * Shows the provided logo to the left in the action bar.
         * 
         * This is ment to be used instead of the setHomeAction and does not draw
         * a divider to the left of the provided logo.
         * 
         * @param resId The drawable resource id
         */
        public void SetHomeLogo(int resId)
        {
            // TODO: Add possibility to add an IntentAction as well.
            mLogoView.SetImageResource(resId);
            mLogoView.Visibility = ViewStates.Visible;
            mHomeLayout.Visibility = ViewStates.Gone;
            ((RelativeLayout.LayoutParams)mTitleLayout.LayoutParameters).AddRule(LayoutRules.RightOf, Resource.Id.actionbar_home_logo);
        }

        /* Emulating Honeycomb, setdisplayHomeAsUpEnabled takes a boolean
         * and toggles whether the "home" view should have a little triangle
         * indicating "up" */
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

        /**
         * Set the enabled state of the progress bar.
         * 
         * @param One of {@link View#VISIBLE}, {@link View#INVISIBLE},
         *   or {@link View#GONE}.
         */
        public void SetProgressBarVisibility(ViewStates visibility)
        {
            mProgress.Visibility = visibility;
        }

        /**
         * Returns the visibility status for the progress bar.
         * 
         * @param One of {@link View#VISIBLE}, {@link View#INVISIBLE},
         *   or {@link View#GONE}.
         */
        public ViewStates GetProgressBarVisibility()
        {
            return mProgress.Visibility;
        }

        /**
         * Function to set a click listener for Title TextView
         * 
         * @param listener the onClickListener
         */
        public void SetOnTitleClickListener(IOnClickListener listener)
        {
            mTitleView.SetOnClickListener(listener);
        }

        public void OnClick(View v)
        {
            var tag = v.Tag;
            if (tag is ActionBarAction) {
                ActionBarAction action = (ActionBarAction)tag;
                action.PerformAction(v);
            }
        }

        /**
         * Adds a list of {@link Action}s.
         * @param actionList the actions to add
         */
        public void AddActions(ActionList actionList)
        {
            int actions = actionList.Count;
            for (int i = 0; i < actions; i++)
            {
                AddAction(actionList.ElementAt(i));
            }
        }

        /**
         * Adds a new {@link Action}.
         * @param action the action to add
         */
        public void AddAction(ActionBarAction action) 
        {
            int index = mActionsView.ChildCount;
            AddAction(action, index);
            
        }

        /**
      * Adds a new {@link Action}.
      * @param action the action to add
      */
        public void AddOverflowAction(ActionBarAction action)
        {
            int index = mActionsView.ChildCount;
            mActionsView.AddView(InflateOverflowAction(action), index);
            m_OverflowAction.Index = index;
        }

        /**
         * Adds a new {@link Action} at the specified index.
         * @param action the action to add
         * @param index the position at which to add the action
         */
        public void AddAction(ActionBarAction action, int index)
        {
            bool addActionBar = false;

            bool hideAction = false;
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

        /**
     * Removes all action views from this action bar
     */
        public void RemoveAllActions()
        {
            mActionsView.RemoveAllViews();
            MenuItemsToHide.Clear();
        }

        /**
         * Remove a action from the action bar.
         * @param index position of action to remove
         */
        public void RemoveActionAt(int index)
        {
            if (index >= 1)
            {
                var menuItemAction = mActionsView.GetChildAt(index).Tag as MenuItemActionBarAction;
                if (menuItemAction != null)
                    MenuItemsToHide.Remove(menuItemAction.MenuItemId);

                mActionsView.RemoveViewAt(index);
            }
        }

        /**
       * Remove a action from the action bar.
       * @param index position of action to remove
       */
        public void RemoveActionAtMenuId(int id)
        {
            int childCount = mActionsView.ChildCount;
            for (int i = 0; i < childCount; i++)
            {
                View view = mActionsView.GetChildAt(i);
                if (view != null)
                {
                    var tag = view.Tag;
                    var actionBarAction = tag as MenuItemActionBarAction;
                    if (actionBarAction != null && id == actionBarAction.MenuItemId)
                    {

                        MenuItemsToHide.Remove(actionBarAction.MenuItemId);

                        mActionsView.RemoveView(view);
                    }
                }
            }
        }

        public int ActionCount
        {
            get
            {
                return mActionsView.ChildCount;
            }
        }

        /**
         * Remove a action from the action bar.
         * @param action The action to remove
         */
        public void RemoveAction(ActionBarAction action)
        {
            int childCount = mActionsView.ChildCount;
            for (int i = 0; i < childCount; i++)
            {
                View view = mActionsView.GetChildAt(i);
                if (view != null)
                {
                    var tag = view.Tag;
                    var actionBarAction = tag as ActionBarAction;
                    if (actionBarAction != null && actionBarAction.Equals(action))
                    {
                        var menuItemAction = tag as MenuItemActionBarAction;
                        if (menuItemAction != null)
                            MenuItemsToHide.Remove(menuItemAction.MenuItemId);

                        mActionsView.RemoveView(view);
                    }
                }
            }
        }

        /**
         * A {@link LinkedList} that holds a list of {@link Action}s.
         */
        public class ActionList : LinkedList<ActionBarAction>
        {
        }


        /**
         * Inflates a {@link View} with the given {@link Action}.
         * @param action the action to inflate
         * @return a view
         */
        private View InflateAction(ActionBarAction action)
        {
            View view = mInflater.Inflate(Resource.Layout.ActionBar_Item, mActionsView, false);

            ImageButton labelView =
                view.FindViewById<ImageButton>(Resource.Id.actionbar_item);
            labelView.SetImageResource(action.GetDrawable());

            view.Tag = action;
            view.SetOnClickListener(this);
            view.SetOnLongClickListener(this);
            return view;
        }

        private View InflateOverflowAction(ActionBarAction action)
        {
            View view = mInflater.Inflate(Resource.Layout.OverflowActionBar_Item, mActionsView, false);

            ImageButton labelView =
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
            if (tag is ActionBarAction)
            {
                ActionBarAction action = (ActionBarAction)tag;
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