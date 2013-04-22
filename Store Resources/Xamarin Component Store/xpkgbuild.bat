:: Grab xpkg from http://components.xamarin.com/submit/xpkg and put contents of the zip into xpkg

xpkg\xpkg.exe create legacybar-1.0.xam ^
	--name="LegacyBar" ^
	--summary="Add an ActionBar to all your Android Apps!" ^
	--website="https://github.com/Cheesebaron/LegacyBar" ^
	--details="Details.md" ^
	--license="License.md" ^
	--getting-started="GettingStarted.md" ^
	--icon="icons/legacybar_128x128.png" ^
	--icon="icons/legacybar_512x512.png" ^
	--library="android":"../../binaries/Release/LegacyBar.Library.dll" ^
	--publisher "LegacyBar" ^
	--sample="Android Sample. Demonstrates LegacyBar":"../../LegacyBar.sln"