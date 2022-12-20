using CPUConsole;
using CPUConsole.Commands;
using CPUConsole.Commands.Flow;
using CPUConsole.Memory;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CpuGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CPU cpu;
        bool superUser = false;
        public MainWindow()
        {
            cpu = new CPU();
            cpu.CommandWasExecute += UpdateRegView;
            cpu.CommandWasExecute += UpdateButtonVisible;
            cpu.CommandWasDump += UpdateCommandDumpView;




            InitializeComponent();
            InitRegister();

           // codeTextBox.Text = p.textCode;
           // cpu.AddCommands(p.GetCommands(codeTextBox.Text));
            //cpu.ExecuteCommands();


        }

        public void InitRegister()
        {
            foreach (var reg in cpu.registers.Integer)
                intRegListBox.Items.Add(reg);
            foreach (var reg in cpu.registers.Float)
                floatRegListBox.Items.Add(reg);
            foreach (var reg in cpu.registers.Flags)
                flagsListBox.Items.Add($"{reg.Key}[{reg.Value}]");
            foreach (var mem in cpu.mem.mem)
                memoryListBox.Items.Add(mem);

            ProgramCounterLabel.Content = cpu.registers.ProgrammCounter;

        }

        public void UpdateRegView()
        {
            for (int i = 0; i < cpu.registers.Integer.Length; i++)
                intRegListBox.Items[i] = cpu.registers.Integer[i];

            for (int i = 0; i < cpu.registers.Float.Length; i++)
                floatRegListBox.Items[i] = cpu.registers.Float[i];

            for (int i = 0; i < cpu.registers.Float.Length; i++)
                memoryListBox.Items[i] = cpu.mem.mem[i];

            var flag = cpu.registers.Flags.ToList();
            for (int i = 0; i < cpu.registers.Flags.Count; i++)
            {

                flagsListBox.Items[i] = ($"{flag[i].Key}[{flag[i].Value}]");
            }

            ProgramCounterLabel.Content = cpu.registers.ProgrammCounter;

        }

        public void UpdateButtonVisible()
        {
            if (cpu.registers.ProgrammCounter == 0)
            {
                prevExecuteCommandButton.IsEnabled = false;
                startCpuButton.Content = "Start";
            }
            else
            {
                prevExecuteCommandButton.IsEnabled = true;
                startCpuButton.Content = "Restart";
            }


            if (cpu.registers.ProgrammCounter >= cpu.CountCommand)
                nextExecuteCommandButton.IsEnabled = false;
            else
                nextExecuteCommandButton.IsEnabled = true;
        }

        private void startCpuButton_Click(object sender, RoutedEventArgs e)
        {
            cpu.Clear();
            cpu.AddCommands(Parser.GetCommands(codeTextBox.Text));
            cpu.ExecuteCommands();
        }

        public void UpdateCommandDumpView(IDump dump)
        {
            commandDumperTextBlock.Text = dump.Dump();
        }
        private void nextExecuteCommandButton_Click(object sender, RoutedEventArgs e)
        {
            cpu.registers.Flags[FlagsRegister.StepByStep] = true;
            cpu.ExcuteCommandNext();
        }

        private void prevExecuteCommandButton_Click(object sender, RoutedEventArgs e)
        {

            cpu.ExcuteCommandPrev();
            cpu.registers.Flags[FlagsRegister.StepByStep] = true;
            flagsListBox.Items[5] = ($"{FlagsRegister.StepByStep}[True]");
        }

        private void intRegListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var curItem = intRegListBox.SelectedItem;
            var index = intRegListBox.SelectedIndex;
            if (index != -1 && superUser)
                createChangeItemofListBoxWindow(curItem, index);


        }

        void createChangeItemofListBoxWindow(object item, int index)
        {
            Window w1 = new Window();

            StackPanel g1 = new StackPanel();
            TextBox txt1 = new TextBox();
            Button btn1 = new Button();

            void Btn1_Click(object sender, RoutedEventArgs e)
            {

                if (item is int)
                {
                    int num;
                    if (int.TryParse(txt1.Text, out num))
                    {
                        cpu.registers.Integer[index] = num;
                        intRegListBox.Items[index] = num;
                        w1.Close();
                    }
                }
                if (item is float)
                {
                    float num;
                    if (float.TryParse(txt1.Text, out num))
                    {
                        cpu.registers.Float[index] = num;
                        floatRegListBox.Items[index] = num;
                        w1.Close();
                    }
                }

            }



            btn1.Height = 20;
            btn1.Width = 75;
            btn1.Margin = new Thickness(0, 5, 0, 5);
            btn1.Content = "Ok";

            txt1.Text = item.ToString();
            txt1.Width = 200;
            txt1.Height = 20;

            g1.Children.Add(txt1);
            g1.Children.Add(btn1);

            w1.Title = $"change item {item}";
            w1.Content = g1;
            w1.Activate();
            w1.Width = 300;
            w1.Height = 100;
            w1.Visibility = Visibility.Visible;

            btn1.Click += Btn1_Click;


        }

        private void floatRegListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var curItem = floatRegListBox.SelectedItem;
            var index = floatRegListBox.SelectedIndex;
            if (index != -1 && superUser)
                createChangeItemofListBoxWindow(curItem, index);
        }

        private void superUserButton_Click(object sender, RoutedEventArgs e)
        {
            superUser = !superUser;
            cpu.registers.Flags[FlagsRegister.SuperUser] = superUser;
            UpdateRegView();
        }

        private void openFileCodeButton_Click(object sender, RoutedEventArgs e)
        {
           
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                codeTextBox.Text = File.ReadAllText(ofd.FileName);
                
            }
                

        }
    }
}
