# Umbraco.Community.SecretManager

## Installation

Installing through command line:

```bash
dotnet add package Umbraco.Community.SecretManager
```

Or package reference: 

```
<PackageReference Include="Umbraco.Community.SecretManager" />
```

## Configuration

You can configure the package by using the `ConfigureSecretManager()` extension method.


```csharp
builder.ConfigureSecretManager(secretClient);
```
If you also want to add the UI, you can use the `ConfigureSecretManagerUI()` extension method.

```csharp
builder.ConfigureSecretManagerUI();
```

## Contributions
Contributions are welcome! If you encounter bugs, have feature requests, or want to improve the code, feel free to open an issue or submit a pull request.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Support
If you have questions or need assistance, please create an issue on the issue tracker.

