CellRendererMulti
=================

This project shows one way to produce a GTK# NodeView with per-row CellRenderer usage.  It's definitely not the only way, and maybe not the best way, but it's simple enough to understand and practical enough to use.

-Rant on-

Having never used GTK#, or even GTK+, I found the process of doing something so simple as this to be vastly overcomplicated and underdocumented.  Since I couldn't find any other working C# CellRenderer subclassing examples on the net, I thought I'd stick this here so that no one else has to go through all the fruitless searching plus trial and error that I did before getting it to finally work.  

Gtk# has many ways to get to the same result, and isn't the most intuitive beast; It's too easy to think you've gone down entirely the wrong path when you're inches from the finish line - only needing to override one more property or cast a value to make an override work.  I'm glad we're _able_ to read the GTK# library source to figure out how to do these things, but we shouldn't _have_ to read the library source to figure out how to do these things.

I'll shut my mouth now before anyone challenges me to rewrite NodeView and do better, which I probably can't.  :)

-Rant off-

Note:  I'll clean up this ugly ugly example code when I get the time, today it's still cluttered with some of my own dead ends and some temporary hacks, I just want to have it on a second computer in case mine dies before I can apply the technique to the program I was actually working on before I fell down this hours-long GTK# rabbit-hole.
