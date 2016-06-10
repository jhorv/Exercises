# Haskell Exercises

5/21/2016:

Some of the exercises I did in Haskell based on the HackerRank NLP section. The purpose of this is so I can learn how to program Haskell effectively. [`Byte The Correct Apple`](https://www.hackerrank.com/challenges/byte-the-correct-apple) and [`Stitch The Torn Wiki`](https://www.hackerrank.com/challenges/stitch-the-torn-wiki) currently do not give that much points. I tried using cosine similarity for the first, but it did not turn out that well, I haven't gotten even a single point. For comparison a random algorithm got me 6/100. The second did a bit better at 31/100, but it should be possible to get 95% without cheating.

Everyone is cheating on HackerRank though. The leaderboards are completely meaningless.

For the aforementioned ML flavored problems, I am tempted to throw a NN at them and that is in fact what I would do in real life, but this is not something that is an option here, so I'll have to do everything by hand.

I'll take this chance to learn some Bayesian stuff while I am at this. I am really picking up Haskell so I can do some work on Futhark, but it would be good to take a chance to do something more inline with my interests. I did not do well during the PGM course, but maybe learning some probabilistic programming would be more up my alley?

5/23/2016: No, not really. Much like last time I cannot into Bayes. Given that it is one of the main branches of machine learning, I would expect that I could get more from it, but I had enough. It is frustrating. It is an aberration that I cannot figure out how to integrate with the rest of my skills, unlike with neural nets. Backpropagation when generalized to automatic differentiation, has wide ranging uses, including function approximation for reinforcement learning, but the field of probabilistic reasoning is an isolated island in the middle of the ocean.

I had enough of the NLP section as well. That thing really needs NLP libraries. The reason why I might be getting poor performance on the `Wiki` and `Byte` assignments might be simply due to not doing word stemming.

So instead, given that the Discrete Optimization course did not run this year, I'll do one of the two NP Complete challenges for starters. [`TBS Problem`](https://www.hackerrank.com/challenges/tbsp) after reading the description seems like an immensely difficult problem, but thanks to my experience from the course I can see that it is pretty similar to the vehicle routing problem.

I thought about it a little last night and a workable plan of action seems to be to incrementally add cities ranked by distance from the origin while optimizing the route using 2-opt swaps.

That later part is exactly the vehicle routing problem.

It seems like it will be a pain in the ass to do in Haskell. As of now, I do have about two weeks of experience total in the language, so let me do a little review of it. It won't be flattering. I'll be comparing it to F# throughout.

It might be a mistake to do it at all given that it will demotivate me and I need all the motivation I can muster, but I believe in myself. After another week of these exercises, I will be going to Python and as much as I'd like to, I can't do all of my programming in F#.

---

Haskell Review:

- The first point that surprised me in Haskell compared to F#, is that contrary to rumors, F# in fact has better type inference than Haskell. Now, I am not sure about working programs - I'd guess Haskell is better there, but given that 99% of the time while you are working on the program it is broken, in Haskell that breakage will wreck the type checker as well unlike in F#.

In the Atom editor for example, you can see what I mean by dragging the cursor against variables in your code while working on a new piece. Opening a new code segment will make the program forget what types the variables are. The effect of this, is that compared to F#, the user has to store more information in working memory with regards to types. It meshes poorly with the style of programming that relies on the IDE to keep track of things.

I saw a [presentation](https://secure.plaimi.net/~alexander/tmp/pres/2016-05-11-why-haskell-sucks.html) yesterday where it was written that the lack of IDE for Haskell is a problem, but now I see that the actual problem might lie a bit deeper than is commonly thought. The GHC compiler services might be lacking.

Besides the above, an obvious consequence of this is that my Haskell code has a lot more type annotations than does my F# code. In terms of type annotation verbosity, it is on par with Scala.

It is interesting what a different feel Haskell gives out when programming. You can really get a sense that it is doing at least two passes under the hood during the type checking phase. In F# and Scala those have been rolled into one step.

- I've seen it frequently cited as a flaw that F# does not have higher kinded types compared to Haskell, but on closer observation, insofar as Haskell has them, they seem to be completely wasted. For unboxed vectors, maps, set, Text and such I have to import them in a qualified manner and each time preface the functions with `B`,`M`,`S` or `T` even if they ostensibly have very similar type signatures and identical function. If a function is named `map` or `fold` then there is only that much it can do, so does it really have to be prefaced? This is how it is done in F# as well, but I would have expected more from Haskell given that it has HKT. It seems that only Scala got this one right.

- It is difficult for me to trust Haskell when it comes to performance. This is not due to Haskell being pure and lazily evaluated, but due to having a massive overuse of lists everywhere. In general, in F# I very rarely use lists due to [performance reasons](https://www.youtube.com/watch?v=YQs6IC-vgmo). Vectors actually come as a library function and Haskell cannot tell on its own when to use boxed and unboxed versions. Likewise, the users also seem to be expected to roll his own indexing functions for 2D and higher dimensional arrays - much like when doing GPU Cuda programming.

How well a language supports the plain array type natively is a good litmus test to how suitable it is for performance intensive computing. An experimental language like Idris for example, does not even have it.

I am aware that Haskell does more advanced optimizations than other languages - F# currently does not even inline the basic library functions which makes them much slower than they should be - but in F# I can just rewrite the performance sensitive parts in an imperative fashion. I suppose I could try that in Haskell using the ST monad, but Haskell is clearly not designed for imperative work - will there be any overheads to taking such an approach?

Haskell asks the user to trust the optimizing compiler to do the right thing and yet does not have a focus on the most efficient data structure, but a cache wrecking one. Had this been a focus, the issue with `String` being `[Char]` would not have existed.

Likewise, that Haskell quicksort example I often see on the net is an abomination from a performance standpoint.

---

This last point makes me wonder. Now that I've written it out, the point about arrays comes out weird. The review seems to be heading in the direction of being a general critique of the community's focus.

Ahhhh...I do not get it. Just what am I trying to accomplish? Really, the Haskell community has a lot of in common with the Bayesian community.

I guess if, I crystalize my thoughts, I would say that my ideal language would be F# with a ultra fast component for GPU programming - hence my interest in Futhark.

There is an analogy with Probabilistic Programming here.

If it was not for scaling issue with PP, it would be the superior alternative to neural nets. That is because probabilistic models have one significant advantage over neural nets, namely control. Despite that, a review of PP would have much the same tone as in the end it would be a note on my difficulties of integration. It is irritating and obnoxious, that purity in probabilistic models. I'd like to drag them through the mud.

Haskell and Probabilistic Programming are of a kind.

There is also an analogy between F# and the GPU programming piece that it is missing, and neural nets.

Neural nets have a missing piece - namely effective memory. [Recent](http://arxiv.org/abs/1605.06065) [tests](http://gitxiv.com/posts/jpfdiFPsu5c6LLsF4/associative-long-short-term-memory) demonstrate the [great viability](https://www.reddit.com/r/MachineLearning/comments/4jsh1l/what_machine_learning_techniques_are_under/) of this research approach that is linked to [metalearning](https://arxiv.org/abs/1604.00289), and that missing piece needs to be put together.

With metalearning, it might be possible to train a NN to reach the level of AlphaGo at the time of the Lee Sedol match in only a single day. It took Alpha go over a 100M games to get to its level, compared to 50,000 games that Lee Sedol played in his lifetime. A 1,000x performance improvement in algorithmic efficiency would be quite something wouldn't it?

And it is quite likely, that one would need effective metalearning accelerators for ones neural nets to crush poker at the very highest stakes as well. What a sweet, sweet dream - an ideal language and network attained through integration at different levels.

At that point, the technological Singularity would move from being an abstract possibility to an immediate threat.


...I need to start on the problem. Let me get on with it.

5/24/2016:

It turns out that on Futhark `ghc-mod` such up more memory than my computer can muster. When I try to load Futhark, it takes up 5Gb and then runs out of memory. This caused me quite a bit of frustration. I now what people mean when they say that Haskell's tooling is not as good as F#'s.

At any rate, I am done with [2-opt](https://en.wikipedia.org/wiki/2-opt) for the TBS problem. If I give the output from the problem example, it will improve the solution from 78 to about 83. Near the end, I used unfold to great effect for the first time. I also figured out how to use Debug.Trace as printf replacement. Excellent.

The next comes figuring out how to do randomness. I need to do that to make the algorithm complete. Right now it is only a basic greedy optimizer. Once I add a stochastic component such as iterated local search, it will become something great. This would be trivial in F#, but currently I do not really understand how to deal with mutable state in Haskell even though I've done quite a bit of reading.

I'll deal with that tomorrow.

5/25/2016:

Yeah, this part will be difficult. I tried looking for Haskell imperative style tutorials, but most of those are about how to stop programming in imperative style and do functional. But for things like keeping track of time and randomness, state would really be good here.

Adding the stochastic optimization elements to the TBS program would be trivial in an impure language, but here I am going to have to do some extra effort. I think I will pick some really easy HackerRank assignments and do them in an imperative fashion in Haskell.

UPDATE: Oh, lol. “People who solved Time Conversion attempted this next: TBS Problem.”

I think that one would be quite a leap after doing time conversion, holy crap. I think, I'll continue with the aforementioned problem. I think I have a decent grasp of how to do stateful computation using IO and ST monads now. One thing left to figure out is how to use mutable vector types and adapt the shuffle algorithm for it.

UPDATE: I decided to go for random restarts instead of ILS. At any rate, it seems there is something wrong with the cost function. I'll double check it tomorrow. Except for that, I got all the machinery in place to beat this thing. Interesting that nobody got more than 22.63 points on this problem.

5/26/2016:

I checked the cost function in F# and it is close. Well, it is still wrong, but that does not explain why I get runtime errors on test cases #3 and #4, nor why does it timeout on #2 even when I restrict it to only a single iteration.

But generally, I've realized now that my approach to this problem has been completely wrong. Instead of doing the cost function in reverse, it would have been much better to do two passes.

On the face of it, that would be more inefficient, but what would that allow me it to take the max over all the subsequences easily. It would have been exponentially better than what I have now. It would also allow me to add stochasticity easily by allowing the swap if the swap does not change the cost.

Also, one thing that I should have done at the start is account for the fact that HackerRank is an online judge and I have strict limits there. So the algorithm has to do well on a variety of test cases. I should not have used the N^2 2-opt, but made stochastic swaps instead.

The reason why I have not done so is because I started out this problem not knowing how to handle mutable state and deferred learning it until I got into the problem.

Now that I have tried it, I can see that a pure language like Haskell is particularly poorly suited for stochastic optimization. I am going to call bullshit on it being the [best imperative language](http://stackoverflow.com/questions/6622524/why-is-haskell-sometimes-referred-to-as-best-imperative-language). It would be a contender for the best functional language, if the [`ghc-mod` issue](https://github.com/DanielG/ghc-mod/issues/797) can be resolved, but imperative - nah.

So now that, I got some exp, I will actually give up on the TBS problem. I am tempted to do it in F# since I like discrete optimization, but my focus now is to upgrade my Haskell skills to work on a compiler. Thankfully, that task can be done in a purely functional manner.

In general, the test cases for the TBS problem cannot be paid for, and on the leaderboards, the highest score is 22.63 out of 100. That just tells you how hard the thing is.

From here, I'll pick problems that do not require specialized libraries nor handling of mutable state. Let's see...

UPDATE: I did the first sorting the first [graph theory](https://www.hackerrank.com/challenges/bfsshortreach) problem. Doing breadth first search in a functional manner for that later one pretty much killed me. Hybrid function/imperative style is definitely not the same as programming in a purely functional manner. Before I started this, I genuinely though I was good at functional programming, but I think now it turns out that I am just using it where it is the most convenient. And there are places where it is indeed much better than just imperative programming.

These past two weeks of programming with Haskell have been a wild rollercoaster. Five more days left.

I think my problem is that I am approaching Haskell with restraints on. If I was doing this in F#, I would just do whatever is the most effective in terms of programming effort, safety and efficiency tradeoffs, but here I seem to be solely focusing on the safety term and greatly paying for it in terms of programming effort.

For one-man projects, the only kind of projects I've done so far, I think the hybrid style is definitely the strongest.

There might be benefits to purity on larger team projects. With my own code, I pretty much know all the mutable state and it is not difficult to keep track of in F# anyway, but in a large project this might be more of an issue.

Still, I am not convinced that Haskell style monads are the way to go.

Dlang for example has the [`pure` keyword](http://klickverbot.at/blog/2012/05/purity-in-d/). I did not do any programming in D to speak off, but having something like that would be nice in F#. [The language](https://fslang.uservoice.com/forums/245727-f-language/suggestions/5670335-pure-functions-pure-keyword) suggestion got nixed by Don Syme though.

5/27/2016:

The next problem up is [Fibonacci Modified](https://www.hackerrank.com/challenges/fibonacci-modified). Once could do it using a fold or a loop in an imperative language, but as it is an intro problem to the dynamic programming section, the way to solve it that would set up the field for the later problems would be to use memoization.

In F# this would be easy - I could just use a Dictionary to cache the function call values and using first class functions hide all of that from the rest of the code. An incredibly elegant and efficient solution.

The easy way of doing it in Haskell would be using infinite lists which would have linear access times. A better way would be to [use trees](http://stackoverflow.com/questions/3208258/memoization-in-haskell) or perhaps [tries](http://conal.net/blog/posts/elegant-memoization-with-functional-memo-tries) as suggested [here](http://stackoverflow.com/questions/22790284/translating-imperative-memoization-code-to-haskell?rq=1).

Unfortunately, memoTries are not a part of the standard library. And looking at the above links, one might get a sense of why it took me 8h instead 30m to do BFS yesterday. Today will be much the same it seems.

Learning Haskell gives me such complicated feelings. Back in March while I was on working on the GVGAI-Fsharp library, it was time to put pathfinding into it for the Pacman game and I ended up spending a good three weeks studying. At the end of that I ended up internalizing a great deal of knowledge on searches and as I side effect, I completely internalized dynamic programming during that time. Given its link to optimization, it was a subject that greatly interested me for a long time, so I considered it a great achievement.

I do understand dynamic programming now.

During my programming journey I also took great care to understand what makes code performant and how to maximize both that along with my own programming effort. Making code fast generally boils down to using the right algorithm for the job and - optimizing memory access patterns. That last one means not using cache wrecking structures like lists and trees, which Haskell likes to use everywhere. I read in the Real World Haskell book that Map structures based on trees are competitive with hash based approaches, but that is a bold faced lie. O (log n) is really not O(1), not in the real world.

I hate going against my own best practices that I've so painstakingly built up. By trees instead of imperative structures, not only will I be introducing complexities into my code and therefore unsafety, but also giving up power as well. Programming is not math.

Haskell is a language most suited for writing compilers, which is what I am learning it for.

In the real world is there a feeling that better describes being stuck in a local minima than hatred of giving up power?

UPDATE: I ended up adapting [this example](http://jelv.is/blog/Lazy-Dynamic-Programming/) to use the `Data.Vector`. Actually, the end result is quite satisfactory. Given that I've only seen list and tree examples, until I've stumbled on the above, I had no idea this was even possible.

While unintuitive, I can't say that the final result is not elegant.

Hmmmm...it might be possible to make the same thing work in F# as well by populating an array with lazy function calls, though that would be slower than storing values inside a dictionary. ~~They would get evaluated only once, the same as in Haskell due to being lazy.~~ **(Edit: Strike that. Laziness has nothing to do with it.)**

UPDATE: As there is still a third of the day left, at random I picked a [different problem](https://www.hackerrank.com/challenges/ncr-table) and it turns out that it is quite a good fit for dynamic programming.

Pascal's triangle has a recursive definition of:
0 C 0 = 1
n C k = (n-1) C k + (n-1) C (k-1)

In fact, calculating the above recursively via dynamic programming is actually the correct way to go as taking the factorial of 1000 would require 10e^300 memory.

So now I know how to do dynamic programming efficiently with arrays in Haskell, but I am not sure to make the program return the calculated array instead of the end result which is what I would need to print all the results in a row.

...Actually, just have the topmost function return the array itself. Easy as pie.

UPDATE: Only took me an hour and a half. Done with the nCr table problem using dynamic programming. It was easy enough to adapt the fibonacci assignment code for this.

I'll try this [one next](https://www.hackerrank.com/challenges/separate-the-chocolate). I picked that one purely for points. I haven't figured out the way to solve it yet, but given the complex constraints that it has the problem reminds me of the constraint satisfaction problems in the Modeling Discrete Optimization course that I used MiniZinc to solve.

Doing constraint satisfaction by hand is not something I know how to do, but it should be interesting to research. Hopefully I will be able to beat this problem if I give it a day or two.

UPDATE: I thought about it for a bit. That 250 point problem is really way beyond me at the moment. I can hardly even place it into the dynamic programming framework, but I suppose I could spend some time doing research on CP.

If it asked me to do a single optimal solution I could deal with it somehow using local search methods, but these geometric constraints combined with exhaustive search requirement are quite something. Even just the subgoals for this would be significant problems on their own.

With the `Seperate the chocolate` problem as a goal, I'll spend the next few days putting the pieces together. It is more fun to do work when I have something unreachable to make the small gains worthwhile pursuing.

UPDATE: I found [two](http://kti.mff.cuni.cz/~bartak/downloads/CPschool05notes.pdf) [papers](http://www.lirmm.fr/~bessiere/stock/TR06020.pdf) on CP and a [bigass book.](http://cswww.essex.ac.uk/CSP/papers/CP_Handbook-20060315-final.pdf) The first paper in particular seems quite readable. It might be worth implementing techniques inside it as I work on some simpler problems.

This is killing two birds with one stone. Acquiring new techniques while learning a new language. Unlike probabilistic programming, HackerRank and similar sites are the ideal platform for this.

I'll do the above in parallel with going through the dynamic programming section.

5/28/2016:

Spent the last 3.5 hours just thinking about the Choco problem. I also went through the [first paper](http://kti.mff.cuni.cz/~bartak/downloads/CPschool05notes.pdf) on Constraint Propagation. I actually do not think this problem has anything to do with CP. It really is a DP problem that tests the user's skill at reasoning with compressed graph representations.

I figured out how to solve it on a small scale using search techniques, but when I started thinking how I could combine BFS with compression (in other words DP) I just arrived back at the original representation which made me wonder whether it is possible to solve it with DP after all. It should be possible given that it is in the DP section and all.

My best idea currently-

Suppose I had a sequence of graphs like this:

```
Q-W
E-R
A-A
A-B
B-A
B-B
```

I could compress it like this.
```
Q-W
E-R
{A-B}-{A-B}
```

When adding a new node, I just unpack the above, test all the constraints and the compress it again. One positive aspect to this would be that it would be easy to compress the nodes using a sort. Though, as I noted the above makes me wonder whether the it would be possible to do inference directly on the compressed graph.

I definitely can't figure it out just like this. I would have to solve a small scale problem using BFS and then look for patterns.

For the time being, I'll do some of the easier problems, starting with [Maximum Subarray](https://www.hackerrank.com/challenges/maxsubarray).

UPDATE: That one was simple enough. I did not even have to formalize the problem as a DP problem anyway. Now, [Coin Change](https://www.hackerrank.com/challenges/coin-change?h_r=next-challenge&h_v=legacy) is a true DP problem, similar to the knapsack one.

UPDATE: Done with Coin Exchange. It took me 3h and 2.5h of those hours was figuring out how to emulate a double loop in Haskell. My solution was pretty bad - list comprehensions.

Though just as I finished that last sentence, I thought of how I would do this in Futhark and realized a much better solution that could be realized using nested maps.

Ah, damn. I am distracted for some reason. I completely forgot that I already have aspects of the functional array style in my arsenal. It also now occurs to me now why the `accum` function exists in `Data.Vector`.

I might be overwhelmed by Haskell's size...

Writing good code is partly a mindset for me.

For the [Candies](https://www.hackerrank.com/challenges/candies?h_r=next-challenge&h_v=legacy) I'll see whether I can realize the lessons that I realized that now. Now that I know how to do DP in a purely functional style, I'll try avoiding lists and maps do the next problem using vector types.

UPDATE: I am nearly done with Candies, but might have coded myself into a hole with that one due to its laziness. I won't be able to finish it today at rate. I might try it in F# tomorrow if I do not get an [answer on SO](http://stackoverflow.com/questions/37501967/how-to-make-fromlist-lazy-in-this-dynamic-programming-example) just to make sure the algorithm works correctly. Right now I am getting runtime errors, probably due to running out memory.

5/29/2016:

The answer I got is good, but it obligates me to study the [continuation passing style](https://en.wikibooks.org/wiki/Haskell/Continuation_passing_style) for a bit, until I can fully understand the code written in the answer. Once I do, I will replicate it in F# and submit that instead. I have only cursory understanding of CPS, just enough to recognize it, but not quite enough to use it. I never expected to run into this again. As expected from Haskell, I guess.

UPDATE: chainCPS has a [confusing type signature.](http://stackoverflow.com/questions/37508652/what-is-the-type-of-the-variables-in-chaincps) Once I figure out what these continuation monads do, I will finally be able to implement the GOTO statement in Haskell.

5/30/2016:

Yesterday I did not write much in the way of code, but I put in a ridiculous effort into figuring out what the [solution given to me](http://stackoverflow.com/questions/37501967/how-to-make-fromlist-lazy-in-this-dynamic-programming-example/) did. My analysis is really basic lambda calculus. I watched the lectures on it while doing the parser for GVGAI, but forgot the lessons of it until now. Also, I did some reading and watching on continuation passing style, so I think I understand that now a bit better as well. As a side note, the aforementioned lectures came in handy a few months ago once when I had to use a Y combinator for a parser in F# as well.

Haskell is such a mind bender. I got two more days of this left. After that I think I will have a change of pace.

Regarding the Candies, the difficult part of it was figuring out the that two passes are much better than one. I missed that one as I was too focused on trying to figure out how to make the DP approach work. Just a few days ago I realized the same lesson for the TBS problem, but did not generalize it for this problem. That was foolish of me.

For the [Stock Maximize](https://www.hackerrank.com/challenges/stockmax?h_r=next-challenge&h_v=legacy) problem, that one strikes me like it could be reduced to Subset Sum.

...Er, no. Simple top down DP would the best for this problem. Like for the Candies problem, there are probably optimizations for this one...

...That I am going to have to find because Haskell cannot do proper top down DP due to lack of mutability. Shit.

UPDATE: Actually, for this problem bottom up is no problem. I'll be able to do it using the Vector as well.

UPDATE: I think I have the algorithm down, but my solution using Vector is quite slow and runs out of memory. Not at all what I expected. Yeah, I am really having difficulty understanding how to write efficient code in Haskell.

At this point, I should look into profiling Haskell programs.

UPDATE: Nah, the N^2 / 2 dynamic programming algorithm that I am using for this problem is not cutting it. Thankfully, I figured out how to do it in N time.

UPDATE: Done. The solution is similar to the last time, in that I need a double pass.

For every stock I just need to sell it on its high, and that high can easily be gotten in linear time by doing a scanr' beforehand.

I'll do [Grid Walking](https://www.hackerrank.com/challenges/grid-walking?h_r=next-challenge&h_v=legacy) next. Actually, it bothers me how slow even the linear algorithm is. I am tempted to rewrite this in F# just to figure out what is wrong.

UPDATE: [Strings are](http://stackoverflow.com/questions/37526740/why-is-the-f-version-of-this-program-6x-faster-than-the-haskell-one) what is wrong it seems. I should have looked into `Bytestring` functions instead of relying `Data.Text`.

I'll try [Grid Walking](https://www.hackerrank.com/challenges/grid-walking?h_r=next-challenge&h_v=legacy) next.

UPDATE: At first I thought this problem might be too difficult for me, but then my brain did come through in the end. I do not think it has been doing a particularly stellar job in the past few weeks, but whoever is playing me finally rolled well.

Let me explain a rough outline of how to do it.

Assuming you know the DP programming basics by now, you could probably do a simpler version of the Grid Walking problem, that with one dimension.

Assuming the dimension X is from [0..4] and you start at 2, you iterate something like this. Imagine you are doing breadth first search.

```
 0 1 2 3 4
[0,1,0,1,0]
```

This represents starting at position 2 and then making 1 move `LEFT` and 1 move `RIGHT`.

Then you do another sweep of BFS.

```
 0 1 2 3 4
[1,1,2,1,1]
```

The 2 in the middle is because the two positions merge - this merging of invariant representations is the heart of dynamic programming. But rather than do BFS, on each iteration you do an (outplace) update by adding the values at x to x-1 and x+1. And the **sum** of that array on each update represents the total number of moves possible per step.

You would want to store the sum for each move in a vector.

Now that takes care of the problem for a single dimension, but Grid Walking is for multiple dimensions. For two dimensions for example, to get the total number of moves for step two for example, you would need to add T(0,2)+T(1,1)+T(2,0).

It would be difficult to do that thing for 10 dimensions, because you would in essence have a 10 fold iteration. Even if the total number steps is only 30, 30^10 would be quite a lot to iterate over.

The solution is to apply dynamic programming techniques to this particular subproblem.

All one needs to do is simply flatten the two `x`, `y` dimensions to a single dimension `q`. Then you flatten `q` and `z` to `w` and so on. All those recursive applications of dynamic programming allow one to escape the exponential naive scaling and solve the problem in at most N^2 time.

5/31/2016:

The last day of my Haskell practice marathon is here. And to top it off I figured out how to deal with the Choco problem. It came to me last night while I was grinding away at it fruitlessly, trashing it back for in my mind, but perhaps spurned by the lessons of past problem, this time I managed to channel it successfully.

Here is the trick and it build on my compressed graph representation idea. I'll demonstrate it on the 2x2 puzzle to start with.

```
UU
UU
```

What I realized last night is that is possible to eliminate a waste swathe of the search space, by expanding it like so.

```
#1
DD
DU
```
```
#2 (!!!)
TD
DU
```
```
#3
DT
DU
```
```
#4
TT
DU
```
```
#5
DD
TU
```
```
#6
TD
TU
```
```
#7 (!!!)
DT
TU
```
```
#8
TT
TU
```

Now, based on the problem constraints, the `2` and `7` nodes above will always be invalid and can be removed from the expanded pool. So with that one move you have essentially removed 2/8 of all the search space. In 2x2 puzzle that is insignificant, but on the 8x8 puzzle that would be 1/4th of 2^64.

After that you just iterate the process above, always expanding the nodes in the way that would cause the most failure. With that it might be doable to push back against the exponential nature of the problem.

The key is to realize that expanding in groups of at least 3 is necessary. If one does the expansion one by one, the with DFS one will get stuck iterating over the entire search space and BFS will similarly run out of memory because the search space will not get pruned properly.

Now that I've written the above, it amazing that I did not figure this earlier. I knew that CP solvers had the fail first strategy, but I could not generalize it to this problem at all. In fact the way I arrived at the above had nothing to do with that at all. It was just a matter of giving up on the problem in such a way that different parts of my brain could work on it.

Well, that still leaves the tough job of figuring out how to implement the fail-first strategy effectively, but the idea does give a basis to work from.

For today, my task will be to implement the [Grid Walking](https://www.hackerrank.com/challenges/grid-walking) idea. Also, [the SO post](http://stackoverflow.com/questions/37526740/why-is-the-f-version-of-this-program-6x-faster-than-the-haskell-one) for my previous problem really exploded and it singlehandedly tripled my point total on that site. The SO site itself gave me 200 points gratis. **Edit: It seems it got linked on the [Haskell subreddit.](https://www.reddit.com/r/haskell/comments/4lpoej/why_is_the_f_version_of_this_program_6x_faster/)**

So the lesson from the above is, given the title I picked - if one wants to make points, start a language war. I'll keep that lesson close to my heart as I go into the future.

6/1/2016:

A whole lot of work got dumped on me yesterday, so I'll have to put off doing the Grid Walking problem for a few days.

Before I got interrupted, I was having trouble getting my ideas to work. The problem is not in the code, but I have a logic error somewhere in my ideas. I've thought a bit and as a last resort, I did figure out one certain source of invariance - the dimensions are symmetric.

What that means is that in a single dimension with six members, `[1,2,3,4,5,6]` the items can be folded in half like a piece of paper. What that would do is transform the problem in an equivalent one, but one that requires half as much memory and computation for each dimensions.

What that means more specifically, is that `[1,2,3,4,5,6]` would become `[1,2,3]` and the `1`,`2`,`3` nodes would get merged with `6`,`5`,`4` nodes respectively.

And in the newly transformed problem the `3` node, instead of dropping the moves if they attempt to go past the edge, it would wrap them around to node `2`.

For 10 dimensional problems, this would cut their total size from x^10 to (x/2)^10, a 2^10 reduction in size. Not bad.

I am decently sure that this would work, but I do not really understand why I cannot cleanly separate all of the dimensions yet which would allow me to achieve 10*x. I am still missing some pieces. I'll get back to it I when can make time. I'll leave that one day of Haskell in reserve until then.

6/7/2016:

Done familiarizing myself with the basics of networking, so now I have time to resume and finish this. I've done the first naive version of this problem in Haskell which passes only 4 out of 12 test cases and will move to do the rest tomorrow. There is the more advanced version of the algorithm that I have to do, and a bunch of optimizations in addition to that.

I am glad to be back in the programming game. Studying the basics of Linux networking and will no doubt pay off, but I am enjoying this much more, no doubt about it. I've thought so much about the Grid Walking problem that I had to force myself to start doing it as I did not feel like it, but as usual, once I got into it, things started going smoothly for me.

This is the final problem I will do here, before embarking to do reinforcement learning with the help of Theano in Python. I'd far rather be doing it in F#, but as long the programming languages are in this state, without native support for the GPU, I do not have a choice but to go for a big library if I want performance.

I haven't given up on Spiral though. Eventually, I will exhaust the possibilities with regular architectures and will move on to exploring metalearning using nested differentiation. Before that, I want to roll up my sleeve and carve out a niche with what exists in the present.

I am really looking forward to that. I already tried regular RL on a toy poker game and it was absolutely beautiful.

6/8/2016:

My idea to cut the dimensions in half will really not be enough. It seems the harder problems are really hard. There probably exists that linear time algorithm. I also did not see that the size of the individual dimension can go up to 100. I misread and thought it was 10.

At any rate, I am finally done with that side job for good this time and will have time to put into this. Also, I am done with Haskell. For this problem I'll move to F# for a bit to make my life easier and I'll do the complete solution in Haskell assuming I can figure out how to go up a level in DP.

I deserve a break from the past week at any rate. Let me see what I can do.

UPDATE:

```
let t = dp2D ar 3

// Proof that sum (dp1D (sumrow t) 1) + sum (dp1D (sumcol t) 1) = sumall (dp2D t 1)
let t2 =
    let sum = Array.sum
    let r1 = sum (dp1D (sumrow t) 1) + sum (dp1D (sumcol t) 1)
    let r2 = sumall (dp2D t 1)
    r1 = r2
```
I've come thus far. dp2D is the 2D function for slowing the 2D problem while dp1D solves for the 1D problem. The above is tantalizing proof that the two are related, but I can't seem to move from here. The sums match up for a single step and then everything goes to hell.

...At any rate, I am not thinking about this correctly. Like with all problems, up to now I haven't peeked at the editorial or looked at the other solutions, but maybe I'll have to study some math?

It strikes me that what I get with those 1D arrays is a bit like a linear system of equations...

No, I am at my end with this. It is true that I only got 2-3 hours today, but I do not think I would have solved it even with 10. I'll give it one more day.

6/9/2016:

I am up and I finally have a full day to do programming for the first time in 9 days.

I am going to throw in the towel on doing Grid Walking on my own - it is too difficult. I cannot figure out how to split up the dimension no matter how long I think. To solve this, I probably need something meteoric at this point. I doubt it will turn out that my idea to cut the dimensions in half was right and the reason it failed was because Haskell spews memory all over the place.

At any rate, let me take a look at the forums. I'd rather avoid looking at the editorial.

UPDATE: http://stackoverflow.com/questions/33749876/how-to-apply-recursion

It seems I was close, but not enough. I did not see the binomial calculation at all. Damn. Let me see if I can figure out the principle behind this.

mcervera: "I will try to help by means of an example that shows what the binomial coefficient C(n,k) is exactly counting in this problem.

Let's assume that we have already calculated the matrix paths(d, k), which stores the number of possible paths for each dimension "d" (individually) and for each number of steps "k" (from 0 to M).

Now we want, for example, to integrate the paths of dimensions 1 and 2. If the total number of steps is 3 (M = 3), this means that we have to consider four cases:
1) paths(1,0) and paths(2,3) --> 0 steps are performed in dimension 1, and 3 steps are performed in dimension 2.
2) paths(1,1) and paths(2,2) --> 1 step is performed in dimension 1, and 2 steps are performed in dimension 2.
3) paths(1,2) and paths(2,1) --> 2 steps are performed in dimension 1, and 1 step is performed in dimension 2.
4) paths(1,3) and paths(2,0) --> 3 steps are performed in dimension 1, and 0 steps are performed in dimension 2.

For example, in case 2), we obtain three possible sequences: {d1,d2,d2}, {d2,d1,d2}, and {d2,d2,d1}. This means that we take one step in dimension 1 and two steps in dimension 2, in any order. Here is where the binomial coefficient C(n,k) comes into the picture, since it counts "in how many ways we can select k elements out of n elements". In this example, we need C(3,1) = 3.
In general, the number of paths considering dimension 1 and dimension 2 will be the sum from i=0 to i=M of C(M,i) * paths(1,i) * paths(2,M-i).

I hope this helps."

Yeah, the above helps a lot. Agh, I can't believe I missed taking into account all the possible combinations of the two paths. Shit.

This is quite abstract, and I assumed that path(1,i) * path(2,M-i) should take all the combinations in the account naturally - multiplication generally does that sort of thing. I've painstakingly thought of all sorts of representations, such as using the diagonal and even solving linear systems, but to use C(n,k) never even occurred to me.

I guess if I skipped this problem and wandered into a different problem that made me calculate all the possible paths in a standard 1,000,000x1,000,000 grid where the cost of each move is 1, it would have occurred to me how to solve this using the C(n,k) and I would have been able to go back to this problem.

Out of all the sciences, combinatorics never fails to impress me when it comes to practical applications for optimization problems.

UPDATE: Finally got the problem done in F#. 55/55. What a hard problem.

And I am done with the Haskell adventure. It was nice. I learned quite a few things.

Let me make some addendums to Haskell review from earlier, the good stuff:

- The practice I got from this, made me see function composition in a whole new light and what I taken from this that I will use in F# from here will be how to generate arrays immutably. Also, thanks to the practice I got, I completely internalized the unfold function.

- The lambda-stacking fold technique by Behzad Nouri particularly caught my eye. At first I thought doing it that way is crazy, but now I see it is a particularly beautiful way of doing tail recursive functions. It is much safer than the usual thing and it even terminates with a effect. It cannot be done in F# without gross cost in efficiency.

- Despite my misgiving about performance, Haskell's Vector class is quite nice. Too bad it lacks a 2D variant. When it comes to generating immutable Vectors, it made me see the generate (analogous to F#'s init) in a whole new light.

The bad:

- Haskell's code looks very pretty once everything falls into place and you reduce everything to folds, maps and filters and generates. When you have trouble doing that, then Haskell gets annoying to work with. When I started this mid-May, I thought I would be able to got to about 80% effectiveness compared to F# after two and a half weeks from about 20%. Right now I think I am only at 50%, and I do not think I will be able to go above this without significant effort.

- Haskell's learning cure is much steeper than I thought it would be. If it was this hard for me even with significant prior experience in functional programming, I can scarcely imagine how hard it would be for a beginner with no prior knowledge. When I first started F#, I do not remember having this much difficulty with it.

- Haskell's tooling is horrendous compared to F#. The problem is that `ghc-mod`'s feedback is much worse than Intellisense's and when I attempted to open Futhark, I ran into a space leak bug that made it consume over 5Gb of memory which rendered it unusable on my machine. What Haskell's GHC should do is re-implement itself as an API service like C# and F# as per this talk by [Anders Hejlsberg](https://www.reddit.com/r/haskell/comments/4jhhrj/anders_hejlsberg_on_modern_compiler_construction/). As it is now, `ghc-mod` has the unenviable task of catching up to every new development in the main compiler.

- Haskell can do functional programming very well, and can do imperative programming inside the IO and ST monads, but surprisingly, coming from F#, I've had significant difficulty adapting the hybrid imperative/functional style that I've developed in Haskell. I have found it far harder to enter the full flow state like I would do in F#. Also, I would get stuck thinking about things for hours that would take me 10m in F#.

Haskell is surprisingly bad at merging the imperative and functional style, though I might change my mind with more experience.

The final conclusion:

- Considered the best programming language by its proponents, I would rather say that Haskell is the best programming language laboratory along with Scala. It is certainly wildly innovative, but even if I mastered it fully, I cannot imagine myself being as productive in it as in F#. In general, I would place it on par with Python, but with a caveat that the languages were designed for very different things. Haskell's field of expertise seem to be compilers, which is why I picked it up in the first place, while Python is a glue language for various libraries written in much faster languages.

---

That is about it for the review. No doubt I'll be doing more programming in the future in Haskell, but for now it is time I try out Theano with Python for reinforcement learning. Tensorflow should come to Windows eventually as well, and I have no intention of doing my programming in Python forever. .NET should get a solid high level GPU language eventually, and I'll give my small bit towards that goal with Futhark. The recent open sourcing of everything .NET by Microsoft is an enormous stroke of good fortune for the programming community. No company in the world comes even close to MS with regards to programming language research.

Currently, the lack of a high level GPU language is the main bottleneck in machine learning. Maybe not for running experiments on static datasets, but for reinforcement learning and making my own ML library it certainly is.

As an addendum, before I leave this repo, I took a look at the next puzzle - [Red John Is Back](https://www.hackerrank.com/challenges/red-john-is-back). Compared to the last assignment is seems straightforward and it can be cleanly separated into two parts - the first is to calculate the total number of brick configurations and the latter part is to calculate the number of primes up to x. This last part can be done using the sieve of Eristhathones, so I won't get into it here, but the first part while it seems difficult is a variant of the string length problem.

Basically for an give point x, you can either place a vertical brick or 4 horizontal bricks that that up [x,x+3] space. So from the last position, the recursive definition would be as follows:

f(x) = f(x-1) + 4*f(x-3)

f when x < 0 = 0

f 0 = 1

Something like that. Just memoize f(x) and then you have the solution. It is certainly not as complex as the previous problem.

6/10/2016:

I did some thinking last night. There is really a wide gap between my intention and how much I actually feel like programming in Python. I already have a 2k library for ML that is okay and 1k LOC poker game with basic tabular RL players.

In regards to the later, do I really feel like rewriting it all in Python. I wrote yesterday that Haskell is on par with Python, but now I really regret writing that. No way, seriously, there is no way I feel like doing any serious RL work in Python.

In the first place, what was my plan for RL in the first place? I just decided that I need Python + Theano or Tensorflow because I wanted to offload the work on dealing with recurrent nets.

But it is not like I can't do it with Spiral, it will just be slower.

Indeed, having it all made for me would be the right thing to have, but once I start expanding the games the programming burden on me will only increase. 1k is already a bit much to do in Python already and I do not want the burden of dealing with an expanding code base in a dynamic language.

In the first place, I haven't even tried the online poker tables with tabular RL methods, maybe with properly designed coarse features, the lowest stakes would be beatable. Futhermore, if one looks at the Atari games which Deepmind made its first claim to fame, they used feedforward convolutional nets? AlphaGo? The same kind of nets.

Feedforward nets in Spiral are just as good as anywhere else.

In a few years, the F# community will push it as the [data science language](https://github.com/Microsoft/visualfsharp/issues/1123) of choice on the .NET platform, and then what would I do with a large codebase in Python? Scrap it again?

---

So in a nutshell, let me scrap the Python plan and go with what I have. The recurrent net situation will resolve itself on its own, but while seemingly trivial, things like interfacing my bots with online sites will be a lot to keep me busy. Thousands of lines of code will have to be written just for that, and I do not want to rely on a dynamic language for that.

I've looked at Python's features and the functional programming story is dismal.

Haskell on the other hand really does offer a window into the world beyond - I am sure I can get to 70-80% even with its poor tooling, but what I will gain from that is something that I will be able to take back to F#.

In the end it comes to down to style. The code one writes should be beyond all beautiful and efficient. The skills I've painstakingly developed in F# and then augmented in the last few weeks in Haskell are not something I want to let go. It was different than with Haskell - Python won't make me a better programmer. I do not want to change my thinking patterns in ways that are a regression. I certainly don't and I won't.

Coding style is of utmost importance to me. Style is power.

---

So with that, I'll do the wrong thing in the short term and just forget about recurrent nets for the time being. And with that I'll open up a door to more time. The time I would spend rewriting stuff in Python, I will spend to boost my skills even further. And in actuality, I will be able to start RL a lot sooner because of that.

That's beauty.

For today, I think I'll actually do one more problem in Haskell. I think I am finally getting the hang of how to do pure functional programming. I think I have a good idea of how to do everything I could with imperative programming in Haskell now including loops that look backward. I got confused by those DP examples - I did not realize that they are exactly equivalent to loops in an imperative language. In fact, it is amazing that I did not see the connection earlier.

I'll also watch the lectures for this [Parallel Programming](https://www.coursera.org/learn/parprog1/) course while I am at it.

Really, I need to take it easy.
