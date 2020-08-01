using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryApi.Helpers
{
    public static class ActionHelpers
    {
        public static void RetryOnException(Action action, int maxTries = 10)
        {
            for (int i = 0; i < maxTries; i++)
            {
                try
                {
                    action();
                    return;
                }
                catch { }
            }
        }
    }
}
