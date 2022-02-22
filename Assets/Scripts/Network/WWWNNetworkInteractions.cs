using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Goons.Network
{
    public class WWWNNetworkInteractions : INetworkInteractions
    {
        private class AuthBody
        {
            public string metamask_token;
        }

        private class AuthResult
        {
            public string status;
            public string new_user;
            public string auth_token;
        }
        private string _metamaskToken = "0x3455ABb96D9D300CD7D4C11606CE57D31da67642";
        public string MetamaskToken { get; private set; }

#if UNITY_WEBGL && !UNITY_EDITOR
        private JavascriptHook _hook;
        public WWWNNetworkInteractions(JavascriptHook hook)
        {
            _hook = hook;
        }

#endif

        public string Token { get; private set; } = null;

        public async Task<string> GetToken()
        {
            if (Token == null)
                await Authorize();

            return Token;

        }

        public async Task Authorize()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            _metamaskToken = await _hook.GetMetamaskToken();
#endif
            MetamaskToken = _metamaskToken;
            var json = JsonUtility.ToJson(new AuthBody() { metamask_token = _metamaskToken });
            var result = await SentPostRequest("https://dev-api.getagoon.com/api/v1/login", json, false);
            var authResult = JsonUtility.FromJson<AuthResult>(result);
            Token = authResult.auth_token;
        }

        public async Task<string> SentPostRequest(string url, string json, bool needAuth = true)
        {
            Debug.Log($"SentPostRequest ({url})");
            string token = null;
            if(needAuth)
                token = await GetToken();
            var response = await PostRequest(url, json, token);
            Debug.Log($"SentPostRequest responce {response}");
            return response;
        }

        public async Task<string> SentGetRequest(string url, bool needAuth = true)
        {
            var token = await GetToken();
            if (needAuth)
                return await GetRequest(url, token);
            else
                return await GetRequestNoAuth(url);
        }

        private async Task<string> PostRequest(string url, string json, string token = null)
        {
            using (var webRequest = UnityWebRequest.Post(url, ""))
            {
                webRequest.SetRequestHeader("Content-Type", "application/json");
                if (token != null)
                    webRequest.SetRequestHeader("Authorization", $"Token {token}");

                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                await webRequest.SendWebRequest();
                var responce = webRequest.downloadHandler.text;
                return responce;
            }

        }
        private async Task<string> GetRequestNoAuth(string url)
        {
            Debug.Log($"GetRequestNoAuth {url} ");
            using (var webRequest = UnityWebRequest.Get(url))
            {
                webRequest.SetRequestHeader("Content-Type", "application/json");

                await webRequest.SendWebRequest();

                Debug.Log($"GetRequestNoAuth {url} response {webRequest.downloadHandler.text}");
                return webRequest.downloadHandler.text;
            }
        }

        private async Task<string> GetRequest(string url, string token)
        {
            Debug.Log($"GetRequest {url} ");
            using (var webRequest = UnityWebRequest.Get(url))
            {
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", $"Token {token}");
                await webRequest.SendWebRequest();
                Debug.Log($"GetRequestNoAuth {url} response {webRequest.downloadHandler.text}");
                return webRequest.downloadHandler.text;
            }
        }
    }
}