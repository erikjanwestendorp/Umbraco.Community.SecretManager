namespace Umbraco_UIBuilder_KeyVault_Example;

internal static class AppConstants
{
    public static class Icons
    {
        public const string CombinationLock = "icon-combination-lock";
        public const string IconKey = "icon-key";
    }

    public static class Applications
    {
        public const string ConfigurationName = "Configuration";
    }

    public static class Collections
    {
        public const string SecretPluralName = "Secrets";
        public const string SecretSingularName = "Secret";
        public const string SecretDescription = "A collection of KeyVault secrets";
        public const string SecretDetailTab = "Details";
        public const string SecretDetailFieldset = "Details";
    }

    public static class WebhookEvents
    {
        public const string SecretsExpiringAlias = "Custom.SecretsExpiring";
        public const string SecretsExpiringName = "Secrets Expiring";
    }
}
