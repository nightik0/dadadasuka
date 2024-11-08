using GTANetworkAPI;
using NeptuneEvo.Handles;
using NeptuneEvo.Accounts;
using NeptuneEvo.Players.Models;
using NeptuneEvo.Players;
using NeptuneEvo.Character.Models;
using NeptuneEvo.Character;
using NeptuneEvo.Chars;
using Newtonsoft.Json;
using Redage.SDK;
using System;

namespace NeptuneEvo.Core
{
    class BasicSync : Script //СКИБИДИ ТУАЛЕТ
    {
        private static readonly nLog Log = new nLog("Core.BasicSync");
        [RemoteEvent("invisible")]
        public static void SetInvisible(ExtPlayer player, bool toggle)
        {
            try
            {
                var accountData = player.GetAccountData();
                if (accountData == null)
                    return;

                var characterData = player.GetCharacterData();
                if (characterData == null)
                    return;

                if (characterData.AdminLVL == 0 && accountData.VipLvl != 5) return;
                player.Transparency = (toggle) ? 0 : 255;
                if (toggle)
                {
                    player.SetSharedData("INVISIBLE", true);
                    Trigger.ClientEvent(player, "SetINVISIBLE", true);
                }
                else
                {
                    player.ResetSharedData("INVISIBLE");
                    Trigger.ClientEvent(player, "SetINVISIBLE", false);
                }
                var adminConfig = characterData.ConfigData.AdminOption;
                adminConfig.Invisible = toggle;
            }
            catch (Exception e)
            {
                Log.Write($"SetInvisible Exception: {e.ToString()}");
            }
        }

        public static void AttachObjectToPlayer(Player player, uint model, int bone, Vector3 posOffset, Vector3 rotOffset)
        {
            var attObj = new AttachedObject(model, bone, posOffset, rotOffset);
            player.SetSharedData("attachedObject", JsonConvert.SerializeObject(attObj));
            Trigger.ClientEventInRange(player.Position, 550, "attachObject", player);
        }

        public static void DetachObject(Player player)
        {
            player.ResetSharedData("attachedObject");
            Trigger.ClientEventInRange(player.Position, 550, "detachObject", player);
        }

        internal class AttachedObject
        {
            public uint Model { get; set; }
            public int Bone { get; set; }
            public Vector3 PosOffset { get; set; }
            public Vector3 RotOffset { get; set; }

            public AttachedObject(uint model, int bone, Vector3 pos, Vector3 rot)
            {
                Model = model;
                Bone = bone;
                PosOffset = pos;
                RotOffset = rot;
            }
        }

        public static bool GetInvisible(ExtPlayer player)
        {
            try
            {
                var characterData = player.GetCharacterData();
                if (characterData == null)
                    return false;

                var adminConfig = characterData.ConfigData.AdminOption;
                return adminConfig.Invisible;
            }
            catch (Exception e)
            {
                Log.Write($"GetInvisible Exception: {e.ToString()}");
                return false;
            }
        }
    }
} //СКИБИДИ ТУАЛЕТ
