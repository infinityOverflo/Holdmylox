using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using static CraftingInterpreters.Lox.TokenType;

namespace CraftingInterpreters.Lox
{
    internal class Scanner
    {
        private readonly string? _source;
        private readonly List<Token> _tokens = new List<Token>();
        private int _start = 0;
        private int _current = 0;
        private int _line = 1;

        internal Scanner(string source)
        {
            _source = source;
        }

        internal List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                _start = _current;
                ScanTokens();
            }

            _tokens.Add(new Token(Eof, "", null, _line));
            return _tokens;
        }

        private bool IsAtEnd()
        {
            return _current >= _source!.Length;
        }
    }
}