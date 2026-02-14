using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Windows;
using System.Windows.Controls; 
using System.Runtime.InteropServices;

namespace ProcessManagerUI
{
    public class ProcessInfo
    {
        public string ProcessName { get; set; }
        public int ProcessID { get; set; }
        public string RamUsage { get; set; }
    }

    public partial class MainWindow : Window
    {
        [DllImport("SystemCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetProcessListStr(StringBuilder buffer, int bufferSize);

        [DllImport("SystemCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool KillProcess(int pid);

      
        private List<ProcessInfo> _fullProcessList = new List<ProcessInfo>();

        public MainWindow()
        {
            InitializeComponent();
            RefreshProcessList();
        }

        
        private void RefreshProcessList()
        {
            try
            {
                StringBuilder buffer = new StringBuilder(200000);
                GetProcessListStr(buffer, buffer.Capacity);

                string rawData = buffer.ToString();
                string[] rows = rawData.Split(';');

                _fullProcessList.Clear(); 

                foreach (string row in rows)
                {
                    if (string.IsNullOrWhiteSpace(row)) continue;
                    string[] parts = row.Split('|');
                    if (parts.Length >= 3)
                    {
                        _fullProcessList.Add(new ProcessInfo
                        {
                            ProcessName = parts[0],
                            ProcessID = int.Parse(parts[1]),
                            RamUsage = parts[2] + " MB"
                        });
                    }
                }

                
                FilterList(txtSearch.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        
        private void FilterList(string searchText)
        {
           
            if (string.IsNullOrWhiteSpace(searchText))
            {
                gridProcesses.ItemsSource = _fullProcessList;
                lblStatus.Text = $"{_fullProcessList.Count} işlem";
            }
            else
            {
                
                var filteredList = _fullProcessList
                    .Where(p => p.ProcessName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                gridProcesses.ItemsSource = filteredList;
                lblStatus.Text = $"{filteredList.Count} işlem bulundu";
            }
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RefreshProcessList();
        }

       
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterList(txtSearch.Text);
        }

        
        private void BtnKill_Click(object sender, RoutedEventArgs e)
        {
            if (gridProcesses.SelectedItem is ProcessInfo selectedProc)
            {
                var result = MessageBox.Show(
                    $"{selectedProc.ProcessName} (ID: {selectedProc.ProcessID})\nSonlandırmak istiyor musun?",
                    "Dikkat", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    bool success = KillProcess(selectedProc.ProcessID);
                    if (success)
                    {
                       
                        RefreshProcessList();
                    }
                    else
                    {
                        MessageBox.Show("İşlem sonlandırılamadı! (Yetki sorunu).");
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.");
            }
        }
    }
}