/// <summary>
/// Author: skqu \n 
/// Date: 09-09-2025 \n 
/// Class Name: Program \n 
/// Description: This program demonstrates XML documentation comments in C#. \n 
/// </summary>
class Program
{
    /// <summary>
    /// Author: skqu \n 
    /// Date: 09-09-2025 \n 
    /// Method name: Main \n
    /// Description: The main entry point of the program. \n 
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <returns>void</returns>
    /// <exception >None</exception>
    static void Main(string[] args)
    {
        SampleClass sample = new SampleClass();
        sample.PrintString("Hello, World!");

        // Show comments when looking for method info
        Console.WriteLine(sample.getValue());
    }
}
