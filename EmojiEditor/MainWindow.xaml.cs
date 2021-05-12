using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Emoji.Wpf;
using System.Text;
using System.Linq;

namespace EmojiEditor
{
    public partial class MainWindow
    {
        Microsoft.Win32.OpenFileDialog mDlgOpen = new();
        Microsoft.Win32.SaveFileDialog mDlgSave = new();

        private bool _IsInChange = false;

        public readonly Dictionary<char, string> emojiTable = new Dictionary<char, string>() { {'A', "💖"}, { 'B', "😁" }, { 'C', "🐨" },
            { 'D', "🐱‍" }, { 'E', "‍🐉" }, { 'F', "👻" }, { 'G', "🦑" }, { 'H', "🔥" },{'I', "✔"},{'J', "👁"},{'K', "🔔"},{'L', "💣"},{'M', "🕹"},{'N', "📞"},
            {'O', "📀"},{'P', "📖"}, {'Q', "📧"}, {'R', "🔒"}, {'S', "⚖"}, {'T', "📡"}, {'U', "🛒"}, {'V', "😂"}, {'X', "😶"}, {'Y', "🥶"},{'Z', "😖"},
            {'a', "🤖"}, { 'b', "🖖" }, { 'c', "🤙" },{ 'd', "‍🧠" }, { 'e', "‍👩" }, { 'f', "‍🏫" }, { 'g', "💂" }, { 'h', "🕵" },{'i', "🍑"},{'j', "🥜"},
            {'k', "🍄"},{'l', "🧂"},{'m', "🧈"},{'n', "🏆"},{'o', "⚾"},{'p', "🎪"}, {'q', "🧭"}, {'r', "📻"}, {'s', "💻"}, {'t', "🔋"}, {'u', "📟"},
            {'v', "🔦"}, {'x', "💡"}, {'y', "📺"},{'z', "📽"},{'1' , "📦"},{'2' , "🛑"},{'3' , "♨️"},{'4' , "📯"},{'5' , "☢️"},{'6' , "➕"},{'7' , "⁉️"},{'8' , "⁉️"},{'9' , "©️"},{' ', " "},{ '\n',"\n"},{'.',"."}
        };

        public MainWindow()
        {
            InitializeComponent();
            ResetDlgs();
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
                string emojify = reader.ReadToEnd();
                StringBuilder sb = new StringBuilder();

                foreach (var item in emojify)
                {
                    string val;
                    emojiTable.TryGetValue(item, out val);
                    sb.Append(val);
                }

                mTB.Text = sb.ToString();
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

        private void mTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_IsInChange) return;
            var c = e.Changes.ElementAt(0).Offset == 0 ? e.Changes.ElementAt(0).Offset : e.Changes.ElementAt(0).Offset - 3;
            _IsInChange = true;
            string val;
            char symbol = mTB.Text[c];
                                            
            if (emojiTable.TryGetValue(symbol, out val))
            {
                mTB.Text.Remove(c, 1);
                mTB.Text = mTB.Text.Insert(c, val);
            }
                _IsInChange = false;
        }
    }
}