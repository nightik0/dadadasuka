using System;
using System.Collections.Generic;
using GTANetworkAPI;
using NeptuneEvo.Character;
using NeptuneEvo.Functions;
using NeptuneEvo.Handles;
using Redage.SDK;
using NeptuneEvo.Players;

namespace NeptuneEvo.Fractions
{

    class CleaningSartir : Script //скибиди туалет
    {

        private static nLog Log = new nLog("CleaningSartir");


        [ServerEvent(Event.ResourceStart)]
        public void Event_Cleaning_SartirsStart()
        {
            try
            {
                CustomColShape.CreateCylinderColShape(new Vector3(1703.6437, 2459.4624, 45.802986), 1, 2, 0, ColShapeEnums.ClearPoint, 12); 
                NAPI.Marker.CreateMarker(1, new Vector3(1703.6437, 2459.4624, 45.802986) - new Vector3(0, 0, 1.25), new Vector3(), new Vector3(), 1f, new Color(255, 255, 255, 220)); 

                CustomColShape.CreateCylinderColShape(new Vector3(1705.2689, 2459.4487, 45.802967), 1, 2, 0, ColShapeEnums.ClearPoint, 12); 
                NAPI.Marker.CreateMarker(1, new Vector3(1705.2689, 2459.4487, 45.802967) - new Vector3(0, 0, 1.25), new Vector3(), new Vector3(), 1f, new Color(255, 255, 255, 220)); 

                CustomColShape.CreateCylinderColShape(new Vector3(1706.8789, 2459.453, 45.8024), 1, 2, 0, ColShapeEnums.ClearPoint, 12); 
                NAPI.Marker.CreateMarker(1, new Vector3(1706.8789, 2459.453, 45.8024) - new Vector3(0, 0, 1.25), new Vector3(), new Vector3(), 1f, new Color(255, 255, 255, 220)); 

                CustomColShape.CreateCylinderColShape(new Vector3(1708.4637, 2459.5813, 45.802967), 1, 2, 0, ColShapeEnums.ClearPoint, 12); 
                NAPI.Marker.CreateMarker(1, new Vector3(1708.4637, 2459.5813, 45.802967) - new Vector3(0, 0, 1.25), new Vector3(), new Vector3(), 1f, new Color(255, 255, 255, 220)); 

                CustomColShape.CreateCylinderColShape(new Vector3(1710.0122, 2459.4792, 45.80273), 1, 2, 0, ColShapeEnums.ClearPoint, 12); 
                NAPI.Marker.CreateMarker(1, new Vector3(1710.0122, 2459.4792, 45.80273) - new Vector3(0, 0, 1.25), new Vector3(), new Vector3(), 1f, new Color(255, 255, 255, 220)); 
                Log.Write("Loaded", nLog.Type.Success);
            }
            catch (Exception e)
            {
                Log.Write(e.ToString(), nLog.Type.Error);
            }
        }

        [Interaction(ColShapeEnums.ClearPoint)]
        private static void OpenClearPoint(ExtPlayer player)
        {
            var sessionData = player.GetSessionData();
            if (sessionData == null)
                return;

            var characterData = player.GetCharacterData();
            if (characterData == null)
                return;
            if (sessionData.TimersData.ArrestTimer != null)
            {
                Main.OnAntiAnim(player);
                player.PlayAnimation("anim@amb@drug_field_workers@rake@male_b@base", "base", 39);
                NAPI.Task.Run(() => {
                    try
                    {
                        if (player != null)
                        {
                            player.StopAnimation();
                            Main.OffAntiAnim(player);
                            MoneySystem.Wallet.Change(player, 300);
                            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы успешно почистили сартир!", 3000);
                        }
                    }
                    catch { }
                }, 4000);
            } else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не отсиживаете срок в тюрьме", 3000);
            }
        }
        

    }
}
