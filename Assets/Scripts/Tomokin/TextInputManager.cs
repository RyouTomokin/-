using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using C;
namespace Tomokin
{

    public class TextInputManager : MonoBehaviour
    {
        public InputField Input;
        public Text Show;

        public ScrollRect scro;

        void Start()
        {
            transform.GetComponent<InputField>().onEndEdit.AddListener(SendMsg);
        }
        public void RessiveMsg(string msg)
        {
            if (msg == "") return;
            Show.text += "\n" + msg;
            scro.verticalNormalizedPosition = -1f;
        }

        public void SendMsg(string msg)
        {
            Input.text = "";
            Net.SendChat(msg);
        }
    }
}