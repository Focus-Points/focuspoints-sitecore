namespace FocusPoints.ImageService.Client
{
	using System;
	using System.Web;

	public class UrlBuilder
	{
		public string EndpointUrl { get; set; }

		public string TokenRequestParameterName { get; set; }

		public string Token { get; set; }

		public UrlBuilder(string endpointUrl, string tokenRequestParameterName, string token)
		{
			if (string.IsNullOrWhiteSpace(endpointUrl))
			{
				throw new ArgumentException("EndpointUrl is required");
			}

			if (string.IsNullOrWhiteSpace(tokenRequestParameterName))
			{
				throw new ArgumentException("TokenRequestParameterName is required");
			}

			if (string.IsNullOrWhiteSpace(token))
			{
				throw new ArgumentException("Token is required");
			}

			this.EndpointUrl = endpointUrl;
			this.TokenRequestParameterName = tokenRequestParameterName;
			this.Token = token;
		}

		public override string ToString()
		{
			var uriBuilder = new UriBuilder(this.EndpointUrl);
			var query = HttpUtility.ParseQueryString(uriBuilder.Query);
			query[this.TokenRequestParameterName] = this.Token;
			uriBuilder.Query = query.ToString();

			return uriBuilder.ToString();
		}
	}
}