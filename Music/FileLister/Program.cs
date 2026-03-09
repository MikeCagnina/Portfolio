// See https://aka.ms/new-console-template for more information

using System.IO;

if (args.Length == 0)
{
    Console.WriteLine("Please provide a folder path as an argument.");
    return;
}

string folderPath = args[0];

if (!Directory.Exists(folderPath))
{
    Console.WriteLine($"The folder '{folderPath}' does not exist.");
    return;
}

string[] files = Directory.GetFiles(folderPath);

if (files.Length == 0)
{
    Console.WriteLine("No files found in the specified folder.");
}
else
{
    Console.WriteLine("Files in the folder:");
    foreach (string file in files)
    {
        Console.WriteLine(Path.GetFileName(file));
    }
}
