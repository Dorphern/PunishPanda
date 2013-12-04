using InAudio;
using UnityEditor;
using UnityEngine;

public class AuxWindow : InAudioBaseWindow
{
    private int selectedToolbar = 0;
    private readonly string[] toolbarOptions = {"Busses", "Banks", "Integrity", "Project Data"};

    private AudioBus selectedBus;

    private AudioBankCreatorGUI bankGUI;
    private AudioBusCreatorGUI busGUI;
    private IntegrityGUI integrityGUI;

    public static void Launch()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(AuxWindow));

        window.Show();
        window.minSize = new Vector2(400,400);
        window.title = "Aux Window";

    }
     
    public void OnEnable()
    {
        BaseEnable();
        if(bankGUI == null)
            busGUI = new AudioBusCreatorGUI(this);
        if(bankGUI == null)
            bankGUI = new AudioBankCreatorGUI(this);
        if (integrityGUI == null)
            integrityGUI = new IntegrityGUI(this);

        busGUI.OnEnable();
        bankGUI.OnEnable();
        integrityGUI.OnEnable();

    }

    void Update()
    {
        BaseUpdate();
        busGUI.OnUpdate();
        bankGUI.OnUpdate();
    }

    void OnGUI()
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
            bool missingaudio = Manager.AudioTree == null;
            bool missingaudioEvent = Manager.EventTree == null;
            bool missingbus = Manager.BusTree == null;
            bool missingBank = Manager.BankLinkTree == null;

            bool areAnyMissing = missingaudio || missingaudioEvent || missingbus || missingBank;

            if (areAnyMissing)
            {
                DrawMissingDataCreation();
                return;
            }

        }
        else
        {
            return;
        }

        isDirty = false;

        EditorGUILayout.BeginVertical();
        EditorGUILayout.EndVertical();
        selectedToolbar = GUILayout.Toolbar(selectedToolbar, toolbarOptions);

        if (selectedToolbar == 0)
        {
            isDirty |= busGUI.OnGUI(LeftWidth, (int)position.height);
        }

        if (selectedToolbar == 1)
        {
            isDirty |= bankGUI.OnGUI(LeftWidth, (int)position.height);
        }

        if (selectedToolbar == 2)
            isDirty |= integrityGUI.OnGUI();

        if (selectedToolbar == 3)
        {
            DrawMissingDataCreation();

            DrawStartFromScratch();
        }

        if (isDirty)
            Repaint();

        PostOnGUI();
    }

    private void DrawMissingDataCreation()
    {
        bool missingaudio = Manager.AudioTree == null;
        bool missingaudioEvent = Manager.EventTree == null;
        bool missingbus = Manager.BusTree == null;
        bool missingbankLink = Manager.BankLinkTree == null;

        bool areAnyMissing = missingaudio | missingaudioEvent | missingbus | missingbankLink;
        if(areAnyMissing)
        {
            string missingAudioInfo = missingaudio ? "Audio Data\n" : "";
            string missingEventInfo = missingaudioEvent ? "Event Data\n" : "";
            string missingBusInfo = missingbus ? "Bus Data\n" : "";
            string missingBankInfo = missingbankLink ? "BankLink Data\n" : ""; 
            EditorGUILayout.BeginVertical();
            EditorGUILayout.HelpBox(missingAudioInfo + missingEventInfo + missingBusInfo + missingBankInfo + "is missing.",
                    MessageType.Error, true);

            bool areAllMissing = missingaudio && missingaudioEvent && missingbus && missingbankLink;
            if (!areAllMissing)
            {

                if (GUILayout.Button("Create missing content", GUILayout.Height(30)))
                {
                    int levelSize = 3;
                        //How many subfolders by default will be created. Number is only a hint for new people
                    if (missingbus)
                        CreateBusPrefab();
                    if (missingaudio)
                        CreateAudioPrefab(levelSize, Manager.BusTree);
                    if (missingaudioEvent)
                        CreateEventPrefab(levelSize);
                    if (missingbankLink)
                        CreateBankLinkPrefab();
                    Manager.Load(true);
                    if (missingaudio)
                        NodeWorker.AssignToNodes(Manager.AudioTree, node => node.Bus = Manager.BusTree);
                    if (Manager.AudioTree != null && Manager.BankLinkTree != null)
                        NodeWorker.AssignToNodes(Manager.AudioTree,
                            node => node.BankLink = Manager.BankLinkTree.GetChildren[0]);

                    EditorApplication.SaveCurrentSceneIfUserWantsTo();
                }
            }
            DrawStartFromScratch();
            EditorGUILayout.EndVertical();
        }
        
    }

    private bool AreAllMissing()
    {
        bool missingaudio = Manager.AudioTree == null;
        bool missingaudioEvent = Manager.EventTree == null;
        bool missingbus = Manager.BusTree == null;
        bool missingbankLink = Manager.BankLinkTree == null;
        return missingaudio && missingaudioEvent && missingbus && missingbankLink;
    }

    private void DrawStartFromScratch()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Start over from scratch", GUILayout.Height(30)))
        {
            if (AreAllMissing() || EditorUtility.DisplayDialog("Create new project?", "This will delete ALL data!", "Start over from scratch", "Do nothing"))
            {
                SystemFolderHelper.DeleteFolderContent(FolderSettings.BankRelativeDictory);

                int levelSize = 3;
                GameObject audioGO = new GameObject();
                GameObject eventGO = new GameObject();
                GameObject busGO = new GameObject();
                GameObject bankGO = new GameObject();

                Manager.BusTree = AudioBusWorker.CreateTree(busGO);
                Manager.BankLinkTree = AudioBankWorker.CreateTree(bankGO);
                Manager.AudioTree = AudioNodeWorker.CreateTree(audioGO, levelSize, Manager.BusTree);
                Manager.EventTree = AudioEventWorker.CreateTree(eventGO, levelSize);
                
                SaveAndLoad.CreateDataPrefabs(Manager.AudioTree.gameObject, Manager.EventTree.gameObject, Manager.BusTree.gameObject, Manager.BankLinkTree.gameObject);

                Manager.Load(true);

                if (Manager.BankLinkTree != null)
                {
                    var bankLink = AudioBankWorker.CreateBank(Manager.BankLinkTree.gameObject, Manager.BankLinkTree,
                        GUIDCreator.Create());
                    bankLink.Name = "Default - Auto loaded";
                    bankLink.AutoLoad = true;

                    NodeWorker.AssignToNodes(Manager.AudioTree, node => node.BankLink = Manager.BankLinkTree.GetChildren[0]);
                }
                else
                {
                    Debug.LogError("There was a problem creating the data.");
                }

                NodeWorker.AssignToNodes(Manager.AudioTree, node => node.Bus = Manager.BusTree);

                AssetDatabase.Refresh();

                EditorApplication.SaveCurrentSceneIfUserWantsTo();
            }
        }
    }

    public void SelectBusCreation()
    {
        selectedToolbar = 0;
    }

    public void SelectBankCreation()
    {
        selectedToolbar = 1;
    }

    public void SelectIntegrity()
    {
        selectedToolbar = 2;
    }

    public void SelectDataCreation()
    {
        selectedToolbar = 3;
    }
    
    public void FindBank(AudioBankLink bankLink)
    {
        selectedToolbar = 1;
        bankGUI.Find(bankLink);
    }

    public void FindBus(AudioBus bus)
    {
        selectedToolbar = 0;
        busGUI.Find(bus);
    }


    private AudioBus CreateBusPrefab()
    {
        GameObject go = new GameObject();
        Manager.BusTree = AudioBusWorker.CreateTree(go);
        SaveAndLoad.CreateAudioBusRootPrefab(go);
        return Manager.BusTree;
    }

    private void CreateEventPrefab(int levelSize)
    {
        GameObject go = new GameObject();
        Manager.EventTree = AudioEventWorker.CreateTree(go, levelSize);
        SaveAndLoad.CreateAudioEventRootPrefab(go);
    }

    private void CreateBankLinkPrefab()
    {
        GameObject go = new GameObject();
        Manager.BankLinkTree = AudioBankWorker.CreateTree(go);
        SaveAndLoad.CreateAudioBankLinkPrefab(go);
    }

    private void CreateAudioPrefab(int levelSize, AudioBus bus)
    {
        GameObject go = new GameObject();
        Manager.AudioTree = AudioNodeWorker.CreateTree(go, levelSize, bus);
        SaveAndLoad.CreateAudioNodeRootPrefab(go);
    }  
}
