﻿using Cofoundry.Core;
using Cofoundry.Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Cofoundry.Domain
{
    /// <summary>
    /// A collection of settings thats can be used to dynamically resize an image. Based
    /// on ImageResizer.ResizerSettings.
    /// </summary>
    public class ImageResizeSettings : IImageResizeSettings
    {
        const string QS_ANCHOR = "anchor";
        const string QS_WIDTH = "width";
        const string QS_HEIGHT = "height";
        const string QS_MODE = "mode";
        const string QS_SCALE = "scale";
        const string QS_BACKGROUND_COLOR = "bgcolor";
        const ImageAnchorLocation DEFAULT_ANCHOR_LOCATION = ImageAnchorLocation.MiddleCenter;

        public ImageResizeSettings()
        {
            Anchor = DEFAULT_ANCHOR_LOCATION;
        }

        /// <summary>
        /// How to anchor the image when cropping or adding whitespace to meet sizing
        /// requirements.
        /// </summary>
        public ImageAnchorLocation Anchor { get; set; }

        /// <summary>
        /// Hex color to use as the background color if the image is padded
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Sets the desired height of the image. (minus padding, borders,
        /// margins, effects, and rotation) The only instance the resulting image will
        /// be smaller is if the original source image is smaller. Set Scale=Both to
        /// upscale these images and ensure the output always matches 'width' and 'height'.
        /// If both width and height are specified, the image will be 'letterboxed' to
        /// match the desired aspect ratio. Change the Mode property to adjust this behavior.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Sets the fit mode for the image. max, min, pad, crop, carve, stretch
        /// </summary>
        public ImageFitMode Mode { get; set; }

        /// <summary>
        ///  Whether to downscale, upscale, upscale the canvas, or both upscale
        ///  or downscale the image as needed. Defaults to DownscaleOnly.
        /// </summary>
        public ImageScaleMode Scale { get; set; }

        /// <summary>
        /// Sets the desired width of the image. (minus padding, borders,
        /// margins, effects, and rotation). The only instance the resulting image will
        /// be smaller is if the original source image is smaller. Set Scale=Both to
        /// upscale these images and ensure the output always matches 'width' and 'height'.
        /// If both width and height are specified, the image will be 'letterboxed' to
        /// match the desired aspect ratio. Change the Mode property to adjust this behavior.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets a querystring that represent this set of settings in the format ?w=20&amp;h=30
        /// </summary>
        public string ToQueryString()
        {
            var qs = new QueryStringBuilder();

            if (Anchor != DEFAULT_ANCHOR_LOCATION) qs.Add(QS_ANCHOR, Anchor.ToString());
            if (Width > 0) qs.Add(QS_WIDTH, Width.ToString());
            if (Height > 0) qs.Add(QS_HEIGHT, Height.ToString());
            if (Mode != ImageFitMode.None) qs.Add(QS_MODE, Mode.ToString().ToLower());
            if (Scale != ImageScaleMode.DownscaleOnly) qs.Add(QS_SCALE, Scale.ToString().ToLower());
            if (!string.IsNullOrWhiteSpace(BackgroundColor)) qs.Add(QS_BACKGROUND_COLOR, BackgroundColor.ToLower());

            return qs.OrderAndRender();
        }

        public static IImageResizeSettings ParseFromQueryString(string queryString)
        {
            var qs = HttpUtility.ParseQueryString(queryString);
            var settings = new ImageResizeSettings();

            settings.Width = IntParser.ParseOrDefault(qs[QS_WIDTH]);
            settings.Height = IntParser.ParseOrDefault(qs[QS_HEIGHT]);
            settings.Anchor = EnumParser.ParseOrDefault<ImageAnchorLocation>(qs[QS_ANCHOR], settings.Anchor);
            settings.Mode = EnumParser.ParseOrDefault<ImageFitMode>(qs[QS_MODE], settings.Mode);
            settings.Scale = EnumParser.ParseOrDefault<ImageScaleMode>(qs[QS_SCALE], settings.Scale);
            settings.BackgroundColor = StringHelper.EmptyAsNull(qs[QS_BACKGROUND_COLOR]);

            return settings;
        }
    }
}
