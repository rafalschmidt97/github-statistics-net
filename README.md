This repository is a part of **Allegro** recruitment process. 
It includes the api for gathering information about user repositories.
Created with .NET Core and ASP.NET Core.

CI configured with Bitbucket Pipeline (build + tests).

## Recruitment task

Create a simple REST service which will return statistics of Github user repositories. Statistics should include:

- Letters used in repository names, case insensitive (for example, ‘a’ was used 108 times, ‘b’ 30, ‘c’ 42 etc.)
- Average count of stargazers
- Average count of watchers
- Average count of forks
- Average size

The API of the service should look as follows:

GET /repositories/{owner}
```json
{
    "owner": "...",
    "letters": { "a": 3, "b": 4 }, 
    "avgStargazers": 0.0,
    "avgWatchers": 0.0,
    "avgForks": 0.0,
    "avgSize": 0.0
}
```

Non functional requirements:

- Should be able to serve 20 requests per second (concurrently; assuming we have GitHub account; simply put: application should not have obvious scaling bottlenecks)
- A set of end-to-end tests that can be run using build tool
- Good design and quality of code
- Almost ready to deploy to production (if additional work is needed, please describe it in a README file)

## Contributing guidelines

I really hope that good instructions for contributing will make the history of the app more readable. 
Take a look [here](CONTRIBUTING.md).

## Stay in touch

* Website - [rafalschmidt.com](https://rafalschmidt.com/)
* Facebook - [rafalschmidt97](https://facebook.com/rafalschmidt97/)
* Twitter - [rafalschmidt97](https://twitter.com/rafalschmidt97/)
* Instagram - [rafalschmidt97](https://instagram.com/rafalschmidt97/)
* Email - [rafalschmidt97@gmail.com](mailto:rafalschmidt97@gmail.com)
