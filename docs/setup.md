# Setup

## Creating Your Very Own Fork

Follow [this link](https://classroom.github.com/a/sjjhHC6i) and pick your student ID from the list. If your ID is not in the list, follow the instructions further below in the Troubleshooting section.

Clone your repo using the following command, replacing `<<URL>>` by the url of your fork
(omit the `<` and `>`).

```bash
# Clone your repo
$ git clone <<URL>> picross

# Enter the repository
$ cd picross

# Add upstream remote
$ git remote add upstream https://github.com/UCLeuvenLimburg/picross-student
```

**Do not dare downloading the code as a zip.** You will be stripped
of your very existence, thereby ending up in Finland
where you'll have to watch The Last Airbender until you've learned your lesson.

Also, **do not clone your repository into a DropBox/OneDrive/Google Drive managed directory**.
The punishment for this is even worse, but it'll take us a while to determine
how we can top The Last Airbender.

## Opening in Visual Studio

Open the solution `PiCross.sln`. In the Solution Explorer,
right click View and choose Set as StartUp Project.

## GitHub

On the [main repository's website](https://github.com/UCLeuvenLimburg/picross-student),
click the Watch button in order to receive notifications about updates.

When updates are made, you can pull them as follows (note that you should first commit/stash all your changes, otherwise git might complain):

```bash
$ git pull upstream master
```

## Troubleshooting

### **My ID Is Not On The List!**

If your ID does not appear on the GitHub Classroom page, we need to add you manually.
Send a mail to frederic.vogels@ucll.be asking for access to the VGO project. Don't forget
to mention your student ID.

In the meantime, you can start working on your project as follows:

```bash
$ git clone https://github.com/UCLeuvenLimburg/picross-student.git picross

$ cd picross

$ git remote add upstream https://github.com/UCLeuvenLimburg/picross-student
```

This clones our repository directly, which means you can't push your work.

After you got access to the GitHub classroom and have created your fork,
use the following commands to link your local repository to your fork.
Replace `<<URL>>` by the url of your fork (please do omit the `<` and `>`).

```bash
# Check your remotes. If the output does not match, stop here and ask for help.
$ git remote -v
origin  https://github.com/UCLeuvenLimburg/picross-student.git (fetch)
origin  https://github.com/UCLeuvenLimburg/picross-student.git (push)
upstream  https://github.com/UCLeuvenLimburg/picross-student (fetch)
upstream  https://github.com/UCLeuvenLimburg/picross-student (push)

$ git remote set-url origin <<URL>>

# Verification. If output does not match, ask for help.
$ git remote -v
origin  <<URL>> (fetch)
origin  <<URL>> (push)
upstream  https://github.com/UCLeuvenLimburg/picross-student (fetch)
upstream  https://github.com/UCLeuvenLimburg/picross-student (push)
```

After this step, you should be able to push. If git complains about
a missing upstream branch, use the following command:

```bash
$ git push --set-upstream origin master
```

Afterwards, `git push` should suffice.

Now, follow the rest of the instructions above.