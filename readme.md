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
