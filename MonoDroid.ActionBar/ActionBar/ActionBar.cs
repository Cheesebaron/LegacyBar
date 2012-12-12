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
using Android.Views;
using Android.Widget;
using Android.Util;

namespace MonoDroid.ActionBarSample
{
    public class ActionBar : RelativeLayout, View.IOnClickListener, View.IOnLongClickListener
    {

        #region Fields
        private LayoutInflater _inflater;
        private RelativeLayout _barView;
        private ImageView _logoView;
        private View _backIndicator;
        private TextView _titleView;
        private LinearLayout _actionsView;
        private ImageButton _homeBtn;
        private RelativeLayout _homeLayout;
        private ProgressBar _progress;
        private RelativeLayout _titleLayout;
        private Context _context;
        private OverflowActionBarAction _overflowAction;

        //Used to track what we need to hide in the pop up menu.
        public List<int> MenuItemsToHide = new List<int>();
        #endregion

        #region Properties
        /// <summary>
        /// According to Android documentation, all devices prior to Android 3.0 (API11) were required to have a menu button.
        /// From API11 it was optional, and in API14 a method to detect if a button was present was presented.
        /// Some devices like Samsung Galaxy S2 and S3 still have hardware buttons!
        /// </summary>
        public bool HasMenuButton
        {
            get 
            {
#if __ANDROID_14__//ICS+ we can tell
                return Android.Views.ViewConfiguration.HasPermanentMenuKey;
#elif __ANDROID_11__//Honey Comb never does
                return false;
#else//Pre 3.0 always has menu button
                return true;
#endif
            }
        }

        public Activity CurrentActivity { get; set; }

        /// <summary>
        /// Set the color of the seperators between Action Items
        /// </summary>
        public Color SeparatorColor
        {
            set
            {
                _actionsView.SetBackgroundColor(value);
                _homeLayout.SetBackgroundColor(value);
            }
        }

        public int SeparatorColorRaw
        {
            set
            {
                _actionsView.SetBackgroundResource(value);
                _homeLayout.SetBackgroundResource(value);
            }
        }

        /// <summary>
        /// Set the drawable of the seperators between Action Items
        /// </summary>
        public Drawable SeparatorDrawable
        {
            set
            {
                _actionsView.SetBackgroundDrawable(value);
                _homeLayout.SetBackgroundDrawable(value);
            }
        }

        public int SeparatorDrawableRaw
        {
            set
            {
                _actionsView.SetBackgroundResource(value);
                _homeLayout.SetBackgroundResource(value);
            }
        }


        /// <summary>
        /// Set the color of the Title in the Action Bar
        /// </summary>
        public Color TitleColor
        {
            set { _titleView.SetTextColor(value); }
        }

        public int TitleColorRaw
        {
          set {_titleView.SetTextColor(Resources.GetColor(value));}   
        }

        /// <summary>
        /// Set the title in the Action Bar
        /// </summary>
        public string Title
        {
            set { _titleView.Text = value; }
        }

        /// <summary>
        /// Set the title in the Action Bar from a Resource Id
        /// </summary>
        public int TitleRaw
        {
            set { _titleView.SetText(value); }
        }


        /// <summary>
        /// Set the background color of the Action Bar
        /// </summary>
        public Color BackgroundColor
        {
            set { SetBackgroundColor(value); }
        }

        public int BackgroundColorRaw
        {
            set {SetBackgroundColor(Resources.GetColor(value));}   
        }

        /// <summary>
        /// Set the background drawable of the Action Bar
        /// </summary>
        public Drawable BackgroundDrawable
        {
            set { SetBackgroundDrawable(value); }
        }

        public int BackgroundDrawableRaw
        {
            set { SetBackgroundResource(value);}   
        }

        /// <summary>
        /// Set the background drawable of the Action Bar Items
        /// </summary>
        public Drawable ItemBackgroundDrawable { get; set; }

        public int ItemBackgroundDrawableRaw { get; set; }

        /// <summary>
        /// Returns the amount of Action Items in the Action Bar
        /// </summary>
        public int ActionCount
        {
            get
            {
                return _actionsView.ChildCount;
            }
        }

        /// <summary>
        /// The visibility of the circular progress bar in the Action Bar
        /// </summary>
        public ViewStates ProgressBarVisibility
        {
            get { return _progress.Visibility; }
            set { _progress.Visibility = value; }
        }

        #endregion

