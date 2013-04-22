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
