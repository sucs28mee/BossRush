﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent;

namespace BossRush.Common
{
    internal class BossRushColor : ModSystem
    {
        public override void PostUpdateEverything()
        {
            YellowPulseYellowWhiteColorAnimation();
            SynergyColorAnimation();
            RedToBlackColorAnimation();
        }


        public static Color YellowPulseYellowWhite => new Color(YWRed, YWGreen, YWBlue);
        static int YWRed = 150, YWGreen = 150, YWBlue = 0;
        int Switch2 = 0;
        private void YellowPulseYellowWhiteColorAnimation()
        {
            //Pulsing Yellow
            if (Switch2 != 1)
            {
                if (YWRed < 255)
                {//default value 100, 100, 0
                    Math.Clamp(++YWRed, 150, 255);
                    Math.Clamp(++YWGreen, 150, 255);
                    Math.Clamp(++YWBlue, 0, 255);
                }
                else
                {
                    Switch2 = 1;
                }
            }
            else
            {
                if (YWRed > 150)
                {
                    Math.Clamp(--YWRed, 150, 255);
                    Math.Clamp(--YWGreen, 150, 255);
                    Math.Clamp(--YWBlue, 0, 255);
                }
                else
                {
                    Switch2 = 0;
                }
            }
        }
        public static Color SynergyColor => new Color(BWRed, BWGreen, BWBlue);
        static int BWRed = 50, BWGreen = 175, BWBlue = 175;
        int Switch3 = 0;
        private void SynergyColorAnimation()
        {

            if (Switch3 != 1)
            {
                BWRed = Math.Clamp(BWRed + 4, 50, 225);
                BWGreen = Math.Clamp(BWGreen + 1, 175, 225);
                BWBlue = Math.Clamp(BWBlue + 1, 175, 225);
                if (BWBlue >= 225)
                {
                    Switch3 = 1;
                }
            }
            else
            {
                BWRed = Math.Clamp(BWRed - 4, 50, 225);
                BWGreen = Math.Clamp(BWGreen - 1, 175, 225);
                BWBlue = Math.Clamp(BWBlue - 1, 175, 225);
                if (BWBlue <= 175)
                {
                    Switch3 = 0;
                }
            }
        }
        public static Color RedToBlack => new Color(RBred, 0, 0);
        static int RBred = 255;
        int Switch4 = 0;
        private void RedToBlackColorAnimation()
        {
            //Pulsing Purple
            if (Switch4 != 1)
            {
                if (RBred < 255)
                {
                    ++RBred;
                }
                else
                {
                    Switch4 = 1;
                }
            }
            else
            {
                if (RBred > 50)
                {
                    --RBred;
                }
                else
                {
                    Switch4 = 0;
                }
            }
        }
        /// <summary>
        /// This one will keep track somewhat, will reset if the list you put in is different
        /// This is a semi util that does color tranfering like disco color from <see cref="Main.DiscoColor"/><br/>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color MultiColor(List<Color> color, int speed)
        {
            if (progress >= 255)
            {
                progress = 0;
            }
            else
            {
                progress = Math.Clamp(progress + 1 * speed, 0, 255);
            }
            if (color.Count < 1)
            {
                return Color.White;
            }
            if (color.Count < 2)
            {
                return color[0];
            }
            int count = 0;
            foreach(Color c in listcolor)
            {
                if(color.Contains(c))
                {
                    count++;
                }
            }
            if (count != color.Count)
            {
                listcolor = color;
                color1 = new Color();
                color2 = new Color();
            }
            if (color1.Equals(color2))
            {
                color1 = color[currentIndex];
                color3 = color[currentIndex];
                currentIndex = Math.Clamp((currentIndex + 1 >= color.Count) ? 0 : currentIndex + 1, 0, color.Count - 1);
                color2 = color[currentIndex];
                progress = 0;
            }
            if (!color1.Equals(color2))
            {
                color1 = Color.Lerp(color3, color2, Math.Clamp(progress / 255f, 0, 1f));
            }
            return color1;
        }
        private static int currentIndex = 0, progress = 0;
        static Color color1 = new Color(), color2 = new Color(), color3 = new Color();
        static List<Color> listcolor = new List<Color>();
    }
}