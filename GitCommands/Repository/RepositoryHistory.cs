﻿using System;

namespace GitCommands.Repository
{
    public class RepositoryHistory : RepositoryCategory
    {
        public RepositoryHistory()
        {
            Description = "Recent Repositories";
        }

        public override void SetIcon()
        {
            foreach (var recentRepository in Repositories)
            {
                recentRepository.RepositoryType = RepositoryType.History;
            }
        }

        public void RemoveRecentRepository(string repo)
        {
            if (string.IsNullOrEmpty(repo))
                return;
            foreach (var recentRepository in Repositories)
            {
                if (!recentRepository.Path.Equals(repo, StringComparison.CurrentCultureIgnoreCase))
                    continue;
                Repositories.Remove(recentRepository);
                break;
            }
        }

        public void AddMostRecentRepository(string repo)
        {
            if (string.IsNullOrEmpty(repo))
                return;

            repo = repo.Trim();

            if (string.IsNullOrEmpty(repo))
                return;

            repo = repo.Replace(Settings.PathSeparatorWrong, Settings.PathSeparator);
            if (!repo.EndsWith(Settings.PathSeparator.ToString()) &&
                !repo.StartsWith("http", StringComparison.CurrentCultureIgnoreCase) &&
                !repo.StartsWith("git", StringComparison.CurrentCultureIgnoreCase) &&
                !repo.StartsWith("ssh", StringComparison.CurrentCultureIgnoreCase))
                repo += Settings.PathSeparator;

            Repository.RepositoryAnchor anchor = Repository.RepositoryAnchor.None;
            foreach (var recentRepository in Repositories)
            {
                if (!recentRepository.Path.Equals(repo, StringComparison.CurrentCultureIgnoreCase))
                    continue;
                anchor = recentRepository.Anchor;
                Repositories.Remove(recentRepository);
                break;
            }

            var repository = new Repository(repo, null, null) {
                RepositoryType = RepositoryType.History,
                Anchor = anchor
            };
            Repositories.Insert(0, repository);

            if (Repositories.Count > 30)
            {
                Repositories.RemoveAt(30);
            }
        }
    }
}