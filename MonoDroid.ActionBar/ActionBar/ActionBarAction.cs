using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.ActionBarSample
{
    /**
         * Definition of an action that could be performed, along with a icon to
         * show.
         */
    public abstract class ActionBarAction : Java.Lang.Object, IActionBarAction 
    {
        protected int mDrawable;
        protected Context mContext;
        protected Intent mIntent;

        public abstract int GetDrawable();

        public abstract void PerformAction(View view);

    }
}