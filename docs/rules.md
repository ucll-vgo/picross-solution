# Rules

Below are the rules which you have to follow
if you want to get a passing grade.

In the past, we (the lecturers) would
examine your code beforehand in search
for mistakes and confront you with them
during your defense. Once the defense
was over, the student had a good idea
about whether or not.

However, times have changed. Due to
the way the exams are organized,
there is very little time between the project submission deadline
and the actual defense. We will therefore
not able to take a thorough look at
your code beforehand.

A consequence of this is that the a seemingly
good defense means very little: we'll still
have to take a look afterwards, and if we find
fatal mistakes, your grade might take an unforeseen
plunge. 

It is our understanding that students
are not fond of this kind of uncertainty.
Neither are we. So, we took some countermeasures to alleviate
this current state of affairs.

First, we suggest you don't try to bluff
your way through the defense. If you think
you've made a mistake, or if you think
that something ought to be done differently
but you didn't implement it that way
for one reason or another (e.g. lack of time),
do mention this. The more you can show
you are aware of the shortcomings
of your project, the better.

Second, we will do our best to give you clear-cut
rules about what is okay and what isn't.
Design guidelines are often vague
and even contradictory at times,
so we don't want people to get
the impression that our demands
are subjective or whimsical.

## Rules

# MVVM

The M/VM/V layering must be respected at all cost.
The "knows-of" relation flows downwards in the diagram
below:

<center>

| Structure |
|:---:|
| V |
| VM |
| M |

</center>

* The M does not know about the VM or V and
must not mention any type of the layer above it.
* The VM knows the M, but does not know anything about the V.
* The V knows both the VM and M, but ideally, the V
does not speak directly to the M.

The reasoning behind this is:

* We want to be able to reuse code in other contexts. If the M
were to contain WPF types, it becomes impossible to reuse the M
on a platform for which WPF is not available, e.g., mobile devices. Similary, we want to be able to reuse the VM.

An necessary (but not sufficient) test
to check that the layering is respected,
is to verify the project references.

* The Model project should not contain a reference
to the ViewModel, View project or to WPF.
* The ViewModel can (and should) have a reference
to the model, but not to the View of WPF.
* The View can have any references you want.

References can be viewed in the Solution Explorer,
i.e., the pane which lists all your projects.
Each project is shows as a tree which contains
your files, but also has a References node under
which all references are listed.
WPF appears here as PresentationCore
and PresentationFramework.

The solution we provided normally contains
only valid references, but each year
there are students who add forbidden references.
It is possible that this happens automatically
if you try to use a type from another project.
Say you mention `Brush` in your ViewModel:
the compiler complains as that type is unavailable.
Asking Visual Studio to fix it will then add
a reference to WPF in your VM, which is definitely
not what you want. So, even though
your references were originally correct,
it is still a good idea to check.

An easy test to verify the layering is to check
the references of each the three projects in your solution.
The Model project should not contain a reference to the ViewModel
or to the View projects, etc. Everything should still compile.

The code we have provided should already be configured correctly,
i.e. it does not contain forbidden references. Each year, however,
there are students that somehow still manage to link
the projects


* No click
* No class duplication (e.g. commands)
* 