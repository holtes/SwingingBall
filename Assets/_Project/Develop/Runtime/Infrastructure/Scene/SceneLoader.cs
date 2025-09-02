using Infrastructure.Fade.Controllers;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace Infrastructure.Scene
{
    public class SceneLoader
    {
        private FadeController _fadeController;

        public SceneLoader(FadeController fadeController)
        {
            _fadeController = fadeController;
        }

        public async UniTask LoadSceneAsync(string sceneName, bool additive = false)
        {
            var mode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            var op = SceneManager.LoadSceneAsync(sceneName, mode);
            op.allowSceneActivation = true;

            while (!op.isDone)
                await UniTask.Yield();
        }

        public async UniTask UnloadSceneAsync(string sceneName)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                var op = SceneManager.UnloadSceneAsync(sceneName);
                while (!op.isDone)
                    await UniTask.Yield();
            }
        }

        public async UniTask SwitchToScene(string toScene, string fromScene = null, bool additive = false)
        {
            await _fadeController.FadeIn();

            await LoadSceneAsync(toScene, additive);
            if (!string.IsNullOrEmpty(fromScene))
                await UnloadSceneAsync(fromScene);

            await _fadeController.FadeOut();
        }
    }
}