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

using Android.Views;

namespace LegacyBar.Library.BarActions
{
    public enum LegacyBarTheme
    {
        Custom,
        HoloLight,
        HoloGray,
        HoloBlack,
        HoloBlue
    }

    public enum ActionType
    {
        IfRoom,
        Never,
        WithText,//not supported
        Always,
        CollapseActionView
    }
    public interface ILegacyBarAction
    {
        int GetDrawable();

        void PerformAction(View view);

        /// <summary>
        /// Sets the current position to determine if it shoudl be put in the action bar or not.
        /// </summary>
        int CurrentPosition { get; set;}

        ActionType ActionType { get; set; }

        /// <summary>
        /// Displays if user holds down action bar item.
        /// </summary>
        int PopUpMessage { get; set; }
    }
}