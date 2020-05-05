using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using C;
namespace Tomokin
{
    public class LoginManager : MonoBehaviour
    {
        //登录UI
        public GameObject login_root;
        public InputField PlayerName;

        public static string Name;              //玩家姓名+ID

        //房间列表UI
        public GameObject room_root;
        public Text room_text;
        public InputField room_input_filed;

        //等待房间UI
        public GameObject RoomScene;

        public void Login()
        {
            if (PlayerName.text == "") return;
            CilentManager.PlayerName = PlayerName.text;
            CilentManager.PlayerID = "|" + MyMath.IdGen();
            Name = PlayerName.text + CilentManager.PlayerID;
            Net.Login(Name);

        }
        public void LoginSuccess()
        {
            login_root.SetActive(false);
            room_root.SetActive(true);
            Net.GetCurrentRoomList("");
        }

        public void JoinRoom()
        {
            string room = room_input_filed.text;
            if (room == "") return;
            Net.JoinRoom(room);
            //可以添加加载界面
        }
        public void JoinSuccess()
        {
            this.gameObject.SetActive(false);
            RoomScene.SetActive(true);
        }


        public void RoomListGet(List<string> rooms)
        {
            room_text.text = "";
            foreach (var item in rooms)
            {
                room_text.text += item + "\n";
            }
        }
    }
}