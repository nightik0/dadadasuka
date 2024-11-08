using GTANetworkAPI;
using System.Collections.Generic;
using System;
using NeptuneEvo.GUI;
using NeptuneEvo.Core;
using Redage.SDK;
using NeptuneEvo.Handles;
using NeptuneEvo.Functions;
using NeptuneEvo.Players;
using NeptuneEvo.Character;

namespace NeptuneEvo.Jobs
{
    class ClothesLoaderPrison : Script
    {
        private static Vector3 ResetPackages = new Vector3(1747.292, 2597.082, 44.677624);

        private static nLog Log = new nLog("ClothesLoaderPrison");

        [ServerEvent(Event.ResourceStart)]
        public void Event_ResourceStart()
        { //СКИБИДИ ТУАЛЕт
            try
            {
                // NAPI.TextLabel.CreateTextLabel("~w~Пахан\nРабота", new Vector3(1743.0337, 2598.09, 45.677624), 30f, 0.3f, 0, new Color(255, 255, 255), true, NAPI.GlobalDimension);
                PedSystem.Repository.CreateQuest("mp_m_securoguard_01", new Vector3(1743.0337, 2598.09, 45.677624), 164.93776f, title: "~w~Пахан\nРабота", colShapeEnums: ColShapeEnums.ClothesLoaderPrisonWork);
                // CustomColShape.CreateCylinderColShape(new Vector3(1743.0337, 2598.09, 44.677624), 1, 2, 0, ColShapeEnums.ClothesLoaderPrisonWork);

                int i = 0;
                foreach (var Check in Checkpoints)
                {
                    CustomColShape.CreateCylinderColShape(Check.Position, 2, 2, 0, ColShapeEnums.ClothesLoaderPrisonPoint, i);
                    i++;
                };

               CustomColShape.CreateCylinderColShape(ResetPackages, 2, 2, 0, ColShapeEnums.ClothesLoaderPrisonReset);
            }
            catch (Exception e) { Log.Write("ResourceStart: " + e.Message, nLog.Type.Error); }
        }

        #region Чекпоинты
        private static List<Checkpoint> Checkpoints = new List<Checkpoint>()
        {
            new Checkpoint(new Vector3(1749.0356, 2574.5205, 44.672787), -149.16914), // Взять ящик 0
            new Checkpoint(new Vector3(1739.7655, 2571.6172, 44.67758), -102.7481), // Взять ящик 1
            new Checkpoint(new Vector3(1731.0586, 2578.2852, 44.67758), -137.2688), // Взять ящик 2
            new Checkpoint(new Vector3(1720.9757, 2581.8801, 44.677597), 57.02116), // Взять ящик 3
            new Checkpoint(new Vector3(1723.0612, 2572.4907, 44.677597), -144.57974), // Взять ящик 4
            new Checkpoint(new Vector3(1719.3806, 2577.187, 45.695305), -51.41109), // Взять ящик 5
            new Checkpoint(new Vector3(1726.6848, 2582.3699, 50.88234), 36.996304), // Взять ящик 6
            new Checkpoint(new Vector3(1723.1581, 2588.9502, 50.882725), -134.91966), // Взять ящик 7
            new Checkpoint(new Vector3(1739.0577, 2583.702, 50.882675), 128.99614), // Взять ящик 8
            new Checkpoint(new Vector3(1734.5684, 2569.263, 50.882683), -47.08526), // Взять ящик 9
            new Checkpoint(new Vector3(1723.4706, 2571.028, 50.882713), -2.7784252), // Взять ящик 10
            new Checkpoint(new Vector3(1745.6943, 2590.9993, 44.677586), 161.0474), // Взять ящик 11
            new Checkpoint(new Vector3(1734.3076, 2567.6748, 44.67758), -67.442444), // Взять ящик 12
            new Checkpoint(new Vector3(1740.2888, 2566.7712, 44.67758), -144.42505), // Взять ящик 13
            new Checkpoint(new Vector3(1754.3486, 2570.9521, 44.67758), 8.331219), // Взять ящик 14
            new Checkpoint(new Vector3(1747.3105, 2566.6665, 44.67758), 20.071125), // Взять ящик 15
        };
        #endregion

