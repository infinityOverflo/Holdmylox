using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using static CraftingInterpreters.Lox.TokenType;
using System.ComponentModel;
using System.Runtime.InteropServices.Swift;

namespace CraftingInterpreters.Lox
{
    internal class Scanner
    {
        private readonly string _source;
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
                ScanToken();
            }

            _tokens.Add(new Token(Eof, "", null, _line));
            return _tokens;
        }
        private void ScanToken()
        {
            char c = Advance();

            switch (c)
            {
                case '(': AddToken(LeftParen); break;
                case ')': AddToken(RightParen); break;
                case '{': AddToken(LeftBrace); break;
                case '}': AddToken(RightBrace); break;
                case ',': AddToken(Comma); break;
                case '.': AddToken(Dot); break;
                case '-': AddToken(Minus); break;
                case '+': AddToken(Plus); break;
                case ';': AddToken(Semicolon); break;
                case '*': AddToken(Star); break;
                case '!': AddToken(Match('=') ? BangEqual : Bang); break;
                case '=': AddToken(Match('=') ? EqualEqual : Equal); break;
                case '<': AddToken(Match('=') ? LessEqual : Less); break;
                case '>': AddToken(Match('=') ? GreaterEqual : Greater); break;
                case '/':
                    {
                        if (Match('/'))
                        {
                            while (Peek() != '\n' && !IsAtEnd()) Advance();
                        }
                        else
                        {
                            AddToken(Slash);
                        }
                        break;
                    }
                case ' ':
                case '\r':
                case '\t': break;
                case '\n': this._line++; break;
                case '"': String(); break;
                default: Lox.Error(_line, "Unexpected character."); break;
            }
        }
        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') this._line++;
                Advance();
            }
            if (IsAtEnd())
            {
                Lox.Error(this._line, "Unterminated string.");
            }
            Advance();
            string value = this._source[(this._start + 1)..(this._current - 1)];
            AddToken(TokenType.String, value);
        }
        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (_source[_current] != expected) return false;

            _current++;
            return true;
        }
        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return this._source![_current];
        }
        private bool IsAtEnd()
        {
            return _current >= _source.Length;
        }
        private char Advance()
        {
            return _source[_current++];
        }
        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }
        private void AddToken(TokenType type, object? literal)
        {
            string text = _source.Substring(_start, _current - _start);
            _tokens.Add(new Token(type, text, literal, _line));
        }
    }
}