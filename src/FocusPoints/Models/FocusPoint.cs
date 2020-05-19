namespace FocusPoints.Models
{
	using System.Globalization;

	public class FocusPoint
	{
		public double X { get; set; }

		public double Y { get; set; }

		public FocusPoint(double x, double y)
		{
			X = x;
			Y = y;
		}

		public FocusPoint(string focusPoint)
		{
			if (string.IsNullOrWhiteSpace(focusPoint))
			{
				return;
			}

			var focusPointArray = focusPoint.Split(',');
			if (focusPointArray.Length != 2)
			{
				return;
			}

			try
			{
				X = double.Parse(focusPointArray[0], CultureInfo.InvariantCulture);
				Y = double.Parse(focusPointArray[1], CultureInfo.InvariantCulture);
			}
			catch
			{
				return;
			}
		}

		public override string ToString()
		{
			var x = X.ToString(CultureInfo.InvariantCulture);
			var y = Y.ToString(CultureInfo.InvariantCulture);
			return string.Concat(x, ",", y);
		}
	}
}