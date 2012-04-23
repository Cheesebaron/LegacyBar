using Android.Views;

namespace MonoDroid.ActionBarSample
{
    public interface IActionBarAction
    {
        int GetDrawable();
        void PerformAction(View view);
    }
}