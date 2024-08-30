using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace HotFix.Utils
{
    public class Monitor : MonoBehaviour
    {
        private void OnDisable()
        {
            StopBatteryMon();
            StopPing();
            StopTrackNetwork();
        }

        #region battery
        ulong batteryPos = 0;
        BatteryStatus oldBatteryStatus = BatteryStatus.Unknown;
        float oldBatteryLevel = 0;
        Coroutine corBattery = null;

        Dictionary<ulong, Action<BatteryStatus, float>> dictBatteryFunc =
            new Dictionary<ulong, Action<BatteryStatus, float>>();

        public ulong AddBatteryMon(Action<BatteryStatus, float> callback)
        {
            batteryPos++;
            dictBatteryFunc.Add(batteryPos, callback);

            if (corBattery == null)
            {
                corBattery = StartCoroutine(ItorBattery());
            }

            return batteryPos;
        }

        private IEnumerator ItorBattery()
        {
            while (true)
            {
                if (dictBatteryFunc.Count > 0)
                {
                    var tmpStatus = SystemInfo.batteryStatus;
                    var tmpLevel = SystemInfo.batteryLevel;

                    if (tmpStatus != oldBatteryStatus || tmpLevel != oldBatteryLevel)
                    {
                        oldBatteryStatus = tmpStatus;
                        oldBatteryLevel = tmpLevel;

                        foreach (var kv in dictBatteryFunc)
                        {
                            kv.Value?.Invoke(tmpStatus, tmpLevel);
                        }
                    }
                }

                yield return Yielders.WaitForSeconds(1);
            }
        }

        public void StopBatteryMon()
        {
            dictBatteryFunc.Clear();

            if (corBattery != null)
            {
                StopCoroutine(corBattery);
                corBattery = null;
            }
        }

        public void RemoveBatteryMon(ulong id)
        {
            if (dictBatteryFunc.ContainsKey(id))
            {
                dictBatteryFunc.Remove(id);
            }
        }

        #endregion

        #region ping

        Coroutine corPing = null;
        ulong pingPos = 0;
        public static string PingWatchIP = "";

        Dictionary<ulong, KeyValuePair<Action<int>, Action>> dictPingFunc =
            new Dictionary<ulong, KeyValuePair<Action<int>, Action>>();

        public void StopPing()
        {
            dictPingFunc.Clear();

            if (corPing != null)
            {
                StopCoroutine(corPing);
                corPing = null;
            }
        }

        private IEnumerator ItorPing()
        {
            while (dictPingFunc.Count > 0)
            {
                if (!IPAddress.TryParse(PingWatchIP, out var ip))
                {
                    var ips = Dns.GetHostAddresses(PingWatchIP);
                    if (ips.Length == 0)
                    {
                        continue;
                    }

                    PingWatchIP = ips[0].ToString();
                    continue;
                }

                var ping = new Ping(PingWatchIP);
                while (!ping.isDone)
                {
                    yield return Yielders.WaitForSeconds(0.5f);
                }

                var time = ping.time;
                foreach (var kv in dictPingFunc)
                {
                    kv.Value.Key?.Invoke(time);
                }

                yield return null;

                ping.DestroyPing();

                yield return Yielders.WaitForSeconds(1);
            }
        }

        public ulong AddPing(Action<int> callback, Action timeout)
        {
            pingPos++;
            dictPingFunc.Add(pingPos, new KeyValuePair<Action<int>, Action>(callback, timeout));

            if (corPing == null)
            {
                corPing = StartCoroutine(ItorPing());
            }

            return pingPos;
        }

        public void RemovePing(ulong id)
        {
            if (dictPingFunc.ContainsKey(id))
            {
                dictPingFunc.Remove(id);
            }

            if (dictPingFunc.Count == 0)
            {
                StopPing();
            }
        }

        #endregion

        #region netstatus

        NetworkReachability curReachability = NetworkReachability.NotReachable;
        Coroutine corNetworkingCheck = null;

        Dictionary<ulong, Action<NetworkReachability>> dictNetworkStatus =
            new Dictionary<ulong, Action<NetworkReachability>>();

        ulong curTrackNetworkPos = 0;

        public ulong AddTrackNetwork(Action<NetworkReachability> curStatus)
        {
            curTrackNetworkPos++;

            curStatus?.Invoke(Application.internetReachability);

            dictNetworkStatus[curTrackNetworkPos] = curStatus;

            if (corNetworkingCheck == null)
            {
                curReachability = NetworkReachability.NotReachable;
                corNetworkingCheck = StartCoroutine(TrackNetwork());
            }

            return curTrackNetworkPos;
        }

        private IEnumerator TrackNetwork()
        {
            while (true)
            {
                if (dictNetworkStatus.Count > 0)
                {
                    var tmp = Application.internetReachability;
                    if (tmp != curReachability)
                    {
                        foreach (var kv in dictNetworkStatus)
                        {
                            kv.Value?.Invoke(tmp);
                        }

                        curReachability = tmp;
                    }
                }

                yield return Yielders.WaitForSeconds(1);
            }
        }

        public void RemoveTrackNetwork(ulong id)
        {
            if (dictNetworkStatus.ContainsKey(id))
            {
                dictNetworkStatus.Remove(id);
            }

            if (dictNetworkStatus.Count == 0)
            {
                StopTrackNetwork();
            }
        }

        public void StopTrackNetwork()
        {
            dictNetworkStatus.Clear();

            if (corNetworkingCheck != null)
            {
                StopCoroutine(corNetworkingCheck);
                corNetworkingCheck = null;
            }
        }

        #endregion
    }
}
