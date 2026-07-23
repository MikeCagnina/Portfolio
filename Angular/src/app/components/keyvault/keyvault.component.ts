import { Component } from '@angular/core';

interface SecretProperties {
  name: string;
  enabled: boolean;
  version?: string;
  expiresOn?: Date;
  createdOn?: Date;
  updatedOn?: Date;
}

interface KeyVaultSecret {
  name: string;
  value: string;
  properties: SecretProperties;
}

@Component({
  selector: 'app-keyvault',
  templateUrl: './keyvault.component.html',
  styleUrls: ['./keyvault.component.css']
})
export class KeyVaultComponent {
  secretName = '';
  retrievedSecret: KeyVaultSecret | null = null;
  allSecrets: SecretProperties[] | null = null;
  isLoading = false;
  errorMessage = '';
  showSecretValue = false;

  /**
   * Retrieves a secret from Azure Key Vault.
   * Note: This is a demo implementation. In a real application, this would call a backend service.
   */
  async retrieveSecret(): Promise<void> {
    if (!this.secretName.trim()) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.retrievedSecret = null;

    try {
      // Simulate API call delay
      await this.delay(500);

      // Demo implementation - in real app, this would call a service
      // For demo purposes, we'll create a mock secret
      this.retrievedSecret = {
        name: this.secretName,
        value: 'demo-secret-value-' + Math.random().toString(36).substring(7),
        properties: {
          name: this.secretName,
          enabled: true,
          version: '1.0.0',
          createdOn: new Date(),
          expiresOn: new Date(Date.now() + 365 * 24 * 60 * 60 * 1000) // 1 year from now
        }
      };
      this.showSecretValue = false;
    } catch (error) {
      this.errorMessage = `Failed to retrieve secret: ${error instanceof Error ? error.message : 'Unknown error'}`;
      console.error('Error retrieving secret:', error);
    } finally {
      this.isLoading = false;
    }
  }

  /**
   * Loads a secret by name into the retrieve section.
   */
  async loadSecret(name: string): Promise<void> {
    this.secretName = name;
    await this.retrieveSecret();
  }

  /**
   * Loads all secrets from Azure Key Vault.
   * Note: This is a demo implementation. In a real application, this would call a backend service.
   */
  async loadAllSecrets(): Promise<void> {
    this.isLoading = true;
    this.errorMessage = '';

    try {
      // Simulate API call delay
      await this.delay(500);

      // Demo implementation - in real app, this would call a service
      this.allSecrets = [
        { name: 'DatabaseConnectionString', enabled: true, updatedOn: new Date() },
        { name: 'ApiKey', enabled: true, updatedOn: new Date() },
        { name: 'StorageAccountKey', enabled: true, updatedOn: new Date() }
      ];
    } catch (error) {
      this.errorMessage = `Failed to list secrets: ${error instanceof Error ? error.message : 'Unknown error'}`;
      console.error('Error listing secrets:', error);
    } finally {
      this.isLoading = false;
    }
  }

  /**
   * Toggles the visibility of the secret value.
   */
  toggleSecretVisibility(): void {
    this.showSecretValue = !this.showSecretValue;
  }

  /**
   * Delays execution for the specified number of milliseconds.
   */
  private delay(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  /**
   * Formats a date to a readable string.
   */
  formatDate(date: Date | undefined): string {
    if (!date) {
      return '';
    }
    return date.toLocaleString('en-US', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit'
    });
  }
}
