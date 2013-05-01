This guide assumes that you have loaded LegacyBar either through reference to the DLL you have built or the project or by loading the xpkg from the Xamarin Component Store.

<blockquote><p>After loading xpkg or adding reference to DLL ensure you also add a reference to Mono.Android.Support.v4 to your project that is using LegacyBar</p></blockquote>

## Creating the first Action Bar
Start by creating a new Android Layout similar to this, lets call it `main.axml`:
```
<?xml version="1.0" encoding="utf-8"?>

<!-- xmlns:ab must match your app namespace defined in AndroidManifest.xml -->
<LinearLayout 
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:ab="http://schemas.android.com/apk/res/legacybar.sample"
    android:orientation="vertical"
    android:layout_width="fill_parent"
    android:layout_height="fill_parent">

    <legacybar.library.bar.LegacyBar
        android:id="@+id/actionbar"
        style="@style/actionbar"
        ab:title="Cool beans!"/>

</LinearLayout>
```

Then create a new `Activity` but instead of inheriting from `Activity` inherit from either of these:
* `LegacyBarActivity`
* `LegacyBarListActivity`
* `LegacyBarFragmentActivity`

The classes above contain helpers and properties which makes it easier to use LegacyBar, however you can use LegacyBar from a normal `Activity` as well. In this example we are simply using `LegacyBarActivity`:

```csharp
[Activity(Label = "Home Activity", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
        Icon = "@drawable/icon", Theme = "@style/MyTheme")]
public class HomeActivity : LegacyBarActivity
{
    protected override void OnCreate(Bundle bundle)
    {
        base.OnCreate(bundle);

        // Set our view from the "main.axml" layout resource
        SetContentView(Resource.Layout.main);
        // Find LegacyBar and assign the value to the LegacyBar property
        LegacyBar = FindViewById<Library.Bar.LegacyBar>(Resource.Id.actionbar);
        // Needs to be assigned to be able to calculate screen values
        LegacyBar.CurrentActivity = this;
        // Set a nice home logo
        LegacyBar.SetHomeLogo(Resource.Drawable.MyLogo);
    }
}
```

We added a bit to the newly created `Activity`. After setting the `ContentView` you will notice that we are locating the `LegacyBar` `View` which is located in `main.axml` view we created just before. Then we have to set the `CurrentActivity` property in order for `LegacyBar` to be able to calculate positions in the `View` for menu items. Then we set a Home Logo.

That is it. You have created your first Activity with a LegacyBar! But wait we are not quite done yet, you will probably want to have some Actions up in the LegacyBar.

## Adding Actions
Adding Actions to the LegacyBar is pretty straight forward and has a couple of different ways to add Actions, Action and HomeActions. Let us start by adding one to the Home button, we created in the example above.

### Home Actions
In the Base Activities we supply there is a nice helper method called `AddHomeAction` it supports two types of ways to use it. Either by supplying it with a `Type` of the `Activity` you want it to launch or an instance of `System.Action`.

**1.**
First way is to supply it with the `Type` of `Activity` you want to launch when pushing the button. This is simple you just do:

```csharp
AddHomeAction(typeof(MyOtherActivity), Resources.Drawable.MyLogo);
```

This will launch `MyOtherActivity` when the Action is pressed, and the Home Action Icon will be presented with an arrow indicating navigation up the stack.

**2.**
The other way is to supply it with an `System.Action` instance:

```csharp
AddHomeAction(() => 
              {
                  //Your code here
              }, Resource.Drawable.MyIcon, true);
```

The `true` argument indicates that the arrow for the Home Action is shown.

### Actions
If you want to support devices with menu buttons, you will want to create a menu resource with definitions of the Actions, which will be represented in a old style menu, when pressing the menu button. This is the behavior of LegacyBar when it detects a hardware menu button on the device. Lets call the menu resource `mymenu.xml` with the following contents:

```
<?xml version="1.0" encoding="utf-8" ?>
<menu xmlns:android="http://schemas.android.com/apk/res/android">
  <item android:id="@+id/menu_search"
      android:icon="@drawable/ic_menu_search"
      android:title="@string/menu_string_search"/>
</menu>
```

This defines a menu item with the ID `menu_search`, using the Drawable `ic_menu_search` as an icon and the string `menu_string_search` from `Strings.xml`. The most important part here is the ID, as we are going to use that for the Action we are going to add to the LegacyBar.

Now in your Activity to add an Action to the LegacyBar we use the following code:

```csharp
var itemActionBarAction = new MenuItemLegacyBarAction(this, this, 
	Resource.Id.menu_search, 
	LegacyBar.LightIcons ? Resource.Drawable.ic_action_search : Resource.Drawable.ic_action_search_dark,
	Resource.String.menu_string_search)
						{
							ActionType = ActionType.Always
						};
LegacyBar.AddAction(itemActionBarAction);
```

Now note that we reuse the ID, which we defined for the menu item we just created. This is due to in the code behind, when activating an Action, `MenuItemLegacyBarAction` calls the `Activity`s `OnOptionsItemSelected` method, which allows the user to do whatever they want on that activation, by overriding `OnOptionsItemSelected`:

```csharp
public override bool OnOptionsItemSelected(IMenuItem item)
{
	switch (item.ItemId)
	{
		case Resource.Id.menu_search: 
			//Whatever you want here
			return true;
	}
	return base.OnOptionsItemSelected(item); 
}
```

You will also need to specify the Icon for the Action item, where you can check whether to use a Light or Dark version of the icon by checking `LegacyBar.LightIcons`, and also set the text representation for the item when it is shown in the overflow drop down.

The `ActionType` defined is an `Enum` and the different enumerations are described as follows:

* IfRoom - Dynamically calculates if the action can fit based on screen res
* Always - Force it in the action bar no matter what
* Never - Always put it in overflow (3.0+) or leave it in the old school menu bar.

The Action is then simply added with: `LegacyBar.AddAction(itemActionBarAction);`.

## Other Resources

* [Our GitHub repository](https://github.com/Cheesebaron/LegacyBar)
