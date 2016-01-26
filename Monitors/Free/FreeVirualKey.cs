using System;
using System.Drawing;
using System.IO;
using System.Net;
using Tesseract;

namespace CloudDaemon.Monitors.Free
{
    public class FreeVirtualKey
    {
        private const string BaseUrl = "https://mobile.free.fr/moncompte/";

        public FreeVirtualKey(int order, string source)
        {
            this.Order = order;
            this.Source = BaseUrl + source;
        }

        public int Order { get; private set; }

        public int Key { get; set; }

        public string Source { get; private set; }

        public void InitKey(WebClient client)
        {
            Stream pngStream = client.OpenRead(Source);
            Image image = Image.FromStream(pngStream);
            MemoryStream tiffStream = new MemoryStream();
            image.Save(tiffStream, System.Drawing.Imaging.ImageFormat.Tiff);

            // Tesseract OCR

            Rect region = new Rect(
                image.Width / 4,        // remove the left 25% (should be 10px)
                image.Height / 4,       // remove the bottom 25% (should be 10px)
                image.Width / 2,        // take the next 50% of the width (should be 20px)
                image.Height / 2);      // take the next 50% of the height (should be 20px)

            string datapath = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).Parent.Parent.FullName;

            Pix pix = Pix.LoadTiffFromMemory(tiffStream.ToArray());
            TesseractEngine engine = new TesseractEngine(datapath, "eng");
            engine.SetVariable("tessedit_char_whitelist", "0123456789");
            Page page = engine.Process(pix, "", region, PageSegMode.SingleChar);
            Key = Int32.Parse(page.GetText());
        }
    }
}
