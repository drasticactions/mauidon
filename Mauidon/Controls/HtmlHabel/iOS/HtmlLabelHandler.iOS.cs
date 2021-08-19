#if __IOS__
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform.iOS;
using UIKit;
using NativeFont = UIKit.UIFont;

namespace Mauidon.Controls.iOS
{
    /// <summary>
    /// Html Label Handler.
    /// </summary>
    public class HtmlLabelHandler : ViewHandler<ILabel, UITextViewFixedWithKludge>
    {
        /// <summary>
        /// Html Label Mapper.
        /// </summary>
        private static readonly PropertyMapper<ILabel, HtmlLabelHandler> HtmlLabelMapper = new PropertyMapper<ILabel, HtmlLabelHandler>(ViewHandler.ViewMapper)
        {
            [nameof(ILabel.Text)] = MapText,
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
        /// <param name="mapper">Property Mapper.</param>
        public HtmlLabelHandler(PropertyMapper? mapper = null)
            : base(mapper ?? HtmlLabelMapper)
        {
        }

        /// <summary>
        /// Map HTML to text fields.
        /// </summary>
        /// <param name="handler">LabelHandler.</param>
        /// <param name="label">ILabel.</param>
        public static void MapText(HtmlLabelHandler handler, ILabel label)
        {
            var element = label as HtmlLabel;
            UITextView control = handler.NativeView;
            ProcessText(control, element);
        }

        /// <summary>
        /// Navigate To Url.
        /// </summary>
        /// <param name="url">Url.</param>
        /// <returns>Boolean if we can navigate.</returns>
        protected bool NavigateToUrl(NSUrl url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            // Try to handle uri, if it can't be handled, fall back to IOS his own handler.
            // return !RendererHelper.HandleUriClick(Element, url.AbsoluteString);
            return true;
        }

        /// <inheritdoc/>
        protected override UITextViewFixedWithKludge CreateNativeView()
        {
            var control = new UITextViewFixedWithKludge(CGRect.Empty)
            {
                Editable = false,
                ScrollEnabled = false,
                ShowsVerticalScrollIndicator = false,
                BackgroundColor = UIColor.Clear,
            };
            control.DataDetectorTypes = UIDataDetectorType.All;
            control.Selectable = true;
            control.Delegate = new TextViewDelegate(this.NavigateToUrl);
            return control;
        }

        private static void UpdatePadding(UITextView control, HtmlLabel element)
        {
            if (element.Padding.IsEmpty)
            {
                return;
            }

            control.TextContainerInset = new UIEdgeInsets(
                    (float)element.Padding.Top,
                    (float)element.Padding.Left,
                    (float)element.Padding.Bottom,
                    (float)element.Padding.Right);
            UpdateLayout(control);
        }

        private static void UpdateTextDecorations(UITextView control, HtmlLabel element)
        {
            if (element?.TextType != TextType.Text)
            {
                return;
            }

            if (!(control.AttributedText?.Length > 0))
            {
                return;
            }

            var textDecorations = element.TextDecorations;
            var newAttributedText = new NSMutableAttributedString(control.AttributedText);
            var strikeThroughStyleKey = UIStringAttributeKey.StrikethroughStyle;
            var underlineStyleKey = UIStringAttributeKey.UnderlineStyle;
            var range = new NSRange(0, newAttributedText.Length);

            if ((textDecorations & TextDecorations.Strikethrough) == 0)
            {
                newAttributedText.RemoveAttribute(strikeThroughStyleKey, range);
            }
            else
            {
                newAttributedText.AddAttribute(strikeThroughStyleKey, NSNumber.FromInt32((int)NSUnderlineStyle.Single), range);
            }

            if ((textDecorations & TextDecorations.Underline) == 0)
            {
                newAttributedText.RemoveAttribute(underlineStyleKey, range);
            }
            else
            {
                newAttributedText.AddAttribute(underlineStyleKey, NSNumber.FromInt32((int)NSUnderlineStyle.Single), range);
            }

            control.AttributedText = newAttributedText;
            UpdateCharacterSpacing(control, element);
        }

        private static void UpdateCharacterSpacing(UITextView control, HtmlLabel element)
        {

            if (element?.TextType != TextType.Text)
            {
                return;
            }

            var textAttr = control.AttributedText.AddCharacterSpacing(element.Text, element.CharacterSpacing);

            if (textAttr != null)
            {
                control.AttributedText = textAttr;
            }
        }

        private static void UpdateLayout(UITextView control)
        {
            control.LayoutSubviews();
        }

        private void UpdateHorizontalTextAlignment(UITextView control, HtmlLabel element)
        {
            control.TextAlignment = element.HorizontalTextAlignment.ToNativeTextAlignment(((IVisualElementController)element).EffectiveFlowDirection);
        }

        private static void UpdateLineBreakMode(UITextView control, HtmlLabel element)
        {
            switch (element.LineBreakMode)
            {
                case LineBreakMode.NoWrap:
                    control.TextContainer.LineBreakMode = UILineBreakMode.Clip;
                    break;
                case LineBreakMode.WordWrap:
                    control.TextContainer.LineBreakMode = UILineBreakMode.WordWrap;
                    break;
                case LineBreakMode.CharacterWrap:
                    control.TextContainer.LineBreakMode = UILineBreakMode.CharacterWrap;
                    break;
                case LineBreakMode.HeadTruncation:
                    control.TextContainer.LineBreakMode = UILineBreakMode.HeadTruncation;
                    break;
                case LineBreakMode.MiddleTruncation:
                    control.TextContainer.LineBreakMode = UILineBreakMode.MiddleTruncation;
                    break;
                case LineBreakMode.TailTruncation:
                    control.TextContainer.LineBreakMode = UILineBreakMode.TailTruncation;
                    break;
            }

        }

        private static void ProcessText(UITextView control, HtmlLabel element)
        {
            if (control == null || element == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(element?.Text))
            {
                control.Text = string.Empty;
                return;
            }

            control.Font = FontExtensions.ToUIFont(element);
            if (!element.TextColor.IsDefault())
            {
                control.TextColor = element.TextColor.ToUIColor();
            }

            var linkColor = element.LinkColor;
            if (!linkColor.IsDefault())
            {
                control.TintColor = linkColor.ToUIColor();
            }
            var isRtl = Device.FlowDirection == Microsoft.Maui.Controls.FlowDirection.RightToLeft;
            var styledHtml = new RendererHelper(element, element.Text, Device.RuntimePlatform, isRtl).ToString();
            SetText(control, element, styledHtml);
            control.SetNeedsDisplay();
        }

        private static void SetText(UITextView control, HtmlLabel element, string html)
        {
            // Create HTML data sting
            var stringType = new NSAttributedStringDocumentAttributes
            {
                DocumentType = NSDocumentType.HTML,
            };
            var nsError = new NSError();

            var htmlData = NSData.FromString(html, NSStringEncoding.Unicode);

            using var htmlString = new NSAttributedString(htmlData, stringType, out _, ref nsError);
            var mutableHtmlString = htmlString.RemoveTrailingNewLines();

            mutableHtmlString.EnumerateAttributes(new NSRange(0, mutableHtmlString.Length), NSAttributedStringEnumeration.None, (NSDictionary value, NSRange range, ref bool stop) =>
                {
                    var md = new NSMutableDictionary(value);
                    var font = md[UIStringAttributeKey.Font] as UIFont;

                    if (font != null)
                    {
                        md[UIStringAttributeKey.Font] = control.Font.WithTraitsOfFont(font);
                    }
                    else
                    {
                        md[UIStringAttributeKey.Font] = control.Font;
                    }

                    var foregroundColor = md[UIStringAttributeKey.ForegroundColor] as UIColor;
                    if (foregroundColor == null || foregroundColor.IsEqualToColor(UIColor.Black))
                    {
                        md[UIStringAttributeKey.ForegroundColor] = control.TextColor;
                    }

                    mutableHtmlString.SetAttributes(md, range);
                });

            mutableHtmlString.SetLineHeight(element);
            mutableHtmlString.SetLinksStyles(element);
            control.AttributedText = mutableHtmlString;
        }
    }

