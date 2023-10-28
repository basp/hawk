# Hawk
This is a word generator inspired by [Awkwords](#).

## Quickstart
The easiest way (for now) is to clone the repo and `cd` into the `src/Hawk` 
directory:
```
> git clone https://github.com/basp/hawk
> cd hawk/src/Hawk
> dotnet run
```

If all went well you should be presented by a basic prompt:
```
hwk>
```

From here you can execute *expressions* or *definitions* or both. Let's define
some basic consonants for our language. These are usually designated by `C`.
```
hwk> C : p/b/k/g;
     .
```

We can inspect this definition and see what it does:
```
hwk> C.
k
hwk> CC.
bg
hwk> CCC.
kkp
```

What is happening here is that `C` is replaced by a random choice of `p`, `b`, 
`k`, or `g` every time we evaluate it. During an interactive session, the
interpreter will remember any definitions you make. And if you later change `C`
to include `m` as well then it will use that in any subsequent cycles.

There's a few things to note here. First, we are creating a *definition* by
saying that `C` should equal one of `p`, `b`, `k`, or `g`. This has the format of
```
X : a/b/c ;
```

The name (or *identifier*) can only be a single character and it must be a
capital letter from the Latin alphabet. If you think this is a severe
restriction then please see the *restricted by design* section. The choice 
of which token to output will be made dynamically by **Hawk** during runtime 
and it is random. By default, each of the options specified will have equal 
chance of occurring but this can be tweaked. 

The next thing to notice is that we need to close our definition with a 
semicolon (`;`). Every **Hawk** program can have a number of definitions 
before its root (generator) expression. Those definitions could potentially 
span multiple lines for formatting or other purposes. And thus we have the 
semicolon as a straightforward way to separate definitions. Although, staying 
true to the grammar, it's probably best to think of the semicolon as a 
conclusion to a definition.

The final thing is the period at the end. This is not part of the **Hawk** 
language but it is a feature of the interpreter to make supporting multiple 
prompts more straightforward. We will see this in action for a little bit. 

> For now it is enough to know that the period is part of the interactive and
> not the **Hawk** definition language. Note that semicolons *are* definitely
> part of the language and also required in script based use.

### Baby Language
In the interactive, each *statement* will be concluded with a period (`.`).
So in a session were we specify a very basic language with `C` and `V` this
woiuld resemble the following.
```
hwk> C : o/u;
     V : k/p;
     .
C
V
```

In the above example we specify two definitions (`C` and `V`) and then tell the
interactive that we are done by specifying the period (`.`). We are then told
`C` and `V` which is a sign that the interactive has recorded our definitions.

As another example, let's define a very basic baby language. We start by 
defining consonants and vowels (`C` and `V`) respectively.
```
hwk> C: k/g/w/b/p;
     V: a/i/u;
     .
```

> Here we enter input that spans over multiple lines in the interactive,
> specifying both `C` and `V`. The session is closed with a period (`.`) which
> signals to the interactive that the input should be evaluated.

In baby language we start with a consonant first and end with a vowel. The
basic structure of a baby word is something like `CVCV` and we can try this
out in the interactive.
```
hwk> CVCV.
wiwu
hwk> CVCV.
waki
```

But it doesn't have to be that way though. Maybe we have `VCV` as well.
```
hwk> VCV.
api
hwk> [VCV]VCV.
uwiuga
```

## Groups
There are two kinds of groups:
* Static groups use brackets `[` and `]` and are *always* included.
* Dynamic groups use parenthesis (*parens*) `(` and `)` and are included only
50% of the time.

## Design Restrictions
* Only single letter identifiers for definitions.
* Identifier characters must be of Latin alphabet (i.e. `[A-Z]`).
* Composite definitions need to be singular (grouped). This means they need to 
be contained within *parens* `(` and `)` or *brackets* `[` and `]`. For example,
the `Q : [VC][VC]` will not work as expected and should be written as 
static groups `Q : [[VC][VC]]` instead. Using dynamic groups (`(` and `)` is 
also allowed).