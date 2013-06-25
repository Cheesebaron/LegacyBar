/*
 * Copyright (C) 2010 Johan Nilsson <http://markupartist.com>
 *
 * Original (https://github.com/johannilsson/android-actionbar) Ported to Mono for Android
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

using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Util;
using LegacyBar.Library.BarActions;

namespace LegacyBar.Library.Bar
{
    public sealed class LegacyBar : RelativeLayout, View.IOnClickListener, View.IOnLongClickListener
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
        private OverflowLegacyBarAction _overflowLegacyBarAction;
        private Spinner _titleDropdown;

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
                if ((int) Build.VERSION.SdkInt >= 14)
                {
#if __ANDROID_14__
                    return ViewConfiguration.Get(Context).HasPermanentMenuKey;
#endif
                }
                return (int) Build.VERSION.SdkInt != 11;
            }
        }

        public LegacyBarTheme Theme { get; set; }
        public bool LightIcons { get; set; }
        public bool IsBottom { get; set; }

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
        /// Set the drawable of the dropdown
        /// </summary>
        public Drawable DropdownDrawable
        {
            set
            {
                _titleDropdown.SetBackgroundDrawable(value);
            }
        }

        /// <summary>
        /// Sets the raw background of the dropdown
        /// </summary>
        public int DropdownDrawableRaw
        {
            set
            {
                _titleDropdown.SetBackgroundResource(value);
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
            get { return _actionsView.ChildCount; }
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

        #region Events
        public EventHandler TitleClick;
        public EventHandler TitleLongClick;
        #endregion

        public LegacyBar(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
			ResourceIdManager.UpdateIdValues();

            _inflater = LayoutInflater.From(Context);
            //_inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

            _barView = (RelativeLayout)_inflater.Inflate(Resource.Layout.actionbar, null);
            AddView(_barView);

            _logoView = _barView.FindViewById(Resource.Id.actionbar_home_logo) as ImageView;
            _homeLayout = _barView.FindViewById<RelativeLayout>(Resource.Id.actionbar_home_bg);
            _homeBtn = _barView.FindViewById<ImageButton>(Resource.Id.actionbar_home_btn);
            _backIndicator = _barView.FindViewById(Resource.Id.actionbar_home_is_back);

            _titleView = _barView.FindViewById<TextView>(Resource.Id.actionbar_title);
            _actionsView = _barView.FindViewById<LinearLayout>(Resource.Id.actionbar_actions);

            _progress = _barView.FindViewById<ProgressBar>(Resource.Id.actionbar_progress);
            _titleLayout = _barView.FindViewById<RelativeLayout>(Resource.Id.actionbar_title_layout);
            _titleDropdown = _barView.FindViewById<Spinner>(Resource.Id.actionbar_spinner);

            _overflowLegacyBarAction = new OverflowLegacyBarAction(Context);

            //Custom Attributes (defined in Attrs.xml)
            var a = Context.ObtainStyledAttributes(attrs,
                    Resource.Styleable.actionbar);

            //grab theme attributes
            Theme = (LegacyBarTheme)a.GetInt(Resource.Styleable.actionbar_theme, 0);
            IsBottom = a.GetBoolean(Resource.Styleable.actionbar_is_bottom, false);
            LightIcons = a.GetBoolean(Resource.Styleable.actionbar_light_icons, false);

            //if we are not custom don't let them set it.
            if (Theme != LegacyBarTheme.Custom)
            {
                LightIcons = Theme == LegacyBarTheme.HoloLight;
            }

            _overflowLegacyBarAction.SetIconColor(LightIcons);

            var title = a.GetString(Resource.Styleable.actionbar_title);
            if (null != title)
                Title = title;

            var titleColor = a.GetColor(Resource.Styleable.actionbar_title_color, Resources.GetColor(Resource.Color.actionbar_title));
            TitleColor = titleColor;

            var separatorColor = a.GetColor(Resource.Styleable.actionbar_separator, Resources.GetColor(Resource.Color.actionbar_separator));
            _actionsView.SetBackgroundColor(separatorColor);
            _homeLayout.SetBackgroundColor(separatorColor);

            using (var background = a.GetDrawable(Resource.Styleable.actionbar_background)) //recycling the drawable immediately
            {
                if (null != background)
                    BackgroundDrawable = background;
            }

            var backgroundItem = a.GetDrawable(Resource.Styleable.actionbar_background_item);
            if (null != backgroundItem)
                ItemBackgroundDrawable = backgroundItem;

            if (IsBottom)
            {
                LegacyBarUtils.SetBottomLegacyBarTheme(this, Theme);
            }
            else
            {
                LegacyBarUtils.SetLegacyBarTheme(this, Theme);
            }

            a.Recycle();

            _titleView.Click += (s, e) => { if (null != TitleClick) TitleClick(s, e); };
            _titleView.LongClick += (s, e) => { if (null != TitleLongClick) TitleLongClick(s, e); };
        }

        public void SetHomeAction(LegacyBarAction legacyBarAction)
        {
            _homeBtn.SetOnClickListener(this);
            _homeBtn.Tag = legacyBarAction;
            _homeBtn.SetImageResource(legacyBarAction.GetDrawable());
            _homeLayout.Visibility = ViewStates.Visible;
            _backIndicator.SetBackgroundResource(LightIcons ? Resource.Drawable.actionbar_back_indicator : Resource.Drawable.actionbar_back_indicator_dark);
            

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

 		 public int DropDownSelectedItemPosition
        {
            get { return _titleDropdown.SelectedItemPosition; }
            set
            {
                try
                {
                    _titleDropdown.SetSelection(value);
                }
                catch (Exception)
                {
                }
                
            }
        }


        /// <summary>
        /// Sets teh drop down list with a customer adapter and callback when items change
        /// if adapter.Count == 0 then the spinner will NOT be shown and you will have to call this method again.
        /// </summary>
        /// <param name="adapter">Adapter to display</param>
        /// <param name="eventHandler">Event handler to callback. (can be null)</param>
         public void SetDropDown(BaseAdapter adapter, EventHandler<AdapterView.ItemSelectedEventArgs> eventHandler)
         {
             if (adapter == null)
                 return;

             

             if (adapter.Count == 0)
             {
                 _titleView.Visibility = ViewStates.Visible;
                 _titleDropdown.Visibility = ViewStates.Gone;
             }
             else
             {
                 var previousSelected = _titleDropdown.SelectedItemPosition;
                 _titleView.Visibility = ViewStates.Gone;

                 _titleDropdown.Visibility = ViewStates.Visible;
                 _titleDropdown.Adapter = adapter;
                 if (eventHandler != null)
                     _titleDropdown.ItemSelected += eventHandler;

                 if (previousSelected >= 0 && previousSelected < adapter.Count)
                     _titleDropdown.SetSelection(previousSelected);
             }
        }
        
        /// <summary>
        /// Sets the drop down items with a SimpleDropDownItem1Line Array Adapter
         /// if items.Length == 0 then the spinner will NOT be shown and you will have to call this method again.
        /// </summary>
        /// <param name="context">context to create under for adapter</param>
        /// <param name="items"> items to set</param>
         /// <param name="eventHandler">event handler for item selected change (can be null)</param>
       public void SetDropDown(Context context, string[] items, EventHandler<AdapterView.ItemSelectedEventArgs> eventHandler)
        {
            if (items == null)
                return;

            var adapter = new ArrayAdapter(context, Android.Resource.Layout.SimpleSpinnerItem, items);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleDropDownItem1Line);

           SetDropDown(adapter, eventHandler);
        }



        public void ClearHomeAction()
        {
            _homeLayout.Visibility = ViewStates.Gone;
        }

        /// <summary>
        /// Shows the provided logo to the left in the legacyBarAction bar.
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
        public void AddActions(LegacyBarUtils.ActionList actionList)
        {
            for (var i = 0; i < actionList.Count; i++)
            {
                AddAction(actionList.ElementAt(i));
            }
        }

        /// <summary>
        /// Adds a new Action.
        /// </summary>
        /// <param name="legacyBarAction">Action to add.</param>
        public void AddAction(LegacyBarAction legacyBarAction) 
        {
            AddAction(legacyBarAction, _actionsView.ChildCount);
        }

        /// <summary>
        /// Adds new legacyBarAction in the overflow
        /// </summary>
        /// <param name="legacyBarAction">Action to add.</param>
        public void AddOverflowAction(LegacyBarAction legacyBarAction)
        {
            var index = _actionsView.ChildCount;
            _actionsView.AddView(InflateOverflowAction(legacyBarAction), index);
            _overflowLegacyBarAction.Index = index;
        }

        /// <summary>
        /// Adds a new Action at the specified index.
        /// </summary>
        /// <param name="legacyBarAction">the legacyBarAction to add</param>
        /// <param name="index">the position at which to add the legacyBarAction</param>
        public void AddAction(LegacyBarAction legacyBarAction, int index)
        {
            var addActionBar = false;

            var hideAction = false;

            if (!LegacyBarUtils.ActionFits(Context.Resources.DisplayMetrics.WidthPixels, Context.Resources.DisplayMetrics.Density, index, HasMenuButton, legacyBarAction.ActionType))
            {
                if (!HasMenuButton)
                {
                    addActionBar = _overflowLegacyBarAction.ActionList.Count == 0;
                    _overflowLegacyBarAction.AddAction(legacyBarAction);
                    hideAction = true;
                }
            }
            else
            {
                if (_overflowLegacyBarAction.ActionList.Count != 0)//exists
                    index = _overflowLegacyBarAction.Index;//bring it inside

                hideAction = true;

                _actionsView.AddView(InflateAction(legacyBarAction), index);
            }

            //simply put it in the menu items to hide if we are a menu item.
            var taskAction = legacyBarAction as MenuItemLegacyBarAction;
            if (taskAction != null && hideAction)
                MenuItemsToHide.Add(taskAction.MenuItemId);

            if (addActionBar)
                AddOverflowAction(_overflowLegacyBarAction);
        }

        /// <summary>
        /// Removes all legacyBarAction views from this legacyBarAction bar
        /// </summary>
        public void RemoveAllActions()
        {
            _actionsView.RemoveAllViews();
            _overflowLegacyBarAction.ClearActions();
            MenuItemsToHide.Clear();
        }

        /// <summary>
        /// Remove a legacyBarAction from the legacyBarAction bar.
        /// </summary>
        /// <param name="index">position of legacyBarAction to remove</param>
        public void RemoveActionAt(int index)
        {
            if (index < 1) return;

            var menuItemAction = _actionsView.GetChildAt(index).Tag as MenuItemLegacyBarAction;
            if (menuItemAction != null)
                MenuItemsToHide.Remove(menuItemAction.MenuItemId);

            _actionsView.RemoveViewAt(index);
        }

        /// <summary>
        /// Remove a legacyBarAction from the legacyBarAction bar.
        /// </summary>
        /// <param name="id">position of legacyBarAction to remove</param>
        public void RemoveActionAtMenuId(int id)
        {
            for (var i = 0; i < _actionsView.ChildCount; i++)
            {
                var view = _actionsView.GetChildAt(i);
                
                if (view == null) continue;

                var tag = view.Tag;
                var actionBarAction = tag as MenuItemLegacyBarAction;
                
                if (actionBarAction == null || id != actionBarAction.MenuItemId) continue;

                MenuItemsToHide.Remove(actionBarAction.MenuItemId);

                _actionsView.RemoveView(view);
            }
        }

        /// <summary>
        /// Remove a legacyBarAction from the legacyBarAction bar.
        /// </summary>
        /// <param name="legacyBarAction">The legacyBarAction to remove</param>
        public void RemoveAction(LegacyBarAction legacyBarAction)
        {
            for (var i = 0; i < _actionsView.ChildCount; i++)
            {
                var view = _actionsView.GetChildAt(i);

                if (view == null) continue;

                var tag = view.Tag;
                var actionBarAction = tag as LegacyBarAction;

                if (actionBarAction == null || !actionBarAction.Equals(legacyBarAction)) continue;

                var menuItemAction = tag as MenuItemLegacyBarAction;
                if (menuItemAction != null)
                    MenuItemsToHide.Remove(menuItemAction.MenuItemId);

                _actionsView.RemoveView(view);
            }
        }
        
        /// <summary>
        /// Inflates a View with the given Action.
        /// </summary>
        /// <param name="legacyBarAction">the legacyBarAction to inflate</param>
        /// <returns>a view</returns>
        private View InflateAction(LegacyBarAction legacyBarAction)
        {
            var view = _inflater.Inflate(Resource.Layout.actionbar_item, _actionsView, false);

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
            labelView.SetImageResource(legacyBarAction.GetDrawable());

            view.Tag = legacyBarAction;
            view.SetOnClickListener(this);
            view.SetOnLongClickListener(this);

            if (legacyBarAction.PopUpMessage > 0)
                view.ContentDescription = Resources.GetString(legacyBarAction.PopUpMessage);

            return view;
        }

        private View InflateOverflowAction(LegacyBarAction legacyBarAction)
        {
            var view = _inflater.Inflate(Resource.Layout.overflowactionbar_item, _actionsView, false);


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

            labelView.SetImageResource(legacyBarAction.GetDrawable());

            var spinner = view.FindViewById<Spinner>(Resource.Id.overflow_spinner);
            _overflowLegacyBarAction.OverflowSpinner = spinner;

            labelView.Tag = legacyBarAction;
            labelView.SetOnClickListener(this);
            //view.SetOnLongClickListener(this);

            return view;
        }

        #region Android OnClick Listeners and Event handlers
        public void OnClick(View v)
        {
            var tag = v.Tag;
            var action = tag as LegacyBarAction;
            if (action != null)
            {
                action.PerformAction(v);
            }
        }

        public bool OnLongClick(View v)
        {
            var tag = v.Tag;
            var action = tag as LegacyBarAction;
            if (action != null)
            {
                if (action.PopUpMessage == 0)
                    return true;

                Toast.MakeText(Context, action.PopUpMessage, ToastLength.Short).Show();

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
                if (null != _overflowLegacyBarAction)
                    _overflowLegacyBarAction.Dispose();
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
                _overflowLegacyBarAction = null;
            }

            base.Dispose(disposing);
        }
    }
}
