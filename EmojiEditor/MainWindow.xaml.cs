using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Emoji.Wpf;
using System.Text;
using System.Linq;
using System.Windows.Media;

namespace EmojiEditor
{
    public partial class MainWindow
    {
        Microsoft.Win32.OpenFileDialog mDlgOpen = new();
        Microsoft.Win32.SaveFileDialog mDlgSave = new();

        public readonly Dictionary<char, string> emojiTable = new Dictionary<char, string>() { {'A', "💖"}, { 'B', "😁" }, { 'C', "🐨" },
            { 'D', "🐱‍" }, { 'E', "‍🐉" }, { 'F', "👻" }, { 'G', "🦑" }, { 'H', "🔥" },{'I', "✔"},{'J', "👁"},{'K', "🔔"},{'L', "💣"},{'M', "🕹"},{'N', "📞"},
            {'O', "📀"},{'P', "📖"}, {'Q', "📧"}, {'R', "🔒"}, {'S', "⚖"}, {'T', "📡"}, {'U', "🛒"}, {'V', "😂"},{'W', "💦" }, {'X', "😶"}, {'Y', "🥶"},{'Z', "😖"},
            {'a', "🤖"}, { 'b', "🖖" }, { 'c', "🤙" },{ 'd', "‍🧠" }, { 'e', "‍👩" }, { 'f', "‍🏫" }, { 'g', "💂" }, { 'h', "🕵" },{'i', "🍑"},{'j', "🥜"},
            {'k', "🍄"},{'l', "🧂"},{'m', "🧈"},{'n', "🏆"},{'o', "⚾"},{'p', "🎪"}, {'q', "🧭"}, {'r', "📻"}, {'s', "💻"}, {'t', "🔋"}, {'u', "📟"},
            {'v', "🔦"},{'w', "💀" }, {'x', "💡"}, {'y', "📺"},{'z', "📽"},{'1' , "📦"},{'2' , "🛑"},{'3' , "♨️"},{'4' , "📯"},{'5' , "☢️"},{'6' , "➕"},{'7' , "⁉️"},{'8' , "⁉️"},{'9' , "©️"},{' ', " "},{ '\n',"\n"},{'.',"."}
        };

        public MainWindow()
        {
            InitializeComponent();
            ResetDlgs();
            mCbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            mCbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            mCbDictionaryChar.ItemsSource = emojiTable.Keys;
        }

        private void UpdateStatBar(string message)
        {
            mStatText.Text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + message;
        }

        private void ResetDlgs()
        {
            mDlgOpen.FileName = "";
            mDlgSave.FileName = "";
            UpdateStatBar("Ready");
        }

        private void mBtnClear_Click(object sender, RoutedEventArgs e)
        {
            ResetDlgs();
            mTB.Text = "";
        }

        private void mBtnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (mDlgOpen.ShowDialog() == true)
            {
                System.IO.StreamReader reader = new(mDlgOpen.FileName);
                mTB.Text = reader.ReadToEnd();
                reader.Close();
                UpdateStatBar("Read " + mDlgOpen.FileName);
            }
        }

        private void mCBReadOnly_Toggle(object sender, RoutedEventArgs e)
        {
            if (mTB != null)
            {
                mTB.IsReadOnly = (mCBReadOnly.IsChecked == true);
                UpdateStatBar((mTB.IsReadOnly ? "En" : "Dis") + "abled read only");
            }
        }

        private void mBtnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileAs();
        }

        private void mBtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.Compare(mDlgSave.FileName, "") == 0)
                SaveFileAs();
            else
                SaveFile();
        }

        private void SaveFileAs()
        {
            if (mDlgSave.ShowDialog() == true)
                SaveFile();
            else
                UpdateStatBar("Text not saved to file.");
        }

        private void SaveFile()
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(mDlgSave.FileName);
            writer.Write(mTB.Text);
            writer.Close();
            UpdateStatBar("Wrote " + mTB.Text.Length.ToString() + " chars in " + mDlgSave.FileName);
        }

        private void Emojify_Click(object sender, RoutedEventArgs e)
        {
            string emojify = mTB.Text;
            StringBuilder sb = new();
            foreach (var item in emojify)
            {
                string val = "";
                if (emojiTable.TryGetValue(item, out val))
                {
                    sb.Append(val);
                }
                else
                {
                    sb.Append(item);
                }
            }

            mTB.Text = sb.ToString();
            UpdateStatBar("Emojified text");
        }

        private void mCbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mCbFontFamily.SelectedItem != null)
                mTB.FontFamily = (FontFamily)mCbFontFamily.SelectedItem;
        }

        private void mCbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            mTB.FontSize = Convert.ToDouble(mCbFontSize.Text);
        }
        private void mPDictPicker_SelectionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            emojiTable[Char.Parse(mCbDictionaryChar.Text)] = mPDictPicker.Selection;
        }
    }
}