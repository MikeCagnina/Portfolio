using AITraining;
using Microsoft.ML;
using Microsoft.ML.Data;

// ---------------------------------------------------------------------------
// Generate and train an AI model (regression) using ML.NET
// ---------------------------------------------------------------------------

// 1. Create synthetic training data (e.g. size + rooms -> price)
List<ModelInput> trainingData = new List<ModelInput>
{
    new ModelInput { Size = 1200, Rooms = 3, Label = 250000 },
    new ModelInput { Size = 1500, Rooms = 4, Label = 320000 },
    new ModelInput { Size = 1800, Rooms = 4, Label = 380000 },
    new ModelInput { Size = 2000, Rooms = 5, Label = 450000 },
    new ModelInput { Size = 2200, Rooms = 5, Label = 490000 },
    new ModelInput { Size = 1000, Rooms = 2, Label = 180000 },
    new ModelInput { Size = 1600, Rooms = 4, Label = 340000 },
    new ModelInput { Size = 1400, Rooms = 3, Label = 290000 },
};

// 2. Create trainer, build pipeline, and train the model
ModelTrainer trainer = new ModelTrainer(seed: 42);
ITransformer model = trainer.Train(trainingData);

Console.WriteLine("Model trained successfully.");

// 3. Optional: evaluate on a small test set
List<ModelInput> testData = new List<ModelInput>
{
    new ModelInput { Size = 1300, Rooms = 3, Label = 270000 },
    new ModelInput { Size = 1900, Rooms = 5, Label = 410000 },
};

RegressionMetrics metrics = trainer.Evaluate(testData);
Console.WriteLine($"Evaluation - R-Squared: {metrics.RSquared:F4}, Mean Absolute Error: {metrics.MeanAbsoluteError:F2}");

// 4. Save the model to disk
string modelPath = Path.Combine(AppContext.BaseDirectory, "TrainedModel.zip");
trainer.SaveModel(modelPath);
Console.WriteLine($"Model saved to: {modelPath}");

// 5. Make a prediction
ModelInput newInput = new ModelInput { Size = 1700, Rooms = 4 };
ModelOutput prediction = trainer.Predict(newInput);
Console.WriteLine($"Prediction for Size={newInput.Size}, Rooms={newInput.Rooms}: {prediction.PredictedLabel:F0}");
