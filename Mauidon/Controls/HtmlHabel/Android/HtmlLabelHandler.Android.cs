// <copyright file="HtmlLabelHandler.Android.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>
#if __ANDROID__
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Java.Lang;
using Java.Net;
using Java.Util.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;
using Org.Xml.Sax;

namespace Mauidon.Controls.Droid
{
    /// <summary>
    /// Html Label Handler.
    /// </summary>
    public class HtmlLabelHandler : LabelHandler
    {
        private const string TagUlRegex = "[uU][lL]";
        private const string TagOlRegex = "[oO][lL]";
        private const string TagLiRegex = "[lL][iI]";

        private static readonly PropertyMapper<HtmlLabel, HtmlLabelHandler> HtmlLabelMapper = new PropertyMapper<HtmlLabel, HtmlLabelHandler>(LabelHandler.LabelMapper)
        {
            ["Text"] = MapHtml,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlLabelHandler"/> class.
        /// </summary>
        public HtmlLabelHandler()
            : base(HtmlLabelMapper)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlLabelHandler"/> class.
        /// </summary>
        /// <param name="mapper">PropertyMapper.</param>
        public HtmlLabelHandler(PropertyMapper mapper = null)
            : base(mapper)
        {
        }

        /// <summary>
        /// Map HTML to text fields.
        /// </summary>
        /// <param name="handler">LabelHandler.</param>
        /// <param name="label">ILabel.</param>
        public static void MapHtml(LabelHandler handler, ILabel label)
        {
            var element = label as HtmlLabel;
            AppCompatTextView control = handler.NativeView;
            ProcessText(control, element);
        }

        private static void ProcessText(AppCompatTextView control, HtmlLabel element)
        {
            if (control == null || element == null)
            {
                return;
            }

            Microsoft.Maui.Graphics.Color linkColor = ((HtmlLabel)element).LinkColor;
            if (!linkColor.IsDefault())
            {
                control.SetLinkTextColor(linkColor.ToAndroid());
            }

            control.SetIncludeFontPadding(false);
            var isRtl = Device.FlowDirection == Microsoft.Maui.Controls.FlowDirection.RightToLeft;
            var styledHtml = new RendererHelper(element, element.Text, Device.RuntimePlatform, isRtl).ToString();

            // Android's TextView doesn't support lists.
            // List tags must be replaces with custom tags,
            // that it will be renderer by a custom tag handler.
            styledHtml = styledHtml
                ?.ReplaceTag(TagUlRegex, ListTagHandler.TagUl)
                ?.ReplaceTag(TagOlRegex, ListTagHandler.TagOl)
                ?.ReplaceTag(TagLiRegex, ListTagHandler.TagLi);

            if (styledHtml != null)
            {
                SetText(control, element, styledHtml);
            }
        }

        private static void SetText(TextView control, HtmlLabel element, string html)
        {
            var htmlLabel = (HtmlLabel)element;

            // Set the type of content and the custom tag list handler
            using var listTagHandler = new ListTagHandler(htmlLabel.AndroidListIndent); // KWI-FIX: added AndroidListIndent parameter
            var imageGetter = new UrlImageParser(control);
            FromHtmlOptions fromHtmlOptions = htmlLabel.AndroidLegacyMode ? FromHtmlOptions.ModeLegacy : FromHtmlOptions.ModeCompact;
            ISpanned sequence = Build.VERSION.SdkInt >= BuildVersionCodes.N ?
                Html.FromHtml(html, fromHtmlOptions, imageGetter, listTagHandler) :
#pragma warning disable CS0618 // Type or member is obsolete
                Html.FromHtml(html, imageGetter, listTagHandler);
#pragma warning restore CS0618 // Type or member is obsolete
            using var strBuilder = new SpannableStringBuilder(sequence);

            // Make clickable links
            if (!element.GestureRecognizers.Any())
            {
                control.MovementMethod = LinkMovementMethod.Instance;
                URLSpan[] urls = strBuilder
                    .GetSpans(0, sequence.Length(), Class.FromType(typeof(URLSpan)))
                    .Cast<URLSpan>()
                    .ToArray();
                foreach (URLSpan span in urls)
                {
                    MakeLinkClickable(element, strBuilder, span);
                }
            }

            // Android adds an unnecessary "\n" that must be removed
            using ISpanned value = RemoveTrailingNewLines(strBuilder);

            // Finally sets the value of the TextView
            control.SetText(value, TextView.BufferType.Spannable);
        }

        private static ISpanned RemoveTrailingNewLines(ICharSequence text)
        {
            var builder = new SpannableStringBuilder(text);

            var count = 0;
            for (int i = 1; i <= text.Length(); i++)
            {
                if (!'\n'.Equals(text.CharAt(text.Length() - i)))
                {
                    break;
                }

                count++;
            }

            if (count > 0)
            {
                _ = builder.Delete(text.Length() - count, text.Length());
            }

            return builder;
        }

        private static void MakeLinkClickable(HtmlLabel element, ISpannable strBuilder, URLSpan span)
        {
            var start = strBuilder.GetSpanStart(span);
            var end = strBuilder.GetSpanEnd(span);
            SpanTypes flags = strBuilder.GetSpanFlags(span);
            var clickable = new HtmlLabelClickableSpan((HtmlLabel)element, span);
            strBuilder.SetSpan(clickable, start, end, flags);
            strBuilder.RemoveSpan(span);
        }

        private class HtmlLabelClickableSpan : ClickableSpan
        {
            private readonly HtmlLabel label;
            private readonly URLSpan span;

            public HtmlLabelClickableSpan(HtmlLabel label, URLSpan span)
            {
                this.label = label;
                this.span = span;
            }

            public override void UpdateDrawState(TextPaint ds)
            {
                base.UpdateDrawState(ds);
                ds.UnderlineText = this.label.UnderlineText;
            }

            public override void OnClick(Android.Views.View widget)
            {
                RendererHelper.HandleUriClick(this.label, this.span.URL);
            }
        }
    }

