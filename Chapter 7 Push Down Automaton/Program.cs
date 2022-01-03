using System;
using static System.Console;

namespace Chapter_7_Push_Down_Automaton
{
    class Program
    {
        static void Main(string[] args)
        {
            string poem = "Mary had a little lamb\nIts fleece was white as snow";
            int foundIt;

            // standard algorithm
            foundIt = Find1(poem, "little");
            WriteLine("found it at " + foundIt);

            // KMP algorithm
            foundIt = Find2(poem, "little");
            WriteLine("found it at " + foundIt);

            // BM algorithm
            foundIt = Find3(poem, "little");
            WriteLine("found it at " + foundIt);

            ReadLine();
        }
        // This is the standard algorithm
        static int Find1(string text, string substring)
        {
            int subLoc = 0;
            int textLoc = 0;
            int textStart = 0;

            while (textLoc < text.Length && subLoc < substring.Length)
            {
                // comparing each character
                // text.charAt(textLoc)
                if(text[textLoc] == substring[subLoc])
                {
                    textLoc++;
                    subLoc++;
                }

                else
                {
                    // start over at a new location
                    textStart++;
                    textLoc = textStart;
                    subLoc = 0;
                }
            }

            if(subLoc >= substring.Length)
            {
                // we found it at...
                return textStart;
            }

            else
            {
                return -1;
            } 
        }

        // This is the Knuth-Morris-Pratt algorithm
        static int Find2(string text, string substring)
        {
            int subLoc = 0;
            int textLoc = 0;
            // calculate the "fail pattern" so we know how many to skip
            int[] fail = getFail(substring);

            while(textLoc < text.Length && subLoc < substring.Length)
            {
                if(subLoc == 0 || text[textLoc] == substring[subLoc])
                {
                    textLoc++;
                    subLoc++;
                }

                else
                {
                    subLoc = fail[subLoc - 1];
                }
            }

            if(subLoc >= substring.Length)
            {
                // we found it at...
                return textLoc - substring.Length;
            }

            else
            {
                return -1;
            }
        }

        static int[] getFail(string substring)
        {
            int[] fail = new int[substring.Length];
            int temp;

            fail[0] = 0;
            for(int i=1; i<substring.Length; i++)
            {
                temp = fail[i - 1];
                while(temp >= 0 && substring[temp] != substring[i - 1])
                {
                    temp = fail[temp];
                }

                fail[i] = temp + 1;
            }

            return fail;
        }

        // This is the Boyer-Moore algorithm
        static int Find3(string text, string pattern)
        {
            int textLoc = pattern.Length - 1;
            int patternLoc = pattern.Length - 1;

            // there is a sizable initial overhead that occurs before the search starts
            int[] slide = getSlide(pattern);

            // treat characters as numbers
            Write("slide: ");
            for(int i = 'a'; i <= 'z'; i++)
            {
                Write($"{char.ConvertFromUtf32(i)} = {slide[i]} ");
            }

            WriteLine();
            int[] jump = getJump(pattern);

            Write("jump: ");
            foreach(int i in jump)
            {
                Write($"{i} ");
            }

            WriteLine();

            while (textLoc < text.Length && patternLoc >= 0)
            {
                // text.CharAt(textLoc)
                if (text[textLoc] == pattern[patternLoc])
                {
                    // notice that this algorithm starts at the back and works forward
                    textLoc--;
                    patternLoc--;
                }

                else
                {
                    textLoc = textLoc + max(slide[text[textLoc]], jump[patternLoc]);
                    patternLoc = pattern.Length - 1;
                }
            }

            if(patternLoc == 0)
            {
                return textLoc + 1;
            }

            else
            {
                return -1;
            }
        }

        static int max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        static int min(int a, int b)
        {
            return (a > b) ? b : a;
        }

        // this is the "Bad character" array
        static int[] getSlide(string pattern)
        {
            // let's assume 8-bit character set
            int[] slide = new int[256];

            // start with a maximum slide value
            for(int i = 0; i < 256; i++)
            {
                slide[i] = pattern.Length;
            }

            // then calculate the real value for every character
            for(int i = 0; i < pattern.Length; i++)
            {
                slide[pattern[i]] = pattern.Length - 1 - i;
            }

            return slide;
        }

        // this is the "Good prefix" array
        static int[] getJump(string pattern)
        {
            int[] jump = new int[pattern.Length];
            int[] link = new int[pattern.Length + 1];

            // beginning values for jump
            for(int i = 0; i < pattern.Length; i++)
            {
                jump[i] = 2 * pattern.Length - i;
            }

            int test = pattern.Length;
            int target = pattern.Length + 1;

            while(test >= 0)
            {
                link[test] = target;
                while(target < pattern.Length && pattern[test] != pattern[target])
                {
                    jump[target] = min(jump[target], pattern.Length - test);
                    target = link[target];
                }

                test--;
                target--;
            }

            for(int i = 0; i < pattern.Length; i++)
            {
                jump[i] = min(jump[i], pattern.Length + target - i);
            }

            int temp = link[target];
            while(target < pattern.Length)
            {
                while(target < temp)
                {
                    jump[target] = min(jump[target], temp - target + pattern.Length);
                    target++;
                }

                temp = link[temp];
            }

            return jump;
        }
    }
}