namespace FocusPoints.Helpers
{
	using System;
	using FocusPoints.Extensions;
	using FocusPoints.ImageService.Client;
	using Sitecore.Configuration;
	using Sitecore.Data.Items;
	using Sitecore.Resources.Media;

	public static class FocusPointsHelper
	{
		public static string GetMediaUrl(MediaItem mediaItem, MediaUrlOptions mediaUrlOptions, bool resizeOnly = false)
		{
			if ((Settings.GetBoolSetting("FocusPoints.DisabledInExperienceEditor", true) && Sitecore.Context.PageMode.IsExperienceEditor)
				|| !Settings.GetBoolSetting("FocusPoints.Enabled", true))
			{
				return MediaManager.GetMediaUrl(mediaItem, mediaUrlOptions);
			}

			if (mediaItem == null)
			{
				throw new ArgumentException("MediaItem should not be null");
			}

			if (mediaUrlOptions.Width <= 0)
			{
				throw new ArgumentException("Width must be a positive integer");
			}

			if (mediaUrlOptions.Height <= 0)
			{
				throw new ArgumentException("Height must be a positive integer");
			}

			var tokenBuilder = new TokenBuilder(
				Settings.GetSetting("FocusPoints.Client.Issuer"),
				Settings.GetSetting("FocusPoints.Client.Secret"))
			{
				Action = resizeOnly ? TokenBuilder.ActionType.Resize : TokenBuilder.ActionType.Transform,
				Url = new Uri(HashingUtils.ProtectAssetUrl(
					MediaManager.GetMediaUrl(
						mediaItem,
						new MediaUrlOptions()
						{
							AlwaysAppendRevision = true,
							AlwaysIncludeServerUrl = true,
						}))),
				Width = mediaUrlOptions.Width,
				Height = mediaUrlOptions.Height,
			};

			var focusPoint = mediaItem.InnerItem.GetFocusPoint();
			if (!resizeOnly && focusPoint != null)
			{
				tokenBuilder.FocusPointX = focusPoint.X;
				tokenBuilder.FocusPointY = focusPoint.Y;
			}

			var urlBuilder = new UrlBuilder(
				Settings.GetSetting("FocusPoints.Client.EndpointUrl", "https://image.focuspoints.io"),
				Settings.GetSetting("FocusPoints.Client.TokenRequestParameterName", "_jwt"),
				tokenBuilder.ToString());

			return urlBuilder.ToString();
		}
	}
}
