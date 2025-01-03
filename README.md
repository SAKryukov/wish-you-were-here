## Wish You Were Here&hellip; Only Once

Think about how many times you wanted to write something like this:
~~~
if (First Time Here)
    ThisWillHappenButOnlyOnce();
~~~
The only problem is that the condition should work separately for every call of the predicate of the “if” block; ideally, it must be sensitive to the location of the code line.

This is something the presented technique does.

[Original publication](https://SAKryukov.GitHub.io/publications/2009-10-24.Wish-You-Were-Here-Only-Once.html)