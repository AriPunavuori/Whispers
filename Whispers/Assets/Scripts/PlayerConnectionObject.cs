using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Picture = System.Collections.Generic.List<LineData>;
using UnityEngine.UI;

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
            um.roomCodeTxt.text = "Room #" + hg.roomCode;
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
        pm.ServersPlayerDataList.Add(pd);
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
        var pm = FindObjectOfType<PlayerManager>();

        pm.playerDataList = new List<PlayerData>(pd);
        UIContainer = Instantiate(UIContainerPrefab);
        var wUI = GameObject.Find("WaitingUI");
        UIContainer.transform.SetParent(wUI.transform);
        UIContainer.transform.localScale = Vector3.one;
        UIContainer.GetComponent<RectTransform>().localPosition = Vector3.zero;
        for (int i = 0 ; i < pm.playerDataList.Count ;i++){
            var PlayerInfo = Instantiate(playerUIPrefab);
            print("luodaan Player info: " + PlayerInfo);
            PlayerInfo.GetComponent<GetPlayerInfo>().nameText.text = pd[i].playerName;
            PlayerInfo.transform.SetParent(UIContainer.transform);
            PlayerInfo.transform.localScale = Vector3.one;
            print(pm.playerDataList[i].playerName);
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
        um.roomCodeTxt.text = "Room# is: " + roomCode;
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
    public void CmdThisClientIsReady() {
        var gm = FindObjectOfType<GameManager>();
        var hg = FindObjectOfType<HostGame>();
        gm.playersReady++;
        if (gm.playersReady >= hg.numberOfPlayers) {
            StartCoroutine(WaitDelay());
        }
    }

    [ClientRpc]
    public void RpcStartNextRound() {
        var gm = FindObjectOfType<GameManager>();
        print("Clienttien round number kasvaa: " + gm.roundNumbr);
        gm.roundNumbr++;
        gm.Gameplay();
    }

    IEnumerator WaitDelay() {
        var gm = FindObjectOfType<GameManager>();
        yield return new WaitForSeconds(0.4f);
        RpcStartNextRound();
        gm.playersReady = 0;
    }
}