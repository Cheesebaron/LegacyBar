This guide assumes that you have loaded LegacyBar either through reference to the DLL you have built or the project or by loading the component from the Xamarin Component Store. Also make sure to have Support.v4 referenced in your Application project.

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
In your Activity to add an Action to the LegacyBar we use the following code:

```csharp
var search = new LegacyBarAction
{
    ActionType = ActionType.IfRoom,
    Drawable =
        LegacyBar.LightIcons
            ? Resource.Drawable.ic_action_search
            : Resource.Drawable.ic_action_search_dark,
    PopUpMessage = Resource.String.ab_string_search
};
// Assign a click action
search.Clicked += delegate { /* do an action when clicked */ };
// Add it to the bar
LegacyBar.AddAction(search);
```

You will need to specify the Icon for the Action item, where you can check whether to use a Light or Dark version of the icon by checking `LegacyBar.LightIcons`, and also set the text representation for the item when it is shown in the overflow drop down or as a toast when long-pressed.

The `ActionType` defined is an `Enum` and the different enumerations are described as follows:

* IfRoom - Dynamically calculates if the action can fit based on screen res
* Always - Force it in the action bar no matter what
* Never - Always put it in overflow (3.0+) or leave it in the old school menu bar.

The Action is then simply added with: `LegacyBar.AddAction(myItem);`.

## Other Resources

* [Our GitHub repository](https://github.com/Cheesebaron/LegacyBar)
