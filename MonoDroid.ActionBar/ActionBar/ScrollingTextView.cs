using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace MonoDroid.ActionBarSample
{
    public class ScrollingTextView : TextView
    {
        public ScrollingTextView(System.IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public ScrollingTextView(Context context, IAttributeSet attrs,
            int defStyle) : base(context, attrs, defStyle)
        {
        }

        public ScrollingTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public ScrollingTextView(Context context) : base(context)
        {
        }

        protected override void OnFocusChanged(bool gainFocus, FocusSearchDirection direction, Android.Graphics.Rect previouslyFocusedRect)
        {
            if (gainFocus)
                base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);
        }

        public override void OnWindowFocusChanged(bool hasWindowFocus)
        {
            if (hasWindowFocus)
                base.OnWindowFocusChanged(hasWindowFocus);
        }

        public override bool IsFocused
        {
            get
            {
                return true;
            }
        }
    }
}