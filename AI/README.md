# AITraining (.NET / C#)

This solution shows how to **generate and train** an AI model using **ML.NET** in C#, with a **desktop UI** to load data, train, and ask the model.

## What it does

- Defines input/output types (`ModelInput`, `ModelOutput`) for a regression problem (e.g. Size + Rooms → Label/price).
- Builds an ML pipeline: concatenate features (Size, Rooms) and train with the SDCA regression trainer.
- **Desktop app (WPF)**: load a CSV data file, train the model, then use the UI to enter Size/Rooms and get a prediction.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows (for the WPF desktop app)
- Visual Studio 2022 (optional)

## Run the desktop app

From the repo root (where `AITraining.sln` is):

```bash
dotnet build AITraining.sln
dotnet run --project AITraining.Desktop
```

Or open `AITraining.sln` in Visual Studio 2022, set **AITraining.Desktop** as the startup project, and run (F5).

1. **Data file**: Click **Browse...** and select a CSV with columns **Size**, **Rooms**, **Label** (in that order; first row can be a header).
2. **Train**: Click **Train model on loaded data**.
3. **Ask**: Enter **Size** and **Rooms**, then click **Get prediction** to see the predicted label (e.g. price).

A sample CSV is in `SampleData/training_data.csv`.

## Run the console app

```bash
dotnet run --project AITraining
```

This trains on in-memory sample data, saves the model, and runs a sample prediction (no UI).

## Project layout

| File / project | Purpose |
|----------------|--------|
| **AITraining** | Core ML: `ModelInput`, `ModelOutput`, `ModelTrainer`, `CsvDataLoader`. |
| **AITraining.Desktop** | WPF app: load CSV, train, and ask (predict). |
| `ModelInput.cs` | Input schema (features + label) for training; `[LoadColumn]` for CSV. |
| `ModelOutput.cs` | Prediction output schema. |
| `ModelTrainer.cs` | Pipeline, training, evaluation, save, and predict. |
| `CsvDataLoader.cs` | Loads CSV into `List<ModelInput>`. |
| `Program.cs` | Console: sample data, train, evaluate, save, predict. |
| `SampleData/training_data.csv` | Example CSV for the desktop app. |

To use your own data in code, pass an `IEnumerable<ModelInput>` into `ModelTrainer.Train()` (e.g. from `CsvDataLoader.LoadFromCsv(path)` or from a database).
