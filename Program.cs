/// <summary>
/// <para>Author: skqu </para> 
/// <para>Date: 09-09-2025 </para>
/// <para>Class Name: Program </para>
/// <para>Description: This program demonstrates XML documentation comments in C#. </para>
/// </summary>
class Program
{
    /// <summary>
    /// <para>Author: skqu </para>
    /// <para>Date: 09-09-2025 </para>
    /// <para>Method name: Main </para>
    /// <para>Description: The main entry point of the program. </para>
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

        string testVar = sample.getValue();
    }
}