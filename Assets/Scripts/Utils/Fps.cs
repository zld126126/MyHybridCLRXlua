using UnityEngine;
using System.Text;
using Unity.Profiling;
using UnityEngine.UI;

namespace HotFix.Utils
{
    public class Fps : MonoBehaviour
    {
        [SerializeField] private Text txtInfo;

        float updateInterval = 0.33333f;

        private float passed = 0;

        ProfilerRecorder mainThreadTimeRecorder;
        ProfilerRecorder systemMemoryRecorder;
        ProfilerRecorder gcMemoryRecorder;
        ProfilerRecorder drawCallsRecorder;
        ProfilerRecorder vertRecorder;
        ProfilerRecorder trisRecorder;
        ProfilerRecorder batchesRecorder;
        ProfilerRecorder skinnedmeshRecorder;

        ProfilerRecorder setPassCallRecorder;
        ProfilerRecorder shadowCastersRecorder;

        string deviceModel,
            deviceName,
            graphicsMemorySize,
            graphicsName,
            maxTextureSize,
            systemMemorySize,
            graphicsDeviceType,
            graphicsDeviceVendor,
            networkStatus,
            pingStatus,
            batteryStatus,
            batteryVal;

        ulong networkId, pingId, batteryId;

        string showInfo = "", customInfo = "";

        private int _mParticleCount = 0;
        private int _mParticleSystemCount = 0;

        private Monitor _mMonitor = null;

        private void Awake()
        {
            if (txtInfo == null)
            {
                var fm = GameObject.Find("FpsMonitor");
                if (fm != null)
                {
                    txtInfo = fm.GetComponent<Text>();
                    if (txtInfo != null)
                    {
                        var rt = txtInfo.GetComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(Screen.width - 100, Screen.height - 100);
                    }
                }
            }

            passed = 0;

            deviceModel = SystemInfo.deviceModel;
            deviceName = SystemInfo.deviceName;
            graphicsMemorySize = $"{SystemInfo.graphicsMemorySize}MB";
            maxTextureSize = $"{SystemInfo.maxTextureSize}";
            systemMemorySize = $"{SystemInfo.systemMemorySize}MB";
            graphicsName = SystemInfo.graphicsDeviceName;
            graphicsDeviceType = SystemInfo.graphicsDeviceType.ToString();
            graphicsDeviceVendor = SystemInfo.graphicsDeviceVendor;


            _mMonitor = gameObject.GetComponent<Monitor>();
            if (_mMonitor == null)
            {
                _mMonitor = gameObject.AddComponent<Monitor>();
            }

            Application.targetFrameRate = 1024;
        }

        private void OnEnable()
        {
            mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Main Thread", 8);
            systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
            gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
            drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
            vertRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
            trisRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
            batchesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Batches Count");
            setPassCallRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");

            skinnedmeshRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Visible Skinned Meshes Count");
            shadowCastersRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Shadow Casters Count");

            if (string.IsNullOrEmpty(Monitor.PingWatchIP))
            {
                Monitor.PingWatchIP = "www.baidu.com";
            }

            networkId = _mMonitor.AddTrackNetwork(cur =>
            {
                networkStatus = cur switch
                {
                    NetworkReachability.ReachableViaCarrierDataNetwork => "移动网络",
                    NetworkReachability.ReachableViaLocalAreaNetwork => "WIFI",
                    _ => "无网"
                };
            });

            pingId = _mMonitor.AddPing(delay => { pingStatus = $"{delay} ms!"; }, () => { pingStatus = "timeout"; });

            batteryId = _mMonitor.AddBatteryMon((s, v) =>
            {
                batteryStatus = s.ToString();
                if (v < 0 || v > 0.99f)
                {
                    batteryVal = "满电";
                    if (s == BatteryStatus.Charging)
                    {
                        batteryStatus = "未在充电";
                    }
                }
                else
                {
                    batteryVal = $"{(int) (v * 100)}%";
                }
            });
        }

        void OnDisable()
        {
            _mMonitor.RemovePing(pingId);
            _mMonitor.RemoveBatteryMon(batteryId);
            _mMonitor.RemoveTrackNetwork(networkId);

            mainThreadTimeRecorder.Dispose();
            systemMemoryRecorder.Dispose();
            gcMemoryRecorder.Dispose();
            drawCallsRecorder.Dispose();
            vertRecorder.Dispose();
            trisRecorder.Dispose();
            batchesRecorder.Dispose();
            setPassCallRecorder.Dispose();

            skinnedmeshRecorder.Dispose();

            shadowCastersRecorder.Dispose();
        }

