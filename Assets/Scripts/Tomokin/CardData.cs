using UnityEngine;
using UnityEditor;
namespace Tomokin
{
    [CreateAssetMenu(fileName = "Card001", menuName = "NewCard")]
    public class CardData : ScriptableObject
    {
        [SerializeField]
        private int[] RGB = new int[3];
        [SerializeField]
        private Preferences pref;
        [SerializeField]
        private string c_name;
        public enum Preferences
        {
            win_win,
            Cooperate,
            Combat
        };
        private int order;
        private bool IsInLib = true;

        public Sprite icon = null;

        public int Get_R
        {
            get { return RGB[0]; }
        }
        public int Get_G
        {
            get { return RGB[1]; }
        }
        public int Get_B
        {
            get { return RGB[2]; }
        }
        public int GetByNum(int n)
        {
            return RGB[n];
        }
        public int GetPreference
        {
            get { return (int)pref; }
        }
        public int Get_Order
        {
            get { return order; }
            set { order = value; }
        }
        public string Get_CardName
        {
            get { return c_name; }
        }

        public bool Get_IsInLib { get => IsInLib; set => IsInLib = value; }
    }
}