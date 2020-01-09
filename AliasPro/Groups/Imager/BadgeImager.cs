using AliasPro.API.Groups.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace AliasPro.Groups.Imager
{
	public class BadgeImager
	{
		private IList<IGroupBadgePart> _badgeParts;

		public BadgeImager()
		{

		}

		public void Initialize(IList<IGroupBadgePart> badgeParts)
		{
			_badgeParts = badgeParts;
		}

        public void GenerateImage(IGroup group)
        {
            string badge = group.Badge;
            File outputFile;
            try
            {
                outputFile = new File("", badge + ".png");

                if (outputFile.exists())
                    return;
            }
            catch { return; }

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


            BufferedImage image = new BufferedImage(39, 39, BufferedImage.TYPE_INT_ARGB);
            Graphics graphics = image.getGraphics();

            foreach (string s in parts)
            {
                if (s.isEmpty())
                    continue;

                String type = s.charAt(0) + "";
                int id = Integer.valueOf(s.substring(1, 4));
                int c = Integer.valueOf(s.substring(4, 6));
                int position = Integer.valueOf(s.substring(6));

                GuildPart part;
                GuildPart color = Emulator.getGameEnvironment().getGuildManager().getPart(GuildPartType.BASE_COLOR, c);

                if (type.equalsIgnoreCase("b"))
                {
                    part = Emulator.getGameEnvironment().getGuildManager().getPart(GuildPartType.BASE, id);
                }
                else
                {
                    part = Emulator.getGameEnvironment().getGuildManager().getPart(GuildPartType.SYMBOL, id);
                }

                BufferedImage imagePart = BadgeImager.deepCopy(this.cachedImages.get(part.valueA));

                Point point;

                if (imagePart != null)
                {
                    if (imagePart.getColorModel().getPixelSize() < 32)
                    {
                        imagePart = convert32(imagePart);
                    }

                    point = getPoint(image, imagePart, position);

                    recolor(imagePart, colorFromHexString(color.valueA));

                    graphics.drawImage(imagePart, point.x, point.y, null);
                }

                if (!part.valueB.isEmpty())
                {
                    imagePart = BadgeImager.deepCopy(this.cachedImages.get(part.valueB));

                    if (imagePart != null)
                    {
                        if (imagePart.getColorModel().getPixelSize() < 32)
                        {
                            imagePart = convert32(imagePart);
                        }

                        point = getPoint(image, imagePart, position);
                        graphics.drawImage(imagePart, point.x, point.y, null);
                    }
                }
            }

            try
            {
                ImageIO.write(image, "PNG", outputFile);
            }
            catch (Exception e)
            {
                Emulator.getLogging().logErrorLine("Failed to generate guild badge: " + outputFile + ".png Make sure the output folder exists and is writable!");
            }

            graphics.dispose();
        }

    }
}
