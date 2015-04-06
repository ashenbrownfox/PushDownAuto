using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushDownAutomata
{
    public class Transition
    {
        public string StartState { get; private set; }
        public char Symbol { get; private set; }
        public string EndState { get; private set; }
        public char pop1 { get; private set; }
        public char push1 { get; private set; }
        public char Pop2 { get; private set; }
        public char Push2 { get; private set; }
        /**
         * Basic Constructor initializes the StartState, endState and symbol
         * **/
        public Transition(string startState, char symbol, string endState,char one,char two, char three, char four)
        {
            StartState = startState;
            Symbol = symbol;
            EndState = endState;
            pop1 = one;
            push1 = two;
            Pop2 = three;
            Push2 = four;
        }

        /**
         * This method will make it easier later on for printing out the states.
         * **/
        public override string ToString()
        {
            return string.Format("({0}, {1}) -> {2}", StartState, Symbol, EndState);
        }
    }
}
