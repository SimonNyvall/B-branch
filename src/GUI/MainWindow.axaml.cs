using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System.Threading.Tasks;
using System.IO;
using GUI.Storage.Repository;

namespace GUI;

public partial class MainWindow : Window
{
    private readonly IRepository _repository;

    public MainWindow()
    {
        InitializeComponent();

        _repository = new Repository();
    }

    public async void OpenDirectory_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog(); // TODO: This is deprecated

        dialog.Title = "Select a directory";

        dialog.Directory = "/home/sn";

        var result = await dialog.ShowAsync(this);

        if (string.IsNullOrWhiteSpace(result))
        {
            return;
        }

        PathTextBox.Text = result; 
    }

    public async void Continue_Click(object sender, RoutedEventArgs e)
    {
        string? userInuptPath = PathTextBox.Text;

        if (string.IsNullOrWhiteSpace(userInuptPath))
        {
            return;
        }

        if (!Directory.Exists(userInuptPath))
        {
            PathTextBox.Background = new SolidColorBrush(Colors.Red);
            return;
        }

        if (!_repository.DoesSaveFileExist())
        {
            _repository.CreateSaveFile();
        }

        await _repository.SavePathToFileAsync(userInuptPath);

        // TODO: go to the next window and show all the git repositories
    }

}