using Microsoft.AspNetCore.Components;

namespace AzurePortfolio.Components.Pages;

/// <summary>
/// Code-behind for the Counter page component.
/// </summary>
public partial class Counter : ComponentBase
{
    private int currentCount = 0;

    /// <summary>
    /// Increments the current count.
    /// </summary>
    private void IncrementCount()
    {
        currentCount++;
    }
}

