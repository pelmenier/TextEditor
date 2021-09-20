using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TextCut.Models;
using TextCut.Services;

namespace TextCut
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Text (*.txt)|";
            openFileDialog1.Multiselect = true;
            EnableDisplays(false);
        }

        List<TextFile> _itemsFile = new List<TextFile>();
        int _caracters = 0;
        string _filePathToBeSaved = "";

        #region btn_Click
        private void buttonEditFile_Click(object sender, EventArgs e)
        {
            _caracters = TextBoxValueToInt();

            if (_itemsFile.Count != 0)
                TextEditor(_itemsFile);
        }
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            EnableDisplays(true);
            ClearDispleyList();
            ClearList();
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string[] filename = openFileDialog1.FileNames;
            // читаем файл в строку
            foreach(var item in filename)
            {
                var s = item.Split('\\');
                //openFileDialog1
                _itemsFile.Add(new TextFile(item,s[s.Length-1]));
            }            
            MessageBox.Show("Файл открыт");

            DisplaySelectedFiles(_itemsFile);
        }

        private void buttonChooseSaveFolder_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void forProgrammToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа обрабатывает выбранные вами текстовые файлы.\nЧисло N указывает на кол-во символов меньше которых слово будет удалено из текстового файла.\nЕсли файл отмечен в нем будут удалены знаки припенания.\nВыбранные файлы сохранятся в указанную вами директорию.");
        }
        #endregion

        #region created methods

        private void DisplaySelectedFiles(List<TextFile> items)
        {
            foreach(var item in items)
            {
                checkedListBox1.Items.Add(item.FilePath);
            }
        }  
        
        private async void TextEditor(List<TextFile> items)
        {
            EnableDisplays(false);
            foreach (var item in items)
            {          
                string text = "";                
                using (FileStream file = new FileStream(item.FilePath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        bool flag = false;
                        string line;
                        foreach(var checkedItem in checkedListBox1.CheckedItems)
                        {
                            if (checkedItem == item.FilePath)
                                flag = true;
                        }
                        while ((line = await sr.ReadLineAsync()) != null)
                        {
                            if (flag)
                                line = EditorService.RemovePunctuationMarks(line);
                            if (_caracters < 0)
                                return;

                            text += EditorService.RemoveWorld(line, _caracters);                            
                        }
                    }
                    File.WriteAllText($"{_filePathToBeSaved}\\{item.FileName}", text);  
                }
                              
            }
            if(_caracters >= 0)
                MessageBox.Show("Файлы обработанны!");
            ClearList();
            ClearDispleyList();            
        }   

        void EnableDisplays(bool enable)
        {
            textBoxCharAmount.Enabled = enable;
            checkedListBox1.Enabled = enable;
            buttonChooseSaveFolder.Enabled = enable;
        }

        void ClearList()
        {
            _itemsFile.Clear();
        }

        void ClearDispleyList()
        {
            checkedListBox1.Items.Clear();
        }

        int TextBoxValueToInt()
        {
            int value = -1;
            if (textBoxCharAmount.TextLength > 0)
            {
                try
                {
                    value = Convert.ToInt32(textBoxCharAmount.Text);
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);                    
                }                
            }                
            return value;
        }

        void SaveFile()
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                labelFileSavePath.Text = folderBrowserDialog1.SelectedPath;
                _filePathToBeSaved = folderBrowserDialog1.SelectedPath;
            }
        }

        #endregion
        
    }
}
