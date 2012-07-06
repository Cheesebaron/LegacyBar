/*
 * Copyright (C) 2010 Johan Nilsson <http://markupartist.com>
 *
 * Original (https://github.com/johannilsson/android-actionbar) Ported to Mono for Android
 * Copyright (C) 2012 Tomasz Cielecki <tomasz@ostebaronen.dk>
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

using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.ActionBarSample
{
    public class MyActionBarAction : ActionBarAction
    {
        public MyActionBarAction(Context context, Intent intent, int drawable)
        {
            mDrawable = drawable;
            mContext = context;
            mIntent = intent;
        }

        public override int GetDrawable()
        {
            return mDrawable;
        }

        public override void PerformAction(View view)
        {
            try
            {
                mContext.StartActivity(mIntent);
            }
            catch (ActivityNotFoundException e)
            {
                Toast.MakeText(mContext,
                        mContext.GetText(Resource.String.actionbar_activity_not_found),
                        ToastLength.Short).Show();
            }
        }
    }
}