using System.Windows;
using System.Windows.Controls;
using QuizCreate.Classes.Models;
using QuizCreate.Classes.Controllers;
using System.Diagnostics;
using System.IO;

namespace QuizCreate.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Theme forest = new("Forest", "#F0F7F0", "#3A7D44", "#1A2E1C", "#1C2E1E", "#5A7A5C", "#276221", "#9B2335", "#F5FBF5");
        Theme oceanBlue = new("Ocean Blue", "#EEF8FF", "#0088CC", "#00334D", "#002233", "#5C7B8A", "#2E7D32", "#C62828", "#FFFFFF");
        Theme royalPurple = new("Royal Purple", "#F7F3FF", "#6A0DAD", "#2D1047", "#261538", "#7A6893", "#388E3C", "#D32F2F", "#FFFFFF");
        Theme autumn = new("Autumn", "#FFF8F0", "#C56A1A", "#5A3410", "#3A2415", "#8B6A50", "#2E7D32", "#B71C1C", "#FFFFFF");
        Theme sakura = new("Sakura", "#FFF5F8", "#E56B8A", "#6D2C3C", "#40212A", "#8A6570", "#43A047", "#E53935", "#FFFFFF");
        Theme vintagePaper = new("Vintage Paper", "#F6F1E7", "#8B5E34", "#4A3521", "#2F2418", "#7A6A57", "#3E6B35", "#A63A3A", "#FCF8F0");

        private bool _webViewReady = false;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;

            cbxTheme.Items.Add(forest);
            cbxTheme.Items.Add(oceanBlue);
            cbxTheme.Items.Add(royalPurple);
            cbxTheme.Items.Add(autumn);
            cbxTheme.Items.Add(sakura);
            cbxTheme.Items.Add(vintagePaper);

            cbxTheme.SelectedIndex = 0;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await webPreview.EnsureCoreWebView2Async();
            _webViewReady = true;
        }

        private void cbxTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string html = getHTML();

            if (_webViewReady)
            {
                webPreview.NavigateToString(html);
            }
        }

        private async void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string html = getHTML();

            string title = string.Concat(txtQuizTitle.Text.ToLowerInvariant().Replace(" ", "_").Where(c => char.IsLetterOrDigit(c) || c == '_'));
            string outputPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), title + "_quiz.html");

            await File.WriteAllTextAsync(outputPath, html);

            MessageBox.Show($"Quiz HTML file generated successfully at:\n{outputPath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnOpenOutput_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = Directory.GetCurrentDirectory(),
                UseShellExecute = true
            });
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string html = getHTML();

            if (_webViewReady)
            {
                webPreview.NavigateToString(html);
            }
        }

        private string getHTML()
        {
            if (txtInput.Text == null)
            {
                return "";
            }

            string input = txtInput.Text;

            ParseResult result = Parser.Parse(input);

            Theme theme = (cbxTheme != null && cbxTheme.SelectedItem != null) ? (Theme)cbxTheme.SelectedItem : forest;

            string html = HTMLGenerator.Generate(result, txtQuizTitle.Text, theme);

            return html;
        }
    }
}