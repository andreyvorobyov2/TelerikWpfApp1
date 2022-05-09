using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace TelerikWpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //StyleManager.ApplicationTheme = new Office2019Theme();

            StyleManager.ApplicationTheme = new VisualStudio2019Theme();
            VisualStudio2019Palette.LoadPreset(VisualStudio2019Palette.ColorVariation.Blue);
            
            this.InitializeComponent();
        }
    }
}
