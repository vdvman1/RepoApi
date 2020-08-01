using System;
using RepositoryApi;

namespace RepoApiTest
{
    class Program
    {
        static void Main(string[] _)
        {
            using var api = RepoApi.Load("https://github.com/MinecraftPhi/mc-tools.git", "mc-tools", "dev");
        }
    }
}
