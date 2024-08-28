using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using Apparent.Model;
using System.Threading.Tasks;
using Azure.Core;

namespace Apparent
{
    public class ImageService
    {
        public async void createImages()
        {

            string name = "Sourav";

            using (Bitmap bmp = new Bitmap(200, 100))
            {
                // Create a Graphics object from the Bitmap
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // Set the background color
                    g.Clear(Color.White);

                    // Set the font and brush for drawing
                    Font font = new Font("Arial", 30);
                    SolidBrush brush = new SolidBrush(Color.Black);

                    // Get the first letter of the name
                    char firstLetter = char.ToUpper(name[0]);

                    // Draw the first letter on the image
                    string text = firstLetter.ToString();
                    g.DrawString(text, font, brush, 50, 25);
                }

                // Convert the image to a MemoryStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    bmp.Save(memoryStream, ImageFormat.Png);

                    // Return the image as a file response
                   
                }
            }
        }

        
        static string CleanFileName(string fileName)
        {
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(invalidChar, '_');
            }
            return fileName;
        }
    }


}

        
    
