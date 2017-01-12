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




namespace KassaSystee
{
    /// <summary>
    /// Interaction logic for dialog.xaml
    /// </summary>
    public partial class dialog : Window
    {
        public static string value;
        //static TextBox tb;
        public static dialog dg;
        
        public dialog()
        {
            dg = this;
                
            InitializeComponent();
           //tb = tbValue;
        }
        public void Button_Click(object sender, RoutedEventArgs e)
        {
           if(tbValue.Text != "")
            {
                value = tbValue.Text;
                MainWindow.getValue();
                this.Hide();
                enter.Click += enterKeyDialog;
            }
            else
            {
                MessageBox.Show("U moet een aantal invullen");
            }  
        }
        private void enterKeyDialog(object sender, RoutedEventArgs e)
        {
            btnSendValue.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            
        }
        private void backKeyDialog(object sender, RoutedEventArgs e)
        {
            if (tbValue.Text != "")
            {
                string back = tbValue.Text.Substring(0, tbValue.Text.Length - 1);
                tbValue.Text = back;
            }
        }

        private void keyboardDialog(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            tbValue.Text = tbValue.Text + (string)button.Content;
        }

        private void SpaceBarDialog(object sender, RoutedEventArgs e)
        {
            tbValue.Text = tbValue.Text + "0";
        }
    }
}
