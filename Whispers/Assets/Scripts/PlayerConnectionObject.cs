using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Picture = System.Collections.Generic.List<LineData>;
using TMPro;

public class PlayerConnectionObject : NetworkBehaviour {
    //UIManager um;
    //HostGame hg;
    //PlayerManager pm;
    RoundDataManager rdm;
    //DrawingMachine dm;
    //GameManager gm;

    public GameObject playerUIPrefab;
    public GameObject UIContainerPrefab;
    
    bool rdmCreated;
    GameObject UIContainer;

    NetworkIdentity target;

    private void Awake() {
        //um = UIManager.instance;
        //hg = HostGame.instance;
        //pm = PlayerManager.instance;
        //dm = DrawingMachine.instance;
        //gm = GameManager.instance;
        //rdm = RoundDataManager.instance;
    }

    void Start () {
        target = GetComponent<NetworkIdentity>();
        // Is this actually my own local PlayerConnectionObject?
        if (isLocalPlayer == false) {
            // this object belong to another player.
            return; 
        }
        if (isServer) {
            var um = FindObjectOfType<UIManager>();
            var hg = FindObjectOfType<HostGame>();
            um.roomCodeTxt.text = "Room# " + hg.roomCode;
        }
        var pm = FindObjectOfType<PlayerManager>();
        CmdAddPlayer(pm.playerData.playerName);
        CmdShowRoomCode();
    }

    void Update () {
        // update runs on EVERYONE's computer wheter or not they own this particular object
        if (isLocalPlayer == false) {
            return;
        }
    }

    [TargetRpc]
    void TargetSetNetworkId(NetworkConnection _target, int id){
        var pm = FindObjectOfType<PlayerManager>();
        pm.playerData.playerID = id;
        transform.name = "" + id;
    }

    [Command]
    void CmdAddPlayer(string name){
        var hg = FindObjectOfType<HostGame>();
        var pm = FindObjectOfType<PlayerManager>();
        hg.numberOfPlayers++;
        var pd = new PlayerData();
        pd.playerName = name;
        pd.playerID = hg.numberOfPlayers - 1;
        pd.playerRDY = true;
        pm.ServersPlayerDataList.Add(pd);
        Fabric.EventManager.Instance.PostEvent("connect");
        RpcUpdatePlayerCount(hg.numberOfPlayers);
        RpcClearPlayerNameList();
        RpcUpdatePlayerNameList(pm.ServersPlayerDataList.ToArray());
        TargetSetNetworkId(target.connectionToClient, pd.playerID);
    }

    [ClientRpc]
    void RpcUpdatePlayerCount(int newPlayerCount){
        var hg = FindObjectOfType<HostGame>();
        hg.numberOfPlayers = newPlayerCount;
    }

    [ClientRpc]
    void RpcClearPlayerNameList(){
        var pm = FindObjectOfType<PlayerManager>();
        pm.playerDataList.Clear();
    }

    [ClientRpc]
    void RpcUpdatePlayerNameList(PlayerData[] pd){
        var textToShow = "";
        var pm = FindObjectOfType<PlayerManager>();
        pm.playerDataList = new List<PlayerData>(pd);
        print("Lista ennen looppia: " + textToShow);
        for (int i = 0 ; i < pm.playerDataList.Count ;i++){
            if(pm.playerDataList[i].playerRDY == true){
                textToShow += pm.playerDataList[i].playerName + "\n";
                print("Tämä pitäisi olla listalla: " + pm.playerDataList[i].playerName);
            } else {
                print("Tämä taas ei: " + pm.playerDataList[i].playerName);
            }
            print(pm.playerDataList[i].playerName);
        }
        var um = FindObjectOfType<UIManager>();
        if(pm.playMode == PlayerManager.PlayMode.Wait||pm.playMode == PlayerManager.PlayMode.Menu) {
            um.waitText.text = textToShow;
        }
    }

    [Command]
    void CmdShowRoomCode() {
        var hg = FindObjectOfType<HostGame>();
        RpcUpdateRoomCode(hg.roomCode);
    }

