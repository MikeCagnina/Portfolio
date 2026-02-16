using Microsoft.ML;

namespace AITraining;

/// <summary>
/// Loads training data from a CSV file with columns Size, Rooms, and Label (by index 0, 1, 2).
/// </summary>
public static class CsvDataLoader
{
    /// <summary>
    /// Loads model input rows from a CSV file. Expected columns (in order): Size, Rooms, Label (header optional).
    /// </summary>
    /// <param name="csvPath">Full path to the CSV file.</param>
    /// <param name="hasHeader">Whether the first row is a header.</param>
    /// <param name="separatorChar">Column separator character.</param>
    /// <returns>List of <see cref="ModelInput"/> loaded from the file.</returns>
    public static List<ModelInput> LoadFromCsv(
        string csvPath,
        bool hasHeader = true,
        char separatorChar = ',')
    {
        MLContext mlContext = new MLContext();
        IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>(
            csvPath,
            separatorChar: separatorChar,
            hasHeader: hasHeader);
        List<ModelInput> rows = mlContext.Data.CreateEnumerable<ModelInput>(dataView, reuseRowObject: false).ToList();
        return rows;
    }
}