    /// <summary>
    /// String Extensions.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Replace Tags in HTML.
        /// </summary>
        /// <param name="html">HTML.</param>
        /// <param name="oldTagRegex">Old Tag Regex.</param>
        /// <param name="newTag">New Tag.</param>
        /// <returns>Updated html string.</returns>
        public static string ReplaceTag(this string html, string oldTagRegex, string newTag) =>
            Regex.Replace(html, @"(<\s*\/?\s*)" + oldTagRegex + @"((\s+[\w\-\,\.\(\)\=""\:\;]*)*>)", "$1" + newTag + "$2");
    }

    /// <summary>
    /// List Builder.
    /// </summary>
    internal class ListBuilder
    {
        private readonly int listIndent = 20; // KWI-FIX : changed from constant to prop

        private readonly int gap = 0;
        private readonly LiGap liGap;
        private readonly ListBuilder parent = null;

        private int liIndex = -1;
        private int liStart = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBuilder"/> class.
        /// </summary>
        /// <param name="listIndent">List Indent Amount.</param>
        public ListBuilder(int listIndent) // KWI-FIX: added listIndent
        {
            this.listIndent = listIndent;
            this.parent = null;
            this.gap = 0;
            this.liGap = GetLiGap(null);
        }

        private ListBuilder(ListBuilder parent, bool ordered, int listIndent) // KWI-FIX: added listIndent
        {
            this.listIndent = listIndent;
            this.parent = parent;
            this.liGap = parent.liGap;
            this.gap = parent.gap + this.listIndent + this.liGap.GetGap(ordered);
            this.liIndex = ordered ? 0 : -1;
        }

        /// <summary>
        /// Start List.
        /// </summary>
        /// <param name="ordered">Is Ordered List.</param>
        /// <param name="output">Output.</param>
        /// <returns>ListBuilder.</returns>
        public ListBuilder StartList(bool ordered, IEditable output)
        {
            if (this.parent == null && output.Length() > 0)
            {
                _ = output.Append("\n ");
            }

            return new ListBuilder(this, ordered, this.listIndent); // KWI-FIX: pass thru listIndent
        }

