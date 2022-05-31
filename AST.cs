using System;
using System.Collections.Generic;
using Tokens;

namespace ASTns
{
    public class Node
    {
        public Node Paren {get;set;}
        public Node Left {get;set;}
        public Node Right {get;set;}
        public Token Term {get;set;}

        public Node(Token term)
        {
            Term = term;
        }

        public override string ToString()
        {

            return Term.ToString();
            Right = null;
            Left = null;
        }

    }

     public class ASTree
    {
        public Node Root {get;}

        public ASTree(Node root)
        {
            Root = root;
        }

        public void PostOrder()
        {
            postOrder(Root);
        }

        private void postOrder(Node node)
        {
            if(node.Left != null)
            {
                postOrder(node.Left);
            }
            if (node.Right != null)
            {
                postOrder(node.Right);
            }
            Console.Write(node.Term);
        }

        
    }
}