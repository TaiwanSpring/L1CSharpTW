using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using System;
namespace LineageServer.Serverpackets
{
    class S_PetMenuPacket : ServerBasePacket
    {

        private byte[] _byte = null;

        public S_PetMenuPacket(L1NpcInstance npc, int exppercet)
        {
            buildpacket(npc, exppercet);
        }

        private void buildpacket(L1NpcInstance npc, int exppercet)
        {
            WriteC(Opcodes.S_OPCODE_SHOWHTML);

            if (npc is L1PetInstance)
            { // ペット
                L1PetInstance pet = (L1PetInstance)npc;
                WriteD(pet.Id);
                WriteS("anicom");
                WriteC(0x00);
                WriteH(0x000b);
                switch (pet.CurrentPetStatus)
                {
                    case 1:
                        WriteS("$469"); // 攻撃態勢
                        break;
                    case 2:
                        WriteS("$470"); // 防御態勢
                        break;
                    case 3:
                        WriteS("$471"); // 休憩
                        break;
                    case 5:
                        WriteS("$472"); // 警戒
                        break;
                    default:
                        WriteS("$471"); // 休憩
                        break;
                }
                WriteS(Convert.ToString(pet.CurrentHp)); // 現在のＨＰ
                WriteS(Convert.ToString(pet.MaxHp)); // 最大ＨＰ
                WriteS(Convert.ToString(pet.CurrentMp)); // 現在のＭＰ
                WriteS(Convert.ToString(pet.MaxMp)); // 最大ＭＰ
                WriteS(Convert.ToString(pet.Level)); // レベル

                // 名前の文字数が8を超えると落ちる
                // なぜか"セント バーナード","ブレイブ ラビット"はOK
                // String pet_name = pet.get_name();
                // if (pet_name.equalsIgnoreCase("ハイ ドーベルマン")) {
                // pet_name = "ハイ ドーベルマ";
                // }
                // else if (pet_name.equalsIgnoreCase("ハイ セントバーナード")) {
                // pet_name = "ハイ セントバー";
                // }
                // WriteS(pet_name);
                WriteS(""); // ペットの名前を表示させると不安定になるので、非表示にする

                string s = "$610";
                if (pet.get_food() > 80)
                {
                    s = "$612"; // 非常飽。
                }
                else if (pet.get_food() > 60)
                {
                    s = "$611"; // 稍微飽。
                }
                else if (pet.get_food() > 30)
                {
                    s = "$610"; // 普通。
                }
                else if (pet.get_food() > 10)
                {
                    s = "$609"; // 稍微餓。
                }
                else if (pet.get_food() >= 0)
                {
                    s = "$608"; // 非常餓。
                }
                WriteS(s); // 飽食度
                WriteS(Convert.ToString(exppercet)); // 経験値
                WriteS(Convert.ToString(pet.Lawful)); // アライメント
            }
            else if (npc is L1SummonInstance)
            { // サモンモンスター
                L1SummonInstance summon = (L1SummonInstance)npc;
                WriteD(summon.Id);
                WriteS("moncom");
                WriteC(0x00);
                WriteH(6); // 渡す引数文字の数の模様
                switch (summon.get_currentPetStatus())
                {
                    case 1:
                        WriteS("$469"); // 攻撃態勢
                        break;
                    case 2:
                        WriteS("$470"); // 防御態勢
                        break;
                    case 3:
                        WriteS("$471"); // 休憩
                        break;
                    case 5:
                        WriteS("$472"); // 警戒
                        break;
                    default:
                        WriteS("$471"); // 休憩
                        break;
                }
                WriteS(Convert.ToString(summon.CurrentHp)); // 現在のＨＰ
                WriteS(Convert.ToString(summon.MaxHp)); // 最大ＨＰ
                WriteS(Convert.ToString(summon.CurrentMp)); // 現在のＭＰ
                WriteS(Convert.ToString(summon.MaxMp)); // 最大ＭＰ
                WriteS(Convert.ToString(summon.Level)); // レベル
                                                        // WriteS(summon.getNpcTemplate().get_nameid());
                                                        // WriteS(Integer.toString(0));
                                                        // WriteS(Integer.toString(790));
            }
        }
    }

}