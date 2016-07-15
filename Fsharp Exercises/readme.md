6/18/2016:

Let me initialize this repo here. Unlike Haskell, I am already quite proficient at F#, so I won't be doing things like HackerRank exercises. I think in the end with regards to Haskell, I would say that I am quite comfortable with programming in it in a purely functional manner. Hopefully that should be enough for the Futhark compiler. I do not think I would be comfortable doing work in anything more than that.

Lately, having learned Haskell, I have come to feel like learning about [OCaml](https://www.fun-mooc.fr/courses/parisdiderot/56002/session01/Table_of_Contents/) while reading the tome by Diehl in tandem.

After that I'll look into creating GUIs in F#, though I've read that C# might be better for that.

UPDATE: Done with Haskell. In the end, I think it is just a meaningless morass of abstraction. Maybe I am too stupid to understand its deep insights, but I think out of roughly 30 days that I spent learning it, I could have left about 20 out. Insofar I've become a better programmer, that is because I programmed in a purely functional way in a while, not because of all that pointless type theory stuff.

The deeper the abstraction is, the more it needs to be beholden to reality - I think the science types are too eager to forget this sometimes. At that point, science becomes merely superstition.

I do not think learning Haskell past this point will make me a better programmer. The one interesting technique, the lambda-stacking fold is not something I can use in eager languages anyway.

Human-made programming languages might have reached [their pinnacle](https://blogs.janestreet.com/the-ml-sweet-spot/) with SML from which F# and OCaml derive. They aren't obsolete anymore than plain arrays and the plus operator is.

I need a break even though it is only 9:15am here.

I think I'll watch the OCaml videos for a bit. I think that might cheer me up. I feel a need to go on a firmer footing.

UPDATE: It seems the course is closed and so are the lectures, so I found these [lecture notes](http://www.ii.uni.wroc.pl/~lukstafi/pmwiki/index.php?n=Functional.Functional) for a different course for a different functional programming course in OCaml to tide me over.

My God, the early lectures are damn brutal. I already know this stuff, so it is not a problem to me, but the instructor does not hold back even a little.

At any rate, I like this. Impure, eager language like OCaml is definitely my cup of tea. My great greed drives to me to take all that I can, but not all can be compressed into a single point. A single great thrust is all that is needed to break the morass of abstraction. I'll be content to let Haskellers play in their own corner of the playground.

It is not my fault, but I really regret how the story SML played out. The greatest language of all time, so great that it has well over a dozen compilers and the eggheads doing them could not get together to form a community around a dominant implementation like with GHC. This probably set back the programming community by at least a decade.

Functional programming courses like the one above do not help much either.

Is it really necessary to start off with massive recursion, symbolic differentiation and continuation passing style in like the first three lectures? **(Edit: Actually, I'll forgive this one since it is obviously not a beginner's class.)**

The greatest thing about the (ML style) functional languages are their type safety and lightweight type inference - the rest is really not that important in comparison. It allows one to use the far stronger hybrid functional/imperative style than would be possible if one programmed in say C++ or Python.

This is pretty much why I gave up on Python after day one. There is no point in programming in a language where I could not be at my best. Scala and Haskell fall into that inconvenient category.

I feel a bit lonely because of this. Two great languages is not much to go on, but it should make do. One is enough in fact.

UPDATE: Actually, this stuff in [lecture 5](http://www.ii.uni.wroc.pl/~lukstafi/pmwiki/uploads/Functional/functional-lecture05.pdf) is quite interesting. Would this alternating type list even be possible in F#? And these function signatures...I never even thought of doing type annotations like this. It is quite interesting.

UPDATE: This stuff on modules in genuinely novel to me. And as matter of fact, tree based structures are exactly the area of my current (temporary) interest.

This is good stuff. I wonder if I could replace the OCaml modules with .NET abstract classes, or are there some limitation to this?

Also going through these notes make me wonder whether there was anything in the Expert F# book on this.

UPDATE: Done with lecture 5. I think as an exercise, I am going to be transcribing everything step by step into F# when I am done going through all the notes. Tree based data structures were one thing I was content to leave to libraries, so when the Scala assignment came out, it hit me right where it hurts. I'll plug this hole in my skillset at all cost.

UPDATE: I see that lecture 6 has some stuff on constraint propagation. Damn, I wish I there were videos along with the lectures.

Actually, the difficulty of course is not the problem. A difficulty of a course is not the problem.

The real problem is generally false advertising, which is not the case here though as nowhere does it state that the assignment that would take 5d in reality should take 3h on paper.

There is definitely a lot of good information here.

UPDATE: A third of the day left to go. These lecture notes are too much to go all through in just one day. And I did a lot besides this as well.

It is difficult to explain how natural the OCaml code seems to me. Despite not having written a line of the language before, it crystal clear to me, unlike the Haskell code. This will do it. OCaml will heal my soul. I know what I stand on here; I am not amongst the clouds.

UPDATE: The course even has stuff on constraint propagation which is great. It uses lists though which is bleh. Though for small lists, depending on how efficient the allocator is, it might not end up being too bad. Still...

Yeah, going through these first six lecture notes was no easy feat. Even though I've been at it for 12h I still have in the tank for a bit more. I want to get this out of the way, so I can start working on trees.

UPDATE: Holy shit, this stuff on infinite streams never even occurred to me. I thought it was a language feature. And it is possible to use `function` with two arguments? ...Er, no that is just a tuple.

UPDATE: I think 13h a day is enough. The seventh lecture on infinite streams and laziness was instructive, I hope that not bothering to internalize it won't bite me in the ass like it did for trees. But at least I know how that it is possible to generate infinite streams and lazily evaluate them using standard ML language features.

I've been doing too much reading at any rate, just for dealing with trees, but at least with Ocaml I feel like I can grasp it in a day unlike with Haskell. The code reads very naturally to me and there aren't a constant stream of language extensions and esoteric features to keep track of.

I can do this. Tomorrow I will finish the rest of the lectures and then deal with trees. After that I will deal with the assignment proper.

6/19/2016:

I've been trying to settle my feelings on Haskell by looking for alternatives to [language purity](http://programmers.stackexchange.com/questions/258011/alternative-to-language-purity). No matter how I look at it, Haskell's type magic is too much mental overhead - programming is hard enough as it is. It might be worth looking into [Koka](http://research.microsoft.com/en-us/projects/koka/). Given how difficult Haskell was given all the experience in functional programming that I had going into it, I am 100% sure that lazy by default, monadic paradigm will never reach any significant uptake. If this was this hard for me, I can hardly imagine how difficult it would be for a beginner to the functional paradigm.

Standard ML-like languages like F# and OCaml do stand a chance, but the way to improve them is to work more on the basics rather than mess with their type systems. Still, maybe an effect system would not be bad.

Above all it has too be unobtrusive, which is something monads completely fail at. Added safety they provide is not worth the cognitive overhead.

UPDATE: Koka seems interesting. On [this thread](https://www.reddit.com/r/programming/comments/4aq3ta/koka_overview_slides_pdf_this_language_deserves/) somebody mentioned [Eff](http://www.eff-lang.org/) as well which I'd never heard of before. I guess today I'll be doing some mostly reading too. Tomorrow I will have to dedicate a bit of time to write an update for the Simulacrum blog.

UPDATE: This page is down, so I'll paste a comment from it:

"One must keep in mind that monad transformers simply lack
expressiveness and fundamentally inefficient. First of all, the order
of effects may matter: when adding transformers for the state and the
backtracking effects, the order matters a great deal. Sometimes we
need both persistent and backtrackable state, so we need two instances
of StateT. We distinguish them via the number of 'lift' operations --
using unary notation. This is error prone, inconvenient let
alone just ridiculous. One should also keep in mind that each layer
adds overhead (e.g., layer of closures for ReaderT and StateT) that is
present even in the code that does not use that particular
effect. Please contrast with OCaml: a code that does not use mutation
or backtracking can be compiled without any knowledge of mutations and
backtracking. In contrast, a monadic code that does no State or
backtracking operation still has to thread the state throughout, even
if that piece of code does nothing with the state.

The most serious problem with monad transformers is that they simply
lack expressiveness. Monad transformers impose a fixed ordering of
effects, which is not sufficient in practice. Our paper on Delimited
Dynamic Binding (ICFP06) discusses the issue and points to Haskell
code. That lack of expressiveness was quite surprising to us when we
realized it. Incidentally, this result helps explain the popularity of
powerful flat monads, such as IO.

I think there are advantages to a functional encoding of effects
(e.g. with monads) over building the effects into your logic/type
system (recently Disciple or Separation Logic). It makes the core of
your language simpler and is generally more powerful

The more powerful claim is provably false. Please see the works of
Filinski and Wadler, Thiemann. In general, type-and-effect systems are
equivalent to (flat) monadic ones. In addition, if you use monad
transformers, you provably lose power.

As to simplicity, it seems that the unending stream of monad tutorials
explaining once again what is essentially a functional composition (or
the A-normal form, to be more precise) points out that perhaps the
concept of a Monad isn't at all a good match for a programming
language practice. Monads may be deeply rooted in Category Theory --
but then Peano numerals are too have the clearest mathematical
foundations and yet nobody seriously proposes to use them for
practical numerical computations.

I'm extremely delighted to see the work by Ben Lippmeier. Finally
someone answers the call Filinski made back in 1994, in his paper
Representing Monads. Please see the Conclusion section of the paper,
especially the paragraph starting with the phrase "But surely there
is more to "functional programming with escape and state" than monadic
effects. After all, monads provide only the lowest-level framework for
sequencing computations."" I'm anxiously looking forward to the
further development of The Disciplined Disciple Compiler.

how do you use an effect system to implement software
transactional memory?

Very easily: the function atomically takes a thunk whose type dictates
that the thunk may not do any observable effect. If the thunk needs to
access or modify a global variable, it has to ask (using
continuations, for example) the 'operating system', the central
arbiter. The arbiter decides on the appropriate policy, be it
optimistic or pessimistic. Please see the Zipper File System, which is
transactional naturally."

UPDATE: I've gone through the Eff examples, and while the syntax looks more appealing than for monads, they haven't been judiciously commented thus leaving me guessing what they are supposed to be doing.

On the face of it, algebraic effect look like exception handling, which does feel more elegant. This is different from how Koka does it.

UPDATE: I see, [it is similar](http://www.eff-lang.org/handlers-tutorial.pdf) to exception handling in the upwards direction.

Actually, this is something that would never have occurred to me. I need to think about this more.

Here is a [tutorial](http://arxiv.org/pdf/1203.1539v1.pdf) for programming with effect handlers. All of this is completely new to me.

UPDATE: Given Haskell's current status, there a lot of good information in the Reddit Haskell sub. Actually, I never got as far as using monad transformers in Haskell, but standard monads are bad enough.

UPDATE: An excerpt from [Programming With Algebraic Effects and Handlers](http://arxiv.org/pdf/1203.1539v1.pdf).

"
let choose_all d = handler
  | d#decide () k -> k true @ k false
  | val x -> [x]

Notice that the handler calls the continuation k twice, once for each choice, and it
concatenates the two lists so obtained. It also transforms a value to a singleton list.
When we run
with choose_all c handle
  let x = (if c#decide () then 10 else 20) in
  let y = (if c#decide () then 0 else 5) in
  x - y
the result is the list [10;5;20;15]. Let us see what happens if we use two instances
of choice with two handlers:
let c1 = new choice in
let c2 = new choice in
  with choose_all c1 handle
  with choose_all c2 handle
    let x = (if c1#decide () then 10 else 20) in
    let y = (if c2#decide () then 0 else 5) in
    x - y
Now the answer is [[10;5];[20;15]] because the outer handler runs the inner one
twice, and the inner one produces a list of two possible results each time."

This is quite interesting indeed, but I still do not see what it has to do with functional purity. This language feature does seem quite powerful though and does not reduce the power of type inference like dependent types do for example.

UPDATE: I went through the paper. Even though it is only 25 pages, it is much more readable than just running the examples. So far, after ranting against monads and reading all I could on effect systems, I find myself adrift once again.

I think I need to have a lie down after this to digest this meal.

Better, better, better...just what is better? I have no idea.

It is not so simple that just adding more language features to F# for example would make it better. So just what? At this point I am wondering whether purity is such a big deal after all in a general purpose language.

ML styled impure and strict languages are such a simple proposition over everything else, and the whole of programming community is starting to converge around their basic design if one looks more carefully. When moving from languages from weaker type systems, there is literally no cost to using them in comparison.

But when I try to move up to Haskell (and even before that I tried Idris,) I run into all sorts of difficulties it is seems and am forced to give the techniques I so painstakingly developed.

Reading up on all this type magic in the past month is making me lose track of what is important.

Ultimately, the type system is kind of like a memory augmenter for the human users. If I had a much better, superhuman memory and speed there would be no need for them and I could just do all my programming in assembly with the same proficiency as in normal languages.

There will always be cases, where language with strong type systems are essentially no better than dynamic ones.

Maybe functional purity is taking it too far and should be reserved for restricted accelerator languages like Futhark? Unlike with Haskell, I cannot see a single disadvantage of having a restricted language like it be functionally pure.

Compared to Haskell, F# does feel like a dynamic language practically...

When you take the exercise of adding stronger and stronger types to a language to its limit, the end point at which you get is that the entire program is a type.

UPDATE: ...

Enough of this. Let me continue with [the lectures](http://www.ii.uni.wroc.pl/~lukstafi/pmwiki/index.php?n=Functional.Functional).

The PL guys might be grasping at straws after all. The kind of types I like are the ones that are just there. They barely need to even be learned. F# was smooth to me from day 1 and then I just feel in love with it. Are exceptions really the answer to everything - unlikely.

This is not power.

Despite my misgivings about Python, its community understands very well what makes a language great. I should take note from them going forward.

For the past month I've really lacked focus on things that matter. Partly that is due to ignorance.

Let me just skim through those last four lectures and tomorrow I will come back to what matters.

I'll deal with those damn trees and with renewed focus do what I really need to do - research stealth.

UPDATE: Holy crap, lecture 8. Nobody has the time to master all of this. Let me just go to the next thing.

"Using lists makes sense for up to about 15 elements."

Yeah, I thought it would be something like this.

UPDATE: The optimization part is pretty straightforward. The last two parts is something that I won't take long, but currently, I have been at this for 10h. After I am done, I will hardly have the energy to start new projects.

For lecture 10, I already knew about zippers from Learn You A Haskell book.

UPDATE: Each and every of the lectures is very difficult. This OCaml course is very hardcore.

Lecture 11 which is the last one goes into what polymorphic variant (discriminated union) types are. There are actually more lectures after this, but their notes are missing.

UPDATE: Done. As expected from the lecture notes, the author did not bother explaining what they are, but just straight up showed the source code from OCaml compiler.

I am done.

I've really gone far from my original goal. Let me open a new Fsharp project and I'll see whether I can at least do the binary search trees today.

UPDATE: Well, that was easy. Now at the very least, let me see if I can replace Ocaml modules with abstract classes.

If by the end of this, I can internalize Red Black trees I will have made a large step forward in my understanding of data structures.

UPDATE: No, I can't really do straight up modules in Fsharp. I thought it might be possible since they seem similar to abstract classes or interfaces to a degree, but now I realize that modules are really abstract classes combined with discriminated unions. I can't really mix and match them. What I can do though is emulate the entirety of their functionality without much difficulty. Thankfully, Fsharp's variant types can have member functions with generic types for values.

Scala could do this though.

In my own programming though, I rarely ever use object oriented features anyway. My code is mostly procedural. I find that closures and nested functions together with tuples, record and union types cover most of my OO needs.

Those saying that F# is lesser to Ocaml because it lacks feature x miss just powerful the ML core is. The killer feature of functional programming are first class functions and the ability to compose them which is largely independent from modules.

It is what made me fall in love with functional programming in the first place.

UPDATE: Let me call it a day. 12h a day as usual is enough. I am trying to figure out how to do the remove in BST. Actually, I forgot when it was, it was for some course I had taken in early 2015 when I was starting out, but back then I implemented the remove function in a BST by simply blanking out the item. Here I will not settle for anything else, but rotating the entire tree.

I thought to use the insert function, but by definition the key type is not the tree type. Hmm...I wonder what I should do? I'll figure it out tomorrow.

6/20/2016:

Here is a good page describing [GADTs](http://mads-hartmann.com/ocaml/2015/01/05/gadt-ocaml.html). So basically they are like ADTs, except can be constrained further. They are like typed ADTs. When I saw this yesterday, I did not have the energy for it, but it is crystal clear to me now at 7:35am. I got up really early today. Hopefully I can work my way up to Red Black trees today.

My bet is that in the future this feature will arrive [in F#](https://fslang.uservoice.com/forums/245727-f-language/suggestions/5664643-add-support-for-gadts). It looks [really convenient](https://blogs.janestreet.com/why-gadts-matter-for-performance/). It has some elements of dependent typing too.

I am not sure what the _ holes in the GADT type declaration are supposed to be doing though.

It seems that just like records, tuples and union types are a match for object-oriented classes, it seems that GADTs are the functional answer to function overloading in some cases. They seem to have features of that.

Well, no matter. I've wasted a full hour on this today.

It is time for me to lock on. Let me deal with the remove function in the binary tree. After that I'll write something for the Simulacrum update. As an aside, I am kind of enjoying making these leading journals, this one, and one private. I'll stop this public one when I am done with these exercises.

UPDATE: Here is an algorithm for node removal in [BST in C++](http://www.algolist.net/Data_structures/Binary_search_tree/Removal). I am not sure if the node splitting algorithm in [lecture 5](http://www.ii.uni.wroc.pl/~lukstafi/pmwiki/uploads/Functional/functional-lecture05.pdf) is doing the same thing.

UPDATE: Yes, yes it is. When `k = k2` the `split_rightmost` function looks for the rightmost key of the left node to switch it with the one being removed.

At any rate, just for show, I made my own `split_leftmost` to look for the leftmost key of the right node. With this I am done with the BST. The next step will be to implement a Red Black tree and maybe a treap after that. After that I'll look into visualizing them and then finally do the quadtrees.

But for now, let me do an update for [Simulacrum](https://gamesoftranscendi.wordpress.com/). Ever since I stopped writing it, there hasn't been a single day I haven't thought about it. Well...I'll save the those thoughts for the update. I'll have it up in a few days.

UPDATE: I am done with the update for the Simulacrum blog.

It is 3,321 words and 19,294 characters (16,008 without whitespaces) long. It took me six hours to write. I'll proofread it twice, once tomorrow and once before I post it.

And looking at my private journal itself, it is 346 pages and 104.8k words long. I had only opened it on May 10th because the previous file had become so large that I could not save it without waiting 10s. This is amazing no matter how one looks at it. Before my 20s I had never penned down my thoughts to anything and only kept them to myself. I had only started this because of the advice that it is a good idea to keep a trading journal.

Actually, sardonically I am tempted to add a piece in the footnote how it would be great to train some bots to learn some programming. Maybe with an unbiased agent we could finally get to the bottom of which programming language is the most superior. ...I'll hold myself back. Those retarded footnotes that make me go back and forth in science articles are the worst thing ever.

No, no, they are useful.

Now, let me proceed with programming. I want to figure out the details for the Red Black tree today.

UPDATE: I've gone through the code from lecture 5 and watched the videos by [zooce](https://www.youtube.com/watch?v=axa2g5oOzCE), but I really need some more. Also that video on optimizing matrix matrix multiplication using dynamic programming seems pretty interesting. As expected, that update took a lot out of me today. I have only a limit amount of focus to spend per day. Internalizing even something like a binary search tree is not an easy task when it is done for the first time.

It might be a good idea to take a look at videos by Sedgewick again, but there are plenty of videos more on the net.

UPDATE: Sedgewick's videos are crystal clear as usual. I feel like I could implement the 2-3 tree right now. I still do not get the Red Black tree. I know the necessary moves, but I do not think the code from lecture 5 implements all the cases from zooce's videos. Maybe that is the simpler left leaning red black trees?

At any rate, I've been awake for around 14h now. I am done.

I've already spent roughly a week on the Scala course. Hopefully I will be done with this trip into the forest in a few days.

For over a month, my focus has been remarkably low due to dealing with Haskell and all sort of things besides that. I'll pick up my output once I clear out the mental baggage that I got from adopting the new languages.

6/21/2016:

I've went through Sedgewick's videos on Left Leaning Red Black trees and completely understand them now. That having said, I still do not understand the regular ones from [lecture 5](http://www.ii.uni.wroc.pl/~lukstafi/pmwiki/uploads/Functional/functional-lecture05.pdf).

...Actually, now that I look at page 35 I can see that all the cases are explicitly written out. Amazing that I never noticed this before.

It is simply too different to look at the pages without focus and with focus.

UPDATE: Ok, actually I think now I can completely visualize the red-black cases after staring at the lecture 5 code for over an hour. This is amazing. ML pattern matching rocks! It also did not occur to me that BSTs will get printed out in order before I implemented it yesterday. Knowing that they are not all over the place helps a lot. Actually, those simple inorder, outorder HackerRank exercises that I did in C++ did help my understanding despite my scoffing at them.

I still do not understand why the RB trees work though. I understand why 2-3 trees do and why LLRB do as they have 1-1 correspondence with 2-3 trees.

I'll have to watch some more videos.

UPDATE: Done with [this one](https://www.youtube.com/watch?v=O3hI9FdxFOM). The last 30m are really tedious. OCaml code is incredibly elegant and actually more understandable than Erik Dehaney's explanation. Unfortunately, he did not cover deletions.

Also, I now understand why RB trees work. That is due to their equivalence to 2-3-4 trees.

Ok, now I almost get everything about them, but I still need to figure out deletions.

Also I think, I've finally settled my feelings in regards to Haskell.

Haskell's problem aren't monads. It is not laziness by default, though that is by a lesser degree. The real problem is functional purity. Monads are just a symptom. I think it is impossible to have a purely functional general purpose language.

The problem with impure languages such as F# though is not that they are impure, but the lack of referential transparency. That could be fixed.

...No actually, referential transparency is directly linked to purity. What I really meant to write is effect tracking.

Maybe something like an effect tracking system like Koka's for ML-like languages would be ideal and would be the true pinnacle of design. It really might be the pinnacle, because if you take the exercise of continually tacking stuff into the type system then you end up with absurd situation where there are no such things as algorithms or even data structures. The entire program would be a type.

...At any rate, after studying Haskell, I am no longer convinced that functional purity is an ideal - despite its advantages, the cost to having the language be like that is significant.

I'll no longer harbor doubts about the way I program and neither will I admire theoretically stuffed language such as Haskell, Scala and Idris.

The irony is that I like F# vs Haskell for the same reason other people would like Python vs C++. That fact is not lost to me.

No matter what, a language does need some dynamism to it. If it is too rigid it is no good.

Edit for the above: As a real world example, the Diffsharp library authors tried to originally implement it in Haskell and found the type system to be an obstacle.

UPDATE: I finally found some time to research deletion. Now that I understand insertion I can see why the courses just skip this part. I wonder if it would be easier in 2-3 trees? My hunch is yes, but I'll have to take a look.

One thing I do not understand why the Fsharp example for LLRB trees is more complex that the imperative version.

Also what is [this](http://www.read.seas.harvard.edu/~kohler/notes/llrb.html)?

Well, no matter. The standard RB tree insert would be trivial to implement right now and is very simple thanks to pattern matching. Though given how difficult delete is here, it might be worth looking for alternatives? 2-3 trees or 2-3-4 trees perhaps.

UPDATE: The [2-3 tree](http://v2matveev.blogspot.hr/2010/03/data-structures-2-3-tree.html) is as one would expect. I would also guess it would be more efficient in general. But what about the deletion in such a thing?

UPDATE: It is conceptually simpler, but I guess it would still require quite a bit of work. For all tree based structures it seems that deletion is significantly more complicated than insertion. Let me add the RBT to the example and then I'll move to the next stage - which is quadtrees and the making a GUI to visualize those things. I do not need a GUI for RB trees, but it might be useful for quadtrees.

UPDATE: I implemented the RB tree on my own without looking at the solution. Benchmarking it against the F# inbuilt set, I see it is around 66% slower.

I am satisfied with this. The next step would be to check out quadtrees. I'll do a bit of that if I have time, but it is time to proofread. I added a bit of extra to the update near the end as well.

UPDATE: Added a few more lines. 3,776 words and 21.8k characters long. Also I did find a few spelling misses, but most likely they were not all. I'll no doubt discover more after I've posted it on the 23rd. It is really hard for me to force myself to read what is actually there instead of skimming what I wrote. One needs to be in the proper mindset for that.

Before calling it a day, let me watch a video on quadtrees.

UPDATE: The lectures I could find on Youtube have really poor video quality or do not go into detail. Here is one that seems [to be watchable](https://www.youtube.com/watch?v=_61ysrPJRDg). I had no idea quadtrees were made for image compression originally.

As usual I was at this for quite a while. I'll continue tomorrow.

It might be worth revisiting Sedgewick's lectures on trees as well. Though he did not cover quadtrees, he did cover kd-trees. I might as well go full hog.

6/22/2016:

I've went through 2/3rds of the way with that video above, but not being able to see the handwritting is really painful. I need a better resource for this.

I am aware that I am doing way more work than is strictly necessary for the Scala assignment, but I'll plug this hole for good. I just remembered that Youtube has a filter function. I think I'll be able to find some better quality videos this way.

UPDATE: No such luck. I am really scouring the internet for information appropriate for my level.

I've moved on to looking at different kind of info.
https://www.cs.umd.edu/class/spring2008/cmsc420/L17-18.QuadTrees.pdf
http://www.mikechambers.com/blog/2011/03/21/javascript-quadtree-implementation/
http://www.i-programmer.info/programming/theory/1679.html

Yeah, I know what quadtrees do, but I am not exactly sure how to use them.

I supposed binary search trees and RB trees were a good intro for me to tree based data structures, but this is ridiculous. At this rate, I am going to end up coding the quadtree on my own and then figure out how to use them myself.

UPDATE: How difficult...I'll go through the relevant section in Sedgewick course on spatial partitioning algorithms and then move straight to coding. This is ridiculous.

UPDATE: Done with the geometric applications in Sedgewick's course. I'll move on to actually implementing them from here. In fact, the section on finding the nearest neighbor from his course could be applicable to quadtrees as well. Last time, I did not focus enough on the lectures, but this time I crystalized it all.

UPDATE: Nope, I still do not have all the pieces here yet. For quadtrees, do I need to determine its sizes ahead of time so I can split on it? It is annoying how little material there is on it.

6/23/2016:

I am nearly over my hangover from yesterday. On the plus side, the injection of liquor into my system was pretty conductive to proofreading. I managed to root out quite a few errors. The whole thing came up to 4.1k words in the end.

You know, fuck that Scala course. I already spent over 10 days on it, most of it because of that retarded quadtree assignment.

My plan is to complete the quadtree that I started yesterday, visualize it using a GUI and then move to dissecting OpenHoldem. Having the skill of building interfaces for RL agents will be a huge benefit going into the future and there is no need to delay this any longer.

It is literally standing between me and making money with machine learning.

To be honest, maybe I'll take a look at the videos for the Approximation Algorithms course, but that will be it.

Without further ado, let me do deal with it.

UPDATE: Well, here is the quadtree. It is quite nicer than the one I found on the net.

```fsharp
// This quadtree is quite nicer that the snippet.
// The question is what do I do with it?

type QuadTree =
    | Empty
    | Leaf of x: float32 * y: float32
    | Fork of x1: float32 * x2: float32 * y1: float32 * y2: float32 * nw: QuadTree * ne: QuadTree * sw: QuadTree * se: QuadTree

    // Unlike standard trees, quadtrees should start from a Fork root
    static member create_empty(x1,x2,y1,y2) =
        if x1 >= x2 && y1 >= y2 then failwithf "x1(%A) >= x2(%A) && y1(%A) >= y2(%A)" x1 x2 y1 y2
        Fork(x1,x2,y1,y2,Empty,Empty,Empty,Empty)

    member t.insert(x,y as q) =
        let insert (x,y as q) (t: QuadTree) = t.insert q

        // The arguments for insert' are boundaries.
        let rec insert' (xy: (float32*float32*float32*float32) option) (t: QuadTree) =
            match t with
            | Empty -> Leaf(x, y)
            | Leaf(x',y') ->
                if x' = x && y' = y then failwith "Cannot insert an item with the same coordinates into a quadtree."

                let x1, x2, y1, y2 = xy.Value
                Fork(x1,x2,y1,y2,Empty,Empty,Empty,Empty) |> insert (x', y') |> insert (x, y)
            | Fork(x1,x2,y1,y2,nw,ne,sw,se) ->
                if x >= x1 && x < x2 && y >= y1 && y < y2 then
                    let mid_x, mid_y = (x1+x2)/2.0f, (y1+y2)/2.0f
                    let left, up = x < mid_x, y < mid_y

                    match left, up with
                    | true, true -> Fork(x1,x2,y1,y2,nw |> insert' (Some (x1,mid_x,y1,mid_y)),ne,sw,se)
                    | false, true -> Fork(x1,x2,y1,y2,nw,ne |> insert' (Some (mid_x,x2,y1,mid_y)),sw,se)
                    | true, false -> Fork(x1,x2,y1,y2,nw,ne,sw |> insert' (Some (x1,mid_x,mid_y,y2)),se)
                    | false, false -> Fork(x1,x2,y1,y2,nw,ne,sw,se |> insert' (Some (mid_x,x2,mid_y,y2)))
                else failwithf "Cannot insert(%A,%A) outside the boundary(%A,%A,%A,%A)." x y x1 x2 y1 y2

        insert' None t    

let a =
    QuadTree
        .create_empty(0.0f,100.0f,0.0f,100.0f).insert(25.0f,25.0f)
        .insert(75.0f,25.0f).insert(25.0f,75.0f).insert(75.0f,75.0f)
        .insert(25.0f,45.0f)
```

What the hell do I do with it now? I am not quite sure. Let me go through the book on multidimensional data structures by Samet that I found on the net.

UPDATE: A lot of stuff on [quadtrees is here](http://donar.umiacs.umd.edu/quadtree/).

...Unfortunately, the demos do seem to be badly out of date. I can't run the Java 1.4 stuff in my browser. Well, nevermind. Let me just go through the book. Hopefully, I should have a grasp of what spatial trees are all about after reading it for a bit.

UPDATE: 37/808. This stuff on 2D range trees is new to me. It seems this will be another book where everything is new to me.

https://www.quora.com/Which-is-the-best-online-course-to-learn-data-structures

http://courses.csail.mit.edu/6.851/spring12/lectures/

Actually, this book I am reading now, and the lectures above are making me realize that I really know nothing about data structures. I also know nothing about computational geometry.

Besides that I think I've been chatty enough so far. I'll keep this journal around for another week and then I'll apply the stealth principle.

In hindsight, one mistake I made over the past week is not look up stuff on the N body simulation. My focus was on tree structures instead of the problem. Had I actually cared about passing the course, this would have been a significant error.

UPDATE: 49/808. The book is quite difficult, no doubt about it. It is making me realize that maybe I should take a computational geometry course instead. Right now I am wondering what are [priority search trees](https://www.youtube.com/watch?v=KO5r0BSRmF4).

I think I'll watch the above instead of trying to churn through the textbook and then I'll look up a proper course on this. And after that, that will be it.

UPDATE: Since I learned to think about tree structures a lot better in the last week or so, I decided to take a look at [treaps](https://www.youtube.com/watch?v=6podLUYinH8) again. I can understand them much better now. In addition to that, on a hunch I checked the deletion function and it seems it is a lot simpler than for other [kinds of trees](http://opendatastructures.org/ods-java/7_2_Treap_Randomized_Binary.html). Nice.

Probabilistic algorithms (such as quicksort) and data structures are pretty great when they aren't being showed into doing learning.

http://t-t-travails.blogspot.hr/2008/07/treaps-versus-red-black-trees.html

This post above looks really interesting. It took him 80 hours to implement LLRB trees. Amazing. It took me like 1h once I internalized them. Now I could probably do it in 10m. Unless he is talking about remove as well. Ah, [he is](http://www.canonware.com/download/rb/rb_new/rb.h). He implemented the thing in pure C no less.

UPDATE: Signed up for the [Edx Computational Geometry](https://courses.edx.org/courses/course-v1:TsinghuaX+70240183x+3T2015/info) course. The instructor is speaking Chinese which is a first for me, but there are English subtitles.

In for a penny, in for a pound. I've been wondering what Delaunay Triangulation was since the time of the Discrete Optimization. Thankfully half the material here is from the Algorithms course.

I think Sedgewick's Algorithms course had the N body problem, so I've considered looking for it, but it seems the course had been taken off the old platform today. Well, nevermind.

6/24/2016:

As it turns out Computational Geometry has more to do with algorithms than geometry. The course is split into bite sized chunks but it did just take me 4h just go through the first half of the Convex Hull section. I do not think I'll be able to go through more than 2 sections a day.

Hopefully by the end of this I will have an intuitive understanding of structures such as quadtrees.

UPDATE: 12h is more than enough of this. I had hoped to have gotten a lot further than I have, but these lectures drag on and on. The lectures are nowhere as clear as Sedgewick's either.

I have no idea how Segment Intersection can be reduced to Element Uniqueness. I also did not understand the Divide And Conquer (1) in the Convex Hull module.

But at any rate, it seems this course will be necessary for my future. I know I said screw it to the Scala course, but if I can finish the CG course in time, I'll take a look at some Barnes Hut examples floating around on the net and try to follow the lead from there.

If not, I am going into GUI and computer security (ie. stealth) ironically.

6/25/2016:

Done with Geometric Intersection. Earlier today and I did a quick Google search and found this:
http://scala-blitz.github.io/home/documentation/examples//barneshut.html

I haven't checked it thoroughly yet, but it seems that page has pretty much all I need to pass the assignment. Here is a general tutorial on [Barnes Hut](http://arborjs.org/docs/barnes-hut) to complement the above. I could not find an F# implementation, but there is some stuff in C#.

That having said, there is a point in taking the long way around when training.

It is not unusual for me to take on extra training with a failure as a catalyst.

As for the Computational Geometry course, it is not in the league of Sedgewick's, but it will do nicely. The Geometric Intersection has been a lot smaller than the previous section so I have enough time in a day to pack away half of next one. I'll get to it.

...One more thing. Given current trends, if I project the least amount of time it will take to develop the tools to use RNNs effectively, I arrive at 2-3 years and probably more.

So in regards to poker, I do not expect to be doing anything too advanced until both the software and the GPUs get there. By GPUs I am not implying that they lack power. Rather, that global synchronization from the persistent RNN paper should be provided by hardware.

I once noted that tabular and feedforward methods should be enough for the lowest stakes. It seems I will be testing that then. But I believe strongly in the power of recurrent nets. They will live up to their promise in reinforcement learning. I hope with all the effort I am putting into this, I will be able to scrunge up some basic income, which would be significant for me.

It is even possible that once recurrent nets start demonstrating their true power that I might not end up sticking in poker for long.

UPDATE: ...No, forget those last two paragraphs. Even at 1% power RNNs should be better than feedforward nets. Poker bots will really need some simple memory in the form of past states to be effective. Literally any memory will be better than 0%. I was not wrong with making Spiral.

UPDATE: Let me call it a day here. I am nearly done with Triangulation. These lectures are quite boring actually.

The next two sections seem like they will be long as well.

Damn, originally I intended to dedicate the entirety of June to reinforcement learning. It seems like in the end I will spend zero.

A good idea would be once I am done with this geometry stuff is to go back to the One Poker for which I trained my tabular players and make RNNs work at any cost on them. So what if I do not feel like making GPU kernels for shuffling stuff on the GPU? Just do that on the CPU!

...Fuck Nvidia.

But it is true that I am going to have to learn GUIs and computer security to figure out how to cover my tracks from the poker rooms. And I will have to figure out how to make interfaces for RL players. All of those things are a net benefit on their own that I need to do anyway. And Computational Geometry is filling out my education. The Scala course did show this huge hole in my programming skill. It needs to be filled and so it will.

I also need to get over this addiction of using this repo journal.

Well, let me do it until the end of the course.

6/26/2016:

"Actually, strangely enough this course is giving me focus. One of my goals is to unify all the different algorithms, but doing this is making me realize that I have no chance in hell of doing it. I am just too stupid.

No, even if I set my sight lower of merely improving neural nets by combining them with other algorithms, I still have no clue. Damn.

...I have no chance at all of inventing the Metaheuristic Algorithm with just my abilities.

With Futhark I wanted to learn the optimizations it is doing in order to make use of them, but I wonder if that is the wrong track? Maybe, my own hands are not the ones that will replace all those tens of thousands of lines of smart optimizations with machine learning?

Learning stealth, learning to set up the environment and learning the basics of machine learning (relative to the realm of the Gods) might be the best I can do.

I do believe backpropagation is truly special and the way it improves is not like the other algorithms. It seems quite a bit more profound.

No, humans simply aren't destined to scratch more than the surface of it, or at least I am not."

Here is the excerpt from the other journal. That is the trouble with this - amongst others it caused me to quit programming in high school. The feeling like I am some dog learning new tricks.

It seems that broadly speaking optimization and more narrowly, machine learning are the closest ordinary humans can get to the realm of the divine.

Learning these tricks will not cut it. But on the other hand, they do come in limit amounts and learning them is a rather cheap way to boost one's programming skill, so I'll pack this course away.

Actually, this course is particularly tedious and long winded, so I can't wait to finish the lectures.

6/27/2016:

Done with Delaunay Triangulation and Point Location. I've skimmed the last part a lot admittedly. I've really spent quite a way just making my way through these lectures and I intend to finish it tomorrow so I can move on to more important things. It was a good idea to watch lectures a 2x speed just like I did yesterday, otherwise I would be spending even more. This allowed me to finish early. I'd go for even longer, but these things are difficult - my brain feels like it will leak out.

The thought that I can win with RNNs even at 1% power is giving me focus. Really, while interesting I do not really care about the Barnes-Hut simulation all that much. I've decided to compromise and just paste as much from that [Scala Blitz link](http://scala-blitz.github.io/home/documentation/examples//barneshut.html).

It is unlikely for me to be so stuck on something even though this time it was on purpose. Though I do not really doubt this particular assignment given that I have the solution right there, sometimes a part of me wonders whether I can even do it...

...Er, actually never mind. I can't really remember a period where I doubted myself. For a period of time, I was depressed over losing to my own image of normal people, and had my kindness spat back in my face, but never doubtful. This is really the one positive thing that I attained with age - firmer grounding in reality and realistic self assessments. Those things would be useless usually, but the one benefit of then is that one can walk deeper into insanity and channel them into creative endeavors.

One sharpens his mind and at that point he see that being confident or being doubtful are both worthless. The only thing one needs to do is watch and listen and focus on the work, and the rest will fall into place.

Though I was not successful at making money, this iron discipline is the one thing I attained over the past decade of my life.

6/29/2016:

Yesterday I threw in the towel on the last chapter of Computation Geometry as it was too tedious and before I went to sleep did most of the Scala assignment. Just now I finished submitting it and got a notification that I passed the course. With this I am done.

It was not that hard with the Scala Blitz example. It took me only a few hours.

Writing this journal and the Haskell one was fun. Maybe I'll do a bunch of GUI examples here later, but since the stuff I will be doing from here need to be kept under wraps, I think I'll bring it to a close here.

7/15/2016:

Here in the Fsharp folder can now be found all the examples from the first 16 chapters of the book `Applications = Markup + Code` by Charles Petzold, apart from the first two or three from chapter one. I had wanted to do part 2 as well, but I decided to put that on hold until the [FsXaml type provider](https://github.com/fsprojects/FsXaml) gets some documentation.

I've been studying WPF non-stop since my last post here as I realized that I really needed a GUI for poker game. I was going to resume work on the game, but I forgot that I actually can't accept key inputs from F# Interactive since last time I touched it and realizing that doing everything from the command line is a stupid idea motivated me to do this study right now. I'll need this knowledge once I start making RL interfaces anyway.

It is just as well that I stopped here as going through the entire book would have taken me another two weeks and believe me, I am tired of it by now. I am really aching to do some real programming.

Actually, I quite liked the book and can recommend it for the first part which does everything from just code. For part two, I suppose I could have hacked it somehow, but F# really can't compete with the Xaml to native code generation feature from the C# compiler. It also lacks partial classes, though it does have type extensions which are better.

The book can be found in the Genesis library as can most other things. It is a wonderful repository. My thanks to that guy on the ML sub for recommending it to me.

My next project will be a really simple calculator using the expression parser. I'll do this to get familiar with this aspect of parser combinators in anticipation of my future work on Futhark. I had intended to spend 1 month on RL and one month on it, but that plan is already wrecked. Last month I ended up studying Linux networking, more Haskell and computational geometry. And half of this month I spent studying graphical user interfaces.

There is no way I can possibly make time for it at this rate. So at least for the sake of future investment in the burgeoning language, until its authors either implement a Cuda backend for it or shared memory optimizations, I will just rewrite the parser to avoid that hideous memory leak bug in `ghc-mod`.

After I do that, I'll have fulfilled my responsibility for the near term and will be able to leave it out of mind for a few quarters or halves.
