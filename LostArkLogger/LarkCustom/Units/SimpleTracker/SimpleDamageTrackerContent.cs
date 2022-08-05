using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using LostArkLogger.LarkCustom;
using LostArkLogger.LarkCustom.UI;
using LostArkLogger.LarkCustom.Extensions;
using LostArkLogger.LarkCustom.Utility;
using LostArkLogger.LarkCustom.Config;

namespace LostArkLogger.LarkCustom.Units
{

    public class SimpleDamageTrackerContent : Content
    {
        public override int MinWidth => 50;

        public override int MinHeight => 50;

        public bool SplitParties { get; set; } = false;
        public bool UserOnTop { get; set; } = false;

        private SimpleDamageTracker SDT;

        private Entity? focusEntity;

        private Entity[] entityOrder = new Entity[8];

        private Dictionary<BrushInd, Brush> brushes = new Dictionary<BrushInd, Brush>();
        private Dictionary<FontInd, Font> fonts = new Dictionary<FontInd, Font>();

        private StringFormat leftAlign;
        private StringFormat centerAlign;
        private StringFormat rightAlign;

        private TimeSpan elapsedRaidTime = default(TimeSpan);

        public SimpleDamageTrackerContent(SimpleDamageTracker sdt)
        {
            SDT = sdt;
            Init();
        }

        public override void Draw(Graphics g, int StartX, int StartY)
        {
            DrawFrame(g, StartX, StartY);
            DrawInfo(g, StartX, StartY);
        }

        private void DrawFrame(Graphics g, int StartX, int StartY)
        {
            g.FillRectangle(brushes[BrushInd.MainBackfill], StartX, StartY, 900, 500);
            g.FillRectangle(brushes[BrushInd.TopInfoBackfill], StartX, StartY, 900, 60);
            g.FillRectangle(brushes[BrushInd.HeaderBackfill], StartX, StartY + 60, 900, 30);
        }

        private void DrawInfo(Graphics g, int StartX, int StartY)
        {
            elapsedRaidTime = SDT.GetEncounterTime();
            if (focusEntity is null)
            {
                List<KeyValuePair<Entity, DamageDataStruct[]>> damageList = new List<KeyValuePair<Entity, DamageDataStruct[]>>();
                Dictionary<Entity, DamageDataStruct[]> PeriodDamage = SDT.GetRaidPeriodDamage();


                if (UserOnTop)
                {
                    damageList.Add(new KeyValuePair<Entity, DamageDataStruct[]>(LarkCustomControl.User, PeriodDamage[LarkCustomControl.User]));
                    PeriodDamage.Remove(LarkCustomControl.User);
                }
                if (SplitParties)
                {
                    var p1DmgFilter = PeriodDamage.Where(x => Config.UserConfig.CurrentUserConfig.PartyMembers.Contains(x.Key));
                    List<KeyValuePair<Entity, DamageDataStruct[]>> p1DmgList = new List<KeyValuePair<Entity, DamageDataStruct[]>>(p1DmgFilter);
                    p1DmgList.Sort((a, b) => a.Value[0].Damage > b.Value[0].Damage ? 1 : -1);
                    foreach (KeyValuePair<Entity, DamageDataStruct[]> item in p1DmgList)
                    {
                        damageList.Add(item);
                        PeriodDamage.Remove(item.Key);
                    }
                }
                List<KeyValuePair<Entity, DamageDataStruct[]>> remainingList = PeriodDamage.ToList();
                remainingList.Sort((a, b) => a.Value[0].Damage > b.Value[0].Damage ? 1 : -1);
                damageList.AddRange(remainingList);

                ulong totalDamage = 0;
                ulong totalPeriodDamage = 0;
                foreach (KeyValuePair<Entity, DamageDataStruct[]> item in damageList)
                {
                    totalDamage += item.Value[0].Damage;
                    totalPeriodDamage += item.Value[1].Damage;
                }

                DrawPlayerInfo(g, StartX, StartY, damageList, totalPeriodDamage, totalDamage);
                DrawTopInfo(g, StartX, StartY, totalDamage);
            }
            
        }

        private void DrawTopInfo(Graphics g, int StartX, int StartY, ulong totalDamage)
        {
            // raid time
            string timeStr = NumberStringFormatter.TimeFormatter(elapsedRaidTime);
            g.DrawString(timeStr, fonts[FontInd.EncounterTimer], whiteBrush, StartX + 10, StartY + 30, leftAlign);

            // DPS Period
            Font topbarInfo = fonts[FontInd.TopBarInfo];

            string dpsPeriodStr = $"DPS Period: {Config.UserConfig.CurrentUserConfig.DPSPeriod}s";
            g.DrawString(timeStr, topbarInfo, whiteBrush, StartX + 400, StartY + 40, leftAlign);

            // Right Info Stuff (DPS and Total Damage);

            g.DrawString("Total Damage ", topbarInfo, whiteBrush, StartX + 685, 20, rightAlign);
            g.DrawString("Total DPS ", topbarInfo, whiteBrush, StartX + 685, StartY + 40, rightAlign);

            g.DrawString($": {totalDamage}", topbarInfo, whiteBrush, StartX + 685, 20, leftAlign);
            g.DrawString($": {totalDamage / elapsedRaidTime.TotalSeconds}", topbarInfo, whiteBrush, StartX + 685, StartY + 40, rightAlign);
        }

