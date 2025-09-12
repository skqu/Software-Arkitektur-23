namespace AnotherNamespace
{
    /// <summary>
    ///  <para>Author: skqu </para>
    ///  <para>Date: 10-09-2025 </para>
    /// <para>Class Name: AnotherClass </para>
    /// <para>Description: This is another sample class to demonstrate XML documentation comments. </para>
    /// </summary>
    public class AnotherClass
    {

        private bool _aliveVar = true;
        /// <summary>
        /// <para>Author: skqu </para>
        /// <para>Date: 10-09-2025 </para>
        /// <para>Method name: IsAlive </para>
        /// <para>Description: This method checks if the object is alive. </para>
        /// </summary>
        /// <returns name="aliveVar">bool: Return wether or not class is alive</returns>
        public bool IsAlive()
        {

            if (_aliveVar)
            {
                _aliveVar = false;
            }
            else
            {
                _aliveVar = true;
            }

            return _aliveVar;
        }

        /// <summary>
        /// <para>Author: skqu </para>
        /// <para>Date: 12-09-2025 </para>
        /// <para>Method name: changeAlive </para>
        /// <para>Description: This method change if the object is alive. </para>
        /// </summary>
        /// <param name="alive">bool: If the object is alive</param>
        /// <returns>void</returns>
        public void changeAlive(bool alive)
        {
            _aliveVar = alive;
        }
    }
}