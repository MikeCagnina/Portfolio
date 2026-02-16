using Azure.Security.KeyVault.Secrets;
using AzureKeyVault;
using Microsoft.AspNetCore.Components;

namespace AzurePortfolio.Components.Pages;

/// <summary>
/// Code-behind for the KeyVault page component.
/// </summary>
public partial class KeyVault : ComponentBase
{
    private string secretName = string.Empty;
    private KeyVaultSecret? retrievedSecret;
    private List<SecretProperties>? allSecrets;
    private bool isLoading = false;
    private string errorMessage = string.Empty;
    private bool showSecretValue = false;

    /// <summary>
    /// Retrieves a secret from Azure Key Vault.
    /// </summary>
    private async Task RetrieveSecret()
    {
        if (string.IsNullOrWhiteSpace(secretName))
        {
            return;
        }

        isLoading = true;
        errorMessage = string.Empty;
        retrievedSecret = null;

        try
        {
            retrievedSecret = await KeyVaultService.GetSecretDetailsAsync(secretName);
            showSecretValue = false;
        }
        catch (Azure.RequestFailedException ex)
        {
            errorMessage = $"Failed to retrieve secret: {ex.Message} (Status: {ex.Status})";
            Logger.LogError(ex, "Failed to retrieve secret '{SecretName}'", secretName);
        }
        catch (Exception ex)
        {
            errorMessage = $"An error occurred: {ex.Message}";
            Logger.LogError(ex, "Error retrieving secret '{SecretName}'", secretName);
        }
        finally
        {
            isLoading = false;
        }
    }

    /// <summary>
    /// Loads a secret by name into the retrieve section.
    /// </summary>
    /// <param name="name">The name of the secret to load.</param>
    private async Task LoadSecret(string name)
    {
        secretName = name;
        await RetrieveSecret();
    }

    /// <summary>
    /// Loads all secrets from Azure Key Vault.
    /// </summary>
    private async Task LoadAllSecrets()
    {
        isLoading = true;
        errorMessage = string.Empty;

        try
        {
            allSecrets = await KeyVaultService.ListSecretsAsync();
        }
        catch (Azure.RequestFailedException ex)
        {
            errorMessage = $"Failed to list secrets: {ex.Message} (Status: {ex.Status})";
            Logger.LogError(ex, "Failed to list secrets");
        }
        catch (Exception ex)
        {
            errorMessage = $"An error occurred: {ex.Message}";
            Logger.LogError(ex, "Error listing secrets");
        }
        finally
        {
            isLoading = false;
        }
    }

    /// <summary>
    /// Toggles the visibility of the secret value.
    /// </summary>
    private void ToggleSecretVisibility()
    {
        showSecretValue = !showSecretValue;
    }
}

