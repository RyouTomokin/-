using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace C
{
    public class ChatManager : MonoBehaviour
    {

        public NetManager netManager;

        //登录用
        public GameObject login_root;
        public InputField inputField;
        public Button login_button;

        //房间
        public GameObject room_root;
        public Text room_user_title;
        public InputField room_name_input;
        public Text room_list_text;
        public Button in_room_button;

        //聊天
        public GameObject chat_root;
        public Text chat_text;
        public InputField chat_input;
        public Scrollbar scrollbar;

        //人数
        public Text num_text;

        private string nickname;

        public void Login()
        {
            if (inputField.text == "") return;
            string nickname = inputField.text + "|" + MyMath.IdGen();
            netManager.Login(nickname);
            login_button.interactable = false;
        }

        public void LoginCallback(string nickname)
        {
            this.nickname = nickname.Split('|')[0]; ;
            login_button.interactable = true;
            inputField.text = "";
            login_root.SetActive(false);
            ShowRoomChose();
        }

        private void ShowRoomChose()
        {
            room_root.SetActive(true);
            string name = nickname;
            room_user_title.text = name + " 已登录";
            FreshRoomList();
        }

        //刷新列表
        public void FreshRoomList()
        {
            netManager.FreshRoomList(room_name_input.text);
        }

        public void FreshRoomListCallback(List<string> rooms)
        {
            room_list_text.text = "";
            if (rooms.Count == 0) return;
            foreach (string item in rooms)
            {
                room_list_text.text += item + "\n\n";
            }
        }

        //显示房间人数
        public void FreshRoomNum(int num)
        {
            num_text.text = "人数：" + num;
        }

        //创建或加入房间
        public void CreateOrJoinRoom()
        {
            if (room_name_input.text == "") return;
            in_room_button.interactable = false;
            netManager.CreateOrJoinRoom(room_name_input.text);
        }

        public void CreateOrJoinRoomCallback(bool b,int num)
        {
            if (b)
            {
                Debug.Log("加入房间成功");
                FreshRoomNum(num);
                room_root.SetActive(false);
                ShowChatRoot();
            }
            else
            {
                in_room_button.interactable = true;
                Debug.Log("加入房间失败");
            }
        }

        private void ShowChatRoot()
        {
            chat_root.SetActive(true);
        }

        //显示聊天数据
        public void ShowChatMessage(string s)
        {
            //chat_text.text += ">>> "  + s + "\n";
            FreshChatText(">>> " + s + "\n");
        }

        public void SendChatMessage()
        {
            //chat_text.text += ">>> " + nickname +"：" +chat_input.text + "\n";
            FreshChatText(">>> " + nickname + "：" + chat_input.text + "\n");
           netManager.SendResponse(NetManager.MessageType.聊天,"", chat_input.text);
        }

        public void ShowPlayerEnter(string name,int num)
        {
            Debug.Log(name + " 进入房间");
            FreshRoomNum(num);
            //chat_text.text += "<color = green>>>> " + name + " 加入房间</color>\n";
            FreshChatText("<color = green>>>> " + name + " 加入房间</color>\n");
        }
        public void ShowPlayerLeft(string name, int num)
        {
            Debug.Log(name + " 离开房间");
            FreshRoomNum(num);
            //chat_text.text += "<color = red>>>> " + name + " 离开房间</color>\n";
            FreshChatText("<color = red>>>> " + name + " 离开房间</color>\n");
        }

        private Coroutine c_c;
        private bool isTouchingChatText = false;

        private void FreshChatText(string ss)
        {
            chat_text.text += ss;
            if (isTouchingChatText) return;
            if (c_c != null) StopCoroutine(c_c);
            c_c = StartCoroutine(chatTextButtomCor());
        }

        public void ChatTextTouch(bool b)
        {
            isTouchingChatText = b;
            if (isTouchingChatText)
            {
                if (c_c != null) StopCoroutine(c_c);
            }
        }

        private IEnumerator chatTextButtomCor()
        {
            while(scrollbar.value > 0)
            {
                scrollbar.value = Mathf.Lerp(scrollbar.value, 0, 0.5f);
                yield return new WaitForSeconds(0.02f);
            }
        }
        //聊天数据到最底部

    }
}

