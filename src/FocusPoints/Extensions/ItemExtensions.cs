namespace FocusPoints.Extensions
{
	using System.Linq;
	using FocusPoints.Models;
	using Sitecore.Data.Items;
	using Sitecore.Data.Managers;

	public static class ItemExtensions
	{
		public static FocusPoint GetFocusPoint(this Item item)
		{
			if (!IsImage(item))
			{
				return null;
			}

			var focusPoint = IsUnversionedImage(item) ? item[UnversionedImageConstants.FocusPointFieldId] : item[VersionedImageConstants.FocusPointFieldId];
			return new FocusPoint(focusPoint);
		}

		public static bool SetFocusPoint(this Item item, FocusPoint focusPoint)
		{
			var focusPointString = focusPoint.ToString();
			if (item == null || focusPoint == null || !IsImage(item) || string.IsNullOrWhiteSpace(focusPointString))
			{
				return false;
			}

			using (new Sitecore.SecurityModel.SecurityDisabler())
			{
				try
				{
					item.Editing.BeginEdit();
					if (IsUnversionedImage(item))
					{
						item[UnversionedImageConstants.FocusPointFieldId] = focusPointString;
					}
					else
					{
						item[VersionedImageConstants.FocusPointFieldId] = focusPointString;
					}

					item.Editing.EndEdit();
					return true;
				}
				catch
				{
					item.Editing.CancelEdit();
				}
			}

			return false;
		}

		private static bool IsImage(Item item)
		{
			if (item.TemplateID == UnversionedImageConstants.TemplateId || item.TemplateID == VersionedImageConstants.TemplateId)
			{
				return true;
			}

			var template = TemplateManager.GetTemplate(item);
			var baseTemplates = template.GetBaseTemplates();

			return baseTemplates.Any(t => t.ID == UnversionedImageConstants.TemplateId || t.ID == VersionedImageConstants.TemplateId);
		}

		private static bool IsUnversionedImage(Item item)
		{
			if (item.TemplateID == UnversionedImageConstants.TemplateId)
			{
				return true;
			}

			var template = TemplateManager.GetTemplate(item);
			var baseTemplates = template.GetBaseTemplates();

			return baseTemplates.Any(t => t.ID == UnversionedImageConstants.TemplateId);
		}
	}
}