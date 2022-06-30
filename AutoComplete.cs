using System;
using System.Collections.Generic;

namespace AutoComplete
{

    public struct FullName
    {
        public string Name;
        public string Surname;
        public string Patronymic;
    }

    public class Node
    {
        public readonly char letter;
        public Dictionary<char, Node> NextPossibleLetters { get; private set; }

        public Node(char _letter)
        {
            letter = _letter;
            NextPossibleLetters = new Dictionary<char, Node>();
        }
        public Node AddToDictionary(char _letter)
        {
            if (NextPossibleLetters.ContainsKey(_letter))
                return NextPossibleLetters[_letter];
            var newNode = new Node(_letter);
            NextPossibleLetters.Add(_letter, newNode);
            return newNode;
        }
    }
    public class SearchTree
    {
        public Dictionary<char, Node> Head = new();
       
        public void Add(string fullname)
        {
            if (!Head.ContainsKey(fullname[0]))
                Head.Add(fullname[0], new Node(fullname[0]));
            var currentNode = Head[fullname[0]];

            foreach (var letter in fullname.Substring(1, fullname.Length - 1))
            {
                currentNode = currentNode.AddToDictionary(letter);
            }
        }

        public void Search(string prefix, string fullname, List<string> result, Node currentNode)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                if (currentNode.NextPossibleLetters.Count == 0)
                    result.Add(fullname);
                else
                    foreach (var node in currentNode.NextPossibleLetters)
                    {
                        Search(prefix, fullname + node.Key, result, node.Value);
                    }
            }
            else
            {
                var currentLetter = prefix[0];
                Search(prefix.Substring(1), fullname + currentLetter, result, currentNode.NextPossibleLetters[currentLetter]);
            }
        }

    }
    public class AutoCompleter
    {
        public SearchTree normalizedFullNames = new();
        public void AddToSearch(List<FullName> fullNames)
        {
            for (var i = 0; i < fullNames.Count; i++)
            {
                string normalizedFullName = "";
                if (!string.IsNullOrEmpty(fullNames[i].Surname))
                    normalizedFullName += fullNames[i].Surname.Trim(' ') + ' ';
                if (!string.IsNullOrEmpty(fullNames[i].Name))
                    normalizedFullName += fullNames[i].Name.Trim(' ') + ' ';
                if (!string.IsNullOrEmpty(fullNames[i].Patronymic))
                    normalizedFullName += fullNames[i].Patronymic.Trim(' ') + ' ';
                normalizedFullName =
                    normalizedFullName.Remove(normalizedFullName.Length - 1);
                normalizedFullNames.Add(normalizedFullName);
            }
        }

        public List<string> Search(string prefix)
        {
            var searchResult = new List<string>();
            if (prefix.Length > 100)
                throw new ArgumentException();
            if (prefix.Trim(' ').Length == 0)
                throw new ArgumentException();
            normalizedFullNames.Search(prefix.Substring(1), "" + prefix[0], searchResult, normalizedFullNames.Head[prefix[0]]);
            return searchResult;
        }
    }
}
