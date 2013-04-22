using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Java.Lang;

namespace LegacyBar.Library.BarActions
{
    /// <summary>
    /// This is just a stub so we can set the id.
    /// In the long run we should really read in the full menu item and use it.
    /// </summary>
    public class LegacyBarMenuItem : IMenuItem
    {
        public LegacyBarMenuItem(int id, IntPtr handle)
        {
            ItemId = id;
            Handle = handle;
        }

        public IntPtr Handle { get; private set; }
        public char AlphabeticShortcut { get; private set; }
        public int GroupId { get; private set; }
        public bool HasSubMenu { get; private set; }
        public Drawable Icon { get; private set; }
        public Intent Intent { get; private set; }
        public bool IsActionViewExpanded { get; private set; }
        public bool IsCheckable { get; private set; }
        public bool IsChecked { get; private set; }
        public bool IsEnabled { get; private set; }
        public bool IsVisible { get; private set; }
        public int ItemId { get; private set; }
        public IContextMenuContextMenuInfo MenuInfo { get; private set; }
        public char NumericShortcut { get; private set; }
        public int Order { get; private set; }
        public ISubMenu SubMenu { get; private set; }
        public ICharSequence TitleFormatted { get; private set; }
        public ICharSequence TitleCondensedFormatted { get; private set; }
        public View ActionView { get; private set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool CollapseActionView()
        {
            throw new NotImplementedException();
        }

        public bool ExpandActionView()
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetActionView(View view)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetActionView(int resId)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetAlphabeticShortcut(char alphaChar)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetCheckable(bool checkable)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetChecked(bool @checked)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetEnabled(bool enabled)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetIcon(Drawable icon)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetIcon(int iconRes)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetIntent(Intent intent)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetNumericShortcut(char numericChar)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetOnMenuItemClickListener(IMenuItemOnMenuItemClickListener menuItemClickListener)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetShortcut(char numericChar, char alphaChar)
        {
            throw new NotImplementedException();
        }



        public IMenuItem SetTitle(int title)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetTitle(ICharSequence title)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetTitleCondensed(ICharSequence title)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetVisible(bool visible)
        {
            throw new NotImplementedException();
        }

#if __ANDROID_14__
        public IMenuItem SetOnActionExpandListener(IMenuItemOnActionExpandListener listener)
        {
            throw new NotImplementedException();
        }

        public void SetShowAsAction(ShowAsAction actionEnum)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetShowAsActionFlags(ShowAsAction actionEnum)
        {
            throw new NotImplementedException();
        }

        public IMenuItem SetActionProvider(ActionProvider actionProvider)
        {
            throw new NotImplementedException();
        }

        public ActionProvider ActionProvider { get; private set; }
#endif
    }
}