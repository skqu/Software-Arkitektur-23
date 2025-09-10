
/// <summary>
/// <para>Author: skqu </para>
/// <para>Date: 09-09-2025 </para> 
/// <para>Class Name: SampleClass </para>
/// <para>Description: This is a sample class to demonstrate XML documentation comments. </para> 
/// </summary>
class SampleClass
{

    /// <summary>
    /// <para> Description: Some important value.</para>
    /// </summary>
    private string myValue = "Value";

    /// <summary>
    /// <para>Author: skqu </para>
    /// <para>Date: 09-09-2025 </para>
    /// <para>Method name: PrintString </para> 
    /// <para>Description: This method prints a string. </para> 
    /// </summary>
    /// <param name="stringPrint">string: The string to print.</param>
    /// <returns>bool: true if printed, else false</returns>
    /// <exception>None</exception>
    public bool PrintString(string stringPrint)
    {
        myValue = stringPrint;
        Console.WriteLine(stringPrint);
        return true;
    }

    /// <summary>
    /// <para>Author: skqu </para>
    /// <para>Date: 09-09-2025 </para>
    /// <para>Method name: getValue </para>
    /// <para>Description: This method returns the value of myValue. </para>
    /// </summary>
    /// <param >None</param>
    /// <returns>string: The value of myValue.</returns>
    /// <exception>None</exception>
    public string getValue()
    {
        return myValue;
    }
}