using System;
using Android.App;
using Android.Content;
using Android.Views;
using LegacyBar.Library.BarActions;

namespace LegacyBar.Library.BarBase
{
    public class LegacyBarActivityUtils
    {
        public static void AddHomeAction(Bar.LegacyBar legacyBar, Activity activity, Type typeOfActivity, int resId)
        {
            var homeIntent = new Intent(activity, typeOfActivity);
            homeIntent.AddFlags(ActivityFlags.ClearTop);
            homeIntent.AddFlags(ActivityFlags.NewTask);
            var action = new LegacyBarAction { Drawable = resId };
            action.Clicked += delegate { activity.StartActivity(homeIntent); };
            legacyBar.SetHomeAction(action);
            legacyBar.SetDisplayHomeAsUpEnabled(true);
        }

        public static void AddHomeAction(Bar.LegacyBar legacyBar, Action action, int resId, bool isHomeAsUpEnabled = true)
        {
            var barAction = new LegacyBarAction
                {
                    Drawable = resId
                };
            barAction.Clicked += delegate { action.Invoke(); };
            legacyBar.SetHomeAction(barAction);
            legacyBar.SetDisplayHomeAsUpEnabled(isHomeAsUpEnabled);
        }
    }
}