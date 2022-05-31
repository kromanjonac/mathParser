using System;
using System.Collections.Generic;
using Tokens;
using ASTns;


namespace mathParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Number n = new Number("4");
            Operator lp = new Operator("(",0,true);
            Operator rp = new Operator(")",0,true);
            Operator plus = new Operator("+",2,true);


            
            
            GetASTreeFromRPN(ConvertToRPN(GetTokens("sin(max(23)/3*4)"))).PostOrder();

        }

        static List<Token> GetTokens(string input)
        {
            //operator definitions
            Operator lp = new Operator("(",0,true);
            Operator rp = new Operator(")",0,true);
            Operator plus = new Operator("+",2,true);
            Operator minus = new Operator("-",2,true);
            Operator mul = new Operator("*",3,true);
            Operator div = new Operator("/",3,true);
            Operator exp = new Operator("^",4,false);
            HashSet<string> operators = new HashSet<string>(){"(",")","+","-","*","/","^"};
            Dictionary<string,Operator> opDict = new Dictionary<string, Operator>();
            opDict.Add("(",lp);
            opDict.Add(")",rp);
            opDict.Add("+",plus);
            opDict.Add("-",minus);
            opDict.Add("*",mul);
            opDict.Add("/",div);
            opDict.Add("^",exp);

            //end

            List<Token> ret = new List<Token>();

            bool parsingNum = false;
            bool parsingFun = false;

            string val = "";
            for(int i = 0; i < input.Length; i++) //implementiraj zarez?
            {
                char curr = input[i];

                if (operators.Contains(curr.ToString()))
                {
                    if(parsingFun)
                    {
                        ret.Add(new Function(val));
                        val = "";
                        parsingFun = false;
                    }

                    if(parsingNum)
                    {
                        ret.Add(new Number(val));
                        val = "";
                        parsingNum = false;
                    }

                    Operator currOp = opDict[curr.ToString()];
                    Operator newOp = new Operator(currOp.Type,currOp.Precedence,currOp.LeftAssociative);
                    ret.Add(newOp);
                }
                else if(Char.IsDigit(curr))
                {
                    if(!parsingNum)
                    {
                        if(parsingFun)
                        {
                            ret.Add(new Function(val));
                            val = "";
                            parsingFun = false;
                        }

                        val += curr.ToString();
                        parsingNum = true;
                    } 
                    else 
                    {
                        parsingNum = true;
                        val += curr.ToString();
                    }
                }
                else if (!parsingFun && !parsingNum && curr == 'x')
                {
                    ret.Add(new Number("x"));
                }
                else
                {
                    if(!parsingFun)
                    {
                        if(parsingNum)
                        {
                            ret.Add(new Number(val));
                            val = "";
                            parsingNum = false;
                        }

                        val += curr.ToString();
                        parsingFun = true;
                    }
                    else
                    {
                        parsingFun = true;
                        val += curr.ToString();
                    }
                }
                
            }
            if(parsingFun)
            {
                ret.Add(new Function(val));
            }
            else if(parsingNum)
            {
                ret.Add(new Number(val));
            }

            return ret;

        }

        static Queue<Token> ConvertToRPN(List<Token> tokens) 
        {
            Stack<Token> operatorStack = new Stack<Token>();
            Queue<Token> outputQueue = new Queue<Token>();
            foreach (var token in tokens)
            {
                if(token is Number)
                {
                    outputQueue.Enqueue(token);
                } 
                else if(token is Function)
                {
                    operatorStack.Push(token);
                }
                else if (((Operator) token).Type != "(" && ((Operator) token).Type != ")")
                {
                    Operator current = (Operator) token;
                    while(true && operatorStack.Count > 0)
                    {
                        if(!(operatorStack.Peek() is Operator)) 
                        {
                            break;
                        }

                        if(operatorStack.Count > 0)
                        {
                            Operator top = (Operator) operatorStack.Peek();
                            
                            

                            if (top.Type == "(" || !(top.Precedence > current.Precedence || top.Precedence == current.Precedence &&  current.LeftAssociative))
                            {
                                break;
                            }

                            outputQueue.Enqueue(operatorStack.Pop());
                        }

                        
                    }

                    operatorStack.Push(token);
                }
                else if(((Operator) token).Type == "(")
                {
                    operatorStack.Push(token);
                }
                else if(((Operator) token).Type == ")")
                {
                    Operator current = (Operator) token;

                    while(operatorStack.Peek() is Operator && ((Operator) operatorStack.Peek()).Type != "(")
                    {
                        outputQueue.Enqueue(operatorStack.Pop()); //add check for stack size != 0
                    }
                    operatorStack.Pop(); //add check that '(' is being popped

                    if(operatorStack.Peek() is Function)
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

                } 
                else
                {
                    //return "Error";
                }
            }

            while (operatorStack.Count != 0)
            {
                outputQueue.Enqueue(operatorStack.Pop()); //add check for '(' as it shouldn't appear
            }

            string output = "";
            foreach (var token in outputQueue)
            {
                output += " " + token.ToString();
            }
            return outputQueue;

        }

        static ASTree GetASTreeFromRPN(Queue<Token> rpn)
        {
            Stack<Number> numberStack = new Stack<Number>();
            Stack<Node> nodeStack = new Stack<Node>();

            while(rpn.Count > 0)
            {
                Token op = rpn.Dequeue();

                if(op is Number)
                {
                    nodeStack.Push(new Node(op));
                }
                else if(op is Function)
                {   
                    if (nodeStack.Count > 0)
                    {
                        Node t = nodeStack.Pop();
                        Node n = new Node(op);
                        n.Left = t;
                        nodeStack.Push(n);
                    }
                }
                else
                {
                    Node r = nodeStack.Pop();
                    Node l = nodeStack.Pop();
                    Node n = new Node(op);
                    n.Left = l;
                    n.Right = r;
                    nodeStack.Push(n);
                }
            } 

            return new ASTree(nodeStack.Pop());
        }
    }
}
