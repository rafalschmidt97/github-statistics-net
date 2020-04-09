# Contributing Guidelines

I would love for you to contribute to github-statistics and help make it even better than it is today! 
As a contributor, here are the guidelines I would like you to follow:

* [Issues and Bugs](#issue)
* [Feature Requests](#feature)
* [Submission Guidelines](#submit)
* [Development Setup](#development)
* [Coding Rules](#rules)
* [Commit Message Guidelines](#commit)

## Found a Bug?

If you find a bug in the source code, you can help us by
[submitting an issue](#submit-issue) to the [Bitbucket Repository][github]. Even better, you can
[submit a Pull Request](#submit-pr) with a fix.

## Missing a Feature?

You can _request_ a new feature by [submitting an issue](#submit-issue) to the
Repository. If you would like to _implement_ a new feature, please submit an issue with
a proposal for your work first, to be sure that I can use it.
Please consider what kind of change it is:

* For a **Major Feature**, first open an issue and outline your proposal so that it can be
  discussed. This will also allow us to better coordinate efforts, prevent duplication of work,
  and help you to craft the change so that it is successfully accepted into the project. For your 
  issue name, please prefix your proposal with `[discussion]`, for example "[discussion]: your feature idea".
* **Small Features** can be crafted and directly [submitted as a Pull Request](#submit-pr).

## Submission Guidelines

### Submitting an Issue

Before you submit an issue, please search the issue tracker, maybe an issue for your problem 
already exists and the discussion might inform you of workarounds readily available.

### Submitting a Pull Request (PR)

Before you submit your Pull Request (PR) consider the following guidelines:

1. Search pulls for an open or closed PR
   that relates to your submission. You don't want to duplicate effort.
1. Fork the repository.
1. Make your changes in a new git branch:

   ```
   git checkout -b my-fix-branch master
   ```

1. Create your patch, **including appropriate test cases**.
1. Follow [Coding Rules](#rules).
1. Run the full test suite
1. Commit your changes using a descriptive commit message that follows 
   [commit message conventions](#commit). Adherence to these conventions
   is necessary because release notes are automatically generated from these messages.

   ```
   git commit -a
   ```

   Note: the optional commit `-a` command line option will automatically "add" edited files.

1. Push your branch to Bitbucket:

   ```
   git push origin my-fix-branch
   ```

1. In Bitbucket, send a pull request to `github-statistics-api:master`.

* If someone suggest changes then:

  * Make the required updates.
  * Re-run the test suites to ensure tests are still passing.
  * Rebase your branch to upstream and force push to your Bitbucket repository (this will update your Pull Request):

```
git checkout master
git pull upstream master
git checkout your-feature-branch
git rebase upstream/master

Once you have fixed conflicts

git rebase --continue
git push -f
```

That's it! Thank you for your contribution!

#### After your pull request is merged

After your pull request is merged, you can safely delete your branch and pull the changes
from the main (upstream) repository:

* Delete the remote branch on Bitbucket either through the Bitbucket web UI or your local shell as follows:

  ```
  git push origin --delete my-fix-branch
  ```

* Check out the master branch:

  ```
  git checkout master -f
  ```

* Delete the local branch:

  ```
  git branch -D my-fix-branch
  ```

* Update your master with the latest upstream version:

  ```
  git pull upstream master
  ```

## Development Setup

You will need .NET Core version 3.1+:

1. After cloning the repo, run:

```bash
$ dotnet restore
```

2. Fill keys (marked as REPLACE_WITH_SECRET) in [secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1) or appsettings

### Commonly used scripts

```bash
# add secret for local development (useful to generate secret file as well)
$ dotnet user-secrets set "GITHUB_ACCESS_TOKEN" "x" --project src/GithubStatistics.WebAPI

# set environment as development (if necessary)
set ASPNETCORE_ENVIRONMENT=Development // cmd
$Env:ASPNETCORE_ENVIRONMENT = "Development" // powershell
export ASPNETCORE_ENVIRONMENT=Development // bash

# publish new image to docker hub
$ docker docker build -t username/github-statistics-api . && docker push username/github-statistics-api
```

## Coding Rules

To ensure consistency throughout the source code, keep these rules in mind as you are working:

* I follow [Microsoft's C# Style Guide](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/)
by using StyleCop and FxCop across the projects.

## Commit Message Guidelines

I have very precise rules over how mine git commit messages can be formatted. This leads to **more
readable messages** that are easy to follow when looking through the **project history**.

### Commit Message Format

Each commit message consists of a **header** and a **body**. The header has a special
format that includes a **type** and a **subject**:

```
<type>: <subject>
<BLANK LINE>
<body>
```

Any line of the commit message cannot be longer 100 characters! This allows the message to be easier
to read on Bitbucket as well as in various git tools.

```
docs: update list of commit types
bugfix: add missing dto field with photo
```

### Type

Must be one of the following:

* **build**: Changes that affect the build system or external dependencies
* **docs**: Documentation only changes
* **feature**: A new feature
* **bugfix**: A bug fix
* **performance**: A code change that improves performance
* **refactor**: A code change that neither fixes a bug nor adds a feature
* **style**: Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc)

### Subject

The subject contains succinct description of the change:

* use the imperative, present tense: "change" not "changed" nor "changes"
* don't capitalize first letter
* no dot (.) at the end

### Body

If necessary, the body should include the motivation for the change and contrast this with previous behavior.

[github]: https://bitbucket.org/rafalschmidt97/github-statistics-api
