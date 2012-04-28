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
 * Modifications by: James Montemagno <motz2k1@oh.rr.com>
 */

using Android.Views;

namespace MonoDroid.ActionBarSample
{
    public interface IActionBarAction
    {
        int GetDrawable();
        void PerformAction(View view);
        //sets the current position to determine if it shoudl be put in the action bar or not.
        int CurrentPosition { get; set;}
        //if set to true then no matter what this actionbaritem will be in the action bar.
        bool ForceInActionBar { get; set; }
        int PopUpMessage { get; set; }//displays if user holds down action bar item.
    }
}