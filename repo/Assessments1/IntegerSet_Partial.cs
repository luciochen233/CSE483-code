/////////////////////////////////////////////////////////////////////////////////////////////////////
// CSE483 Set Class
// Author - Graduate Student
// Simple Set class

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// The IntegerSet class contains members and methods which encapsulate an actual mathematical set.
/// </summary>

namespace MyIntegerSet
{
    public class IntegerSet
    {
        private uint _size = 4096;
        /// <summary>
        /// The private set of bools which make up the integer set.
        /// </summary>
        private bool[] _set;

        /// <summary>
        /// An public accessor for the private set of bools.
        /// </summary>
        public bool[] Set
        {
            get { return _set; }
            set { _set = value; }
        }


        /// <summary>
        /// The constructor for the IntegerSet class.
        /// </summary>
        public IntegerSet()
        {
            Console.Write("\n  IntegerSet Constructed");
            _set = new bool[_size];
            for (uint i = 0; i < _size; i++)
            {
                _set[i] = false;
            }
        }

        public IntegerSet(uint size)
        {
            Console.Write("\n  IntegerSet Constructed");
            _size = size;
            _set = new bool[_size];
            for (uint i = 0; i < _size; i++)
            {
                _set[i] = false;
            }
        }

        public IntegerSet(bool[] val)
            : this()
        {
            uint i = 0; // the index for tracking
            foreach (bool temp in val)
            {
                try
                {
                    _set[i] = val[i];
                }
                catch (Exception)
                {
                    Console.WriteLine("\nError!! The input array is too big\n");
                    break;
                }
                i++;
            }
        }


        /// <summary>
        /// This function finds the union between two IntegerSet objects and returns the set.
        /// </summary>
        /// <param name="otherSet">The other IntegerSet object.</param>
        /// <returns>The resultant set which contains the union of the two sets.</returns>
        public IntegerSet Union(IntegerSet otherSet)
        {
            // TODO Write the Union Method
            // Must use foreach
            // Must use exception handling     
            try
            {
                bool[] _tempSet = new bool[_size];
                uint i = 0;
                foreach (bool temp in otherSet.Set)
                {
                    _tempSet[i] = _set[i] || temp;
                    i++;
                }
                return new IntegerSet(_tempSet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return new IntegerSet(); //if something happened, create a new empty set and return it
        }

        /// <summary>
        /// This function finds the intersection between two IntegerSet objects and returns the set.
        /// </summary>
        /// <param name="otherSet">The other IntegerSet object.</param>
        /// <returns>The resultant set which contains the intersection of the two sets.</returns>
        public IntegerSet Intersection(IntegerSet otherSet)
        {
            // TODO Write the Intersection Method
            // Must use foreach
            // Must use exception handling
            try
            {
                bool[] _tempSet = new bool[_size];
                uint i = 0;
                foreach (bool temp in otherSet.Set)
                {
                    _tempSet[i] = _set[i] && temp;
                    i++;
                }
                return new IntegerSet(_tempSet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return new IntegerSet(); //if something happened, create a new empty set and return it
        }

        /// <summary>
        /// This function inserts an element into an IntegerSet object by setting it to true.
        /// </summary>
        /// <param name="k">The element to be added.</param>
        public void InsertElement(UInt32 k)
        {
            try
            {
                _set[k] = true;
            }
            catch (Exception)
            {
                Console.WriteLine("The number " + k + " is an invalid number.\n Try again.");
            }
        }

        /// <summary>
        /// This function deletes an element from an IntegerSet object by setting it to false.
        /// </summary>
        /// <param name="k">The element to be deleted.</param>
        public void DeleteElement(UInt32 k)
        {
            try
            {
                _set[k] = false;
            }
            catch (Exception ex)
            {
                throw new Exception("The number " + k + " is an invalid number.\n Try again.", ex);
            }
        }

        /// <summary>
        /// This function generates a string which contains all of the values contained in an IntegerSet object
        /// and returns it.
        /// </summary>
        /// <returns>A string which contains all the values contained in an IntegerSet object.</returns>
        public override string ToString()
        {
            string resultString = string.Empty;

            for (uint i = 0; i < _set.Length; i++)
            {
                if (_set[i])
                {
                    resultString = resultString + i + ", ";
                }
            }
            resultString = resultString.TrimEnd(',',' ');
            return resultString;
        }

        /// <summary>
        /// This function compares to IntegerSet objects to see if they are equal.
        /// </summary>
        /// <param name="otherSet">The other IntegerSet object.</param>
        /// <returns>The result of the comparison.</returns>
        public bool IsEqualTo(IntegerSet otherSet)
        {
            bool result = true;
            uint counter = 0;

            foreach (bool value in otherSet.Set)
            {
                if (value != _set[counter])
                    result = false;
                counter++;
            }

            return result;
        }

        /// <summary>
        /// This function empties the set
        /// </summary>
        /// <returns> nothing
        public void Clear()
        {
            for (uint i = 0; i < _set.Length; i++)
            {
                _set[i] = false;
            }
        }
    }
}
