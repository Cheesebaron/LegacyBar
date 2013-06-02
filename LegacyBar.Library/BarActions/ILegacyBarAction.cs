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

    public delegate void LegacyBarActionEventHandler(object sender, LegacyBarActionEventArgs e);

    public interface ILegacyBarAction
    {
        /// <summary>
        /// Sets or gets the drawable of the Action.
        /// </summary>
        int Drawable { get; set; }

        /// <summary>
        /// Event for getting whether the Action was clicked.
        /// </summary>
        event LegacyBarActionEventHandler Clicked;

        /// <summary>
        /// Sets the current position to determine if it should be put in the action bar or not.
        /// </summary>
        int CurrentPosition { get; set;}

        ActionType ActionType { get; set; }

        /// <summary>
        /// Displays if user holds down action bar item.
        /// </summary>
        int PopUpMessage { get; set; }

        void ActionClicked();
    }
}