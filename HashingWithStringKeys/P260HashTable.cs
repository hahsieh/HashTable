using System;
using System.Collections.Generic;

namespace HashingWithStringKeys
{
    public class KeyAndValue
    {
        public string Key { get; internal set; }
        public string Value { get; set; }
        public KeyAndValue(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
    public class P260HashTable
    {
        // hash TABLE is size pSize, stores data in aarray that goes from 0 to pSize-1
        internal LinkedList<KeyAndValue>[] hashTable;  // Relying on new LinkedList is initialized with all values = null
        int tableSize;
        int collisions = 0;


        // constructor --- user specifies how big the table they want to use
        public P260HashTable(int pSize)
        {
            hashTable = new LinkedList<KeyAndValue>[pSize];
            tableSize = pSize;
        }

        public bool AddItem(string key, string value)
        {
            int hashIndex = Hash(key, tableSize);
            KeyAndValue kvp;

            Console.Write($"Key {key} hashes to {hashIndex,2}.  ");
            if (hashTable[hashIndex] == null)  // null value means this slot is empty, so we can write our data (now a string) here.
            {
                hashTable[hashIndex] = new LinkedList<KeyAndValue>();                
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"<<< COLLISION! But we'll take care of it."); // else this spot was used
                Console.ResetColor();
                collisions++;                
            }
            kvp = new KeyAndValue(key, value);
            hashTable[hashIndex].AddLast(kvp);
            Console.WriteLine($"key {key}, Value {value} stored.");
            return true;
        }

        public KeyAndValue GetItem(string key)  
        {
            int hashIndex = Hash(key, tableSize);
            LinkedListNode<KeyAndValue> node;
            if (hashTable[hashIndex] == null)  // null value means this slot is empty
            {
                return null;
            }
            node = hashTable[hashIndex].First;
            while (node != null && node.Value.Key != key)
            {
                node = node.Next;
            }
            if (node == null)
            {
                return null;
            }
            return node.Value;
        }

        public bool DeleteItem(string key)
        {
            int hashIndex = Hash(key, tableSize);
            LinkedListNode<KeyAndValue> node;
            if (hashTable[hashIndex] == null)
            {
                return false;
            }
            node = hashTable[hashIndex].First;
            while (node != null && node.Value.Key!=key)
            {
                node = node.Next;
            }
            if (node.Value.Key == key)
            {
                hashTable[hashIndex].Remove(node);
                if (hashTable[hashIndex].Count >= 1)
                {
                    collisions--;
                }
                else if (hashTable[hashIndex].Count == 0)
                {
                    hashTable[hashIndex] = null;
                }
                else
                {
                    // Throw if non-negative assumption on count doesn't hold.
                    throw new Exception("Count must be positive or zero.");
                }
                return true;
            }
            return false;            
        }


        public KeyAndValue UpdateItem(string key, string value)
        {
            int hashIndex = Hash(key, tableSize);
            LinkedListNode<KeyAndValue> node;
            if (hashTable[hashIndex] == null)
            {
                return null;
            }
            node = hashTable[hashIndex].First;
            while (node != null && node.Value.Key != key)
            {
                node = node.Next;
            }
            if (node.Value.Key == key)
            {
                node.Value.Value = value;
                return node.Value;
            }
            return null;            
        }

        static public int Hash(string key, int numSlots)
        {
            double count = 0;
            double hashIndex = 0;
            double radix = 128;

            foreach (char a in key)
            {
                if (count == 4) count = 0;
                hashIndex += (double)a * (Math.Pow((double)radix, count));
                count++;
            }

            return (int)(hashIndex % numSlots);
        }

        public void PrintTableState()
        {
            LinkedListNode<KeyAndValue> node;
            for (int i = 0; i < hashTable.Length; i++)
            {
                if (hashTable[i] == null)
                {
                    Console.WriteLine($"[{i,2}]\t= <<empty>>");
                }
                else
                {
                    node = hashTable[i].First;
                    for (int j = 0; j < hashTable[i].Count; j++)
                    {
                        Console.WriteLine($"[{i,2}] \t= {node.Value.Key}: {node.Value.Value}");
                        node = node.Next;
                    }                    
                }
            }
            Console.WriteLine("\nTotal number of collisions: " + collisions);
        }
    }

}
