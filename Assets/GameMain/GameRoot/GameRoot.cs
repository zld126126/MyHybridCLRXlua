using System.Collections;
using UnityEngine;

namespace GameMain.Scripts
{
    public partial class GameRoot : MonoBehaviour
    {
        public static GameRoot Instance = null;

        private IEnumerator Start()
        {
            Instance = this;
            yield return LoadGameDll();
            yield return RunDll();
        }
    }
}
