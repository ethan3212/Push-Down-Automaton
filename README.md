# Push-Down-Automaton

This program compares three different algorithms which locate a given word or character: the "standard", KMP, and the BM algorithms. The standard algorithm merely compares elements side-by-side
a match is found. The Knuth-Morris-Pratt algorithm compares the length of the substring to the overall length of the list of elements, and will logically "skip" elements as necessary.
The Boyer-Moore algorithm works similarily to the KMP, except that is will work backwards, "jumping" and "sliding" until a match is found.

Although the BM algorithm requires quite a bit of overhead and is complex to code, it's the most efficient of these three algorithms. 
