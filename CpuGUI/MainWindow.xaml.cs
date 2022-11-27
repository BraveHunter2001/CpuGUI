using CPUConsole;
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

namespace CpuGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            CPU cpu = new CPU();
            InitializeComponent();
            ListBox box = new ListBox();
            foreach (var reg in  cpu.registers.Integer)
            {
                box.Items.Add(reg.ToString());
            }
            

            mainLayout.Children.Add(box);
        }

    }
}
