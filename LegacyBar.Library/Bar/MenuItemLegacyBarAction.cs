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

using System;
using Android.App;
using Android.Content;
using Android.Views;

namespace LegacyBar.Library.Bar
{

    /// <summary>
    /// This is just a stub so we can set the id.
    /// In the long run we should really read in the full menu item and use it.
    /// </summary>
    public class LegacyBarMenuItem : IMenuItem
    {
        private readonly IntPtr _handle;
        public LegacyBarMenuItem(int id, IntPtr handle)
        {
            m_ItemId = id;
            _handle = handle;
        }

        public View ActionView { get; private set; }

        char IMenuItem.AlphabeticShortcut
        {
            get { throw new NotImplementedException(); }
        }

        int IMenuItem.GroupId
        {
            get { throw new NotImplementedException(); }
        }

        bool IMenuItem.HasSubMenu
        {
            get { throw new NotImplementedException(); }
        }

        Android.Graphics.Drawables.Drawable IMenuItem.Icon
        {
            get { throw new NotImplementedException(); }
        }

        Intent IMenuItem.Intent
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsActionViewExpanded { get; private set; }

        bool IMenuItem.IsCheckable
        {
            get { throw new NotImplementedException(); }
        }

        bool IMenuItem.IsChecked
        {
            get { throw new NotImplementedException(); }
        }

        bool IMenuItem.IsEnabled
        {
            get { throw new NotImplementedException(); }
        }

        bool IMenuItem.IsVisible
        {
            get { throw new NotImplementedException(); }
        }

        private int m_ItemId;
        int IMenuItem.ItemId { get { return m_ItemId; } }

        IContextMenuContextMenuInfo IMenuItem.MenuInfo
        {
            get { throw new NotImplementedException(); }
        }

        char IMenuItem.NumericShortcut
        {
            get { throw new NotImplementedException(); }
        }

        int IMenuItem.Order
        {
            get { throw new NotImplementedException(); }
        }

        public bool CollapseActionView()
        {
            throw new NotImplementedException();
        }

        public bool ExpandActionView()
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetActionView(View view)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetActionView(int resId)
        {
            throw new NotImplementedException();
        }

        IMenuItem IMenuItem.SetAlphabeticShortcut(char alphaChar)
        {
            throw new NotImplementedException();
        }

        IMenuItem IMenuItem.SetCheckable(bool checkable)
        {
            throw new NotImplementedException();
        }

        IMenuItem IMenuItem.SetChecked(bool @checked)
        {
            throw new NotImplementedException();
        }

        IMenuItem IMenuItem.SetEnabled(bool enabled)
        {
            throw new NotImplementedException();
        }

        IMenuItem IMenuItem.SetIcon(int iconRes)
        {
            throw new NotImplementedException();
        }

        IMenuItem IMenuItem.SetIcon(Android.Graphics.Drawables.Drawable icon)
        {
            throw new NotImplementedException();
        }

        IMenuItem IMenuItem.SetIntent(Intent intent)
        {
            throw new NotImplementedException();
        }

        IMenuItem IMenuItem.SetNumericShortcut(char numericChar)
        {
            throw new NotImplementedException();
        }

        IMenuItem IMenuItem.SetOnMenuItemClickListener(IMenuItemOnMenuItemClickListener menuItemClickListener)
        {
            throw new NotImplementedException();
        }

        IMenuItem IMenuItem.SetShortcut(char numericChar, char alphaChar)
        {
            throw new NotImplementedException();
        }

        IMenuItem IMenuItem.SetTitle(Java.Lang.ICharSequence title)
        {
            throw new NotImplementedException();
        }

        IMenuItem IMenuItem.SetTitle(int title)
        {
            throw new NotImplementedException();
        }

        IMenuItem IMenuItem.SetTitleCondensed(Java.Lang.ICharSequence title)
        {
            throw new NotImplementedException();
        }

        IMenuItem IMenuItem.SetVisible(bool visible)
        {
            throw new NotImplementedException();
        }

        ISubMenu IMenuItem.SubMenu
        {
            get { throw new NotImplementedException(); }
        }

        Java.Lang.ICharSequence IMenuItem.TitleCondensedFormatted
        {
            get { throw new NotImplementedException(); }
        }

        Java.Lang.ICharSequence IMenuItem.TitleFormatted
        {
            get { throw new NotImplementedException(); }
        }


        IntPtr Android.Runtime.IJavaObject.Handle
        {
            get { return _handle; }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }

    /// <summary>
    /// MenuItemLegacyBarAction will call teh main activitiess "OnOptionsItemSelected", which allows us to re-use code.
    /// </summary>
    public class MenuItemLegacyBarAction: LegacyBarAction
    {
        private readonly Activity _activity;
        private readonly LegacyBarMenuItem _menuItem;
        public int MenuItemId;
        public MenuItemLegacyBarAction(Context context, Activity activity, int menuId, int drawable, int popupId)
        {
            Drawable = drawable;
            Context = context;
            _activity = activity;
            MenuItemId = menuId;
            _menuItem = new LegacyBarMenuItem(menuId, Handle);
            PopUpMessage = popupId;
        }

        public override int GetDrawable()
        {
            return Drawable;
        }

        public override void PerformAction(View view)
        {
            try
            {
                _activity.OnOptionsItemSelected(_menuItem);
            }
            catch(Exception)
            {
            }
        }
    }
}
