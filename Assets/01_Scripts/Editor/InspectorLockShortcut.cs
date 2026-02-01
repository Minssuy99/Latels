using UnityEngine;
using UnityEditor;

public class InspectorLockShortcut : MonoBehaviour
{
    [MenuItem("Extra Tools/Toggle Inspector Lock %#z")]
    private static void ToggleInspectorLock()
    {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }
}