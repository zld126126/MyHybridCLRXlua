using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//=============================================================================
/// Reference : https://forum.unity3d.com/threads/c-coroutine-waitforseconds-garbage-collection-tip.224878/
/// FileName : Yielders.cs
//=============================================================================
namespace HotFix.Utils
{
    public class Yielders : MonoBehaviour
    {
        static Dictionary<float, WaitForSeconds> m_TimeInterval = new Dictionary<float, WaitForSeconds>(128);

        static Dictionary<float, WaitForSecondsRealtime> m_TimeIntervalRealtime =
            new Dictionary<float, WaitForSecondsRealtime>(16);

        public static WaitForEndOfFrame EndOfFrame { get; private set; } = new WaitForEndOfFrame();
        public static WaitForFixedUpdate FixedUpdate { get; private set; } = new WaitForFixedUpdate();

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            if (!m_TimeInterval.TryGetValue(seconds, out var wfs))
            {
                m_TimeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));
            }

            return wfs;
        }

        public static IEnumerator WaitForFixedSeconds(float seconds)
        {
            seconds += Time.fixedTime;
            while (Time.fixedTime < seconds)
            {
                yield return FixedUpdate;
            }
        }

        public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
        {
            // TODO：之前处理有问题，会使用同一个对象
            //if (!m_TimeIntervalRealtime.TryGetValue(seconds, out var wfs))
            //{
            //    m_TimeIntervalRealtime.Add(seconds, wfs = new WaitForSecondsRealtime(seconds));
            //}
            //return wfs;
            return new WaitForSecondsRealtime(seconds);
        }
    }
}
