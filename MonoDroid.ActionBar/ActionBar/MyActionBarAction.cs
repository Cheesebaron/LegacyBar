using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
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