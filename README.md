# BoldBI Embedding Winforms Sample

This BoldBI Winforms sample repository contains the Dashboard embedding sample. This sample demonstrates how to embed the dashboard which is available in your Bold BI server.

## Requirements

The samples require the following requirements to run.

* [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)
* [.NET Framework 4.8](https://dotnet.microsoft.com/en-us/download/dotnet-framework)

### Supported browsers
  
* Google Chrome, Microsoft Edge, and Mozilla Firefox.

## Configuration

* Please ensure you have enabled embed authentication on the `embed settings` page. If it is not currently enabled, please refer to the following image or detailed [instructions](https://help.boldbi.com/site-administration/embed-settings/#get-embed-secret-code?utm_source=github&utm_medium=backlinks) to enable it.

    ![Embed Settings](images/embed-settings.png)

* To download the `embedConfig.json` file, please follow this [link](https://help.boldbi.com/site-administration/embed-settings/#get-embed-configuration-file?utm_source=github&utm_medium=backlinks) for reference. Additionally, you can refer to the following image for visual guidance.

    ![Embed Settings Download](images/embed-settings-download.png)
    ![EmbedConfig Properties](images/embedconfig-properties.png)

* Copy the downloaded `embedConfig.json` file and paste it into the designated [location](https://github.com/boldbi/winforms-sample/tree/master/BoldBI.Winforms) within the application. Please ensure you have placed it in the application, as shown in the following image.

   ![EmbedConfig image](images/embedconfig-image.png)

## Developer IDE

* Visual Studio 2022(<https://visualstudio.microsoft.com/downloads/>)

### Run a Sample Using Visual Studio 2022

* Open the Winforms sample's solution file `BoldBI.Winforms.sln` in Visual Studio and run it.

    ![dashboard image](images/dashboard-view.png)

Please refer to the [help documentation](https://help.boldbi.com/embedded-bi/javascript-based/samples/v3.3.40-or-later/winforms/#how-to-run-the-sample?utm_source=github&utm_medium=backlinks) to know how to run the sample.

## Online Demos

Look at the Bold BI Embedding sample to live demo [here](https://samples.boldbi.com/embed?utm_source=github&utm_medium=backlinks).

## Documentation

A complete Bold BI Embedding documentation can be found on [Bold BI Embedding Help](https://help.boldbi.com/embedded-bi/javascript-based/?utm_source=github&utm_medium=backlinks).

> NOTE:  To mitigate issues related to NuGet packages, run the following command in package manager console Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r.