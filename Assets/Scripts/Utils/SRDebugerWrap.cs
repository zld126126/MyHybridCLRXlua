using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRDebugerWrap
{
    public static void ShowConsolePanel()
    {
        SRDebug.Instance.ShowDebugPanel(SRDebugger.DefaultTabs.Console);
    }
}
