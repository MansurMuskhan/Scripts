using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class JavascriptHook : MonoBehaviour
{
    public async Task< string> GetMetamaskToken()
    {
        if (_metamaskToken != null)
            return _metamaskToken;

        var tcs = new TaskCompletionSource<bool>();
        Action callback = () =>
        {
            tcs.SetResult(true);
        };
        TokenSetup += callback;

        await tcs.Task;
        return _metamaskToken;
    }

    public event Action TokenSetup;
    private string _metamaskToken = null;
    public void SetUserToken(string token)
    {
        _metamaskToken = token;
        TokenSetup?.Invoke();

        Debug.Log($"SetUserToken ({token})");
    }
}
