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
    /// Interaction logic for RefInput.xaml
    /// </summary>
    public partial class RefInput : UserControl
    {
        public static DependencyProperty PathToPropertyProp =
            DependencyProperty.Register(nameof(PathToProperty), typeof(object), typeof(RefInput));

        public object PathToProperty
        {
            get => GetValue(PathToPropertyProp);
            set => SetValue(PathToPropertyProp, value);
        }

        public RefInput()
        {
            InitializeComponent();
        }

        private string GetPathToProperty()
        {
            Binding binding = BindingOperations.GetBinding(this, PathToPropertyProp);
            if (binding == null)
                return null;
            return binding.Path.Path;
        }

        public delegate string RefInputEventHandler();

        public event RefInputEventHandler OnOpenChoiceForm;

        private void choiceButton_Click(object sender, RoutedEventArgs e)
        {
            string bindedPropName = null;

            if (OnOpenChoiceForm == null)
            {
                bindedPropName = GetPathToProperty();
            }
            else
            {
                bindedPropName = OnOpenChoiceForm();
            }
            
            if(bindedPropName != null)
                OpenChoiceForm(bindedPropName);
        }
        public void OpenChoiceForm(string bindedPropName)
        {

            IDynamicList dynamicList = null;
            PropertyInfo propertyInfo = null;

            //string bindedPropName = binding.Path.Path;
            PropertyInfo[] props = DataContext.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == bindedPropName)
                {
                    IReference reference = (IReference)prop.GetValue(DataContext);

                    dynamicList = (IDynamicList)Activator.CreateInstance(reference.ListOfReferences);
                    propertyInfo = prop;
                    break;
                }
            }

            if (dynamicList == null)
                return;

            ChoiceForm choiceForm = new ChoiceForm(dynamicList); // default choice form
            if (choiceForm.ShowDialog() == true)
            {
                propertyInfo.SetValue(DataContext, choiceForm.SelectedObject);
            }
        }
    }
}
