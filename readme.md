
# Table of Contents

- [Introduction](#introduction)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Serving images from your own (custom) domain](#serving-images-from-your-own-custom-domain)
- [Build and Publish solution in Visual Studio](#build-and-publish-solution-in-visual-studio)

# Introduction

This module enables integration of the (paid) services of [FocusPoints.io](https://focuspoints.io/) into your Sitecore solution. It does not work on its own, without these services. To get started you can get a 30-day trial which requires no payment info and it is obligation free

# Installation

## Sitecore module

Download the module from https://github.com/Focus-Points/focuspoints-sitecore/releases or go to the `Sitecore-packages` folder and install the module into Sitecore. Afterwards publish all the items under `/sitecore/templates/System/Media`.

# Configuration

In order to start using the FocusPoints module functionality in the website you need to provide it with the correct credentials. Open the file `/App_config/Include/FocusPoints.config` and change the issuer and secret provided by [FocusPoints.io](https://focuspoints.io/).

## Configuration settings

You can also change some other settings. The following settings are supported.

| Name | Default Value | Required | Description |
|-|-|-|-|
| `FocusPoints.Enabled` | `true` | False | Enable FocusPoints or fallback to default Sitecore MediaManager.GetMediaUrl() |
| `FocusPoints.DisabledInExperienceEditor` | `true`| False | Don't use FocusPoints image when in the Experience Editor |
| `FocusPoints.Client.EndpointUrl` | `https://image.focuspoints.io` | False | The FocusPoints endpoint |
| `FocusPoints.Client.TokenRequestParameterName` | `_jwt` | False | The name of the request parameter containing the token |
| `FocusPoints.Client.Issuer` | | True | Your FocusPoints issuer |
| `FocusPoints.Client.Secret` | | True | Your FocusPoints secret |

# Usage

The FocusPoints module comes with a user interface to set a focus point for images in the Media Library. If an image is used multiple times the same focus point will be used each time. In your code you can use the FocusPointsHelper that will do all the focussing and scaling for you.

## Setting a focus point

Setting a focus point is very easy.

- Go to the image in the Media Library
- Click on **Set focus point** under the Media tab (automatically opened in the ribbon on the top of the page)
- Click on the image to set a focus point (you can see a preview in the same window)
- Save and publish the item

## Using FocusPointsHelper

Instead of using `MediaManager.GetMediaUrl()` to return the Sitecore image URL you can use `FocusPointsHelper.GetMediaUrl()` to return the FocusPoints image URL.

To use the helper, first reference the `FocusPoints.dll`, then add a `using FocusPoints.Helpers;` to your code and then use the example code provided below. This will return a `string` with the URL to the image.

### Example

```csharp
var image = FocusPointsHelper.GetMediaUrl(
	image.MediaItem,
	new MediaUrlOptions { Width = 1200, Height = 250 },
	true) // Optional: resizeOnly (default: false)
```

### FocusPointsHelper.GetMediaUrl() Arguments

| Argument | Type | Default value | Required | Description |
|-|-|-|-|-|
| `mediaItem` | `Sitecore.Data.Items.MediaItem` | | True | The Media Item to resize (if FocusPoints is disabled this argument is passed straight to `MediaManager.GetMediaUrl()`)|
| `mediaUrlOptions` | `Sitecore.Resources.Media.MediaUrlOptions` | | True | Set the width and height of the image (if FocusPoints is disabled this argument is passed straight to `MediaManager.GetMediaUrl()`)|
| `resizeOnly` | `bool` | `false` | False | Only resize and don't use the focus point |

# Serving images from your own (custom) domain

When using FocusPoints the domain your images are served from will be `images.focuspoints.io`. If this is undesirable behavior it is possible to serve the images from your own (custom) domain by using a proxy. In order to do so use the following steps:

- Set the FocusPoints URL to a relative path (e.g. `/images/`) or custom domain (e.g. `https://focuspoints.yourdomain.com`). You can do this by opening `/App_config/Include/FocusPoints.config` and setting `FocusPoints.Client.EndpointUrl` to the relative path or custom domain.
- Create a Reverse Proxy in Internet Information Services (IIS) and proxy all requests for the `/images/` path or custom domain to `https://images.focuspoints.io`.

# Contribute

Instead of directly using the package (found under [installation](#installation)) you can also build your own solution.

## Build and Publish solution in Visual Studio

Follow the steps below to build and publish the solution in Visual Studio.

- Make a copy of `gulp-config.js.example` and remove the `.example` extention. Do the same with `TdsGlobal.config.example`.
- Change the paths in the copied files to match your own environment.
- Open the solution folder in a **Command Prompt** and run `npm install`.
- Open the solution in **Visual Studio**.
- Restore the **NuGet** packages.
- Go to the **Task Runner Explorer** and run the task **Build-and-Publish-Solution**.

## Attribution

This code uses the [jquery-focuspoint](https://github.com/jonom/jquery-focuspoint) plugin created by [Jono Menz (jonom)](https://jonomenz.com/).