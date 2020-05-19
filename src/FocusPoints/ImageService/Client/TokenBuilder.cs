namespace FocusPoints.ImageService.Client
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IdentityModel.Tokens.Jwt;
	using System.Security.Claims;
	using System.Text;
	using Microsoft.IdentityModel.Tokens;

	public class TokenBuilder
	{
		public enum ActionType
		{
			Transform,
			Resize,
		}

		public string Issuer { get; set; }

		public string Secret { get; set; }

		public Uri Url { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public ActionType Action { get; set; }

		public double FocusPointX { get; set; }

		public double FocusPointY { get; set; }

		public TokenBuilder(string issuer, string secret, ActionType action = ActionType.Transform)
		{
			if (string.IsNullOrWhiteSpace(issuer))
			{
				throw new ArgumentException("Issuer is required");
			}

			if (string.IsNullOrWhiteSpace(secret))
			{
				throw new ArgumentException("Secret is required");
			}

			this.Issuer = issuer;
			this.Secret = secret;
			this.Action = action;
		}

		public override string ToString()
		{
			var jwtHandler = new JwtSecurityTokenHandler();

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Secret));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

			var header = new JwtHeader(credentials);
			var payload = new JwtPayload(this.GetClaims());

			var token = new JwtSecurityToken(header, payload);
			var handler = new JwtSecurityTokenHandler();

			return handler.WriteToken(token);
		}

		private List<Claim> GetClaims()
		{
			var claims = new List<Claim>()
			{
				new Claim(Claims.Issuer, this.Issuer),
				new Claim(Claims.Url, this.Url.ToString()),
				new Claim(Claims.Width, this.Width.ToString()),
				new Claim(Claims.Height, this.Height.ToString()),
			};

			switch (this.Action)
			{
				case ActionType.Resize:
					claims.Add(new Claim(Claims.Action, "resize"));
					break;
				case ActionType.Transform:
					claims.Add(new Claim(Claims.Action, "transform"));
					claims.Add(new Claim(Claims.FocusPointX, this.FocusPointX.ToString(CultureInfo.InvariantCulture)));
					claims.Add(new Claim(Claims.FocusPointY, this.FocusPointY.ToString(CultureInfo.InvariantCulture)));
					break;
			}

			return claims;
		}
	}
}