    [ClientRpc]
    void RpcUpdateRoomCode(int roomCode) {
        var um = FindObjectOfType<UIManager>();
        um.roomCodeTxt.text = "Room# " + roomCode;
    }

    [Command]
    public void CmdAddGuessToChain(string text, int chainID) {
        RpcUpdateStringChaindataOnClients(text, chainID);
    }

    [ClientRpc]
    void RpcUpdateStringChaindataOnClients(string text, int chainID) {
        rdm = FindObjectOfType<RoundDataManager>();
        rdm.chains[chainID].guesses.Add(text.RemoveDiacritics());
    }

    [Command]
    public void CmdAddPictureToChain(LineData[] picture, int chainID) {
        RpcUpdatePicChaindataOnClients(picture, chainID);
    }

    [ClientRpc]
    void RpcUpdatePicChaindataOnClients(LineData[] picture, int chainID) {
        rdm.chains[chainID].pictures.Add(picture);
    }

    [Command]
    public void CmdThisClientIsReady(int id) {
        var gm = FindObjectOfType<GameManager>();
        var hg = FindObjectOfType<HostGame>();
        var um = FindObjectOfType<UIManager>();
        var pm = FindObjectOfType<PlayerManager>();
        gm.playersReady++;
        var qqq = pm.ServersPlayerDataList[id];
        qqq.playerRDY = true;
        pm.ServersPlayerDataList[id] = qqq;
        RpcWaitTextState();
        if (gm.playersReady >= hg.numberOfPlayers) {
            StartCoroutine(WaitDelay());
        }
        RpcUpdatePlayerNameList(pm.ServersPlayerDataList.ToArray());
    }

    [ClientRpc]
    public void RpcStartNextRound() {
        var gm = FindObjectOfType<GameManager>();
        var pm = FindObjectOfType<PlayerManager>();
        var um = FindObjectOfType<UIManager>();
        um.waitStatusText.text = "Next round starting...";
        // timer ennen ku pelin flow jatkuu
        StartCoroutine(ExtraWait());
        gm.roundNumbr++;
    }

    [ClientRpc]
    public void RpcWaitTextState() {
        var um = FindObjectOfType<UIManager>();
        //var animator = FindObjectOfType<Animator>();
        // joku textflash kun joutuu odottamaan waitlobbyssa?
        //animator.SetBool("Start", true);
        um.waitStatusText.text = "Waiting others...";
    }

    IEnumerator WaitDelay() {
        var gm = FindObjectOfType<GameManager>();
        var um = FindObjectOfType<UIManager>();
        var pm = FindObjectOfType<PlayerManager>();
        yield return new WaitForSeconds(1f);
        RpcStartNextRound();
        gm.playersReady = 0;
    }

    IEnumerator ExtraWait() {
        var um = FindObjectOfType<UIManager>();
        var gm = FindObjectOfType<GameManager>();
        var pm = FindObjectOfType<PlayerManager>();
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < pm.ServersPlayerDataList.Count; i++) {
            var temp = pm.ServersPlayerDataList[i];
            temp.playerRDY = false;
            pm.ServersPlayerDataList[i] = temp;
        }
        gm.Gameplay();
    }

    [Command]
    public void CmdReadyToQuit(){
        var gm = FindObjectOfType<GameManager>();
        var hg = FindObjectOfType<HostGame>();
        gm.playersReadyToQuit++;
        if(gm.playersReadyToQuit >= hg.numberOfPlayers) {
            StartCoroutine(QuitDelay());
        }
    }

    IEnumerator QuitDelay(){
        yield return new WaitForSeconds(5);
        RpcQuitClients();
    }

    [ClientRpc]
    void RpcQuitClients(){
        var w = FindObjectOfType<Watching>();
        w.quitButton.gameObject.SetActive(true);
        w.nextButton.gameObject.SetActive(false);
        w.previousButton.gameObject.SetActive(false);
        w.uiText.text = "Thats it folks, GMAE Over!";
    }
}