using KassaSystee;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KassaSystee
{
    /// <summary>
    /// Interaction logic for printpdf.xaml
    /// </summary>
    public partial class printpdf : Window
    {
        string afrekenenOfRetour;
        DataTable dt;
        string totaalAantal;
        string totaalPrijsInclBTW;
        string totaalPrijsExcBTW;
        int vertLoc = 25;
        public printpdf(string afrekenenOfRetour, DataTable dt, string totaalAantal, string totaalPrijsInclBTW, string totaalPrijsExcBTW)
        {
            this.afrekenenOfRetour = afrekenenOfRetour;
            this.dt = dt;
            this.totaalAantal = totaalAantal;
            this.totaalPrijsInclBTW = totaalPrijsInclBTW;
            this.totaalPrijsExcBTW = totaalPrijsExcBTW;

            InitializeComponent();
            makepdf();
        }
        private void makepdf()
        {
            System.IO.Directory.CreateDirectory(afrekenenOfRetour);
            DateTime thisDay = DateTime.Today;
            string wanneer = thisDay.ToString("d");
            //string.Format("{yyyy'-'MM'-'dd'T'HH':'mm':'ss}", thisDay);
            string path = afrekenenOfRetour;
            int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;
            string filename = afrekenenOfRetour + @"\" + wanneer + @"_" + fCount + @".pdf";
            PdfDocument document = new PdfDocument();
            document.Info.Title = filename;

            // Create an empty page
            PdfPage page = document.AddPage();
            
            page.Height = (dt.Rows.Count * 30) + 60;
            
            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);
            

            // Create a font
            XFont font = new XFont("Verdana", 10, XFontStyle.Regular);

            // Draw the text
            gfx.DrawString(afrekenenOfRetour, font, XBrushes.Black, new XRect(20, 0, 0, 0), XStringFormats.TopLeft);
            
            for (int count = 0; count < dt.Rows.Count; count++)
            {
                gfx.DrawString(dt.Rows[count][0].ToString()+"    "+dt.Rows[count][1].ToString() + "                     " + dt.Rows[count][2].ToString() + "    " + dt.Rows[count][3].ToString(), font, XBrushes.Black, new XRect(20, vertLoc, page.Width, page.Width), XStringFormats.TopLeft);//horizontal position,vertical position,horizontal width of rect, vertical width of rect
                vertLoc +=20;
                
            }
           
            double totaalPrijsExclBTWWithoutEuroSing = double.Parse(totaalPrijsExcBTW.Remove(0, 2));
            double totaalPrijsInclBTWWithoutEuroSing = double.Parse(totaalPrijsInclBTW.Remove(0, 2));
            string BTW = "€ " + (totaalPrijsInclBTWWithoutEuroSing - totaalPrijsExclBTWWithoutEuroSing).ToString("0.00");
            gfx.DrawString("Totaal: "+ totaalPrijsExcBTW, font, XBrushes.Black, new XRect(20, vertLoc, page.Width, page.Width), XStringFormats.TopLeft);//horizontal position,vertical position,horizontal width of rect, vertical width of rect
            vertLoc += 20;
            gfx.DrawString("BTW: " + BTW, font, XBrushes.Black, new XRect(20, vertLoc, page.Width, page.Width), XStringFormats.TopLeft);//horizontal position,vertical position,horizontal width of rect, vertical width of rect
            vertLoc += 20;
            gfx.DrawString("Subtotaal: " + totaalPrijsInclBTW, font, XBrushes.Black, new XRect(20, vertLoc, page.Width, page.Width), XStringFormats.TopLeft);//horizontal position,vertical position,horizontal width of rect, vertical width of rect

            // Save the document...


            document.Save(filename);
            //MessageBox.Show("bon is opgeslagen!");
            // ...and start a viewer.
            Process.Start(filename);

        }
    }
}
