using System;
using Cysharp.Threading.Tasks;
using GameFramework;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    public static class UGuiExtension
    {
        public static void Set(this UnityEvent unityEvent, UnityAction unityAction)
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(unityAction);
        }

        public static void SetAsync(this UnityEvent unityEvent, Func<UniTask> action)
        {
            async UniTask OnClickAsync()
            {
                try
                {
                    await action();
                }
                catch (Exception e)
                {
                    throw new GameFrameworkException("Unity Event error", e);
                }
            }
            
            void OnClick()
            {
                OnClickAsync().Forget();
            }

            unityEvent.Set(OnClick);
        }

        public static void SetAsync(this Button button, Func<UniTask> action)
        {
            async UniTask OnClickAsync()
            {
                try
                {
                    button.interactable = false;
                    await action();
                }
                catch (Exception e)
                {
                    throw new GameFrameworkException($"{button.name} click error", e);
                }
                finally
                {
                    button.interactable = true;
                }
            }

            void OnClick()
            {
                OnClickAsync().Forget();
            }

            button.onClick.Set(OnClick);
        }

        public static void Set<T>(this UnityEvent<T> unityEvent, UnityAction<T> unityAction) where T : Object
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(unityAction);
        }
    }
}
