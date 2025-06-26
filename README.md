# BoldBI Embedding Winforms Sample

This BoldBI Winforms sample repository contains the Dashboard embedding sample. This sample demonstrates how to embed the dashboard which is available in your Bold BI server.

This section guides you in using the Bold BI dashboard in your Winforms sample application.

* [Requirements to run the demo](#requirements-to-run-the-demo)
* [Using the Winforms sample](#using-the-winforms-sample)
* [Online Demos](#online-demos)
* [Documentation](#documentation)

## Requirements to run the demo

The samples require the following requirements to run.

* [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)
* [.NET Framework 4.8](https://dotnet.microsoft.com/en-us/download/dotnet-framework)

## Using the Winforms sample

* Open the Winforms sample's solution file `BoldBI.Winforms.sln` in Visual Studio.

* Open the EmbedProperties.cs file and change the following properties as per your Bold BI Server.

| RootUrl        | Dashboard Server URL (Eg: <http://localhost:5000/bi>, <http://demo.boldbi.com/bi>).                                                |
|----------------|--------------------------------------------------------------------------------------------------------------------------------|
| SiteIdentifier | For the Bold BI Enterprise edition, it should be like `site/site1`. For Bold BI Cloud, it should be an empty string.          |
| Environment    | Your Bold BI application environment. (If Cloud, you should use `cloud`, if Enterprise, you should use `enterprise`).        |
| DashboardId    | Set the item id of the dashboard to embed from BI server.                                                                      |
| UserEmail      | UserEmail of the Admin in your Bold BI, which would be used to get the dashboard list.                                        |
| EmbedSecret    | Get your EmbedSecret key from the Embed tab by enabling the `Enable embed authentication` on the Administration page. For more details: [Embed settings](https://help.boldbi.com/embedded-bi/site-administration/embed-settings/?utm_source=github&utm_medium=backlinks)     |

Please refer to the [help documentation](https://help.boldbi.com/embedded-bi/javascript-based/samples/v3.3.40-or-later/winforms/#how-to-run-the-sample?utm_source=github&utm_medium=backlinks) to know how to run the sample.

## Online Demos

Look at the Bold BI Embedding sample to live demo [here](https://samples.boldbi.com/embed?utm_source=github&utm_medium=backlinks).

## Documentation

A complete Bold BI Embedding documentation can be found on [Bold BI Embedding Help](https://help.boldbi.com/embedded-bi/javascript-based/?utm_source=github&utm_medium=backlinks).

> NOTE:  To mitigate issues related to NuGet packages, run the following command in package manager console Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r.
