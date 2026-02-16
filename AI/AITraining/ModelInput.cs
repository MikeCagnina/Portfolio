using Microsoft.ML.Data;

namespace AITraining;

/// <summary>
/// Represents one row of input data used to train the regression model.
/// </summary>
public class ModelInput
{
    /// <summary>
    /// Feature: size in square feet (e.g. house size).
    /// </summary>
    [LoadColumn(0)]
    public float Size { get; set; }

    /// <summary>
    /// Feature: number of rooms.
    /// </summary>
    [LoadColumn(1)]
    public float Rooms { get; set; }

    /// <summary>
    /// The value to predict (label), e.g. price.
    /// </summary>
    [LoadColumn(2)]
    public float Label { get; set; }
}