        public ActionBar(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            _context = context;
            _inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

            _barView = (RelativeLayout)_inflater.Inflate(Resource.Layout.ActionBar, null);
            AddView(_barView);

            _logoView = _barView.FindViewById<ImageView>(Resource.Id.actionbar_home_logo);
            _homeLayout = _barView.FindViewById<RelativeLayout>(Resource.Id.actionbar_home_bg);
            _homeBtn = _barView.FindViewById<ImageButton>(Resource.Id.actionbar_home_btn);
            _backIndicator = _barView.FindViewById(Resource.Id.actionbar_home_is_back);

            _titleView = _barView.FindViewById<TextView>(Resource.Id.actionbar_title);
            _actionsView = _barView.FindViewById<LinearLayout>(Resource.Id.actionbar_actions);

            _progress = _barView.FindViewById<ProgressBar>(Resource.Id.actionbar_progress);
            _titleLayout = _barView.FindViewById<RelativeLayout>(Resource.Id.actionbar_title_layout);

            _overflowAction = new OverflowActionBarAction(context);

            //Custom Attributes (defined in Attrs.xml)
            var a = context.ObtainStyledAttributes(attrs,
                    Resource.Styleable.ActionBar);

            var title = a.GetString(Resource.Styleable.ActionBar_title);
            if (null != title)
                Title = title;

            var titleColor = a.GetColor(Resource.Styleable.ActionBar_title_color, Resources.GetColor(Resource.Color.actionbar_title));
            TitleColor = titleColor;

            var separatorColor = a.GetColor(Resource.Styleable.ActionBar_separator, Resources.GetColor(Resource.Color.actionbar_separator));
            _actionsView.SetBackgroundColor(separatorColor);

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
            _homeBtn.SetOnClickListener(this);
            _homeBtn.Tag = action;
            _homeBtn.SetImageResource(action.GetDrawable());
            _homeLayout.Visibility = ViewStates.Visible;

            if (null != ItemBackgroundDrawable)
            {
                _homeBtn.SetBackgroundDrawable(ItemBackgroundDrawable.GetConstantState().NewDrawable());
            }
            else if (ItemBackgroundDrawableRaw > 0)
            {
                _homeBtn.SetBackgroundResource(ItemBackgroundDrawableRaw);
            }


            ((LayoutParams)_titleLayout.LayoutParameters).AddRule(LayoutRules.RightOf, Resource.Id.actionbar_home_bg);
        }

        public void ClearHomeAction()
        {
            _homeLayout.Visibility = ViewStates.Gone;
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
            _logoView.SetImageResource(resId);
            _logoView.Visibility = ViewStates.Visible;
            _homeLayout.Visibility = ViewStates.Gone;

            if (null != ItemBackgroundDrawable)
            {
                _logoView.SetBackgroundDrawable(ItemBackgroundDrawable.GetConstantState().NewDrawable());
            }
            else if(ItemBackgroundDrawableRaw > 0)
            {
                _logoView.SetBackgroundResource(ItemBackgroundDrawableRaw);
            }

            ((LayoutParams)_titleLayout.LayoutParameters).AddRule(LayoutRules.RightOf, Resource.Id.actionbar_home_logo);
        }

        /// <summary>
        /// Emulating Honeycomb, setdisplayHomeAsUpEnabled takes a boolean
        /// and toggles whether the "home" view should have a little triangle
        /// indicating "up"
        /// </summary>
        /// <param name="show"></param>
        public void SetDisplayHomeAsUpEnabled(bool show)
        {
            _backIndicator.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
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
            AddAction(action, _actionsView.ChildCount);
        }

        /// <summary>
        /// Adds new action in the overflow
        /// </summary>
        /// <param name="action">Action to add.</param>
        public void AddOverflowAction(ActionBarAction action)
        {
            var index = _actionsView.ChildCount;
            _actionsView.AddView(InflateOverflowAction(action), index);
            _overflowAction.Index = index;
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
            if (!ActionBarUtils.ActionFits(CurrentActivity, index, HasMenuButton, action.ActionType))
            {
                if (!HasMenuButton)
                {
                    addActionBar = _overflowAction.ActionList.Count == 0;
                    _overflowAction.AddAction(action);
                    hideAction = true;
                }
            }
            else
            {
                if (_overflowAction.ActionList.Count != 0)//exists
                    index = _overflowAction.Index;//bring it inside

                hideAction = true;

                _actionsView.AddView(InflateAction(action), index);
            }

            //simply put it in the menu items to hide if we are a menu item.
            var taskAction = action as MenuItemActionBarAction;
            if (taskAction != null && hideAction)
                MenuItemsToHide.Add(taskAction.MenuItemId);

            if (addActionBar)
                AddOverflowAction(_overflowAction);
        }

        /// <summary>
        /// Removes all action views from this action bar
        /// </summary>
        public void RemoveAllActions()
        {
            _actionsView.RemoveAllViews();
            _overflowAction.ClearActions();
            MenuItemsToHide.Clear();
        }