        #region Устроться на работу
        [Interaction(ColShapeEnums.ClothesLoaderPrisonWork)]
        public static void JobJoin(ExtPlayer player)
        {
            var sessionData = player.GetSessionData();
            if (sessionData == null)
                return;

            var characterData = player.GetCharacterData();
            if (characterData == null)
                return;

            if (sessionData.TimersData.ArrestTimer != null)
            {

                if (NAPI.Data.GetEntityData(player, "ON_WORK") == true)
                {
                    ClothesLoaderPrisonLayoff(player);
                }
                else
                {
                    ClothesLoaderPrisonJoin(player);
                }
                return;
            }else{
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не отсиживаете срок в тюрьме", 3000);
            }
        }
        #endregion

        #region Закончить/Начать рабочий день
        public static void ClothesLoaderPrisonLayoff(ExtPlayer player)
        {
            if (NAPI.Data.GetEntityData(player, "ON_WORK") != false)
            {
                Customization.ApplyCharacter(player);
                player.SetData("ON_WORK", false);
                player.SetData("ON_WORK2", 0);
                Trigger.ClientEvent(player, "deleteCheckpoint", 15);
                Trigger.ClientEvent(player, "deleteWorkBlip");
                player.SetData("PACKAGES", 0);

            }
            else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы уже не работаете", 3000);
            }
        }
        public static void ClothesLoaderPrisonJoin(ExtPlayer player)
        {
            var characterData = player.CharacterData;

            //Customization.ClearClothes(player, characterData.Gender);
            if (characterData.Gender)
            {
                //player.SetAccessories(1, 24, 2);
                //player.SetClothes(3, 2, 0);
                player.SetClothes(8, 59, 0);
                player.SetClothes(11, 1, 0);
                player.SetClothes(4, 0, 5);
                player.SetClothes(6, 48, 0);
            }
            else
            {
                //player.SetAccessories(1, 26, 2);
                //player.SetClothes(3, 11, 0);
                player.SetClothes(8, 36, 0);
                player.SetClothes(11, 0, 0);
                player.SetClothes(4, 1, 5);
                player.SetClothes(6, 49, 0);
            }
            var check = WorkManager.rnd.Next(0, Checkpoints.Count - 1);
            player.SetData("WORKCHECK", check);
            Trigger.ClientEvent(player, "createCheckpoint", 15, 0, Checkpoints[check].Position, 2, 0, 255, 255, 255);
            player.SetData("PACKAGES", 0);

            player.SetData("ON_WORK", true);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы начали работать. Собирайте коробки по тюрьме и относите их Пахану.", 5000);

        }
        #endregion
        #region Когда заходишь в чекпоинт
        [Interaction(ColShapeEnums.ClothesLoaderPrisonReset, In: true)]
        private static void PlayerEnterReset(ExtPlayer player)
        {
            try
            {
                var characterData = player.CharacterData;
                if (player.GetData<int>("WORKCHECK") == 0)
                {
                    var packageplayer = player.GetData<int>("PACKAGES");
                    //player.SetData("PACKAGES", player.GetData<int>("PACKAGES") + 1);
                    player.PlayAnimation("anim@mp_snowball", "pickup_snowball", 49);

                    player.SetData("WORKCHECK", -1);
                    var check = WorkManager.rnd.Next(0, Checkpoints.Count - 1);
                    player.SetData("WORKCHECK", check);
                    Trigger.ClientEvent(player, "createCheckpoint", 15, 0, Checkpoints[check].Position, 2, 0, 255, 255, 255);
                    Trigger.ClientEvent(player, "createWorkBlip", Checkpoints[check].Position);
                    NAPI.Task.Run(() =>
                    {
                        try
                        {
                            if (player != null)
                            {
                                BasicSync.DetachObject(player);
                                player.StopAnimation();
                                Main.OffAntiAnim(player);
                                MoneySystem.Wallet.Change(player, 600);
                            }
                        }
                        catch { }
                    }, 400);

                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы нашли и отнесли {packageplayer} коробок.", 5000);
                }
            }
            catch { }
        }