        /// <summary>
        /// Add List Item.
        /// </summary>
        /// <param name="isOpening">Is Opening.</param>
        /// <param name="output">Output.</param>
        public void AddListItem(bool isOpening, IEditable output)
        {
            if (isOpening)
            {
                EnsureParagraphBoundary(output);
                this.liStart = output.Length();

                var lineStart = this.IsOrdered()
                    ? ++this.liIndex + ". "
                    : "•  ";
                _ = output.Append(lineStart);
            }
            else
            {
                if (this.liStart >= 0)
                {
                    EnsureParagraphBoundary(output);
                    using var leadingMarginSpan = new LeadingMarginSpanStandard(this.gap - this.liGap.GetGap(this.IsOrdered()), this.gap);
                    output.SetSpan(leadingMarginSpan, this.liStart, output.Length(), SpanTypes.ExclusiveExclusive);
                    this.liStart = -1;
                }
            }
        }

        /// <summary>
        /// Close List.
        /// </summary>
        /// <param name="output">IEditable Output.</param>
        /// <returns>ListBuilder.</returns>
        public ListBuilder CloseList(IEditable output)
        {
            EnsureParagraphBoundary(output);
            ListBuilder result = this.parent;
            if (result == null)
            {
                result = this;
            }

            if (result.parent == null)
            {
                _ = output.Append('\n');
            }

            return result;
        }

        private static void EnsureParagraphBoundary(IEditable output)
        {
            if (output.Length() == 0)
            {
                return;
            }

            var lastChar = output.CharAt(output.Length() - 1);
            if (lastChar != '\n')
            {
                _ = output.Append('\n');
            }
        }

        private static LiGap GetLiGap(TextView tv)
        {
            var orderedGap = tv == null ? 40 : ComputeWidth(tv, true);
            var unorderedGap = tv == null ? 30 : ComputeWidth(tv, false);

            return new LiGap(orderedGap, unorderedGap);
        }

        private static int ComputeWidth(TextView tv, bool isOrdered)
        {
            Android.Graphics.Paint paint = tv.Paint;
            using var bounds = new Android.Graphics.Rect();
            var startString = isOrdered ? "99. " : "• ";
            paint.GetTextBounds(startString, 0, startString.Length, bounds);
            var width = bounds.Width();
            var pt = Android.Util.TypedValue.ApplyDimension(Android.Util.ComplexUnitType.Pt, width, tv.Context.Resources.DisplayMetrics);
            return (int)pt;
        }

        private bool IsOrdered()
        {
            return this.liIndex >= 0;
        }

        private class LiGap
        {
            private readonly int orderedGap;
            private readonly int unorderedGap;

            internal LiGap(int orderedGap, int unorderedGap)
            {
                this.orderedGap = orderedGap;
                this.unorderedGap = unorderedGap;
            }

            public int GetGap(bool ordered)
            {
                return ordered ? this.orderedGap : this.unorderedGap;
            }
        }
    }

    /// <summary>
    /// Tag handler to support HTML lists.
    /// </summary>
    internal class ListTagHandler : Java.Lang.Object, Html.ITagHandler
    {
        /// <summary>
        /// TagUl.
        /// </summary>
        public const string TagUl = "ULC";

        /// <summary>
        /// TagOl.
        /// </summary>
        public const string TagOl = "OLC";

        /// <summary>
        /// TagLi.
        /// </summary>
        public const string TagLi = "LIC";

        private ListBuilder listBuilder; // KWI-FIX: removed new, set in constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ListTagHandler"/> class.
        /// </summary>
        /// <param name="listIndent">List Indent Amount.</param>
        public ListTagHandler(int listIndent) // KWI-FIX: added constructor with listIndent property
        {
            this.listBuilder = new ListBuilder(listIndent);
        }

