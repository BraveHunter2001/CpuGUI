using CPUConsole;
using CPUConsole.Commands.Flow;
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
        CPU cpu;
        Parser p;
        public MainWindow()
        {
            cpu = new CPU();
            p = new Parser("C:\\Users\\Ilya\\Desktop\\comm.txt");
            
            InitializeComponent();
            codeTextBox.Text = p.textCode;
            cpu.AddCommands(p.GetCommands(codeTextBox.Text));
            cpu.ExecuteCommands();
            InitRegister();




        }

        public void InitRegister()
        {
            foreach (var reg in cpu.registers.Integer)
                intRegListBox.Items.Add(reg);
            foreach (var reg in cpu.registers.Float)
                floatRegListBox.Items.Add(reg);
            foreach (var reg in cpu.registers.Flags)
                flagsListBox.Items.Add($"{reg.Key}[{reg.Value}]");
            ProgramCounterLabel.Content = cpu.registers.ProgrammCounter;
        }

        public void UpdateRegView()
        {
           for(int i = 0; i< cpu.registers.Integer.Length; i++)
                intRegListBox.Items[i] = cpu.registers.Integer[i];

            for (int i = 0; i < cpu.registers.Float.Length; i++)
                floatRegListBox.Items[i] = cpu.registers.Float[i];

            var flag = cpu.registers.Flags.ToList();
            for (int i = 0; i < cpu.registers.Flags.Count; i++)
            {

                flagsListBox.Items[i] = ($"{flag[i].Key}[{flag[i].Value}]");
            }

            ProgramCounterLabel.Content = cpu.registers.ProgrammCounter;
        }

        private void startCpuButton_Click(object sender, RoutedEventArgs e)
        {
            cpu.Clear();
            cpu.AddCommands(p.GetCommands(codeTextBox.Text));
            cpu.ExecuteCommands();
            UpdateRegView();
        }
    }
}