        static double GetRecorderFrameAverage(ProfilerRecorder recorder)
        {
            var samplesCount = recorder.Capacity;
            if (samplesCount == 0)
                return 0;

            double r = 0;
            unsafe
            {
                var samples = stackalloc ProfilerRecorderSample[samplesCount];
                recorder.CopyTo(samples, samplesCount);
                for (var i = 0; i < samplesCount; ++i)
                    r += samples[i].Value;
                r /= samplesCount;
            }

            return r;
        }

        private string GetFriendly(long length)
        {
            if (length < 1024)
            {
                return $"{length} B";
            }
            else if (length < 1024 * 1024)
            {
                return $"{length * 1.0f / 1024:f2} KB";
            }
            else if (length < 1024 * 1024 * 1024)
            {
                return $"{length * 1.0f / (1024 * 1024):f2} MB";
            }

            return $"{length * 1.0f / (1024 * 1024 * 1024):f2} GB";
        }

        void Update()
        {
            passed += Time.deltaTime;
            if (passed < updateInterval)
            {
                return;
            }

            passed = 0;

            var newline = System.Environment.NewLine;
            var tmp = GetRecorderFrameAverage(mainThreadTimeRecorder);
            showInfo = $" {deviceModel} {deviceName} SysMemory:({systemMemorySize})" +
                       newline +
                       $" {graphicsName}({graphicsMemorySize}) {graphicsDeviceVendor}({graphicsDeviceType})" +
                       newline +
                       $" {1000000000.0f / tmp:f2}FPS ({tmp / 1000000.0f:f2}ms)" +
                       newline +
                       $" FrameRate:{Application.targetFrameRate}" +
                       newline +
                       $" Draw Call: {drawCallsRecorder.LastValue} Batches: {batchesRecorder.LastValue} " +
                       newline +
                       $" Tris: {trisRecorder.LastValue}  Verts: {vertRecorder.LastValue} " +
                       $" Screen: {Screen.width}*{Screen.height}  MaxTextureSize: {maxTextureSize} " +
                       newline +
                       $" SetPass calls: {setPassCallRecorder.LastValue}  Shadow casters: {shadowCastersRecorder.LastValue}" +
                       $" Visible skinned meshes: {skinnedmeshRecorder.LastValue}" +
                       newline +
                       $" Used Mem: {GetFriendly(systemMemoryRecorder.LastValue)}" +
                       $" Reserved Mem: {GetFriendly(gcMemoryRecorder.LastValue)}" +
                       newline +
                       $" Addr: {Monitor.PingWatchIP} Ping: {pingStatus}" +
                       newline +
                       $" 网络状态:{networkStatus} 电池状态: {batteryStatus} 电量:{batteryVal} " +
                       newline +
                       customInfo
                ;
        }

        private GUIStyle fontStyle = null;

        private void OnGUI()
        {
            if (txtInfo != null)
            {
                return;
            }

            if (fontStyle == null)
            {
                fontStyle = new GUIStyle
                {
                    clipping = TextClipping.Clip,
                    border = new RectOffset(0, 10, 10, 0),
                    fontSize = 20,
                    alignment = TextAnchor.MiddleRight,
                };
                fontStyle.normal.textColor = Color.red;
            }

            GUILayout.Label(showInfo, fontStyle);
        }

        private void CheckOtherInfo()
        {
            var particleSystems = Resources.FindObjectsOfTypeAll<ParticleSystem>();
            _mParticleSystemCount = 0;
            _mParticleCount = 0;

            if (particleSystems != null)
            {
                foreach (var ps in particleSystems)
                {
                    if (ps == null || ps.gameObject == null || !ps.gameObject.activeInHierarchy)
                    {
                        continue;
                    }

                    _mParticleSystemCount++;
                    _mParticleCount += ps.particleCount;
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine($" 粒子发射器数量:{_mParticleSystemCount} 粒子数量:{_mParticleCount}");
            customInfo = sb.ToString();
        }

        private void LateUpdate()
        {
            CheckOtherInfo();

            if (txtInfo != null)
            {
                txtInfo.text = showInfo;
            }
        }
    }
}
