using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Goons.ScenesManagement
{
    public interface IScenesManager
    {
        Task LoadBattle();
    }

    public class ScenesManager : IScenesManager
    {
        public async Task LoadBattle()
        {
            var tcs = new TaskCompletionSource<bool>();
            var asyncOperation = SceneManager.LoadSceneAsync("Field", LoadSceneMode.Additive);
            asyncOperation.completed += r =>
            {
                tcs.SetResult(true);
            };
            await tcs.Task;
        }

        public async Task LoadMain()
        {
            var tcs = new TaskCompletionSource<bool>();
            SceneManager.UnloadSceneAsync("Field").completed += o =>
            {
                tcs.SetResult(true);
            };
            await tcs.Task;
        }
    }
}