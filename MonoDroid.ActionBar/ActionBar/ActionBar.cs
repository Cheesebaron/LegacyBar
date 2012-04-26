/*
 * Copyright (C) 2010 Johan Nilsson <http://markupartist.com>
 *
 * Original (https://github.com/johannilsson/android-actionbar) Ported to Mono for Android
 * Copyright (C) 2012 Tomasz Cielecki <tomasz@ostebaronen.dk>
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
using System.Collections.Generic;
using System.Linq;

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
    public class ActionBar : RelativeLayout, Android.Views.View.IOnClickListener
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

        public ActionBar(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {

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

            TypedArray a = context.ObtainStyledAttributes(attrs,
                    Resource.Styleable.ActionBar);
            string title = a.GetString(Resource.Styleable.ActionBar_title);
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
        public void AddAction(ActionBarAction action) {
            int index = mActionsView.ChildCount;
            AddAction(action, index);
        }

        /**
         * Adds a new {@link Action} at the specified index.
         * @param action the action to add
         * @param index the position at which to add the action
         */
        public void AddAction(ActionBarAction action, int index)
        {
            mActionsView.AddView(inflateAction(action), index);
        }

        /**
     * Removes all action views from this action bar
     */
        public void RemoveAllActions()
        {
            mActionsView.RemoveAllViews();
        }

        /**
         * Remove a action from the action bar.
         * @param index position of action to remove
         */
        public void RemoveActionAt(int index)
        {
            if (index >= 1)
                mActionsView.RemoveViewAt(index);
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
        public void RemoveAction(ActionBarAction action) {
            int childCount = mActionsView.ChildCount;
            for (int i = 0; i < childCount; i++) {
                View view = mActionsView.GetChildAt(i);
                if (view != null) {
                    var tag = view.Tag;
                    if (tag is ActionBarAction && tag.Equals(action))
                    {
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
        private View inflateAction(ActionBarAction action)
        {
            View view = mInflater.Inflate(Resource.Layout.ActionBar_Item, mActionsView, false);

            ImageButton labelView =
                view.FindViewById<ImageButton>(Resource.Id.actionbar_item);
            labelView.SetImageResource(action.GetDrawable());

            view.Tag = action;
            view.SetOnClickListener(this);
            return view;
        }
    }
}