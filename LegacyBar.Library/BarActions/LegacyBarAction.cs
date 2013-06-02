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
 * 
 */

namespace LegacyBar.Library.BarActions
{
    public class LegacyBarAction : Java.Lang.Object, ILegacyBarAction
    {
        public int Drawable { get; set; }
        public event LegacyBarActionEventHandler Clicked;

        public int CurrentPosition { get; set; }
        public int PopUpMessage { get; set; }

        public ActionType ActionType { get; set; }

        public virtual void ActionClicked()
        {
            if (null != Clicked)
                Clicked(this, new LegacyBarActionEventArgs
                    {
                        ActionType = ActionType,
                        CurrentPosition = CurrentPosition
                    });
        }
    }
}