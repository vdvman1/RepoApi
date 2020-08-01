using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using RepositoryApi.Helpers;
using System;
using System.IO;
using System.Linq;

namespace RepositoryApi
{
    public class RepoApi : IDisposable
    {
        private readonly Repository repository;

        private RepoApi(Repository repo) => repository = repo;

        public void Dispose() => ((IDisposable)repository).Dispose();

        public static RepoApi Load(string url, string folder, string branch = "master", CredentialsHandler? credentialsHandler = null)
        {
            string path = Path.Combine(Path.GetTempPath(), "repo_api", folder);
            Repository? repo = null;
            try
            {
                if (!Repository.IsValid(path))
                {
                    repo = clone();
                }
                else
                {
                    repo = new Repository(path);
                    Remote origin = repo.Network.Remotes["origin"];
                    Branch master = repo.Branches[branch];
                    if (origin.Url != url || (master != null && (!master.IsTracking || master.RemoteName != "origin")))
                    {
                        repo.Dispose();
                        repo = clone();
                    }
                    else
                    {
                        // Ensure there are no local changes
                        repo.RemoveUntrackedFiles();
                        var specs = origin.FetchRefSpecs.Select(f => f.Specification).ToList();
                        Commands.Fetch(
                            repo,
                            origin.Name,
                            specs,
                            new FetchOptions
                            {
                                TagFetchMode = TagFetchMode.All
                            },
                            logMessage: null
                        );

                        Branch tracked = repo.Branches[$"origin/{branch}"];

                        if (master == null)
                        {
                            master = repo.Branches.Update(
                                repo.CreateBranch(branch, tracked.Tip),
                                b => b.TrackedBranch = tracked.CanonicalName
                            );
                        }

                        Commands.Checkout(
                            repo,
                            master,
                            new CheckoutOptions
                            {
                                CheckoutModifiers = CheckoutModifiers.Force
                            }
                        );
                        repo.Reset(ResetMode.Hard, tracked.CanonicalName);
                    }
                }
            }
            catch
            {
                repo?.Dispose();
                throw;
            }

            return new RepoApi(repo);

            Repository clone()
            {
                FileSystemHelpers.Empty(Directory.CreateDirectory(path));
                return new Repository(Repository.Clone(
                    url,
                    path,
                    new CloneOptions
                    {
                        BranchName = branch,
                        Checkout = true,
                        FetchOptions = new FetchOptions
                        {
                            TagFetchMode = TagFetchMode.All
                        },
                        CredentialsProvider = credentialsHandler ?? delegate { return new DefaultCredentials(); }
                    }
                ));
            }
        }
    }
}
