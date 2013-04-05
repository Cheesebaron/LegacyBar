/*
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
 */

using System.Collections.Generic;
using Android.App;
using LegacyBar.Library.BarActions;

namespace LegacyBar.Library.Bar
{
    public class LegacyBarUtils
    {
        /// <summary>
        /// determins if the action should be added and fits
        /// based on stats from http://developer.android.com/design/patterns/actionbar.html
        /// </summary>
        /// <param name="activity">Current Activity, needed for orientation and density</param>
        /// <param name="currentNumber">current position of the action</param>
        /// <param name="hasMenuButton"></param>
        /// <param name="actionType"></param>
        /// <returns>If it will fit :)</returns>
        public static bool ActionFits(int width, float density, int currentNumber, bool hasMenuButton, ActionType actionType)
        {
            if (actionType == ActionType.Always)
                return true;

            if (actionType == ActionType.Never)
                return false;

            if (density == 0.0)
                return true;
            density = (int)(width / density);//calculator DP of width.


            var max = 5;
            if(density < 360)
            {
                max = 2;
            }
            else if(density < 500)
            {
                max = 3;
            }
            else if(density < 598)//should be 600, but Galaxy nexus returns 598
            {
                max = 4;
            }

            if (!hasMenuButton)
                max--;

            return currentNumber < max;
        }


        public static void SetLegacyBarTheme(LegacyBar legacyBar, LegacyBarTheme theme)
        {
            switch (theme)
            {
                case LegacyBarTheme.HoloLight: //light gray
                    legacyBar.SeparatorColorRaw = Resource.Color.actionbar_separator_lightgray;
                    legacyBar.TitleColorRaw = Resource.Color.actionbar_title_lightgray;
                    legacyBar.ItemBackgroundDrawableRaw = Resource.Drawable.actionbar_btn_lightgray;
                    legacyBar.BackgroundDrawableRaw = Resource.Drawable.actionbar_background_lightgray;

                    legacyBar.DropdownDrawableRaw = Resource.Drawable.dropdown_btn_holo_light;
                    break;
                case LegacyBarTheme.HoloGray: //dark gray
                    legacyBar.SeparatorColorRaw = Resource.Color.actionbar_separator_darkgray;
                    legacyBar.TitleColorRaw = Resource.Color.actionbar_title_darkgray;
                    legacyBar.ItemBackgroundDrawableRaw = Resource.Drawable.actionbar_btn_darkgray;
                    legacyBar.BackgroundDrawableRaw = Resource.Drawable.actionbar_background_darkgray;

                    legacyBar.DropdownDrawableRaw = Resource.Drawable.dropdown_btn_holo_dark;
                    break;
                case LegacyBarTheme.HoloBlue://blue
                    legacyBar.SeparatorColorRaw = Resource.Color.actionbar_separator_blue;
                    legacyBar.TitleColorRaw = Resource.Color.actionbar_title_blue;
                    legacyBar.ItemBackgroundDrawableRaw = Resource.Drawable.actionbar_btn_blue;
                    legacyBar.BackgroundDrawableRaw = Resource.Drawable.actionbar_background_blue;
                    legacyBar.DropdownDrawableRaw = Resource.Drawable.dropdown_btn_holo_dark;
                    break;
                case LegacyBarTheme.HoloBlack: //black
                    legacyBar.SeparatorColorRaw = Resource.Color.actionbar_separator_black;
                    legacyBar.TitleColorRaw = Resource.Color.actionbar_title_black;
                    legacyBar.ItemBackgroundDrawableRaw = Resource.Drawable.actionbar_btn_black;
                    legacyBar.BackgroundDrawableRaw = Resource.Drawable.actionbar_background_black;
                    legacyBar.DropdownDrawableRaw = Resource.Drawable.dropdown_btn_holo_dark;
                    break;
                default:

                    legacyBar.DropdownDrawableRaw = legacyBar.LightIcons ? Resource.Drawable.dropdown_btn_holo_light : Resource.Drawable.dropdown_btn_holo_dark;
                    break;
            }
        }

        public static void SetBottomLegacyBarTheme(LegacyBar legacyBar, LegacyBarTheme theme)
        {
            switch (theme)
            {
                case LegacyBarTheme.HoloLight: //light gray
                    legacyBar.SeparatorColorRaw = Resource.Color.bottomactionbar_separator;
                    legacyBar.TitleColorRaw = Resource.Color.actionbar_title_lightgray;
                    legacyBar.ItemBackgroundDrawableRaw = Resource.Drawable.bottomactionbar_btn_lightgray;
                    legacyBar.BackgroundDrawableRaw = Resource.Drawable.bottomactionbar_background_lightgray;
                    legacyBar.DropdownDrawableRaw = Resource.Drawable.dropdown_btn_holo_light;
                    break;
                case LegacyBarTheme.HoloGray: //dark gray
                    legacyBar.SeparatorColorRaw = Resource.Color.bottomactionbar_separator;
                    legacyBar.TitleColorRaw = Resource.Color.actionbar_title_darkgray;
                    legacyBar.ItemBackgroundDrawableRaw = Resource.Drawable.bottomactionbar_btn_darkgray;
                    legacyBar.BackgroundDrawableRaw = Resource.Drawable.bottomactionbar_background_darkgray;

                    legacyBar.DropdownDrawableRaw = Resource.Drawable.dropdown_btn_holo_dark;
                    break;
                case LegacyBarTheme.HoloBlue://blue
                    legacyBar.SeparatorColorRaw = Resource.Color.bottomactionbar_separator;
                    legacyBar.TitleColorRaw = Resource.Color.actionbar_title_blue;
                    legacyBar.ItemBackgroundDrawableRaw = Resource.Drawable.bottomactionbar_btn_blue;
                    legacyBar.BackgroundDrawableRaw = Resource.Drawable.bottomactionbar_background_blue;
                    legacyBar.DropdownDrawableRaw = Resource.Drawable.dropdown_btn_holo_dark;
                    break;
                case LegacyBarTheme.HoloBlack: //black
                    legacyBar.SeparatorColorRaw = Resource.Color.bottomactionbar_separator;
                    legacyBar.TitleColorRaw = !legacyBar.LightIcons ? Resource.Color.actionbar_title_black : Resource.Color.actionbar_title;
                    legacyBar.ItemBackgroundDrawableRaw = Resource.Drawable.bottomactionbar_btn_black;
                    legacyBar.BackgroundDrawableRaw = Resource.Drawable.bottomactionbar_background_black;
                    legacyBar.DropdownDrawableRaw = Resource.Drawable.dropdown_btn_holo_dark;
                    break;
                default:
                    legacyBar.DropdownDrawableRaw = legacyBar.LightIcons ? Resource.Drawable.dropdown_btn_holo_light : Resource.Drawable.dropdown_btn_holo_dark;
                    break;
            }
        }

        /// <summary>
        /// A LinkedList that holds a list of Action.
        /// </summary>
        public class ActionList : LinkedList<LegacyBarAction>
        {
        }
    }
}