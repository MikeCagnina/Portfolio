using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using AITraining;

namespace AITraining.Desktop;

/// <summary>
/// Main window for loading data, training the model, and asking predictions.
/// </summary>
public partial class MainWindow : Window
{
    private string? _dataFilePath;
    private List<ModelInput>? _loadedData;
    private ModelTrainer? _trainer;
    private bool _modelTrained;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        Log("Ready. Select a CSV file with columns: Size, Rooms, Label.");
    }

    /// <summary>
    /// Handles the Browse button click to select a data file.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event data.</param>
    private void BrowseDataFileButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog
        {
            Title = "Select training data (CSV)",
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
            CheckFileExists = true,
        };

        bool? result = dialog.ShowDialog(this);
        if (result == true && !string.IsNullOrWhiteSpace(dialog.FileName))
        {
            _dataFilePath = dialog.FileName;
            DataFilePathTextBox.Text = _dataFilePath;
            LoadDataFromFile(_dataFilePath);
        }
    }

    /// <summary>
    /// Loads CSV data from the given file path and updates the UI.
    /// </summary>
    /// <param name="path">Full path to the CSV file.</param>
    private void LoadDataFromFile(string path)
    {
        try
        {
            _loadedData = CsvDataLoader.LoadFromCsv(path, hasHeader: true, separatorChar: ',');
            int count = _loadedData.Count;
            DataFileStatusTextBlock.Text = $"Loaded {count} row(s). Columns: Size, Rooms, Label.";
            TrainModelButton.IsEnabled = count > 0;
            TrainStatusTextBlock.Text = count > 0 ? "Click 'Train model on loaded data' to train." : "No rows loaded.";
            _modelTrained = false;
            PredictButton.IsEnabled = false;
            PredictionResultTextBlock.Text = "Train the model first, then enter Size and Rooms.";
            Log($"Loaded {count} rows from {Path.GetFileName(path)}.");
        }
        catch (Exception ex)
        {
            DataFileStatusTextBlock.Text = "Error loading file. Ensure CSV has columns: Size, Rooms, Label (in that order).";
            TrainModelButton.IsEnabled = false;
            TrainStatusTextBlock.Text = "Fix the data file and load again.";
            _loadedData = null;
            Log($"Error loading CSV: {ex.Message}");
        }
    }

    /// <summary>
    /// Handles the Train model button click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event data.</param>
    private void TrainModelButton_Click(object sender, RoutedEventArgs e)
    {
        if (_loadedData == null || _loadedData.Count == 0)
        {
            TrainStatusTextBlock.Text = "No data loaded.";
            return;
        }

        try
        {
            TrainModelButton.IsEnabled = false;
            TrainStatusTextBlock.Text = "Training...";
            Log("Training started...");

            _trainer = new ModelTrainer(seed: 42);
            _trainer.Train(_loadedData);

            string modelPath = Path.Combine(AppContext.BaseDirectory, "TrainedModel.zip");
            _trainer.SaveModel(modelPath);

            _modelTrained = true;
            PredictButton.IsEnabled = true;
            TrainModelButton.IsEnabled = true;
            TrainStatusTextBlock.Text = $"Model trained on {_loadedData.Count} rows and saved.";
            PredictionResultTextBlock.Text = "Enter Size and Rooms above, then click Get prediction.";
            Log($"Model trained and saved to {modelPath}.");
        }
        catch (Exception ex)
        {
            TrainStatusTextBlock.Text = "Training failed. See status log.";
            TrainModelButton.IsEnabled = true;
            Log($"Training error: {ex.Message}");
        }
    }

    /// <summary>
    /// Handles the Get prediction button click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event data.</param>
    private void PredictButton_Click(object sender, RoutedEventArgs e)
    {
        if (_trainer == null || !_modelTrained)
        {
            PredictionResultTextBlock.Text = "Train the model first.";
            return;
        }

        if (!float.TryParse(AskSizeTextBox.Text?.Trim(), out float size))
        {
            PredictionResultTextBlock.Text = "Enter a valid number for Size.";
            return;
        }

        if (!float.TryParse(AskRoomsTextBox.Text?.Trim(), out float rooms))
        {
            PredictionResultTextBlock.Text = "Enter a valid number for Rooms.";
            return;
        }

        try
        {
            ModelInput input = new ModelInput { Size = size, Rooms = rooms };
            ModelOutput output = _trainer.Predict(input);
            PredictionResultTextBlock.Text = $"Predicted label (e.g. price): {output.PredictedLabel:F2}";
            Log($"Prediction for Size={size}, Rooms={rooms} → {output.PredictedLabel:F2}");
        }
        catch (Exception ex)
        {
            PredictionResultTextBlock.Text = "Prediction failed. See status log.";
            Log($"Prediction error: {ex.Message}");
        }
    }

    /// <summary>
    /// Appends a line to the status log.
    /// </summary>
    /// <param name="message">The message to append.</param>
    private void Log(string message)
    {
        string line = $"[{DateTime.Now:HH:mm:ss}] {message}";
        if (StatusLogTextBlock.Text.Length > 0)
        {
            StatusLogTextBlock.Text += Environment.NewLine + line;
        }
        else
        {
            StatusLogTextBlock.Text = line;
        }
    }
}
