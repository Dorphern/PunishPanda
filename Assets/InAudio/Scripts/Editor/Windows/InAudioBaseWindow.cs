using InAudio;
using InAudio.ExtensionMethods;
using UnityEditor;
using UnityEngine;

public class InAudioBaseWindow : EditorWindow
{
    public CommonDataManager Manager;

    protected int topHeight = 0;
    protected int LeftWidth = 350;

    protected bool isDirty;

    protected void BaseEnable()
    {
        autoRepaintOnSceneChange = true;
        EditorApplication.modifierKeysChanged += Repaint;

        Manager = InAudioInstanceFinder.DataManager;

        EditorResources.Reload();
    }

    protected void BaseUpdate()
    {
        if (Event.current != null && Event.current.type == EventType.ValidateCommand)
        {
            switch (Event.current.commandName)
            {
                case "UndoRedoPerformed":
                    Repaint();
                    break;
            }
        }
    }

    protected bool HandleMissingData()
    {
        if (Manager == null)
        {
            Manager = InAudioInstanceFinder.DataManager;
            if (Manager == null)
            {
                ErrorDrawer.MissingAudioManager();
            }
        }

        if (Manager != null)
        {
            bool areAnyMissing = ErrorDrawer.IsDataMissing(Manager);

            if (areAnyMissing)
            {
                Manager.Load();
            }
            if (ErrorDrawer.IsDataMissing(Manager))
            {
                ErrorDrawer.MissingData(Manager);
                return false;
            }
            else
            {
                return true;
            }
        }
        else 
            return false;
    }

    protected void PostOnGUI()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            GUIUtility.keyboardControl = 0;
        }
    }

    protected bool IsKeyDown(KeyCode code)
    {
        return Event.current.type == EventType.keyDown && Event.current.keyCode == code;
    }

    [MenuItem("Window/InAudio/Audio Window #&1")]
    private static void ShowAudioWindow()
    {
        AudioWindow.Launch();
    }

    [MenuItem("Window/InAudio/Event Window #&2")]
    private static void ShowEventWindow()
    {
        EventWindow.Launch();
    }

    [MenuItem("Window/InAudio/Bus Window #&3")]
    private static void ShowBusWindow()
    {
        AuxWindow.Launch();
        AuxWindow window = EditorWindow.GetWindow(typeof(AuxWindow)) as AuxWindow;
        if (window != null)
        {
            window.SelectBusCreation();
        }
    }

    [MenuItem("Window/InAudio/Banks Window #&4")]
    private static void ShowBanksWindow()
    {
        AuxWindow.Launch();
        AuxWindow window = EditorWindow.GetWindow(typeof(AuxWindow)) as AuxWindow;
        if (window != null)
        {
            window.SelectBankCreation();
        }
    }

    [MenuItem("Window/InAudio/Integrity Window #&5")]
    private static void ShowIntegrityWindow()
    {
        AuxWindow.Launch();
        AuxWindow window = EditorWindow.GetWindow(typeof(AuxWindow)) as AuxWindow;
        if (window != null)
        {
            window.SelectIntegrity();
        }
       
    }

    [MenuItem("Window/InAudio/Project Window #&6")]
    private static void ShowProjectWindow()
    {
        AuxWindow.Launch();
        AuxWindow window = EditorWindow.GetWindow(typeof(AuxWindow)) as AuxWindow;
        if (window != null)
        {
            window.SelectDataCreation();
        }
    }
}
