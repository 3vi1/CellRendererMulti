CellRendererMulti
=================

This project shows one way to produce a GTK# NodeView with per-row CellRenderer usage.  It's definitely not the only way, and maybe not the best way, but it's simple enough to understand yet practical enough to use.

I'm using GTK# in a level editor for a FNA game/demo, and put this together as a cleaner/simpler example of what I did to make the properties window work in that project.

-Rant on-

Having never used GTK#, or even GTK+, I found the process of doing something so simple as this with the GTK# API to be vastly overcomplicated and underdocumented.  Since I couldn't find any other working C# CellRenderer subclassing examples on the net, I thought I'd stick this here so that no one else has to go through what (I'm ashamed to say) was at least several hours of fruitless googling, reading the gtksharp source, plus trial and error to get this seemingly _basic_ functionality.

I'll shut my mouth now before anyone challenges me to rewrite NodeView and do better, which I'm pretty sure I can't.  :)

-Rant off-

'Nuff said.
