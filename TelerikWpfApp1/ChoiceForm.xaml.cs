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
    /// Interaction logic for ChoiceForm.xaml
    /// </summary>
    public partial class ChoiceForm : Window
    {
        public IDynamicList DynamicList { get; set; }
        
        public RemoteEntity SelectedObject { get; set; }
        
        public ChoiceForm()
        {
            InitializeComponent();
        }

        public ChoiceForm(IDynamicList dynamicList, RemoteEntity currentItem = null)
        {
            this.DynamicList = dynamicList;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            choiceGrid.DataContext = DynamicList;
            DynamicList?.ReadList();
            //this.choiceGrid.ItemsSource = DynamicList;
        }

        private void choiceGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.SelectedObject = (RemoteEntity)this.choiceGrid.SelectedItem;
            this.DialogResult = true;
            this.Close();
        }
    }
}
