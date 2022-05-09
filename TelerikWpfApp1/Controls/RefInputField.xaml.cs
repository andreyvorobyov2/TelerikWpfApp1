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
using System.Reflection;

namespace TelerikWpfApp1.Controls
{
    /// <summary>
    /// Interaction logic for RefInputField.xaml
    /// </summary>
    public partial class RefInputField : UserControl
    {
        public static DependencyProperty CaptionProp = 
            DependencyProperty.Register(nameof(Caption), typeof(string), typeof(RefInputField));

        public string Caption 
        { 
            get => (string)GetValue(CaptionProp);
            set => SetValue(CaptionProp, value);
        }

        public static DependencyProperty PathToPropertyProp =
            DependencyProperty.Register("PathToProperty", typeof(object), typeof(RefInputField));

        public object PathToProperty 
        {
            get => GetValue(PathToPropertyProp);
            set => SetValue(PathToPropertyProp, value);
        }

        public RefInputField()
        {
            InitializeComponent();
            this.refInput.OnOpenChoiceForm += GetPathToProperty;
        }

        private string GetPathToProperty()
        {    
            Binding binding = BindingOperations.GetBinding(this, PathToPropertyProp);
            if (binding == null)
                return null;
            return binding.Path.Path;
        }

    }
}
