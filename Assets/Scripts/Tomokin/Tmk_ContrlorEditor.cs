//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;
//using Peixi;

//namespace Tomokin
//{
//    [CustomEditor(typeof(Tmk_Controlor))]
//    public class Tmk_ContrlorEditor : Editor
//    {
//        Tmk_Controlor control;
//        private void OnEnable()
//        {
//            control = (Tmk_Controlor)target;
//        }

//        public override void OnInspectorGUI()
//        {
//            if (GUILayout.Button("初始化玩家数据"))
//            {
//                Debug.Log("初始化玩家数据");
//                List<string> names = new List<string>();
//                names.Add("LZQ");
//                names.Add("LPX");
//                names.Add("CJH");
//                CilentManager.PlayerName = "LZQ";
//                FindObjectOfType<RoomManager>().GameStartInit(names, true, false);
//            }
//            if (GUILayout.Button("开始谈判阶段"))
//            {
//                InitProps();
//                GameManager.Stages = 3;
//                GameManager.Instance.StartStage();
//            }
//            if (GUILayout.Button("展示提案"))
//            {
//                InitProps();
//                GameManager.Instance.InputProposol();
//            }
//        }

//        void InitProps()
//        {
//            ProposalManager.PropsofthisTurn = new List<Proposal>();
//            ProposalManager.PropsofthisTurn.Add(
//                    new Proposal(0, null,
//                    GameManager.Instance.CardsInLibarary[20],
//                    CilentManager.PDs[0]));

//            ProposalManager.PropsofthisTurn.Add(
//                new Proposal(0, GameManager.Instance.CardsInLibarary[12],
//                null,
//                CilentManager.PDs[1]));

//            ProposalManager.PropsofthisTurn.Add(
//                new Proposal(0, GameManager.Instance.CardsInLibarary[15],
//                GameManager.Instance.CardsInLibarary[18],
//                CilentManager.PDs[2]));
//            GameManager.Instance.InputProposol();
//        }

//    }
//}