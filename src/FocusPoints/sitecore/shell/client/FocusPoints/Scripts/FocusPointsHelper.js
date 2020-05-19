/*
 * Source taken from: http://jonom.github.io/jquery-focuspoint/js/jquery.focuspoint.helpertool.js
 * Edited by theFactor.e for usage within Sitecore CMS
 * Gets focus point coordinates from an image - adapt to suit your needs.
 */
define(["sitecore", "jquery", "/-/speak/v1/client/jquery.focuspoint.js"], function (Sitecore, $) {
	var FocusPointsHelper = Sitecore.Definitions.App.extend({
		initialized: function () {
			var app = this;
			
			var imageUrl,
				focusPoint,
				$focusPointsContainers,
				$focusPointsImages,
				$helperToolImage;

			// This stores FocusPoints data-attribute values
			var focusPointsAttr = {
				x: 0,
				y: 0,
				w: 0,
				h: 0
			};

			// Initialize helper Tool
			(function () {
				// Initialize Variables
				var queryParameters = Sitecore.Helpers.url.getQueryParameters(window.location.href);
				imageUrl = queryParameters['imageUrl'];
				focusPoint = queryParameters['focusPoint'];

				// Set current focus point if available
				if (focusPoint) {
					var focusPointValues = focusPoint.split(',');
					focusPointsAttr.x = parseFloat(focusPointValues[0]);
					focusPointsAttr.y = parseFloat(focusPointValues[1]);
				}

				$dataAttrInput = $('.helper-tool-data-attr');
				$helperToolImage = $('img.helper-tool-img, img.target-overlay');

				// Create Grid Elements
				for (var i = 1; i < 10; i++) {
					$('.frames').append('<div class="frame' + i + ' focuspoint"><img/></div>');
				}

				// Store focus point containers
				$focusPointsContainers = $('.focuspoint');
				$focusPointsImages = $('.focuspoint img');

				// Set the default source image
				setImage(imageUrl);
			})();

			function setImage(imgURL) {
				// Set focus point attributes
				focusPointsAttr.w = this.width;
				focusPointsAttr.h = this.height;

				// Set src on the thumbnail used in the GUI
				$helperToolImage.attr('src', imgURL);

				// Set src on all .focuspoint images
				$focusPointsImages.attr('src', imgURL);

				// Update focus point container data
				$focusPointsContainers.data('focusX', focusPointsAttr.x);
				$focusPointsContainers.data('focusY', focusPointsAttr.y);
				$focusPointsContainers.data('imageW', focusPointsAttr.w);
				$focusPointsContainers.data('imageH', focusPointsAttr.h);

				// Run FocusPoint for the first time.
				$('.focuspoint').focusPoint();

				// Update focus point
				updateFocusPoint();
			}

			function updateFocusPoint() {
				updateFocusPointsContainers();
				updateTextField();
				updateReticle();
			}

			function updateFocusPointsContainers() {
				$focusPointsContainers.data('focusX', focusPointsAttr.x);
				$focusPointsContainers.data('focusY', focusPointsAttr.y);
				$focusPointsContainers.adjustFocus();
			}

			function updateTextField() {
				app.FocusPoint.set('text', focusPointsAttr.x.toFixed(2) + "," + focusPointsAttr.y.toFixed(2));
			}

			function updateReticle() {
				// Calculate CSS Percentages
				var percentageX = (focusPointsAttr.x + 1) / 2 * 100;
				var percentageY = (1 - focusPointsAttr.y) / 2 * 100;

				// Leave a sweet target reticle at the focus point.
				$('.reticle').css({
					'top': percentageY + '%',
					'left': percentageX + '%'
				});
			}

			$helperToolImage.click(function (e) {
				var imageW = $(this).width();
				var imageH = $(this).height();

				// Calculate FocusPoint coordinates
				var offsetX = e.pageX - $(this).offset().left;
				var offsetY = e.pageY - $(this).offset().top;
				var focusX = (offsetX / imageW - .5) * 2;
				var focusY = (offsetY / imageH - .5) * -2;
				focusPointsAttr.x = focusX;
				focusPointsAttr.y = focusY;

				// Update focus point
				updateFocusPoint();
			});
		}
	});

	return FocusPointsHelper;
});
