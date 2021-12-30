
// (c) 2020-2021 Kazuki KOHZUKI

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using SysBitmapFrame = System.Windows.Media.Imaging.BitmapFrame;
using WinBitmapDecoder = Windows.Graphics.Imaging.BitmapDecoder;
using WinOcrEngine = Windows.Media.Ocr.OcrEngine;

namespace AutoEscapeZoom.Utils
{
    public class OcrEngine
    {
        private readonly WinOcrEngine ocrEngine;
        
        public OcrEngine(Language language)
        {
            this.ocrEngine = WinOcrEngine.TryCreateFromLanguage(language);
        } // ctor (Language)

        public OcrEngine(string language) : this(new Language(language)) { }

        internal OcrEngine() : this(CultureInfo.CurrentUICulture.Name) { }

        public Language RecognizerLanguage
            => this.ocrEngine.RecognizerLanguage;

        public static IReadOnlyList<Language> AvailableRecognizerLanguages
            => WinOcrEngine.AvailableRecognizerLanguages;

        async public Task<string> GetString(Bitmap bitmap)
        {
            if (bitmap == null) return null;

            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            var source = SysBitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(SysBitmapFrame.Create(source));
            using var tmp = new MemoryStream();
            encoder.Save(tmp);

            using var converted = tmp.AsRandomAccessStream();
            var decoder = await WinBitmapDecoder.CreateAsync(converted);
            var image = await decoder.GetSoftwareBitmapAsync();
            return await GetString(image);
        } // async public Task<string> GetString (Bitmap)

        async public Task<string> GetString(SoftwareBitmap bitmap)
        {
            var res = await this.ocrEngine.RecognizeAsync(bitmap);
            return res.Text;
        } // async public Task<string> GetString (SoftwareBitmap)
    } // public class OcrEngine
} // namespace AutoEscapeZoom.Utils
