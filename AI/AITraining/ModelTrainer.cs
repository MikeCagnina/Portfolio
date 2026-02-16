using Microsoft.ML;
using Microsoft.ML.Data;

namespace AITraining;

/// <summary>
/// Generates and trains a regression AI model using ML.NET.
/// </summary>
public class ModelTrainer
{
    private readonly MLContext _mlContext;
    private ITransformer? _trainedModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModelTrainer"/> class.
    /// </summary>
    /// <param name="seed">Optional random seed for reproducibility.</param>
    public ModelTrainer(int seed = 0)
    {
        _mlContext = new MLContext(seed);
    }

    /// <summary>
    /// Builds the ML pipeline (feature column + trainer) and fits the model on the provided data.
    /// </summary>
    /// <param name="trainingData">Training data as an enumerable of <see cref="ModelInput"/>.</param>
    /// <returns>The trained model (transformer).</returns>
    public ITransformer Train(IEnumerable<ModelInput> trainingData)
    {
        IDataView dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

        Microsoft.ML.Transforms.ColumnConcatenatingEstimator concatenateEstimator =
            _mlContext.Transforms.Concatenate("Features", nameof(ModelInput.Size), nameof(ModelInput.Rooms));

        Microsoft.ML.Trainers.SdcaRegressionTrainer trainer =
            _mlContext.Regression.Trainers.Sdca(
                labelColumnName: nameof(ModelInput.Label),
                maximumNumberOfIterations: 100);

        IEstimator<ITransformer> pipeline = concatenateEstimator.Append(trainer);

        _trainedModel = pipeline.Fit(dataView);
        return _trainedModel;
    }

    /// <summary>
    /// Evaluates the trained model on the given test data and returns regression metrics.
    /// </summary>
    /// <param name="testData">Test data as an enumerable of <see cref="ModelInput"/>.</param>
    /// <returns>Regression metrics (RSquared, etc.).</returns>
    public RegressionMetrics Evaluate(IEnumerable<ModelInput> testData)
    {
        if (_trainedModel == null)
        {
            throw new InvalidOperationException("Train the model first by calling Train.");
        }

        IDataView testDataView = _mlContext.Data.LoadFromEnumerable(testData);
        IDataView predictions = _trainedModel.Transform(testDataView);
        RegressionMetrics metrics = _mlContext.Regression.Evaluate(predictions, labelColumnName: nameof(ModelInput.Label));
        return metrics;
    }

    /// <summary>
    /// Saves the trained model to a file.
    /// </summary>
    /// <param name="modelPath">Path for the .zip model file.</param>
    public void SaveModel(string modelPath)
    {
        if (_trainedModel == null)
        {
            throw new InvalidOperationException("Train the model first by calling Train.");
        }

        IDataView schemaView = _mlContext.Data.LoadFromEnumerable(new List<ModelInput>());
        _mlContext.Model.Save(_trainedModel, schemaView.Schema, modelPath);
    }

    /// <summary>
    /// Predicts the label for a single input using the trained model.
    /// </summary>
    /// <param name="input">Single <see cref="ModelInput"/>.</param>
    /// <returns>Prediction result.</returns>
    public ModelOutput Predict(ModelInput input)
    {
        if (_trainedModel == null)
        {
            throw new InvalidOperationException("Train the model first by calling Train.");
        }

        PredictionEngine<ModelInput, ModelOutput> predictionEngine =
            _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(_trainedModel);

        return predictionEngine.Predict(input);
    }
}
