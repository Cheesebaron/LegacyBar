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
 * Addition by: Copyright (C) 2012 James Montemagno (motz2k1@oh.rr.com)
 */

using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.ActionBarSample
{

    /// <summary>
    /// This is just a stub so we can set the id.
    /// In the long run we should really read in the full menu item and use it.
    /// </summary>
    public class ActionBarMenuItem : IMenuItem
    {
        public ActionBarMenuItem(int id)
        {
            m_ItemId = id;
        }

        char  IMenuItem.AlphabeticShortcut
        {
	        get { throw new NotImplementedException(); }
        }

        int  IMenuItem.GroupId
        {
	        get { throw new NotImplementedException(); }
        }

        bool  IMenuItem.HasSubMenu
        {
	        get { throw new NotImplementedException(); }
        }

        Android.Graphics.Drawables.Drawable  IMenuItem.Icon
        {
	        get { throw new NotImplementedException(); }
        }

        Intent  IMenuItem.Intent
        {
	        get { throw new NotImplementedException(); }
        }

        bool  IMenuItem.IsCheckable
        {
	        get { throw new NotImplementedException(); }
        }

        bool  IMenuItem.IsChecked
        {
	        get { throw new NotImplementedException(); }
        }

        bool  IMenuItem.IsEnabled
        {
	        get { throw new NotImplementedException(); }
        }

        bool  IMenuItem.IsVisible
        {
	        get { throw new NotImplementedException(); }
        }

        private int m_ItemId;
        int IMenuItem.ItemId { get { return m_ItemId; } }

        IContextMenuContextMenuInfo  IMenuItem.MenuInfo
        {
	        get { throw new NotImplementedException(); }
        }

        char  IMenuItem.NumericShortcut
        {
	        get { throw new NotImplementedException(); }
        }

        int  IMenuItem.Order
        {
	        get { throw new NotImplementedException(); }
        }

        IMenuItem  IMenuItem.SetAlphabeticShortcut(char alphaChar)
        {
 	        throw new NotImplementedException();
        }

        IMenuItem  IMenuItem.SetCheckable(bool checkable)
        {
 	        throw new NotImplementedException();
        }

        IMenuItem  IMenuItem.SetChecked(bool @checked)
        {
 	        throw new NotImplementedException();
        }

        IMenuItem  IMenuItem.SetEnabled(bool enabled)
        {
 	        throw new NotImplementedException();
        }

        IMenuItem  IMenuItem.SetIcon(int iconRes)
        {
 	        throw new NotImplementedException();
        }

        IMenuItem  IMenuItem.SetIcon(Android.Graphics.Drawables.Drawable icon)
        {
 	        throw new NotImplementedException();
        }

        IMenuItem  IMenuItem.SetIntent(Intent intent)
        {
 	        throw new NotImplementedException();
        }

        IMenuItem  IMenuItem.SetNumericShortcut(char numericChar)
        {
 	        throw new NotImplementedException();
        }

        IMenuItem  IMenuItem.SetOnMenuItemClickListener(IMenuItemOnMenuItemClickListener menuItemClickListener)
        {
 	        throw new NotImplementedException();
        }

        IMenuItem  IMenuItem.SetShortcut(char numericChar, char alphaChar)
        {
 	        throw new NotImplementedException();
        }

        IMenuItem  IMenuItem.SetTitle(Java.Lang.ICharSequence title)
        {
 	        throw new NotImplementedException();
        }

        IMenuItem  IMenuItem.SetTitle(int title)
        {
 	        throw new NotImplementedException();
        }

        IMenuItem  IMenuItem.SetTitleCondensed(Java.Lang.ICharSequence title)
        {
 	        throw new NotImplementedException();
        }

        IMenuItem  IMenuItem.SetVisible(bool visible)
        {
 	        throw new NotImplementedException();
        }

        ISubMenu  IMenuItem.SubMenu
        {
	        get { throw new NotImplementedException(); }
        }

        Java.Lang.ICharSequence  IMenuItem.TitleCondensedFormatted
        {
	        get { throw new NotImplementedException(); }
        }

        Java.Lang.ICharSequence  IMenuItem.TitleFormatted
        {
	        get { throw new NotImplementedException(); }
        }

        IntPtr  Android.Runtime.IJavaObject.Handle
        {
	        get { throw new NotImplementedException(); }
        }
}

    /// <summary>
    /// MenuItemActionBarAction will call teh main activitiess "OnOptionsItemSelected", which allows us to re-use code.
    /// </summary>
    class MenuItemActionBarAction: ActionBarAction
    {
        private Activity m_Activity;
        private ActionBarMenuItem m_MenuItem;
        public int MenuItemId;
        public MenuItemActionBarAction(Context context, Activity activity, int menuId, int drawable, int popupID)
        {
            mDrawable = drawable;
            mContext = context;
            m_Activity = activity;
            MenuItemId = menuId;
            m_MenuItem = new ActionBarMenuItem(menuId);
            PopUpMessage = popupID;
        }

        public override int GetDrawable()
        {
            return mDrawable;
        }

        public override void PerformAction(View view)
        {
            try
            {
                m_Activity.OnOptionsItemSelected(m_MenuItem);
            }
            catch(Exception ex)
            {
            }
        }
    }
}
