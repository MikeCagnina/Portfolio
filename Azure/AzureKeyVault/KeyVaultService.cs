using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureKeyVault;

/// <summary>
/// Service for interacting with Azure Key Vault to retrieve secrets.
/// </summary>
public class KeyVaultService
{
    private readonly SecretClient _secretClient;
    private readonly ILogger<KeyVaultService> _logger;

    /// <summary>
    /// Initializes a new instance of the KeyVaultService class.
    /// </summary>
    /// <param name="configuration">Configuration instance to retrieve Key Vault URL.</param>
    /// <param name="logger">Logger instance for logging operations.</param>
    /// <exception cref="InvalidOperationException">Thrown when Key Vault URL is not configured.</exception>
    public KeyVaultService(IConfiguration configuration, ILogger<KeyVaultService> logger)
    {
        _logger = logger;
        
        string? keyVaultUrl = configuration["AzureKeyVault:VaultUrl"];
        
        if (string.IsNullOrWhiteSpace(keyVaultUrl))
        {
            throw new InvalidOperationException("Key Vault URL is not configured in appsettings.json");
        }

        // Create a credential using DefaultAzureCredential
        DefaultAzureCredential credential = new DefaultAzureCredential();

        // Create the SecretClient
        _secretClient = new SecretClient(new Uri(keyVaultUrl), credential);
    }

    /// <summary>
    /// Retrieves a secret from Azure Key Vault by name.
    /// </summary>
    /// <param name="secretName">The name of the secret to retrieve.</param>
    /// <returns>The secret value if found, otherwise null.</returns>
    public async Task<string?> GetSecretAsync(string secretName)
    {
        try
        {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
            return secret.Value;
        }
        catch (Azure.RequestFailedException ex)
        {
            _logger.LogError(ex, "Failed to retrieve secret '{SecretName}'. Status: {Status}", secretName, ex.Status);
            throw;
        }
    }

    /// <summary>
    /// Retrieves a secret with full details from Azure Key Vault by name.
    /// </summary>
    /// <param name="secretName">The name of the secret to retrieve.</param>
    /// <returns>The KeyVaultSecret object containing all secret properties.</returns>
    public async Task<KeyVaultSecret> GetSecretDetailsAsync(string secretName)
    {
        try
        {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
            return secret;
        }
        catch (Azure.RequestFailedException ex)
        {
            _logger.LogError(ex, "Failed to retrieve secret '{SecretName}'. Status: {Status}", secretName, ex.Status);
            throw;
        }
    }

    /// <summary>
    /// Lists all secrets in the Key Vault.
    /// </summary>
    /// <returns>A list of secret properties.</returns>
    public async Task<List<SecretProperties>> ListSecretsAsync()
    {
        List<SecretProperties> secrets = new List<SecretProperties>();
        
        try
        {
            await foreach (SecretProperties secretProperties in _secretClient.GetPropertiesOfSecretsAsync())
            {
                secrets.Add(secretProperties);
            }
        }
        catch (Azure.RequestFailedException ex)
        {
            _logger.LogError(ex, "Failed to list secrets. Status: {Status}", ex.Status);
            throw;
        }

        return secrets;
    }

    /// <summary>
    /// Checks if a secret exists in the Key Vault.
    /// </summary>
    /// <param name="secretName">The name of the secret to check.</param>
    /// <returns>True if the secret exists, otherwise false.</returns>
    public async Task<bool> SecretExistsAsync(string secretName)
    {
        try
        {
            await _secretClient.GetSecretAsync(secretName);
            return true;
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404)
        {
            return false;
        }
        catch (Azure.RequestFailedException ex)
        {
            _logger.LogError(ex, "Error checking if secret '{SecretName}' exists. Status: {Status}", secretName, ex.Status);
            throw;
        }
    }
}

