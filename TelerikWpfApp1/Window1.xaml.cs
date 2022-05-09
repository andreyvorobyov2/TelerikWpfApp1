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

namespace TelerikWpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        DocumentObject_RetailSales retailSales;

        CatalogList_Items listItem;
        CatalogList_ItemKeys listItemKey;
        public Window1()
        {
            InitializeComponent();            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            retailSales = new DocumentObject_RetailSales();

            listItem = new CatalogList_Items();
            //listItem.ReadList();

            listItemKey = new CatalogList_ItemKeys();

            this.itemListGrid.DataContext = retailSales;
            this.itemGrid.DataContext = listItem;
            this.itemKeyGrid.DataContext = listItemKey;

            this.DataContext = retailSales;
        }

        private void showItemsButton_Click(object sender, RoutedEventArgs e)
        {
            this.paneShowItem.IsHidden =! this.paneShowItem.IsHidden;
            if(!paneShowItem.IsHidden)
                listItem.ReadList();
        }

        private void itemGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            CatalogRef_Items itemRef = (CatalogRef_Items) this.itemGrid.SelectedItem;
         
            listItemKey.FilterByItem = itemRef;
            listItemKey.ReadList();
        }

        private void itemKeyGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CatalogRef_ItemKeys itemKeyRef = (CatalogRef_ItemKeys)this.itemKeyGrid.SelectedItem;
            if (itemKeyRef != null)
            {
                RetailSales_ItemList_Row new_row = (RetailSales_ItemList_Row)retailSales.ItemList.AddNew();
                new_row.Item = itemKeyRef.Item;
                new_row.ItemKey = itemKeyRef;
            }
        }
    }
}
