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
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace MonoDroid.ActionBarSample
{
    public class ActionBar : RelativeLayout, View.IOnClickListener, View.IOnLongClickListener
    {

        #region Fields
        private LayoutInflater m_Inflater;
        private RelativeLayout m_BarView;
        private ImageView m_LogoView;
        private View m_BackIndicator;
        private TextView m_TitleView;
        private LinearLayout m_ActionsView;
        private ImageButton m_HomeBtn;
        private RelativeLayout m_HomeLayout;
        private ProgressBar m_Progress;
        private RelativeLayout m_TitleLayout;
        private Context m_Context;
        private OverflowActionBarAction m_OverflowAction;
        private bool m_HasMenuButton;

        //Used to track what we need to hide in the pop up menu.
        public List<int> MenuItemsToHide = new List<int>();
        #endregion

        #region Properties
        public Activity CurrentActivity { get; set; }

        /// <summary>
        /// Set the color of the seperators between Action Items
        /// </summary>
        public Color SeparatorColor
        {
            set { m_ActionsView.SetBackgroundColor(value); }
        }

        /// <summary>
        /// Set the drawable of the seperators between Action Items
        /// </summary>
        public Drawable SeparatorDrawable
        {
            set { m_ActionsView.SetBackgroundDrawable(value); }
        }

        /// <summary>
        /// Set the color of the Title in the Action Bar
        /// </summary>
        public Color TitleColor
        {
            set { m_TitleView.SetTextColor(value); }
        }

        /// <summary>
        /// Set the title in the Action Bar
        /// </summary>
        public string Title
        {
            set { m_TitleView.Text = value; }
        }

        /// <summary>
        /// Set the title in the Action Bar from a Resource Id
        /// </summary>
        public int TitleRaw
        {
            set { m_TitleView.SetText(value); }
        }

        /// <summary>
        /// Set the background color of the Action Bar
        /// </summary>
        public Color BackgroundColor
        {
            set { m_BarView.SetBackgroundColor(value); }
        }

        /// <summary>
        /// Set the background drawable of the Action Bar
        /// </summary>
        public Drawable BackgroundDrawable
        {
            set { m_BarView.SetBackgroundDrawable(value); }
        }

        /// <summary>
        /// Set the background drawable of the Action Bar Items
        /// </summary>
        public Drawable ItemBackgroundDrawable { get; set; }

        /// <summary>
        /// Returns the amount of Action Items in the Action Bar
        /// </summary>
        public int ActionCount
        {
            get
            {
                return m_ActionsView.ChildCount;
            }
        }

        /// <summary>
        /// The visibility of the circular progress bar in the Action Bar
        /// </summary>
        public ViewStates ProgressBarVisibility
        {
            get { return m_Progress.Visibility; }
            set { m_Progress.Visibility = value; }
        }

        #endregion

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

            //Custom Attributes (defined in Attrs.xml)
            var a = context.ObtainStyledAttributes(attrs,
                    Resource.Styleable.ActionBar);

            var title = a.GetString(Resource.Styleable.ActionBar_title);
            if (null != title)
                Title = title;

            var titleColor = a.GetColor(Resource.Styleable.ActionBar_title_color, Resources.GetColor(Resource.Color.actionbar_title));
            TitleColor = titleColor;

            var separatorColor = a.GetColor(Resource.Styleable.ActionBar_separator, Resources.GetColor(Resource.Color.actionbar_separator));
            m_ActionsView.SetBackgroundColor(separatorColor);

            using (var background = a.GetDrawable(Resource.Styleable.ActionBar_background)) //recycling the drawable immediately
            {
                if (null != background)
                    BackgroundDrawable = background;
            }

            var backgroundItem = a.GetDrawable(Resource.Styleable.ActionBar_background_item);
            if (null != backgroundItem)
                ItemBackgroundDrawable = backgroundItem;

            a.Recycle();
        }

        public void SetHomeAction(ActionBarAction action)
        {
            m_HomeBtn.SetOnClickListener(this);
            m_HomeBtn.Tag = action;
            m_HomeBtn.SetImageResource(action.GetDrawable());
            m_HomeLayout.Visibility = ViewStates.Visible;
            ((LayoutParams)m_TitleLayout.LayoutParameters).AddRule(LayoutRules.RightOf, Resource.Id.actionbar_home_bg);
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
            ((LayoutParams)m_TitleLayout.LayoutParameters).AddRule(LayoutRules.RightOf, Resource.Id.actionbar_home_logo);
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
            if (!ActionBarUtils.ActionFits(CurrentActivity, index, m_HasMenuButton, action.ActionType))
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
            m_OverflowAction.ClearActions();
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

            if (null != ItemBackgroundDrawable)
            {
                view.SetBackgroundDrawable(ItemBackgroundDrawable.GetConstantState().NewDrawable());
            }

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

            if (null != ItemBackgroundDrawable)
            {
                view.SetBackgroundDrawable(ItemBackgroundDrawable.GetConstantState().NewDrawable());
            }

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

        #region Android OnClick Listeners and Event handlers
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
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != ItemBackgroundDrawable)
                    ItemBackgroundDrawable.Dispose();
                if (null != m_Inflater)
                    m_Inflater.Dispose();
                if (null != m_OverflowAction)
                    m_OverflowAction.Dispose();
                ItemBackgroundDrawable = null;
                m_Inflater = null;
                m_BarView = null;
                m_LogoView = null;
                m_BackIndicator = null;
                m_TitleView = null;
                m_ActionsView = null;
                m_HomeBtn = null;
                m_HomeLayout = null;
                m_Progress = null;
                m_TitleLayout = null;
                m_Context = null;
                m_OverflowAction = null;
            }

            base.Dispose(disposing);
        }
    }
}