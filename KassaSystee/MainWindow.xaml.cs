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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;

namespace KassaSystee
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Inlog inlog;
        List<Items> zoekResult = new List<Items>();
        
        TextBox focus;
        string categorie;
        Button lastClicked;
        MySqlCommand cmd;
        public MainWindow()
        {
            this.InitializeComponent();
            inlog = new Inlog(this);
            inlog.Show();
            this.Hide();
            focus = tbZoek;
            lastClicked = alles;
            

            // alles.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private async void keyboard(object sender, RoutedEventArgs e)//te snel tpen gaat fout
        {
            Button button = (Button)sender;
            TextBox typen = focus;
            typen.Text = typen.Text + (string)button.Content;
            //if(focus == tbZoek)
            //{
            //    //filter();
            //}
            //else functie voor typen in tbBarcode
            //{

            //}
        }
      
        private async void filter()
        {
            list.Items.Clear();
            loadExcactGegevensMetFilter();//WERKT NIET
            
            //await inlog.getSalesItemPrices(tbZoek.Text);
            //foreach (Items gla in inlog.Result)
            //{
            //    TextBlock tb = new TextBlock();
            //    tb.Text = gla.Code + "\t" + gla.Description + "\t" + gla.Price.ToString();
            //    tb.Tag = gla;
            //    //  tb.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
            //    tb.Width = 500;
            //    tb.Height = 32;
            //    tb.Padding = new Thickness(20, 0, 0, 0);
            //    tb.Margin = new Thickness(0);
            //    list.Items.Add(tb);//list = control op scherm
            //    // tb.Tapped += TextBlock_Tapped;
            //}

        }
        private void TbBarcode_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            focus = tbBarcode;
        }
        private void tbZoek_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            focus = tbZoek;
        }
        private void Button_Click(object sender, RoutedEventArgs e)//categorie buttons
        {
            if(sender == null)
            {
                sender = alles;
            }
            Button button = (Button)sender;
            lastClicked.FontStyle = FontStyles.Normal;
            lastClicked = button;
            categorie = (string)button.Content;
            button.FontStyle = FontStyles.Italic;
            //mySql();
            loadExcactGegevensMetFilter();
        }
        private void enterKey(object sender, RoutedEventArgs e)
        {
            // mySql();
            if (focus == tbZoek)
            {
                loadExcactGegevensMetFilter();
                //filter();
            }
          //  if (e.KeyCode == Keys.Enter)
                {
                    //enter key is down
                }
            //else functie voor typen in tbBarcode
            //{

            //}
        }
        private void shiftKey(object sender, RoutedEventArgs e)
        { 
            foreach(Grid grid in keyboardKeys.Children)
            {
                foreach(Button button in grid.Children)
                {
                    if(button.Content.ToString().ToUpper() == button.Content.ToString())
                    {
                        button.Content = button.Content.ToString().ToLower();
                        space.Content = space.Content.ToString().ToLower();
                    }
                    else
                    {
                        button.Content = button.Content.ToString().ToUpper();
                        space.Content = space.Content.ToString().ToLower();

                    }
                }   
            }
        }
        private void backKey(object sender, RoutedEventArgs e)
        {
            TextBox typen = focus;
            if(typen.Text != "")
            {
                string back = typen.Text .Substring(0,typen.Text.Length -1);
                typen.Text = back;
            }
           
        }

        //static void tbZoeken_KeyDown(object sender, KeyEventArgs e)
        //{
        //   // if (e.KeyCode == Keys.Enter)
        //    //{
        //        //enter key is down
        //    //}
        //}
        private void ZoekSalesItemPrices()
        {
            zoekResult.Clear();
            
            zoekResult.AddRange(inlog.Result.Where(xx => xx.Description.ToLower().Contains(tbZoek.Text.ToLower())));
        }

        private void loadExcactGegevensMetFilter()
        {

            list.Items.Clear();//list =  listbox.name
            // await inlog.getSalesItemPrices();
            ZoekSalesItemPrices();


            foreach (Items item in zoekResult)
            {
                TextBlock code = new TextBlock();
                TextBlock description = new TextBlock();
                TextBlock prijs = new TextBlock();
               // code.Name = "code";
                //description.Name = "description";
                //prijs.Name = "prijs";

                
                string afgerondePrijs = item.Price.ToString("0.00");
                code.Text = item.Code;
                description.Text = "\n" + item.Description;
                prijs.Text = "\n" + afgerondePrijs;

               // code.Height = 100;
                //description.Height = 100;
                //prijs.Height = 100;

                TextBlock listItem = new TextBlock();
               // listItem.Height = 100;
                listItem.Text = code.Text + description.Text + prijs.Text;
                list.Items.Add(listItem);

                
                
                
                //TextBlock tb = new TextBlock();
                //tb.Text = item.Code + "\t" + item.Description + "\t" + afgerondePrijs;
                //tb.Tag = item;
                ////tb.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                //tb.Width = 500;
                //tb.Height = 32;
                //tb.Padding = new Thickness(20, 0, 0, 0);
                //tb.Margin = new Thickness(0);
              //  list.Items.Add(tb);//list = control op scherm
                //  tb.Tapped += TextBlock_Tapped;

            }
        }

        private async void load_excact(object sender, RoutedEventArgs e)
        {
            await inlog.getSalesItemPrices();
            loadExcactGegevensMetFilter();
        }

        private void tbZoek_TouchDown(object sender, TouchEventArgs e)//werkt evt toevoegen zodat hij bij een touch schem ook werkt
        {
           // tbBarcode.Text = "TouchedTbZoek";
        }

        private void clickListItem(object sender, MouseButtonEventArgs e)
        {
           // MessageBox.Show("klikked");
            object clicked = (object)sender;
          
            data.DataContext = "";
            DataTable dt = new DataTable();
            
            dt.Clear();
            dt.Columns.Add("ID");
            dt.Columns.Add("Description");
            dt.Columns.Add("Prijs");
            dt.Columns.Add("Aantal");
            DataRow r = dt.NewRow();
            r["ID"] = clicked.ToString();
            r["Description"] = "g";
            r["Prijs"] = "g";
            r["Aantal"] = "g";
            data.DataContext = dt;
            
            //for(int ii=0; ii < zoekResult.Count; ii+=3)
            //{
            //    List<Items> range = new List<Items>();
            //    range = zoekResult.GetRange(ii, 3);
            //    DataRow r = dt.NewRow();
            //    for(int iii =0; iii < 3; iii++)
            //    {
            //        r["ID"] = range.GetRange(iii, 1);
            //        r["Description"] = range.GetRange(iii, 1);
            //        r["Prijs"] = range.GetRange(iii, 1);
            //        r["Aantal"] = range.GetRange(iii, 1);
            //    }
            //}

            // DataRow r1 = dt.NewRow();
            //  r1["Name"] = "ravi";
            //  r1["Marks"] = "500";
            //   dt.Rows.Add(r1);

            


            //c1.Binding = new Binding("ID");
            //c2.Binding = new Binding("Description");
            //c3.Binding = new Binding("prijs");
            //c4.Binding = new Binding("Aantal");
            //data.Columns.Add(c1);
            //data.Columns.Add(c2);
            //data.Columns.Add(c3);
            //data.Columns.Add(c4);





            //data.Items.Add(clicked);


            //data.Items.Add( { Num = 1, Start = "2012, 8, 15", Finich = "2012, 9, 15" });
            // data.Items.Add(new Items() { Num = 2, Start = "2012, 12, 15", Finich = "2013, 2, 1" });
            //data.Items.Add(new Items() { Num = 3, Start = "2012, 8, 1", Finich = "2012, 11, 15" });
        }
    }
}