        private void DrawBar(Graphics g, int topLeftX, int topLeftY, Entity entity, DamageDataStruct[] damageData, ulong currentDamage, ulong totalDamage)
        {
            Font font = fonts[FontInd.PlayerInfo];
            Brush textBrushBlack = blackBrush;
            Brush textBrushWhite = whiteBrush;

            int textPosYTop = topLeftY + 17;
            int textPosYBot = topLeftX + 33;

            double damagePercentTotal = damageData[0].Damage / (double)currentDamage; // uses the total damage for bar ordering as opposed to period damage
            int barWidth = (int)(damagePercentTotal * 900);
            PlayerClassEnum pce = PlayerClassMethods.GetPlayerClassEnumFromID((uint)entity.EntityId);
            // draw main rectangle
            g.FillRectangle(Config.UserConfig.CurrentUserConfig.ClassBrushes.GetClassBackgroundColor(pce), 0, topLeftY, 900, 50);

            // write text
            // User and Class
            string playerName = entity.Name;
            string iLevelClass = $"{entity.GearScore} {entity.ClassName}";
            g.DrawString(playerName, font, textBrushWhite, topLeftX + 55, textPosYTop);
            g.DrawString(iLevelClass, font, textBrushBlack, topLeftX + 55, textPosYBot);

            // Data

            string dmgTotal = NumberStringFormatter.FormatLargeInt(damageData[0].Damage);
            g.DrawString(dmgTotal, font, textBrushWhite, topLeftX + 350, textPosYTop, centerAlign);
            string dmgPeriod = NumberStringFormatter.FormatLargeInt(damageData[0].Damage);
            g.DrawString(dmgPeriod, font, textBrushBlack, topLeftX + 350, textPosYBot, centerAlign);

            double dmgPerTotal = damageData[0].Damage / (double)currentDamage;
            string dmgPerTotalStr = NumberStringFormatter.FormatPercent(dmgPerTotal);
            g.DrawString(dmgPerTotalStr, font, textBrushWhite, topLeftX + 450, textPosYTop, centerAlign);
            double dmgPerPeriod = damageData[1].Damage / (double)currentDamage;
            string dmgPerPeriodStr = NumberStringFormatter.FormatPercent(dmgPerTotal);
            g.DrawString(dmgPerPeriodStr, font, textBrushBlack, topLeftX + 450, textPosYBot, centerAlign);

            string dpsTotal = NumberStringFormatter.FormatLargeInt((ulong)(damageData[0].Damage / (double)Config.UserConfig.CurrentUserConfig.DPSPeriod));
            g.DrawString(dpsTotal, font, textBrushWhite, topLeftX + 550, textPosYTop, centerAlign);
            string dpsPeriod = NumberStringFormatter.FormatLargeInt((ulong)(damageData[1].Damage / elapsedRaidTime.TotalSeconds));
            g.DrawString(dpsPeriod, font, textBrushBlack, topLeftX + 550, textPosYBot, centerAlign);

            string critPerTotalStr = NumberStringFormatter.FormatPercent(damageData[0].CritRate);
            g.DrawString(critPerTotalStr, font, textBrushWhite, topLeftX + 650, textPosYTop, centerAlign);
            string critPerPeriodStr = NumberStringFormatter.FormatPercent(damageData[1].CritRate);
            g.DrawString(critPerPeriodStr, font, textBrushBlack, topLeftX + 650, textPosYBot, centerAlign);

            string faPerTotalStr = NumberStringFormatter.FormatPercent(damageData[0].FrontAttackRate);
            g.DrawString(faPerTotalStr, font, textBrushWhite, topLeftX + 730, textPosYTop, centerAlign);
            string faPerPeriodStr = NumberStringFormatter.FormatPercent(damageData[1].FrontAttackRate);
            g.DrawString(faPerPeriodStr, font, textBrushBlack, topLeftX + 730, textPosYBot, centerAlign);

            string baPerTotalStr = NumberStringFormatter.FormatPercent(damageData[0].BackAttackRate);
            g.DrawString(baPerTotalStr, font, textBrushWhite, topLeftX + 810, textPosYTop, centerAlign);
            string baPerPeriodStr = NumberStringFormatter.FormatPercent(damageData[1].BackAttackRate);
            g.DrawString(baPerPeriodStr, font, textBrushBlack, topLeftX + 810, textPosYTop, centerAlign);

            string ctrTotalStr = damageData[0].CounterAttacks.ToString();
            g.DrawString(ctrTotalStr, font, textBrushWhite, topLeftX + 875, textPosYTop, centerAlign);
            string ctrPeriodStr = damageData[1].CounterAttacks.ToString();
            g.DrawString(ctrPeriodStr, font, textBrushBlack, topLeftX + 875, textPosYBot, centerAlign);

            // Draw Class Icon
            Rectangle spriteRect = PlayerClassMethods.GetSpriteLocation(pce);
        }
        private void DrawPlayerInfo(Graphics g, int StartX, int StartY, List<KeyValuePair<Entity, DamageDataStruct[]>> damageList, ulong currentDamage, ulong totalDamage)
        {
            Font font = fonts[FontInd.PlayerInfo];
            Brush textBrush = brushes[BrushInd.InfoTextBlack];

            int offsetY = 0;
            for (int i = 0; i < 4 && i < damageList.Count; i++, offsetY += 50)
            {
                DrawBar(g, StartX, StartY + offsetY, damageList[i].Key, damageList[i].Value, currentDamage, totalDamage);
            }
            if (SplitParties)
            {
                g.FillRectangle(brushes[BrushInd.PartyDivider], 0, StartY + offsetY, 900, 10);
                offsetY += 10;
            }
            for (int i = 4; i < 8 && i < damageList.Count; i++, offsetY += 50)
            {
                DrawBar(g, StartX, StartY + offsetY, damageList[i].Key, damageList[i].Value, currentDamage, totalDamage);
            }
        }

