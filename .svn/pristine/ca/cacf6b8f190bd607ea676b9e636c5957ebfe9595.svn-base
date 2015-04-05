using System;
using System.Collections.Generic;
using System.Linq;

namespace BizInfo.App.Services.Tools
{
    public class EasyFts
    {
        /// <summary>
        /// Query term forms.
        /// </summary>
        protected enum TermForms
        {
            Inflectional,
            Thesaurus,
            Literal,
        }

        /// <summary>
        /// Term conjunction types.
        /// </summary>
        protected enum ConjunctionTypes
        {
            And,
            Or,
            Near,
        }

        /// <summary>
        /// Expression node abstract base class
        /// </summary>
        protected abstract class NodeBase
        {
            public abstract bool IsTerminal();
        }

        /// <summary>
        /// Terminal (leaf) expression node class.
        /// </summary>
        private class TerminalNode : NodeBase
        {
            public string Term { get; set; }
            public TermForms TermForm { get; set; }
            public bool Exclude { get; set; }

            public override bool IsTerminal()
            {
                return true;
            }

            // Convert node to string
            public override string ToString()
            {
                string fmt = null;
                if (TermForm == TermForms.Inflectional)
                    fmt = "{0}FORMSOF(INFLECTIONAL, {1})";
                else if (TermForm == TermForms.Thesaurus)
                    fmt = "{0}FORMSOF(THESAURUS, {1})";
                else if (TermForm == TermForms.Literal)
                    fmt = "{0}\"{1}\"";
                return String.Format(fmt,
                    Exclude ? "NOT " : String.Empty,
                    Term);
            }
        }

        /// <summary>
        /// Internal (non-leaf) expression node class
        /// </summary>
        private class InternalNode : NodeBase
        {
            public NodeBase Child1 { get; set; }
            public NodeBase Child2 { get; set; }
            public ConjunctionTypes Conjunction { get; set; }

            public override bool IsTerminal()
            {
                return false;
            }

            // Convert node to string
            public override string ToString()
            {
                return String.Format("({0} {1} {2})",
                    Child1.ToString(),
                    Conjunction.ToString().ToUpper(),
                    Child2.ToString());
            }
        }

        // Characters not allowed in unquoted search terms
        protected const string Punctuation = "~\"`!@#$%^&*()-+=[]{}\\|;:,.<>?/";

        /// <summary>
        /// Collection of stop words. These words will not
        /// be included in the resulting query unless quoted.
        /// </summary>
        public HashSet<string> StopWords { get; set; }

        // Class constructor
        public EasyFts()
        {
            StopWords = new HashSet<string>();
        }

        /// <summary>
        /// Converts an "easy" search term to a full-text search term.
        /// </summary>
        /// <param name="query">Search term to convert</param>
        /// <returns>A valid full-text search query</returns>
        public string ToFtsQuery(string query)
        {
            NodeBase node = ParseNode(query, ConjunctionTypes.And);
            return (node != null) ? node.ToString() : String.Empty;
        }

        /// <summary>
        /// Parses a query segment and converts it to an expression
        /// tree.
        /// </summary>
        /// <param name="query">Query segment to convert</param>
        /// <param name="defaultConjunction">Implicit conjunction type</param>
        /// <returns>Root node of expression tree</returns>
        private NodeBase ParseNode(string query, ConjunctionTypes defaultConjunction)
        {
            TermForms termForm = TermForms.Inflectional;
            bool termExclude = false;
            ConjunctionTypes conjunction = defaultConjunction;
            bool resetState = true;
            NodeBase root = null;
            NodeBase node;
            string term;

            TextParser parser = new TextParser(query);
            while (!parser.EndOfText)
            {
                if (resetState)
                {
                    // Reset modifiers
                    termForm = TermForms.Inflectional;
                    termExclude = false;
                    conjunction = defaultConjunction;
                    resetState = false;
                }

                parser.MovePastWhitespace();
                if (!parser.EndOfText &&
                    !Punctuation.Contains(parser.Peek()))
                {
                    // Extract query term
                    int start = parser.Position;
                    parser.MoveAhead();
                    while (!parser.EndOfText &&
                        !Punctuation.Contains(parser.Peek()) &&
                        !Char.IsWhiteSpace(parser.Peek()))
                        parser.MoveAhead();

                    // Allow trailing wildcard
                    if (parser.Peek() == '*')
                    {
                        parser.MoveAhead();
                        termForm = TermForms.Literal;
                    }
                    term = parser.Extract(start, parser.Position);

                    if (termForm != TermForms.Literal &&
                        String.Compare(term, "AND", true) == 0)
                        conjunction = ConjunctionTypes.And;
                    else if (termForm != TermForms.Literal &&
                        String.Compare(term, "OR", true) == 0)
                        conjunction = ConjunctionTypes.Or;
                    else if (termForm != TermForms.Literal &&
                        String.Compare(term, "NEAR", true) == 0)
                        conjunction = ConjunctionTypes.Near;
                    else if (termForm != TermForms.Literal &&
                        String.Compare(term, "NOT", true) == 0)
                        termExclude = true;
                    else
                    {
                        root = AddNode(root, term, termForm, termExclude, conjunction);
                        resetState = true;
                    }
                    continue;
                }
                else if (parser.Peek() == '"')
                {
                    // Match next term exactly
                    termForm = TermForms.Literal;
                    // Extract quoted term
                    term = ExtractQuote(parser);
                    root = AddNode(root, term.Trim(), termForm, termExclude, conjunction);
                    resetState = true;
                }
                else if (parser.Peek() == '(')
                {
                    // Parse parentheses block
                    term = ExtractBlock(parser, '(', ')');
                    node = ParseNode(term, defaultConjunction);
                    root = AddNode(root, node, conjunction);
                    resetState = true;
                }
                else if (parser.Peek() == '<')
                {
                    // Parse angle brackets block
                    term = ExtractBlock(parser, '<', '>');
                    node = ParseNode(term, ConjunctionTypes.Near);
                    root = AddNode(root, node, conjunction);
                    resetState = true;
                }
                else if (parser.Peek() == '-')
                {
                    // Match when next term is not present
                    termExclude = true;
                }
                else if (parser.Peek() == '+')
                {
                    // Match next term exactly
                    termForm = TermForms.Literal;
                }
                else if (parser.Peek() == '~')
                {
                    // Match synonyms of next term
                    termForm = TermForms.Thesaurus;
                }
                // Advance to next character
                parser.MoveAhead();
            }
            return root;
        }

