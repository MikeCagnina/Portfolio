# AITraining (.NET / C#)

This solution shows how to **generate and train** an AI model using **ML.NET** in C#.

## What it does

- Defines input/output types (`ModelInput`, `ModelOutput`) for a regression problem.
- Builds an ML pipeline: concatenate features (Size, Rooms) and train with the SDCA regression trainer.
- Trains the model on in-memory data, evaluates it, saves it to `TrainedModel.zip`, and runs a sample prediction.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 (optional)

## Run

From the repo root (where `AITraining.sln` is):

```bash
dotnet build AITraining.sln
dotnet run --project AITraining
```

Or open `AITraining.sln` in Visual Studio 2022 and run the **AITraining** project.

## Project layout

| File | Purpose |
|------|--------|
| `ModelInput.cs` | Input schema (features + label) for training. |
| `ModelOutput.cs` | Prediction output schema. |
| `ModelTrainer.cs` | Pipeline setup, training, evaluation, save, and predict. |
| `Program.cs` | Creates sample data, trains, evaluates, saves model, and runs a prediction. |

To use your own data, pass an `IEnumerable<ModelInput>` into `ModelTrainer.Train()` (e.g. from a CSV via `LoadFromTextFile` or from a database).
