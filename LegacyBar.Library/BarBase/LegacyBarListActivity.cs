/*
 * Copyright (C) 2013 LegacyBar - @Cheesebaron & @JamesMontemagno
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *    
 *          http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using Android.App;
using Android.Content;
using Android.Views;
using LegacyBar.Library.BarActions;

namespace LegacyBar.Library.BarBase
{
    [Activity(Label = "")]
    public class LegacyBarListActivity : ListActivity
    {
        public Bar.LegacyBar LegacyBar { get; set; }
        public int MenuId { get; set; }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            if (LegacyBar == null)
                return base.OnPrepareOptionsMenu(menu);

            menu.Clear();
            MenuInflater.Inflate(MenuId, menu);

            for (var i = 0; i < menu.Size(); i++)
            {
                var menuItem = menu.GetItem(i);
                menuItem.SetVisible(!LegacyBar.MenuItemsToHide.Contains(menuItem.ItemId));
            }
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            if(MenuId > 0)
                MenuInflater.Inflate(MenuId, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public void AddHomeAction(Type activity, int resId)
        {
            var homeIntent = new Intent(this, activity);
            homeIntent.AddFlags(ActivityFlags.ClearTop);
            homeIntent.AddFlags(ActivityFlags.NewTask);
            LegacyBar.SetHomeAction(new DefaultLegacyBarAction(this, homeIntent, resId));
            LegacyBar.SetDisplayHomeAsUpEnabled(true);
        }


        public void AddHomeAction(Action action, int resId, bool isHomeAsUpEnabled = true)
        {
            LegacyBar.SetHomeAction(new ActionLegacyBarAction(this, action, resId));
            LegacyBar.SetDisplayHomeAsUpEnabled(isHomeAsUpEnabled);
        }
    }
}