        /// <summary>
        /// Creates an expression node and adds it to the
        /// give tree.
        /// </summary>
        /// <param name="root">Root node of expression tree</param>
        /// <param name="term">Term for this node</param>
        /// <param name="termForm">Indicates form of this term</param>
        /// <param name="termExclude">Indicates if this is an excluded term</param>
        /// <param name="conjunction">Conjunction used to join with other nodes</param>
        /// <returns>The new root node</returns>
        protected NodeBase AddNode(NodeBase root, string term, TermForms termForm,
            bool termExclude, ConjunctionTypes conjunction)
        {
            if (term.Length > 0 && !IsStopWord(term))
            {
                NodeBase node = new TerminalNode()
                {
                    Term = term,
                    TermForm = termForm,
                    Exclude = termExclude
                };
                root = AddNode(root, node, conjunction);
            }
            return root;
        }

        /// <summary>
        /// Adds an expression node to the given tree.
        /// </summary>
        /// <param name="root">Root node of expression tree</param>
        /// <param name="node">Node to add</param>
        /// <param name="conjunction">Conjunction used to join with other nodes</param>
        /// <returns>The new root node</returns>
        protected NodeBase AddNode(NodeBase root, NodeBase node, ConjunctionTypes conjunction)
        {
            if (node != null)
            {
                if (root != null)
                {
                    root = new InternalNode()
                    {
                        Child1 = root,
                        Child2 = node,
                        Conjunction = conjunction
                    };
                }
                else
                {
                    root = node;
                }
            }
            return root;
        }

        /// <summary>
        /// Extracts a block of text delimited by the specified open and close
        /// characters. It is assumed the parser is positioned at an
        /// occurrence of the open character. The open and closing characters
        /// are not included in the returned string. On return, the parser is
        /// positioned at the closing character or at the end of the text if
        /// the closing character was not found.
        /// </summary>
        /// <param name="parser">TextParser object</param>
        /// <param name="openChar">Start-of-block delimiter</param>
        /// <param name="closeChar">End-of-block delimiter</param>
        /// <returns>The extracted text</returns>
        protected string ExtractBlock(TextParser parser, char openChar, char closeChar)
        {
            // Track delimiter depth
            int depth = 1;

            // Extract characters between delimiters
            parser.MoveAhead();
            int start = parser.Position;
            while (!parser.EndOfText)
            {
                if (parser.Peek() == openChar)
                {
                    // Increase block depth
                    depth++;
                }
                else if (parser.Peek() == closeChar)
                {
                    // Decrease block depth
                    depth--;
                    // Test for end of block
                    if (depth == 0)
                        break;
                }
                else if (parser.Peek() == '"')
                {
                    // Don't count delimiters within quoted text
                    ExtractQuote(parser);
                }
                // Move to next character
                parser.MoveAhead();
            }
            return parser.Extract(start, parser.Position);
        }

        /// <summary>
        /// Extracts a block of text delimited by double quotes. It is
        /// assumed the parser is positioned at the first quote. The
        /// quotes are not included in the returned string. On return,
        /// the parser is positioned at the closing quote or at the end of
        /// the text if the closing quote was not found.
        /// </summary>
        /// <param name="parser">TextParser object</param>
        /// <returns>The extracted text</returns>
        protected string ExtractQuote(TextParser parser)
        {
            // Extract contents of quote
            parser.MoveAhead();
            int start = parser.Position;
            while (!parser.EndOfText && parser.Peek() != '"')
                parser.MoveAhead();
            return parser.Extract(start, parser.Position);
        }

        /// <summary>
        /// Determines if the given word has been identified as
        /// a stop word.
        /// </summary>
        /// <param name="word">Word to check</param>
        protected bool IsStopWord(string word)
        {
            return StopWords.Any(s => String.Compare(s, word, true) == 0);
        }
    }
}
