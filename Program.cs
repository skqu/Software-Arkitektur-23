using System.Runtime.InteropServices.Swift;

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
    static void Main(string []args)
    {
        SampleClass sample = new SampleClass();
        Console.WriteLine(sample.PrintString("Hello, World!"));
    }
}

/// <summary>
/// Author: skqu \n 
/// Date: 09-09-2025 \n 
/// Class Name: SampleClass \n 
/// Description: This is a sample class to demonstrate XML documentation comments. \n 
/// </summary>
class SampleClass
{

    /// <summary>
    /// Some important value.
    /// </summary>
    private string myValue = "Value";

    /// <summary>
    /// Author: skqu \n 
    /// Date: 09-09-2025 \n 
    /// Method name: PrintString \n 
    /// Description: This method prints a string. \n 
    /// </summary>
    /// <param name="stringPrint">The string to print.</param>
    /// <returns>true if printed, else false</returns>
    /// <exception cref="Exception">Thrown if writing to console fails.</exception>
    public bool PrintString(string stringPrint)
    {
        myValue = stringPrint;
        Console.WriteLine(stringPrint);
        return true;
    }
}