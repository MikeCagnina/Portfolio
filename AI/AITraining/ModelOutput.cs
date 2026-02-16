using Microsoft.ML.Data;

namespace AITraining;

/// <summary>
/// Represents the prediction produced by the trained regression model.
/// </summary>
public class ModelOutput
{
    /// <summary>
    /// Predicted value (score) from the model.
    /// </summary>
    [ColumnName("Score")]
    public float PredictedLabel { get; set; }
}
