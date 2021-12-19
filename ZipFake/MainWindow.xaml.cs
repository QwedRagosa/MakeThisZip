﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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

namespace ZipFake
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnArchiving_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Архивчики(*.zip) | *.zip";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (saveFileDialog.ShowDialog() == true)
                    CreateZipFile(saveFileDialog.FileName, openFileDialog.FileNames);
            }
        }

        public void CreateZipFile(string fileName, IEnumerable<string> files)
        {
            try
            {
                var zip = ZipFile.Open(fileName, ZipArchiveMode.Create);
                foreach (var file in files)
                {
                    zip.CreateEntryFromFile(file, System.IO.Path.GetFileName(file), CompressionLevel.Optimal);
                }
                zip.Dispose();
                Process.Start("explorer.exe", "/select," + fileName);
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка","О НЕТ",MessageBoxButton.OK,MessageBoxImage.Error);
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
        }
        private void DisArchiving_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Архивчики(*.zip) | *.zip";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if(openFileDialog.ShowDialog() == true)
            {
                string path = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                ZipFile.ExtractToDirectory(openFileDialog.FileName, path);
                Process.Start("explorer.exe", "/open," + path);
            }
        }
    }
}
