using System;

namespace Tokens
{
    public interface Token
    {
        public string ToString();  
        public string getVal();
    }

    public class Operator : Token 
    {
        public string Type {get; set;}
        public int Precedence {get; set;}
        public bool LeftAssociative {get; set;}

        public Operator(string type, int precedence, bool leftAssociative)
        {
            Type = type;
            Precedence = precedence;
            LeftAssociative = leftAssociative;
        }
        public override string ToString(){
            return Type;
        }  

        public string getVal()
        {
            return Type;
        }
    }

    public class Function : Token 
    {
        public string FunctionName {get; set;}
        public int Precedence {get;}
        
        public Function(string functionName)
        {
            FunctionName = functionName;
            Precedence = 1;
        }
        public override string ToString()
        {
            return FunctionName;
        }  
        public string getVal()
        {
            return FunctionName;
        }
    }

    public class Number : Token 
    {
        public string Value {get; set;}

        public Number(string value)
        {
            Value = value;
        }

        public override string ToString(){
            return Value.ToString();
        }  

        public string getVal()
        {
            return Value;
        }
    }
}
    