    /// <summary>
    /// Link Tap Helper.
    /// </summary>
    internal static class LinkTapHelper
    {
        /// <summary>
        /// Custom Link Attribute.
        /// </summary>
        public static readonly NSString CustomLinkAttribute = new NSString("LabelLink");

        public static void HandleLinkTap(this UILabel control, HtmlLabel element)
        {
            void TapHandler(UITapGestureRecognizer tap)
            {
                var detectedUrl = DetectTappedUrl(tap, (UILabel)tap.View);
                RendererHelper.HandleUriClick(element, detectedUrl);
            }

            var tapGesture = new UITapGestureRecognizer(TapHandler);
            control.AddGestureRecognizer(tapGesture);
            control.UserInteractionEnabled = true;
        }

        private static string DetectTappedUrl(UIGestureRecognizer tap, UILabel control)
        {
            CGRect bounds = control.Bounds;
            NSAttributedString attributedText = control.AttributedText;

            // Setup containers
            using var textContainer = new NSTextContainer(bounds.Size)
            {
                LineFragmentPadding = 0,
                LineBreakMode = control.LineBreakMode,
                MaximumNumberOfLines = (nuint)control.Lines,
            };

            using var layoutManager = new NSLayoutManager();
            layoutManager.AddTextContainer(textContainer);

            using var textStorage = new NSTextStorage();
            textStorage.SetString(attributedText);

            using var fontAttributeName = new NSString("NSFont");
            var textRange = new NSRange(0, control.AttributedText.Length);
            textStorage.AddAttribute(fontAttributeName, control.Font, textRange);
            textStorage.AddLayoutManager(layoutManager);
            CGRect textBoundingBox = layoutManager.GetUsedRectForTextContainer(textContainer);

            // Calculate align offset
            static nfloat GetAlignOffset(UITextAlignment textAlignment)
            {
                return textAlignment switch
                {
                    UITextAlignment.Center => 0.5f,
                    UITextAlignment.Right => 1f,
                    _ => 0.0f,
                };
            }

            nfloat alignmentOffset = GetAlignOffset(control.TextAlignment);
            nfloat xOffset = (bounds.Size.Width - textBoundingBox.Size.Width) * alignmentOffset - textBoundingBox.Location.X;
            nfloat yOffset = (bounds.Size.Height - textBoundingBox.Size.Height) * alignmentOffset - textBoundingBox.Location.Y;

            // Find tapped character
            CGPoint locationOfTouchInLabel = tap.LocationInView(control);
            var locationOfTouchInTextContainer = new CGPoint(locationOfTouchInLabel.X - xOffset, locationOfTouchInLabel.Y - yOffset);
            var characterIndex = (nint)layoutManager.GetCharacterIndex(locationOfTouchInTextContainer, textContainer);

            if (characterIndex >= attributedText.Length)
            {
                return null;
            }

            // Try to get the URL
            NSObject linkAttributeValue = attributedText.GetAttribute(CustomLinkAttribute, characterIndex, out NSRange range);
            return linkAttributeValue is NSUrl url ? url.AbsoluteString : null;
        }
    }

