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
            Show.text += "\n" + msg;
        }

        public void SendMsg(string msg)
        {
            Input.text = "";
            Net.SendChat(msg);
            scro.verticalNormalizedPosition = -0.1f;
        }
    }
}