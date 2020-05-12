using AliasPro.API.Groups.Models;
using AliasPro.Groups.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace AliasPro.Groups.Imager
{
	public class BadgeImager
	{
        private IList<IGroupBadgePart> _badgeParts;
        private readonly IDictionary<string, Bitmap> _cachedImages = new Dictionary<string, Bitmap>();

        public BadgeImager()
		{

        }

		public void Initialize(IList<IGroupBadgePart> badgeParts)
		{
			_badgeParts = badgeParts;

            _cachedImages.Clear();

            foreach (IGroupBadgePart part in _badgeParts)
            {
                if (part.Type != BadgePartType.BASE && part.Type != BadgePartType.SYMBOL)
                    continue;

                if (!string.IsNullOrEmpty(part.AssetOne))
                {
                    string path = Program.ServerSettings.GetString("group.internal_imager.badge_parts") + "badgepart_" + part.AssetOne;
                    try
                    {
                        Bitmap image = new Bitmap(path);

                        if (!_cachedImages.ContainsKey(part.AssetOne))
                            _cachedImages.Add(part.AssetOne, image);
                    }
                    catch
                    {
                        Console.WriteLine("Could not load asset: " + path);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(part.AssetTwo))
                {
                    string path = Program.ServerSettings.GetString("group.internal_imager.badge_parts") + "badgepart_" + part.AssetTwo;
                    try
                    {
                        Bitmap image = new Bitmap(path);

                        if (!_cachedImages.ContainsKey(part.AssetTwo))
                            _cachedImages.Add(part.AssetTwo, image);
                    }
                    catch 
                    {
                        Console.WriteLine("Could not load asset " + path);
                    }
                }
            }
        }

        public void GenerateImage(string badge)
        {
            string path = Program.ServerSettings.GetString("group.internal_imager.output_path") + badge + ".png";

            if (File.Exists(path)) return;

            string[] parts = new string[] { "", "", "", "", "" };

            int count = 0;

            for (int i = 0; i < badge.Length;)
            {
                if (i > 0)
                {
                    if (i % 7 == 0)
                    {
                        count++;
                    }
                }

                for (int j = 0; j < 7; j++)
                {
                    parts[count] += badge[i];
                    i++;
                }
            }


            Bitmap image = new Bitmap(39, 39);
            Graphics graphics = Graphics.FromImage(image);

            foreach (string part2 in parts)
            {
                if (string.IsNullOrEmpty(part2) || part2.Length != 7)
                    continue;

                string type = part2[0] + "";
                int id = int.Parse(part2.Substring(1, 3));
                int colour = int.Parse(part2.Substring(4, 2));
                int position = int.Parse(part2.Substring(6, 1));

                IGroupBadgePart part;
                IGroupBadgePart color = GetBadgePart(BadgePartType.BASE_COLOUR, colour);

                if (type == "b")
                {
                    part = GetBadgePart(BadgePartType.BASE, id);
                }
                else
                {
                    part = GetBadgePart(BadgePartType.SYMBOL, id);
                }

                if (part == null || color == null)
                    continue;

                if (_cachedImages.TryGetValue(part.AssetOne, out Bitmap tempImageOne))
                {
                    Bitmap imageOne = tempImageOne.Clone() as Bitmap;
                    Point point = GetPoint(image, imageOne, position);

                    Recolor(imageOne, ColorTranslator.FromHtml("#" + color.AssetOne));
                    graphics.DrawImage(imageOne, point.X, point.Y);
                }

                if (_cachedImages.TryGetValue(part.AssetTwo, out Bitmap tempImageTwo))
                {
                    Bitmap imageTwo = tempImageTwo.Clone() as Bitmap;
                    Point point = GetPoint(image, imageTwo, position);

                    graphics.DrawImage(imageTwo, point.X, point.Y);
                }
            }

            try
            {
                image.Save(path, ImageFormat.Png);
            }
            catch
            {
                System.Console.WriteLine("Failed to save image!");
            }

            graphics.Dispose();
        }

        private Point GetPoint(Bitmap image, Bitmap imagePart, int position)
        {
            int x = 0;
            int y = 0;

            if (position == 1)
            {
                x = (image.Width - imagePart.Width) / 2;
                y = 0;
            }
            else if (position == 2)
            {
                x = image.Width - imagePart.Width;
                y = 0;
            }
            else if (position == 3)
            {
                x = 0;
                y = (image.Height / 2) - (imagePart.Height / 2);
            }
            else if (position == 4)
            {
                x = (image.Width / 2) - (imagePart.Width / 2);
                y = (image.Height / 2) - (imagePart.Height / 2);
            }
            else if (position == 5)
            {
                x = image.Width - imagePart.Width;
                y = (image.Height / 2) - (imagePart.Height / 2);
            }
            else if (position == 6)
            {
                x = 0;
                y = image.Height - imagePart.Height;
            }
            else if (position == 7)
            {
                x = ((image.Width - imagePart.Width) / 2);
                y = image.Height - imagePart.Height;
            }
            else if (position == 8)
            {
                x = image.Width - imagePart.Width;
                y = image.Height - imagePart.Height;
            }

            return new Point(x, y);
        }

        private void Recolor(Bitmap image, Color maskColour)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color old = image.GetPixel(x, y);

                    Color newColor = Color.FromArgb(
                        old.A,
                        ColorConvert(old.R, maskColour.R),
                        ColorConvert(old.G, maskColour.G),
                        ColorConvert(old.B, maskColour.B)
                    );

                    image.SetPixel(x, y, newColor);
                }
            }
        }

        private int ColorConvert(int oldC, int newC) => 
            (int)((double)newC / 255 * oldC);

        private IGroupBadgePart GetBadgePart(BadgePartType type, int id) => 
            _badgeParts.Where(part => part.Type == type && part.Id == id).FirstOrDefault();
    }
}