        [Interaction(ColShapeEnums.ClothesLoaderPrisonPoint, In:true)]
        private static void PlayerEnterCheckpoint(ExtPlayer player, int colID)
        {
            try
            {
                var characterData = player.CharacterData;

                if (!player.GetData<bool>("ON_WORK")) return;
                var packageplayer = player.GetData<int>("PACKAGES");
                    if (colID == player.GetData<int>("WORKCHECK"))
                    {
                        player.SetData("PACKAGES", player.GetData<int>("PACKAGES") + 1);

                        //NAPI.Entity.SetEntityPosition(player, Checkpoints[colID].Position + new Vector3(0, 0, 1.2));
                        //NAPI.Entity.SetEntityRotation(player, new Vector3(0, 0, Checkpoints[colID].Heading));

                        //Main.OnAntiAnim(player);
                        player.PlayAnimation("anim@mp_snowball", "pickup_snowball", 49);

                        player.SetData("WORKCHECK", -1);
                        player.SetData("WORKCHECK", 0);
                        Trigger.ClientEvent(player, "createCheckpoint", 15, 1, ResetPackages, 3, 0, 255, 255, 255);
                        Trigger.ClientEvent(player, "createWorkBlip", ResetPackages);

                        NAPI.Task.Run(() =>
                        {
                            try
                            {
                                if (player != null)
                                {
                                    player.PlayAnimation("anim@heists@box_carry@", "idle", 49);
                                    BasicSync.AttachObjectToPlayer(player, NAPI.Util.GetHashKey("v_ind_cf_chckbox2"), 18905, new Vector3(0.1, 0.1, 0.3), new Vector3(-10, -75, -40));
                                }
                            }
                            catch { }
                        }, 500);
                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы взяли коробку, сдайте ее Пахану", 5000);
                }

            }
            catch (Exception e) { Log.Write("PlayerEnterCheckpoint: " + e.Message, nLog.Type.Error); }
        }
        #endregion
        #region Если игрок умер
        public static void Event_PlayerDeath(ExtPlayer player, ExtPlayer entityKiller, uint weapon)
        {
            try
            {
                var characterData = player.CharacterData;

                if (player == null) return;
                if (player.GetData<bool>("ON_WORK"))
                {
                    Customization.ApplyCharacter(player);
                    player.SetData("ON_WORK", false);
                    player.SetData("ON_WORK2", 0);
                    Trigger.ClientEvent(player, "deleteCheckpoint", 15);
                    Trigger.ClientEvent(player, "deleteWorkBlip");
                    player.SetData("PACKAGES", 0);

                    player.StopAnimation();
                    Main.OffAntiAnim(player);
                }
            }
            catch (Exception e) { Log.Write("PlayerDeath: " + e.Message, nLog.Type.Error); }
        }
        #endregion
        #region Если игрок вышел из игры или его кикнуло
        public static void Event_PlayerDisconnected(ExtPlayer player, DisconnectionType type, string reason)
        {
            try
            {
                var characterData = player.CharacterData;

                if (player.GetData<bool>("ON_WORK"))
                {
                    Customization.ApplyCharacter(player);
                    player.SetData("ON_WORK", false);
                    player.SetData("ON_WORK2", 0);
                    Trigger.ClientEvent(player, "deleteCheckpoint", 15);
                    Trigger.ClientEvent(player, "deleteWorkBlip");
                    player.SetData("PACKAGES", 0);

                    player.StopAnimation();
                    Main.OffAntiAnim(player);
                    BasicSync.DetachObject(player);
                    //MoneySystem.Wallet.Change(player, player.GetData("PAYMENT"));
                    player.SetData("PAYMENT", 0);
                }
            }
            catch (Exception e) { Log.Write("PlayerDisconnected: " + e.Message, nLog.Type.Error); }
        }
        #endregion
        internal class Checkpoint
        {
            public Vector3 Position { get; }
            public double Heading { get; }

            public Checkpoint(Vector3 pos, double rot)
            {
                Position = pos;
                Heading = rot;
            }
        }
    }
}
