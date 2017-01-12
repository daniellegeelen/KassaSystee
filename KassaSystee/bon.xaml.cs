using System;
using System.Collections.Generic;
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
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;
using System.Data;

namespace KassaSystee
{
    /// <summary>
    /// Interaction logic for Bon.xaml
    /// </summary>
    public partial class bon : Window
    {
        DataTable dt;
        string totaalAantal;
        string totaalPrijsInclBTW;
        string totaalPrijsExcBTW;
        string afrekenenOfRetourneren;
        double wisselgeld;
        public bon(DataTable dt, string totaalAantal, string totaalPrijsInclBTW, string totaalPrijsExcBTW, string afrekenenOfRetourneren)
        {
            InitializeComponent();
            this.dt = dt;
            this.totaalAantal = totaalAantal;
            this.totaalPrijsInclBTW = totaalPrijsInclBTW;
            this.totaalPrijsExcBTW = totaalPrijsExcBTW;
            this.afrekenenOfRetourneren = afrekenenOfRetourneren;
            tbAfrekenenOfRetourneren.Text = afrekenenOfRetourneren;
            gbBerekenWisselgeld.Visibility = Visibility.Hidden;
        }

        private void printBonAfrekenen(object sender, RoutedEventArgs e)
        {
            try
            {
                if(rbPinnen.IsChecked == true)
                {

                }
                else if(rbContant.IsChecked == true)
                {
                    if (tbKlantGeeft.Text != "")
                    {
                    btnBerekenWisselgeld.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    printpdf print = new printpdf("Retourneren", dt, totaalAantal, totaalPrijsInclBTW, totaalPrijsExcBTW);
                    }
                    else
                    {
                    MessageBox.Show("Vul een getal in");
                    }
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Wisselgeld(object sender, RoutedEventArgs e)
        {
            if(tbWisselgeld.Text != "")
            {
                try
                {
                wisselgeld = double.Parse(tbKlantGeeft.Text) -  double.Parse(totaalPrijsInclBTW.Remove(0,2)) ;
                    tbWisselgeld.Text = "€ " + wisselgeld.ToString("0.00");
                }
                catch
                {
                    MessageBox.Show("Vul een geldige prijs in");
                }
                
            }
           
        }

        private void rbPinnen_Checked(object sender, RoutedEventArgs e)
        {
            gbBerekenWisselgeld.Visibility = Visibility.Hidden;
        }

        private void rbContant_Checked(object sender, RoutedEventArgs e)
        {
            gbBerekenWisselgeld.Visibility = Visibility.Visible;
        }

        private void keyboard(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                tbKlantGeeft.Focus();
                tbKlantGeeft.Text = tbKlantGeeft.Text + (string)button.Content;
                
            }
            catch
            {

            }
        }

        private void backKey(object sender, RoutedEventArgs e)
        {
            tbKlantGeeft.Focus();
          
            if (tbKlantGeeft.Text != "")
            {
                string back = tbKlantGeeft.Text.Substring(0, tbKlantGeeft.Text.Length - 1);
                tbKlantGeeft.Text = back;
            }
        }
    }
}
