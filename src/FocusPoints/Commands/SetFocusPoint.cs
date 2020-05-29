namespace FocusPoints.Commands
{
	using System.Collections.Specialized;
	using System.Linq;
	using FocusPoints.Extensions;
	using FocusPoints.Models;
	using Sitecore;
	using Sitecore.Resources.Media;
	using Sitecore.Shell.Framework.Commands;
	using Sitecore.Text;
	using Sitecore.Web.UI.Sheer;

	/// <summary>
	/// Image FocusPoints Command
	/// </summary>
	/// <seealso cref="Command" />
	public class SetFocusPoint : Command
	{
		/// <summary>
		/// Executes the command in the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Execute(CommandContext context)
		{
			var item = context.Items.FirstOrDefault();

			if (item != null)
			{
				var focusPoint = item.GetFocusPoint() ?? new FocusPoint(0, 0);
				var parameters = new NameValueCollection
				{
					{ "itemID", item.ID.ToString() },
					{ "imageUrl", MediaManager.GetMediaUrl(item) },
					{ "focusPoint", focusPoint.ToString() },
				};

				Context.ClientPage.Start(this, nameof(this.OpenDialog), parameters);
			}
		}

		/// <summary>
		/// Opens the dialog.
		/// </summary>
		/// <param name="args">The arguments.</param>
		protected void OpenDialog(ClientPipelineArgs args)
		{
			if (!args.IsPostBack)
			{
				UrlString url = new UrlString("/sitecore/client/Your Apps/FocusPoints");
				url.Add("imageUrl", args.Parameters["imageUrl"]);
				url.Add("focusPoint", args.Parameters["focusPoint"]);

				SheerResponse.ShowModalDialog(
					new ModalDialogOptions(url.ToString())
					{
						Width = "1030px",
						Height = "830px",
						Response = true,
						ForceDialogSize = true,
					});

				args.WaitForPostBack();
			}
			else if (args.HasResult)
			{
				// Return here after closing dialog
				var focusPoint = args.Result;

				if (string.IsNullOrWhiteSpace(focusPoint))
				{
					return;
				}

				var itemID = Sitecore.Data.ID.Parse(args.Parameters["itemID"]);
				var database = Sitecore.Data.Database.GetDatabase("master");
				var item = database.GetItem(itemID);

				if (item != null)
				{
					item.SetFocusPoint(new FocusPoint(focusPoint));
				}

				// Refresh item
				Context.ClientPage.SendMessage(this, string.Format("item:load(id={0},language={1},version={2})", item.ID, item.Language, item.Version));
			}
		}
	}
}