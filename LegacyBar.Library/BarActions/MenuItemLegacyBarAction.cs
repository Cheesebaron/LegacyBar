/*
 * Copyright (C) 2010 Johan Nilsson <http://markupartist.com>
 *
 * Original (https://github.com/johannilsson/android-actionbar) Ported to Xamarin.Android
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
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Java.Lang;

namespace LegacyBar.Library.BarActions
{
    /// <summary>
    /// This is just a stub so we can set the id.
    /// In the long run we should really read in the full menu item and use it.
    /// </summary>
    public class LegacyBarMenuItem : IMenuItem
    {
        public LegacyBarMenuItem(int id, IntPtr handle)
        {
            ItemId = id;
            Handle = handle;
        }

        public IntPtr Handle { get; private set; }
        public char AlphabeticShortcut { get; private set; }
        public int GroupId { get; private set; }
        public bool HasSubMenu { get; private set; }
        public Drawable Icon { get; private set; }
        public Intent Intent { get; private set; }
        public bool IsCheckable { get; private set; }
        public bool IsChecked { get; private set; }
        public bool IsEnabled { get; private set; }
        public bool IsVisible { get; private set; }
        public int ItemId { get; private set; }
        public IContextMenuContextMenuInfo MenuInfo { get; private set; }
        public char NumericShortcut { get; private set; }
        public int Order { get; private set; }
        public ISubMenu SubMenu { get; private set; }
        public ICharSequence TitleFormatted { get; private set; }
        public ICharSequence TitleCondensedFormatted { get; private set; }
        public View ActionView { get; private set; }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public IMenuItem SetAlphabeticShortcut(char alphaChar)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetCheckable(bool checkable)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetChecked(bool @checked)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetEnabled(bool enabled)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetIcon(Drawable icon)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetIcon(int iconRes)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetIntent(Intent intent)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetNumericShortcut(char numericChar)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetOnMenuItemClickListener(IMenuItemOnMenuItemClickListener menuItemClickListener)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetShortcut(char numericChar, char alphaChar)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetTitle(int title)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetTitle(ICharSequence title)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetTitleCondensed(ICharSequence title)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetVisible(bool visible)
        {
            throw new NotImplementedException();
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
        public MenuItemLegacyBarAction(Activity activity, int menuId, int drawable, int popupId)
        {
            Drawable = drawable;
            Context = activity;
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
            catch(System.Exception)
            {
            }
        }
    }
}
