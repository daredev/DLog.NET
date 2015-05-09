# DLog.NET <img src="https://travis-ci.org/daredev/DLog.NET.svg?branch=master" />
Custom Logger for .NET

## Usage
1. Add DLog.NET from nuget: https://www.nuget.org/packages/DLog.NET/ or by Manage Nuget Packages option in Visual Studio
2. Instantiate DLogger class: `DLogger myLogger = new DLogger();`
3. Add path to log file that you want to use by: `myLogger.AddTargetFile(@"C:\Logs\MyLog.txt");`
4. (optional) Add Winforms controls for log output `myLogger.AddTargetTextBox(tbLog);`. Currently supported controls are: `TextBox`, `NotifyIcon`, `ProgressBar`, `ToolStripProgressBar`
5. Use `Write(string message, (optional) int progress);` to output message to all log targets. Optional `progress` parameter is used for setting progress value on ProgressBar controls.

## Plans
1. Change parameters for `AddTarget` methods to accept Interfaces instead of Explicit classes
2. Support for WPF and ASP.NET

### Issues
Issues, pull requests, forks, etc... will be handled here in Github, so feel free to use all github functionalities provided.

## Contact
You can contact me directly by writing email to piczok(at)gmail.com