        /// <summary>
        /// Remove a action from the action bar.
        /// </summary>
        /// <param name="index">position of action to remove</param>
        public void RemoveActionAt(int index)
        {
            if (index < 1) return;

            var menuItemAction = _actionsView.GetChildAt(index).Tag as MenuItemActionBarAction;
            if (menuItemAction != null)
                MenuItemsToHide.Remove(menuItemAction.MenuItemId);

            _actionsView.RemoveViewAt(index);
        }

        /// <summary>
        /// Remove a action from the action bar.
        /// </summary>
        /// <param name="id">position of action to remove</param>
        public void RemoveActionAtMenuId(int id)
        {
            for (var i = 0; i < _actionsView.ChildCount; i++)
            {
                var view = _actionsView.GetChildAt(i);
                
                if (view == null) continue;

                var tag = view.Tag;
                var actionBarAction = tag as MenuItemActionBarAction;
                
                if (actionBarAction == null || id != actionBarAction.MenuItemId) continue;

                MenuItemsToHide.Remove(actionBarAction.MenuItemId);

                _actionsView.RemoveView(view);
            }
        }

        /// <summary>
        /// Remove a action from the action bar.
        /// </summary>
        /// <param name="action">The action to remove</param>
        public void RemoveAction(ActionBarAction action)
        {
            for (var i = 0; i < _actionsView.ChildCount; i++)
            {
                var view = _actionsView.GetChildAt(i);

                if (view == null) continue;

                var tag = view.Tag;
                var actionBarAction = tag as ActionBarAction;

                if (actionBarAction == null || !actionBarAction.Equals(action)) continue;

                var menuItemAction = tag as MenuItemActionBarAction;
                if (menuItemAction != null)
                    MenuItemsToHide.Remove(menuItemAction.MenuItemId);

                _actionsView.RemoveView(view);
            }
        }
        
        /// <summary>
        /// Inflates a View with the given Action.
        /// </summary>
        /// <param name="action">the action to inflate</param>
        /// <returns>a view</returns>
        private View InflateAction(ActionBarAction action)
        {
            var view = _inflater.Inflate(Resource.Layout.ActionBar_Item, _actionsView, false);

            if (null != ItemBackgroundDrawable)
            {
                view.SetBackgroundDrawable(ItemBackgroundDrawable.GetConstantState().NewDrawable());
            }
            else if (ItemBackgroundDrawableRaw > 0)
            {
                view.SetBackgroundResource(ItemBackgroundDrawableRaw);
            }

            var labelView =
                view.FindViewById<ImageButton>(Resource.Id.actionbar_item);
            labelView.SetImageResource(action.GetDrawable());

            view.Tag = action;
            view.SetOnClickListener(this);
            view.SetOnLongClickListener(this);

            if (action.PopUpMessage > 0)
                view.ContentDescription = Resources.GetString(action.PopUpMessage);

            return view;
        }

        private View InflateOverflowAction(ActionBarAction action)
        {
            var view = _inflater.Inflate(Resource.Layout.OverflowActionBar_Item, _actionsView, false);


            var labelView =
                view.FindViewById<ImageButton>(Resource.Id.actionbar_item);


            if (null != ItemBackgroundDrawable)
            {
                labelView.SetBackgroundDrawable(ItemBackgroundDrawable.GetConstantState().NewDrawable());
            }
            else if (ItemBackgroundDrawableRaw > 0)
            {
                labelView.SetBackgroundResource(ItemBackgroundDrawableRaw);
            }

            labelView.SetImageResource(action.GetDrawable());

            var spinner = view.FindViewById<Spinner>(Resource.Id.overflow_spinner);
            _overflowAction.OverflowSpinner = spinner;

            labelView.Tag = action;
            labelView.SetOnClickListener(this);
            //view.SetOnLongClickListener(this);

            _overflowAction.Activity = CurrentActivity;
            return view;
        }

        #region Android OnClick Listeners and Event handlers
        /// <summary>
        /// Function to set a click listener for Title TextView
        /// </summary>
        /// <param name="listener"></param>
        public void SetOnTitleClickListener(IOnClickListener listener)
        {
            _titleView.SetOnClickListener(listener);
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

                Toast.MakeText(_context, action.PopUpMessage, ToastLength.Short).Show();

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
                if (null != _inflater)
                    _inflater.Dispose();
                if (null != _overflowAction)
                    _overflowAction.Dispose();
                ItemBackgroundDrawable = null;
                _inflater = null;
                _barView = null;
                _logoView = null;
                _backIndicator = null;
                _titleView = null;
                _actionsView = null;
                _homeBtn = null;
                _homeLayout = null;
                _progress = null;
                _titleLayout = null;
                _context = null;
                _overflowAction = null;
            }

            base.Dispose(disposing);
        }
    }
}