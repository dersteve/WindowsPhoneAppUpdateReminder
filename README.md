WindowsPhoneAppUpdateReminder
=============================

An update reminder component for your Windows Phone App.

How to use the App Update Reminder
==================================

The Update Reminder as available on [nuget] [1] and can be installed either through the Package Manager Console and typing

Install-Package WPUpdateReminder

or by going to the "Add reference..." -> "manage Nuget Packages..." Dialog and searching for WPUpdateReminder

Once it's part of your solution you can just instanciate the AppUpdateReminder and call the Init method, like so:

    var updateReminder = new Hedgehog.UpdateReminder.AppUpdateReminder
    {
        CurrentVersion = "1.5",
        RecurrencePerUsageCount = 5,
        UpdateValidator = new Hedgehog.UpdateReminder.UpdateValidator.FileUpdateValidator("http://myapp.com/version.txt")
    };
    updateReminder.Init();

CurrentVersion specifies the current version of your app.
RecurrencePerUsageCount defines how often the validator checks for new version. In our example here it looks every 5 times the user opnes the app for an update.
UpdateValidator defines the way it looks for an update. At the moment the AppUpdateReminder ships with one option, which is a filebased implementation. The validator looks at the provided url for that file and checks whether the content of the file matches the content of the CurrentVersion Property.

In our example the file at myapp.com/version.txt need to have the following content assuming the latest version of the app is 1.6
<pre>
1.6
</pre>

That's right! Just "1.6" needs to be the content of that file. That way we keep the network traffic low and reduce the moment of parsing the downloaded file.

How is the user experience
==========================

The initialisation can happen on a single page during the load event. If the update validator does not find a newer version, nothing will happen. If the update validator does find an update available it will popup a messagebox asking the user to go straight to the marketplace app page to update the application. The user can either accept, which brings him straight to the app page of the current application, where he can initiate the update process OR you can decline the question by clicking cancel on message box, which makes the message box disappear.

[1]: https://nuget.org/packages/WPUpdateReminder/ "nuget"