        /// <summary>
        /// Handle Tag.
        /// </summary>
        /// <param name="isOpening">Is Opening.</param>
        /// <param name="tag">Tag.</param>
        /// <param name="output">IEditable Output.</param>
        /// <param name="xmlReader">IXMLReader.</param>
        public void HandleTag(bool isOpening, string tag, IEditable output, IXMLReader xmlReader)
        {
            tag = tag.ToUpperInvariant();
            var isItem = tag == TagLi;

            // Is list item
            if (isItem)
            {
                this.listBuilder.AddListItem(isOpening, output);
            }

            // Is list
            else
            {
                if (isOpening)
                {
                    var isOrdered = tag == TagOl;
                    this.listBuilder = this.listBuilder.StartList(isOrdered, output);
                }
                else
                {
                    this.listBuilder = this.listBuilder.CloseList(output);
                }
            }
        }
    }

    /// <summary>
    /// Url Drawable.
    /// </summary>
#pragma warning disable CS0618 // Type or member is obsolete
    internal class UrlDrawable : BitmapDrawable
    {
        /// <summary>
        /// Gets or sets the Drawable.
        /// </summary>
        public Drawable Drawable { get; set; }

        /// <inheritdoc/>
        public override void Draw(Canvas canvas)
        {
            if (this.Drawable != null)
            {
                this.Drawable.Draw(canvas); ;
            }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete

    /// <summary>
    /// Image Getter Async Task.
    /// </summary>
    internal class ImageGetterAsyncTask : AsyncTask<string, int, Drawable>
    {
        private readonly UrlDrawable urlDrawable;

        private readonly TextView container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageGetterAsyncTask"/> class.
        /// </summary>
        /// <param name="urlDrawable">Url Drawable.</param>
        /// <param name="container">TextView Container.</param>
        public ImageGetterAsyncTask(UrlDrawable urlDrawable, TextView container)
        {
            this.urlDrawable = urlDrawable;
            this.container = container;
        }

        /// <inheritdoc/>
        protected override Drawable RunInBackground(params string[] @params)
        {
            var source = @params[0];
            return this.FetchDrawable(source);
        }

        /// <inheritdoc/>
        protected override void OnPostExecute(Drawable result)
        {
            if (result == null)
            {
                return;
            }

            // Set the correct bound according to the result from HTTP call
            this.urlDrawable.SetBounds(0, 0, 0 + result.IntrinsicWidth, 0 + result.IntrinsicHeight);

            // Change the reference of the current drawable to the result from the HTTP call
            this.urlDrawable.Drawable = result;

            // Redraw the image by invalidating the container
            this.container.Invalidate();

            // For ICS
            this.container.SetHeight(this.container.Height + result.IntrinsicHeight);

            // Pre ICS
            this.container.Ellipsize = null;
        }

        private static Stream Fetch(string urlString)
        {
            var url = new URL(urlString);
            var urlConnection = (HttpURLConnection)url.OpenConnection();
            Stream stream = urlConnection.InputStream;
            return stream;
        }

        private Drawable FetchDrawable(string urlString)
        {
            try
            {
                Stream stream = Fetch(urlString);
                var drawable = Drawable.CreateFromStream(stream, "src");
                drawable.SetBounds(0, 0, 0 + drawable.IntrinsicWidth, 0 + drawable.IntrinsicHeight);
                return drawable;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"            ERROR: ", ex.Message);
                return null;
            }
        }
    }

    /// <summary>
    /// Url Image Parser.
    /// </summary>
    internal class UrlImageParser : Java.Lang.Object, Html.IImageGetter
    {
        private readonly TextView container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlImageParser"/> class.
        /// </summary>
        /// <param name="container">TextView Container.</param>
        public UrlImageParser(TextView container)
        {
            this.container = container;
        }

        /// <summary>
        /// Get Drawable.
        /// </summary>
        /// <param name="source">String url.</param>
        /// <returns>Drawable.</returns>
        public Drawable GetDrawable(string source)
        {
            var urlDrawable = new UrlDrawable();

            var asyncTask = new ImageGetterAsyncTask(urlDrawable, this.container);
            _ = asyncTask.Execute(source);

            return urlDrawable;
        }
    }
}
#endif