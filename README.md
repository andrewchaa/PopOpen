# PopOpen
Open a document in the foreground

* Written in F#, just for fun
* Open a document with an associated application.
* Find the process of the application
* Push the application to the top most active window
* Install the nuget package to use it from C#
* [License: MIT](../master/LICENSE.md)

### To install

```
PM> Install-Package PopOpen
```

### logging

You can pass your logger method as lambda function for debugging purpose. The following examples passes log4net's Debug.

```
PopOpen.OpenD(path, s => _log.Debug(s));
```

Visit https://www.nuget.org/packages/PopOpen/ for more package details

### Technical details.

* Process.Start() will open a document with an associated application
* Restart Manager (rstrtmgr.dll) can identify what process is locking a file (https://msdn.microsoft.com/en-us/magazine/cc163450.aspx)
* It finds the opening process by checking who's locking the file
* GetWindowRect finds the the window
* SetWindowPos sets the window to top-most
