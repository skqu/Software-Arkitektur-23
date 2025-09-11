namespace AnotherNamespace
{
    /// <summary>
    ///  <para>Author: skqu </para>
    ///  <para>Date: 10-09-2025 </para>
    /// <para>Class Name: AnotherClass </para>
    /// <para>Description: This is another sample class to demonstrate XML documentation comments. </para>
    /// </summary>
    class AnotherClass
    {
        /// <summary>
        /// <para>Author: skqu </para>
        /// <para>Date: 10-09-2025 </para>
        /// <para>Method name: IsAlive </para>
        /// <para>Description: This method checks if the object is alive. </para>
        /// </summary>
        /// <returns name="aliveVar">bool: Return wether or not class is alive</returns>
        public bool IsAlive()
        {
            bool aliveVar = true;

            if (aliveVar)
            {
                aliveVar = false;
            }
            else
            {
                aliveVar = true;
            }

            return aliveVar;
        }
    }
}