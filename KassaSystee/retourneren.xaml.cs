using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for retourneren.xaml
    /// </summary>
    public partial class retourneren : Window
    {
        List<Items> zoekResult = new List<Items>();//list met alle producten in alle categorieën
        List<Items> zoekPlanten = new List<Items>();//list met alle producten in de categorie Planten
        List<Items> zoekMestgrond = new List<Items>();//list met alle producten in de categorie Mestgrond
        List<Items> zoekGereedschap = new List<Items>();//list met alle producten in de categorie gereedschap
        List<Items> zoekOpId = new List<Items>();
        List<Items> selectedItems = new List<Items>();

        DataTable dt = new DataTable("MyTable");
        int aantalProducten = 0;
        int aantalOptellen = 0;
        TextBox tb = new TextBox();
        DataGridCell dgcell;
        DataGridColumn dgc;
        static string test = "";
        public static dialog Dialog = new dialog();
        public static loadScreen Loadscreen;
        public static retourneren retour;
        private MainWindow mw;


        TextBox focus;
        Button lastClicked;
        public retourneren(MainWindow mw)
        {
            this.mw = mw;
            InitializeComponent();
            focus = tbZoek;
            lastClicked = alles;
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Prijs", typeof(string));
            dt.Columns.Add("Aantal", typeof(string));
            // alles.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
        private async void keyboard(object sender, RoutedEventArgs e)//te snel tpen gaat fout
        {
            try
            {
                Button button = (Button)sender;
                TextBox typen = focus;
                typen.Text = typen.Text + (string)button.Content;
                focus.Focus();
            }
            catch
            {

            }


            //if(focus == tbZoek)
            //{
            //    //filter();
            //}
            //else functie voor typen in tbBarcode
            //{

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
            try
            {
                if (sender == null)
                {
                    sender = alles;
                }
                Button button = (Button)sender;
                lastClicked.FontStyle = FontStyles.Normal;
                lastClicked = button;
                button.FontStyle = FontStyles.Italic;
                //mySql();
                loadExcactGegevensMetFilter(button.Content.ToString().ToLower());
                focus.Focus();
            }
            catch
            {

            }


        }
        private void enterKey(object sender, RoutedEventArgs e)
        {
            try
            {
                if (focus == tbZoek)
                {
                    loadExcactGegevensMetFilter(lastClicked.Content.ToString().ToLower());
                    //filter();
                }
                if (focus == tbBarcode)
                {
                    loadExcactGegevensMetFilter("barcode");
                    if (list.Items.Count != 1)
                    {
                        MessageBox.Show("Vul een geldige barcode in");
                    }
                    else
                    {
                        string[] barcode = list.Items[0].ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        //string[] barcode = barcodeItem.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        fillDatagrid(barcode);
                        tbBarcode.Text = "";
                    }


                }
                focus.Focus();
            }
            catch
            {

            }
            // mySql();



        }
        private void shiftKey(object sender, RoutedEventArgs e)
        {
            foreach (Grid grid in keyboardKeys.Children)
            {
                foreach (Button button in grid.Children)
                {
                    if (button.Content.ToString().ToUpper() == button.Content.ToString())
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
            focus.Focus();
        }
        private void backKey(object sender, RoutedEventArgs e)
        {
            TextBox typen = focus;
            if (typen.Text != "")
            {
                string back = typen.Text.Substring(0, typen.Text.Length - 1);
                typen.Text = back;
            }
            focus.Focus();
        }
        private void ZoekSalesItemPrices()
        {
            zoekResult.Clear();
            zoekPlanten.Clear();
            zoekMestgrond.Clear();
            zoekGereedschap.Clear();
            zoekOpId.Clear();
            zoekResult.AddRange(Inlog.Result.Where(xx => xx.Description.ToLower().Contains(tbZoek.Text.ToLower()) || xx.Code.ToLower().Contains(tbZoek.Text.ToLower())));
            zoekPlanten.AddRange(Inlog.Planten.Where(xx => xx.Description.ToLower().Contains(tbZoek.Text.ToLower()) || xx.Code.ToLower().Contains(tbZoek.Text.ToLower())));
            zoekMestgrond.AddRange(Inlog.Mestgrond.Where(xx => xx.Description.ToLower().Contains(tbZoek.Text.ToLower()) || xx.Code.ToLower().Contains(tbZoek.Text.ToLower())));
            zoekGereedschap.AddRange(Inlog.Gereedschap.Where(xx => xx.Description.ToLower().Contains(tbZoek.Text.ToLower()) || xx.Code.ToLower().Contains(tbZoek.Text.ToLower())));
            zoekOpId.AddRange(Inlog.opId.Where(xx => xx.Code.ToLower().Contains(tbBarcode.Text.ToLower())));
        }

        private void loadExcactGegevensMetFilter(string categorie)
        {
            list.Items.Clear();//list =  listbox.name
            ZoekSalesItemPrices();
            if (categorie == "alles")
            {
                foreach (Items item in zoekResult)
                {
                    string afgerondePrijs = "€ " + item.Price.ToString("0.00");
                    string test = item.Code + "\n" + item.Description + "\n" + afgerondePrijs + "\n" + item.Categorie;
                    list.Items.Add(test);
                }
            }
            if (categorie == "planten")
            {
                foreach (Items item in zoekPlanten)
                {
                    string afgerondePrijs = "€ " + item.Price.ToString("0.00");
                    string test = item.Code + "\n" + item.Description + "\n" + afgerondePrijs + "\n" + item.Categorie;
                    list.Items.Add(test);
                }
            }
            if (categorie == "mestgrond")
            {
                foreach (Items item in zoekMestgrond)
                {
                    string afgerondePrijs = "€ " + item.Price.ToString("0.00");
                    string test = item.Code + "\n" + item.Description + "\n" + afgerondePrijs + "\n" + item.Categorie;
                    list.Items.Add(test);
                }
            }
            if (categorie == "gereedschap")
            {
                foreach (Items item in zoekGereedschap)
                {
                    string afgerondePrijs = "€ " + item.Price.ToString("0.00");
                    string test = item.Code + "\n" + item.Description + "\n" + afgerondePrijs + "\n" + item.Categorie;
                    list.Items.Add(test);
                }
            }
            if (categorie == "barcode")
            {
                foreach (Items item in zoekOpId)
                {
                    string afgerondePrijs = "€ " + item.Price.ToString("0.00");
                    string test = item.Code + "\n" + item.Description + "\n" + afgerondePrijs + "\n" + item.Categorie;
                    list.Items.Add(test);
                }
            }
        }

        private async void load_excact(object sender, RoutedEventArgs e)
        {
            loadExcactGegevensMetFilter("alles");
        }

        private void tbZoek_TouchDown(object sender, TouchEventArgs e)//werkt evt toevoegen zodat hij bij een touch schem ook werkt
        {
            // tbBarcode.Text = "TouchedTbZoek";
        }
        private void fillDatagrid(string[] selectedItems)
        {
            aantalProducten += 1;
            tbAantalProducten.Text = aantalProducten.ToString();
            ID.Binding = new Binding("ID");
            Description.Binding = new Binding("Description");
            Prijs.Binding = new Binding("Prijs");
            Aantal.Binding = new Binding("Aantal");

            bool KomtVoorInData = false;

            if (dt.Rows.Count != 0)
            {
                for (int count = 0; count < dt.Rows.Count; count++)
                {
                    if (selectedItems[0].ToString() == dt.Rows[count][0].ToString())
                    {
                        dt.Rows[count][3] = ((int.Parse(dt.Rows[count][3].ToString())) + 1).ToString();
                        KomtVoorInData = true;
                    }
                }
                if (KomtVoorInData == false)
                {
                    dt.Rows.Add(selectedItems[0], selectedItems[1], selectedItems[2], "1");
                }
                int tel = dt.Rows.Count;
                tbTotaalExclBTW.Text = "€ 0.00";
                for (int count = 0; count < tel; count++)
                {
                    berekenTotaal(count, tel);
                    data.ItemsSource = dt.DefaultView;
                }
            }
            else
            {
                int count = 0;
                int tel = 1;
                dt.Rows.Add(selectedItems[0], selectedItems[1], selectedItems[2], "1");
                data.ItemsSource = dt.DefaultView;
                berekenTotaal(count, tel);
            }
            focus.Focus();
        }
        private void berekenTotaal(int count, int tel)
        {
            tbTotaalExclBTW.Text = "€ " + (double.Parse(tbTotaalExclBTW.Text.Substring(2)) + (double.Parse(dt.Rows[count][2].ToString().Substring(2))) * int.Parse(dt.Rows[count][3].ToString())).ToString("0.00");
            tbTotaalInclBTW.Text = "€ " + (double.Parse(tbTotaalExclBTW.Text.Substring(2)) + double.Parse(tbTotaalExclBTW.Text.Substring(2)) * 0.21).ToString("0.00");
        }
        private void clickListItem(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (list.Items.Count != 0)
                {
                    string[] selectedItems = list.SelectedItem.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    fillDatagrid(selectedItems);
                }
            }
            catch
            {

            }


        }

        public void changeAantal(object sender, DataGridCellEditEndingEventArgs e)
        {
            tb = e.EditingElement as TextBox;  // Assumes columns are all TextBoxes
            dgc = e.Column;
            tb.Text = test;

            object item = data.SelectedItem;
            string ID = (data.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            string beschrijving = (data.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
            string prijs = (data.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text;
            string aantal = tb.Text;
            int tel = dt.Rows.Count;
            aantalOptellen = 0;
            tbTotaalExclBTW.Text = "€ 0.00";
            for (int count = 0; count < tel; count++)
            {
                if (ID == dt.Rows[count][0].ToString())
                {
                    dt.Rows[count][3] = aantal;
                }
                try
                {
                    aantalOptellen = aantalOptellen + int.Parse(dt.Rows[count][3].ToString());
                    tbAantalProducten.Text = aantalOptellen.ToString();
                    berekenTotaal(count, tel);
                    data.ItemsSource = dt.DefaultView;
                }
                catch
                {

                }

            }
        }
        public static void getValue()
        {
            test = Dialog.tbValue.Text;
        }


        private void data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // dialog Dialog = new dialog();
            Dialog.tbValue.Text = "";
            Dialog.Title = "Aantal veranderen";
            Dialog.ShowDialog();
            //  Dialog.Show();
            enter.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void tbZoek_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                tbBarcode.Text = tbZoek.Text;
            }
        }
        private void tbBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                tbBarcode.Focus();
                focus = tbBarcode;
                loadExcactGegevensMetFilter("barcode");
                if (list.Items.Count != 1)
                {
                    MessageBox.Show("Scan een geldige barcode");
                }
                else
                {
                    string[] barcode = list.Items[0].ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    fillDatagrid(barcode);
                    tbBarcode.Text = "";
                    tbBarcode.Focus();
                    focus = tbBarcode;
                }
            }
            tbBarcode.Focus();
            focus = tbBarcode;
        }

        private void btnVoegToe_Click(object sender, RoutedEventArgs e)
        {
            if (focus == tbBarcode)
            {
                enter.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

        }

        private void btnBonRetourneren(object sender, RoutedEventArgs e)
        {
            
        }

        private void Afrekenen(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            //mw.Show();
        }

        private void deleteItem(object sender, RoutedEventArgs e)
        {
            int index = data.SelectedIndex;
            int aantal = int.Parse(tbAantalProducten.Text) - int.Parse(dt.Rows[index][3].ToString());
            dt.Rows[index].Delete();
            data.ItemsSource = dt.DefaultView;
            berekenTotaal(0, dt.Rows.Count);
            tbAantalProducten.Text = aantal.ToString();
        }
    }
}