    /// <summary>
    /// TextView Delegate.
    /// </summary>
    internal class TextViewDelegate : UITextViewDelegate
    {
        private readonly Func<NSUrl, bool> navigateTo;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextViewDelegate"/> class.
        /// </summary>
        /// <param name="navigateTo">Navigate To Url Function.</param>
        public TextViewDelegate(Func<NSUrl, bool> navigateTo)
        {
            this.navigateTo = navigateTo;
        }

        /// <inheritdoc/>
        public override bool ShouldInteractWithUrl(UITextView textView, NSUrl URL, NSRange characterRange)
        {
            if (this.navigateTo != null)
            {
                return this.navigateTo(URL);
            }

            return true;
        }
    }

    /// <summary>
    /// UI TextView Fixed With Kludge.
    /// </summary>
    public class UITextViewFixedWithKludge : UITextView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UITextViewFixedWithKludge"/> class.
        /// </summary>
        /// <param name="frame">Frame.</param>
        public UITextViewFixedWithKludge(CGRect frame)
            : base(frame)
        {
        }

        /// <inheritdoc/>
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.Setup();
        }

        private void Setup()
        {
            this.TextContainerInset = UIEdgeInsets.Zero;
            this.TextContainer.LineFragmentPadding = 0;

            var b = this.Bounds;
            var h = this.SizeThatFits(new CGSize(
                this.Bounds.Size.Width,
                float.MaxValue)).Height;
            this.Bounds = new CGRect(b.X, b.Y, b.Width, h);
        }
    }

    /// <summary>
    /// Color Extensions.
    /// </summary>
    internal static class ColorExtensions
    {
        /// <summary>
        /// Is Equal To Color.
        /// </summary>
        /// <param name="self">This Color.</param>
        /// <param name="otherColor">The Other Color.</param>
        /// <returns>Boolean if the colors are equal.</returns>
        internal static bool IsEqualToColor(this UIColor self, UIColor otherColor)
        {
            nfloat r;
            nfloat g;
            nfloat b;
            nfloat a;

            self.GetRGBA(out r, out g, out b, out a);
            nfloat r2;
            nfloat g2;
            nfloat b2;
            nfloat a2;

            otherColor.GetRGBA(out r2, out g2, out b2, out a2);

            return r == r2 && g == g2 && b == b2 && a == a2;
        }
    }

    /// <summary>
    /// Font Extensions.
    /// </summary>
    internal static class FontExtensions
    {
        private static readonly string DefaultFontName = UIFont.SystemFontOfSize(12).Name;
        private static readonly Dictionary<ToNativeFontFontKey, NativeFont> ToUiFont = new Dictionary<ToNativeFontFontKey, NativeFont>();

        /// <summary>
        /// To UIFont.
        /// </summary>
        /// <param name="self">Font.</param>
        /// <returns>Native UIFont.</returns>
        public static UIFont ToUIFont(this Font self) => ToNativeFont(self);

        /// <summary>
        /// Is Bold.
        /// </summary>
        /// <param name="font">Font.</param>
        /// <returns>Boolean.</returns>
        internal static bool IsBold(UIFont font)
        {
            UIFontDescriptor fontDescriptor = font.FontDescriptor;
            UIFontDescriptorSymbolicTraits traits = fontDescriptor.SymbolicTraits;
            return traits.HasFlag(UIFontDescriptorSymbolicTraits.Bold);
        }

        /// <summary>
        /// Bold Font.
        /// </summary>
        /// <param name="font">Font.</param>
        /// <returns>UIFont.</returns>
        internal static UIFont Bold(this UIFont font)
        {
            UIFontDescriptor fontDescriptor = font.FontDescriptor;
            UIFontDescriptorSymbolicTraits traits = fontDescriptor.SymbolicTraits;
            traits = traits | UIFontDescriptorSymbolicTraits.Bold;
            UIFontDescriptor boldFontDescriptor = fontDescriptor.CreateWithTraits(traits);
            return UIFont.FromDescriptor(boldFontDescriptor, font.PointSize);
        }

        /// <summary>
        /// Italic Font.
        /// </summary>
        /// <param name="self">Font.</param>
        /// <returns>UIFont.</returns>
        internal static UIFont Italic(this UIFont self)
        {
            UIFontDescriptor fontDescriptor = self.FontDescriptor;
            UIFontDescriptorSymbolicTraits traits = fontDescriptor.SymbolicTraits;
            traits = traits | UIFontDescriptorSymbolicTraits.Italic;
            UIFontDescriptor boldFontDescriptor = fontDescriptor.CreateWithTraits(traits);
            return UIFont.FromDescriptor(boldFontDescriptor, self.PointSize);
        }

        /// <summary>
        /// With Traits Of Font.
        /// </summary>
        /// <param name="self">Self Font.</param>
        /// <param name="font">Font.</param>
        /// <returns>UIFont.</returns>
        internal static UIFont WithTraitsOfFont(this UIFont self, UIFont font)
        {
            UIFontDescriptor fontDescriptor = self.FontDescriptor;
            UIFontDescriptorSymbolicTraits traits = fontDescriptor.SymbolicTraits;
            traits = traits | font.FontDescriptor.SymbolicTraits;
            UIFontDescriptor boldFontDescriptor = fontDescriptor.CreateWithTraits(traits);
            return UIFont.FromDescriptor(boldFontDescriptor, self.PointSize);
        }

        /// <summary>
        /// Is Default.
        /// </summary>
        /// <param name="self">Font.</param>
        /// <returns>Boolean.</returns>
        internal static bool IsDefault(this Span self)
        {
            return self.FontFamily == null && self.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(Label), true) &&
                    self.FontAttributes == FontAttributes.None;
        }

        /// <summary>
        /// To UIFont.
        /// </summary>
        /// <param name="element">IFontElement.</param>
        /// <returns>UIFont.</returns>
        internal static UIFont ToUIFont(this IFontElement element) => ToNativeFont(element);

        /// <summary>
        /// To Native Font Private.
        /// </summary>
        /// <param name="family">Font Family.</param>
        /// <param name="size">Size.</param>
        /// <param name="attributes">Attributes.</param>
        /// <returns>UIFont.</returns>
        private static UIFont ToNativeFontPrivate(string family, float size, FontAttributes attributes)
        {
            var bold = (attributes & FontAttributes.Bold) != 0;
            var italic = (attributes & FontAttributes.Italic) != 0;

            if (family != null && family != DefaultFontName)
            {
                try
                {
                    UIFont result = null;
                    if (UIFont.FamilyNames.Contains(family))
                    {
                        var descriptor = new UIFontDescriptor().CreateWithFamily(family);

                        if (bold || italic)
                        {
                            var traits = (UIFontDescriptorSymbolicTraits)0;
                            if (bold)
                            {
                                traits = traits | UIFontDescriptorSymbolicTraits.Bold;
                            }

                            if (italic)
                            {
                                traits = traits | UIFontDescriptorSymbolicTraits.Italic;
                            }

                            descriptor = descriptor.CreateWithTraits(traits);
                            result = UIFont.FromDescriptor(descriptor, size);
                            if (result != null)
                            {
                                return result;
                            }
                        }
                    }

                    result = UIFont.FromName(family, size);
                    if (family.StartsWith(".SFUI", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        var fontWeight = family.Split('-').LastOrDefault();

                        if (!string.IsNullOrWhiteSpace(fontWeight) && System.Enum.TryParse<UIFontWeight>(fontWeight, true, out var uIFontWeight))
                        {
                            result = UIFont.SystemFontOfSize(size, uIFontWeight);
                            return result;
                        }

                        result = UIFont.SystemFontOfSize(size, UIFontWeight.Regular);
                        return result;
                    }

                    if (result == null)
                    {
                        result = NativeFont.FromName(family, size);
                    }

                    if (result != null)
                    {
                        return result;
                    }
                }
                catch
                {
                    Debug.WriteLine("Could not load font named: {0}", family);
                }
            }

            if (bold && italic)
            {
                var defaultFont = UIFont.SystemFontOfSize(size);

                var descriptor = defaultFont.FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold | UIFontDescriptorSymbolicTraits.Italic);
                return UIFont.FromDescriptor(descriptor, 0);
            }

            if (italic)
            {
                return UIFont.ItalicSystemFontOfSize(size);
            }

            if (bold)
            {
                return UIFont.BoldSystemFontOfSize(size);
            }

            return UIFont.SystemFontOfSize(size);
        }

        /// <summary>
        /// To Native Font.
        /// </summary>
        /// <param name="element">IFontElement.</param>
        /// <returns>NativeFont.</returns>
        private static NativeFont ToNativeFont(this IFontElement element)
        {
            var fontFamily = element.FontFamily;
            var fontSize = (float)element.FontSize;
            var fontAttributes = element.FontAttributes;
            return ToNativeFont(fontFamily, fontSize, fontAttributes, ToNativeFontPrivate);
        }

        /// <summary>
        /// To Native Font.
        /// </summary>
        /// <param name="self">Font.</param>
        /// <returns>NativeFont.</returns>
        private static NativeFont ToNativeFont(this Font self)
        {
            var fontAttributes = self.GetFontAttributes();
            return ToNativeFont(self.Family, (float)self.Size, fontAttributes, ToNativeFontPrivate);
        }

        /// <summary>
        /// To Native Font.
        /// </summary>
        /// <param name="family">Font Family.</param>
        /// <param name="size">Size.</param>
        /// <param name="attributes">Font Attributes.</param>
        /// <param name="factory">Font Factory.</param>
        /// <returns>NativeFont.</returns>
        private static NativeFont ToNativeFont(string family, float size, FontAttributes attributes, Func<string, float, FontAttributes, NativeFont> factory)
        {
            var key = new ToNativeFontFontKey(family, size, attributes);

            lock (ToUiFont)
            {
                NativeFont value;
                if (ToUiFont.TryGetValue(key, out value))
                {
                    return value;
                }
            }

            var generatedValue = factory(family, size, attributes);

            lock (ToUiFont)
            {
                NativeFont value;
                if (!ToUiFont.TryGetValue(key, out value))
                {
                    ToUiFont.Add(key, value = generatedValue);
                }

                return value;
            }
        }

        /// <summary>
        /// To Native Font Key.
        /// </summary>
        private struct ToNativeFontFontKey
        {
#pragma warning disable 0414
            private readonly string family;

            private readonly float size;

            private readonly FontAttributes attributes;
#pragma warning restore 0414

            internal ToNativeFontFontKey(string family, float size, FontAttributes attributes)
            {
                this.family = family;
                this.size = size;
                this.attributes = attributes;
            }
        }
    }

    /// <summary>
    /// Attributed String Extensions.
    /// </summary>
    internal static class AttributedStringExtensions
    {
        /// <summary>
        /// Set Line Height.
        /// </summary>
        /// <param name="mutableHtmlString">NSMutableAttributedString.</param>
        /// <param name="element">HtmlLabel.</param>
        internal static void SetLineHeight(this NSMutableAttributedString mutableHtmlString, HtmlLabel element)
        {
            if (element.LineHeight < 0)
            {
                return;
            }

            using (var lineHeightStyle = new NSMutableParagraphStyle { LineHeightMultiple = (nfloat)element.LineHeight })
            {
                mutableHtmlString.AddAttribute(UIStringAttributeKey.ParagraphStyle, lineHeightStyle, new NSRange(0, mutableHtmlString.Length));
            }
        }

        /// <summary>
        /// Set Link Styles.
        /// </summary>
        /// <param name="mutableHtmlString">NSMutableAttributedString.</param>
        /// <param name="element">HtmlLabel.</param>
        internal static void SetLinksStyles(this NSMutableAttributedString mutableHtmlString, HtmlLabel element)
        {
            UIStringAttributes linkAttributes = null;

            if (!element.UnderlineText)
            {
                linkAttributes ??= new UIStringAttributes();
                linkAttributes.UnderlineStyle = NSUnderlineStyle.None;
            }

            if (!element.LinkColor.IsDefault())
            {
                linkAttributes ??= new UIStringAttributes();
                linkAttributes.ForegroundColor = element.LinkColor.ToUIColor();
            }

            mutableHtmlString.EnumerateAttribute(UIStringAttributeKey.Link, new NSRange(0, mutableHtmlString.Length), NSAttributedStringEnumeration.LongestEffectiveRangeNotRequired, (
                NSObject value,
                NSRange range,
                ref bool stop) =>
                {
                    if (value is not null and NSUrl url)
                    {
                        // Applies the style
                        if (linkAttributes != null)
                        {
                            mutableHtmlString.AddAttributes(linkAttributes, range);
                        }
                    }
                });
        }

        /// <summary>
        /// Remove Trailing New Lines.
        /// </summary>
        /// <param name="htmlString">NSAttributedString Html String.</param>
        /// <returns>NSMutableAttributedString.</returns>
        internal static NSMutableAttributedString RemoveTrailingNewLines(this NSAttributedString htmlString)
        {
            var count = 0;
            for (int i = 1; i <= htmlString.Length; i++)
            {
                if ("\n" != htmlString.Substring(htmlString.Length - i, 1).Value)
                {
                    break;
                }

                count++;
            }

            if (count > 0)
            {
                htmlString = htmlString.Substring(0, htmlString.Length - count);
            }

            return new NSMutableAttributedString(htmlString);
        }

        /// <summary>
        /// Add Character Spacing.
        /// </summary>
        /// <param name="attributedString">NSAttributedString.</param>
        /// <param name="text">Text.</param>
        /// <param name="characterSpacing">Character Spacing.</param>
        /// <returns>NSMutableAttributedString.</returns>
        internal static NSMutableAttributedString AddCharacterSpacing(this NSAttributedString attributedString, string text, double characterSpacing)
        {
            if (attributedString == null && characterSpacing == 0)
            {
                return null;
            }

            NSMutableAttributedString mutableAttributedString = attributedString as NSMutableAttributedString;
            if (attributedString == null || attributedString.Length == 0)
            {
                mutableAttributedString = text == null ? new NSMutableAttributedString() : new NSMutableAttributedString(text);
            }
            else
            {
                mutableAttributedString = new NSMutableAttributedString(attributedString);
            }

            AddKerningAdjustment(mutableAttributedString, text, characterSpacing);

            return mutableAttributedString;
        }

        /// <summary>
        /// Has Character Adjustment.
        /// </summary>
        /// <param name="mutableAttributedString">NSMutableAttributedString.</param>
        /// <returns>Boolean.</returns>
        internal static bool HasCharacterAdjustment(this NSMutableAttributedString mutableAttributedString)
        {
            if (mutableAttributedString == null)
            {
                return false;
            }

            NSRange removalRange;
            var attributes = mutableAttributedString.GetAttributes(0, out removalRange);

            for (uint i = 0; i < attributes.Count; i++)
            {
                if (attributes.Keys[i] is NSString nSString && nSString == UIStringAttributeKey.KerningAdjustment)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Add Kerning Adjustment.
        /// </summary>
        /// <param name="mutableAttributedString">NSMutableAttributedString.</param>
        /// <param name="text">Text.</param>
        /// <param name="characterSpacing">CharacterSpacing.</param>
        internal static void AddKerningAdjustment(NSMutableAttributedString mutableAttributedString, string text, double characterSpacing)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (characterSpacing == 0 && !mutableAttributedString.HasCharacterAdjustment())
                {
                    return;
                }

                mutableAttributedString.AddAttribute(
                    UIStringAttributeKey.KerningAdjustment,
                    NSObject.FromObject(characterSpacing),
                    new NSRange(0, text.Length - 1));
            }
        }

        /// <summary>
        /// Is Horizontal.
        /// </summary>
        /// <param name="layout">ButtonContentLayout.</param>
        /// <returns>Boolean.</returns>
        internal static bool IsHorizontal(this Button.ButtonContentLayout layout)
        {
            return layout.Position is Button.ButtonContentLayout.ImagePosition.Left or Button.ButtonContentLayout.ImagePosition.Right;
        }
    }

    /// <summary>
    /// Alginment Extensions.
    /// </summary>
    internal static class AlignmentExtensions
    {
        /// <summary>
        /// To Native Text Alginment.
        /// </summary>
        /// <param name="alignment">TextAlignment.</param>
        /// <param name="flowDirection">EffectiveFlowDirection.</param>
        /// <returns><see cref="UITextAlignment"/>.</returns>
        internal static UITextAlignment ToNativeTextAlignment(this TextAlignment alignment, EffectiveFlowDirection flowDirection)
        {
            var isLtr = flowDirection.IsLeftToRight();
            switch (alignment)
            {
                case TextAlignment.Center:
                    return UITextAlignment.Center;
                case TextAlignment.End:
                    if (isLtr)
                        return UITextAlignment.Right;
                    else
                        return UITextAlignment.Left;
                default:
                    if (isLtr)
                        return UITextAlignment.Left;
                    else
                        return UITextAlignment.Right;
            }
        }

        /// <summary>
        /// To Native Text Alignment.
        /// </summary>
        /// <param name="alignment">TextAlignment.</param>
        /// <returns><see cref="UIControlContentVerticalAlignment"/>.</returns>
        internal static UIControlContentVerticalAlignment ToNativeTextAlignment(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return UIControlContentVerticalAlignment.Center;
                case TextAlignment.End:
                    return UIControlContentVerticalAlignment.Bottom;
                case TextAlignment.Start:
                    return UIControlContentVerticalAlignment.Top;
                default:
                    return UIControlContentVerticalAlignment.Top;
            }
        }
    }
}

#endif