        private enum BrushInd
        {
            MainBackfill = 0,
            InfoTextWhite = 1,
            HeaderBackfill = 2,
            TopInfoBackfill = 3,
            PartySeperatorBackfill = 4,
            InfoTextBlack = 5,
            PartyDivider = 6
        }

        private enum FontInd
        {
            PlayerInfo = 0,
            TopBarInfo = 1,
            Header = 2,
            EncounterTimer = 3
        }

        private Brush whiteBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        private Brush blackBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));

        public void Init()
        {
            brushes[(BrushInd)0] = new SolidBrush(Color.FromArgb(77, 50, 50, 50));      // Main backfill
            brushes[(BrushInd)1] = whiteBrush;                                          // text color
            brushes[(BrushInd)2] = new SolidBrush(Color.FromArgb(191, 0, 0, 0));        // Header backfill color;
            brushes[(BrushInd)3] = new SolidBrush(Color.FromArgb(128, 0, 0, 0));        // Top Info backfill,
            brushes[(BrushInd)4] = brushes[BrushInd.TopInfoBackfill];                   // PartySeperatorBackfill
            brushes[(BrushInd)5] = blackBrush;                                          // Player Info Text
            brushes[(BrushInd)6] = blackBrush;

            fonts[(FontInd)0] = new Font("Calibri", 16, FontStyle.Regular);  // Player Info Font
            fonts[(FontInd)1] = fonts[FontInd.PlayerInfo];                   // Top bar Info
            fonts[(FontInd)2] = new Font("Calibri", 14, FontStyle.Regular);  // Header Font
            fonts[(FontInd)3] = new Font("Calibri", 44, FontStyle.Regular);  // Encounter Timer

            leftAlign = new StringFormat();
            leftAlign.Alignment = StringAlignment.Near;
            leftAlign.LineAlignment = StringAlignment.Center;

            centerAlign = new StringFormat();
            centerAlign.Alignment = StringAlignment.Center;
            centerAlign.LineAlignment = StringAlignment.Center;

            rightAlign = new StringFormat();
            rightAlign.Alignment = StringAlignment.Far;
            rightAlign.LineAlignment = StringAlignment.Center;
        }

        public override void OnMouseClick(int startX, int startY, MouseButtons button)
        {
            return;
        }

        public override void OnMouseDoubleClick(int startX, int startY, MouseButtons button)
        {
            return;
        }

        public override void OnMouseDown(int startX, int startY, MouseButtons button)
        {
            return;
        }

        public override void OnMouseHover(int startX, int startY)
        {
            return;
        }

        public override void OnMouseMove(int startX, int startY)
        {
            return;
        }

        public override void OnMouseScroll(int startX, int startY, int Delta)
        {
            return;
        }

        public override void OnMouseUp(int startX, int startY, MouseButtons button)
        {
            return;
        }

        public override void Reframe(int width, int height)
        {
            return;
        }
    }
}
