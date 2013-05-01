:: Grab xpkg from http://components.xamarin.com/submit/xpkg and put contents of the zip into xpkg

xpkg\xpkg.exe create legacybar-1.2.xam ^
	--name="LegacyBar" ^
	--summary="A backward compatible ActionBar, which supports Android 2.1 and up." ^
	--website="https://github.com/Cheesebaron/LegacyBar" ^
	--details="Details.md" ^
	--license="License.md" ^
	--getting-started="GettingStarted.md" ^
	--icon="icons/legacybar_128x128.png" ^
	--icon="icons/legacybar_512x512.png" ^
	--library="android":"../../binaries/Xamarin.Android/Release/LegacyBar.Library.dll" ^
	--publisher "LegacyBar" ^
	--sample="Android Sample. Demonstrates LegacyBar":"../../LegacyBar.sln"