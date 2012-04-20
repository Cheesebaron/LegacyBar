using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.ActionBarSample
{
    /**
         * Definition of an action that could be performed, along with a icon to
         * show.
         */
    public class ActionBarAction : Java.Lang.Object
    {
        private int mDrawable;
        private Context mContext;
        private Intent mIntent;

        public ActionBarAction(Context context, Intent intent, int drawable)
        {
            mDrawable = drawable;
            mContext = context;
            mIntent = intent;
        }

        public int GetDrawable()
        {
            return mDrawable;
        }

        public void PerformAction(View view)
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