# FileLister

A simple .NET console application that lists the file names in a given folder.

## Usage

Run the application with a folder path as an argument:

```
dotnet run -- "path/to/folder"
```

For example:

```
dotnet run -- "C:\Users\YourName\Documents"
```

## Requirements

- .NET 9.0 SDK

## Building

To build the application:

```
dotnet build
```

## Running

After building, you can run the executable directly:

```
dotnet run -- "path/to/folder"
```

Or publish for a specific runtime:

```
dotnet publish -c Release -r win-x64 --self-contained
```

Then run the published executable.

## Troubleshooting

- If the folder does not exist, the app will display an error message.
- If no files are found, it will indicate that.
- Ensure you have read permissions for the specified folder.